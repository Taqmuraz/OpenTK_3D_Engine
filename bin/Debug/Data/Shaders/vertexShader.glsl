#version 400 core

in vec3 position;
in vec2 textureCoords;
in vec3 normal;

out vec2 pass_textureCoords;
out vec3 surfaceNormal;
out vec3 toLightVector;
out vec3 toCameraVector;
out float fogVisibility;

uniform mat4 transformationMatrix;
uniform mat4 projectionMatrix;
uniform mat4 viewMatrix;
uniform vec3 lightPosition;
uniform vec2 textureVector;

out vec2 pass_textureCoords;
out vec3 surfaceNormal;
out vec3 toLightVector;

void main ()
{
	mat4 m = transformationMatrix * viewMatrix * projectionMatrix;
	pass_textureCoords = textureCoords * textureVector.xy + textureVector.zw;
	vec4 pos = m * worldPosition;
	
	surfaceNormal = normalize(m * float4(normal.x, normal.y, normal.z, 0.0));
	toLightVector = normalize(lightPosition - pos);
	
	gl_Position = pos;
}



