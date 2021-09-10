using System.Collections.Generic;
using System.IO;

namespace Engine.Rendering
{
	public class OBJFileLoader
    {
        class Vertex
        {
            private const int NO_INDEX = -1;

            public Vertex(int id, Vector3 vertex)
			{
				this.id = id;
				this.vertex = vertex;
			}

			public int id { get; private set; }
            public Vector3 vertex { get; private set; }
            public int textureIndex { get; set; } = NO_INDEX;
            public int normalIndex { get; set; } = NO_INDEX;
            public Vertex duplicateVertex { get; set; } = null;

            public bool isSet
            {
                get => textureIndex != NO_INDEX && normalIndex != NO_INDEX;
            }

            public bool HasSameTextureAndNormal(int textureIndexOther, int normalIndexOther)
            {
                return textureIndexOther == textureIndex && normalIndexOther == normalIndex;
            }
        }


        private static Dictionary<string, Mesh> loadedModels = new Dictionary<string, Mesh>();

        static float ParseFloat(string value)
		{
            return System.Convert.ToSingle(value, System.Globalization.CultureInfo.InvariantCulture);
		}

        public static Mesh LoadOBJ(string objFileName)
        {
            if (string.IsNullOrEmpty(objFileName)) throw new System.ArgumentNullException("objFileName");

            if (loadedModels.ContainsKey(objFileName)) return loadedModels[objFileName];

            List<Vertex> vertices = new List<Vertex>();
            List<Vector2> textures = new List<Vector2>();
            List<Vector3> normals = new List<Vector3>();
            List<int> indices = new List<int>();

            using (FileStream objFile = File.OpenRead(objFileName))
            {
                using (StreamReader reader = new StreamReader(objFile))
                {
                    string line;
                    try
                    {
                        while (true)
                        {
                            line = reader.ReadLine();
                            if (line.StartsWith("v "))
                            {
                                string[] currentLine = line.Split(' ');
                                Vector3 vertex = new Vector3(
                                    ParseFloat(currentLine[1]),
                                    ParseFloat(currentLine[2]),
                                    ParseFloat(currentLine[3]));
                                Vertex newVertex = new Vertex(vertices.Count, vertex);
                                vertices.Add(newVertex);
                            }
                            else if (line.StartsWith("vt "))
                            {
                                string[] currentLine = line.Split(' ');
                                Vector2 texture = new Vector2(
                                    ParseFloat(currentLine[1]),
                                    ParseFloat(currentLine[2]));
                                textures.Add(texture);
                            }
                            else if (line.StartsWith("vn "))
                            {
                                string[] currentLine = line.Split(' ');
                                Vector3 normal = new Vector3(
                                    ParseFloat(currentLine[1]),
                                    ParseFloat(currentLine[2]),
                                    ParseFloat(currentLine[3]));
                                normals.Add(normal);
                            }
                            else if (line.StartsWith("f "))
                            {
                                break;
                            }
                        }
                        while (line != null)
                        {
                            if (!line.StartsWith("f "))
                            {
                                line = reader.ReadLine();
                                continue;
                            }
                            string[] currentLine = line.Split(' ');
                            string[] vertex1 = currentLine[1].Split('/');
                            string[] vertex2 = currentLine[2].Split('/');
                            string[] vertex3 = currentLine[3].Split('/');
                            ProcessVertex(vertex1, vertices, indices);
                            ProcessVertex(vertex2, vertices, indices);
                            ProcessVertex(vertex3, vertices, indices);
                            line = reader.ReadLine();
                        }
                    }
                    catch (System.Exception e)
                    {
                        Debug.LogError(e);
                    }
                }
            }
            RemoveUnusedVertices(vertices);
            float[] verticesArray = new float[vertices.Count * 3];
            float[] texturesArray = new float[vertices.Count * 2];
            float[] normalsArray = new float[vertices.Count * 3];
            float furthest = ConvertDataToArrays(vertices, textures, normals, verticesArray,
                    texturesArray, normalsArray);
            int[] indicesArray = indices.ToArray();
            Mesh mesh = new Mesh(verticesArray, texturesArray, normalsArray, indicesArray,
                    furthest);

            loadedModels.Add(objFileName, mesh);

            return mesh;
        }

        private static void ProcessVertex(string[] vertex, List<Vertex> vertices, List<int> indices)
        {
            int index = int.Parse(vertex[0]) - 1;
            Vertex currentVertex = vertices[index];
            int textureIndex = int.Parse(vertex[1]) - 1;
            int normalIndex = int.Parse(vertex[2]) - 1;
            if (!currentVertex.isSet)
            {
                currentVertex.textureIndex = textureIndex;
                currentVertex.normalIndex = normalIndex;
                indices.Add(index);
            }
            else
            {
                DealWithAlreadyProcessedVertex(currentVertex, textureIndex, normalIndex, indices,
                        vertices);
            }
        }

        private static float ConvertDataToArrays(List<Vertex> vertices, List<Vector2> textures,
                List<Vector3> normals, float[] verticesArray, float[] texturesArray,
                float[] normalsArray)
        {
            float furthestPoint = 0;
            for (int i = 0; i < vertices.Count; i++)
            {
                Vertex currentVertex = vertices[i];
                if (currentVertex.vertex.length > furthestPoint)
                {
                    furthestPoint = currentVertex.vertex.length;
                }
                Vector3 position = currentVertex.vertex;
                Vector2 textureCoord = textures[currentVertex.textureIndex];
                Vector3 normalVector = normals[currentVertex.normalIndex];
                verticesArray[i * 3] = position.x;
                verticesArray[i * 3 + 1] = position.y;
                verticesArray[i * 3 + 2] = position.z;
                texturesArray[i * 2] = textureCoord.x;
                texturesArray[i * 2 + 1] = 1 - textureCoord.y;
                normalsArray[i * 3] = normalVector.x;
                normalsArray[i * 3 + 1] = normalVector.y;
                normalsArray[i * 3 + 2] = normalVector.z;
            }
            return furthestPoint;
        }

        private static void DealWithAlreadyProcessedVertex(Vertex previousVertex, int newTextureIndex,
                int newNormalIndex, List<int> indices, List<Vertex> vertices)
        {
            if (previousVertex.HasSameTextureAndNormal(newTextureIndex, newNormalIndex))
            {
                indices.Add(previousVertex.id);
            }
            else
            {
                Vertex anotherVertex = previousVertex.duplicateVertex;
                if (anotherVertex != null)
                {
                    DealWithAlreadyProcessedVertex(anotherVertex, newTextureIndex, newNormalIndex,
                            indices, vertices);
                }
                else
                {
                    Vertex duplicateVertex = new Vertex(vertices.Count, previousVertex.vertex);
                    duplicateVertex.textureIndex = newTextureIndex;
                    duplicateVertex.normalIndex = newNormalIndex;
                    previousVertex.duplicateVertex = duplicateVertex;
                    vertices.Add(duplicateVertex);
                    indices.Add(duplicateVertex.id);
                }

            }
        }

        private static void RemoveUnusedVertices(List<Vertex> vertices)
        {
            foreach (Vertex vertex in vertices)
            {
                if (!vertex.isSet)
                {
                    vertex.textureIndex = 0;
                    vertex.normalIndex = 0;
                }
            }
        }
    }
}
