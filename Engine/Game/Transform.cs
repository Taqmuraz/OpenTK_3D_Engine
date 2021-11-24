using System.Collections.Generic;

namespace Engine.Game
{
	public sealed class Transform : Component
	{
		public Vector3 localPosition 
		{
			get
			{
				return m_localPosition;
			}
			set
			{
				m_localPosition = value;
				UpdateMatrix();
			}
		}
		/*public Vector3 localEulerAngles
		{
			get
			{
				return m_localEulerAngles;
			}
			set
			{
				m_localEulerAngles = value;
				UpdateMatrix();
			}
		}*/
		Vector3 m_localPosition = Vector3.zero;
		Vector3 m_localScale = Vector3.one;
		Quaternion m_localRotation = Quaternion.identity;
		public Quaternion localRotation
		{
			get
			{
				return m_localRotation;
			}
			set
			{
				m_localRotation = value;
				UpdateMatrix();
			}
		}
		public Vector3 localScale
		{
			get
			{
				return m_localScale;
			}
			set
			{
				m_localScale = value;
				UpdateMatrix();
			}
		}
		Vector3 m_position = Vector3.zero;
		Quaternion m_rotation = Quaternion.identity;
		public Vector3 position
		{
			get => m_position;
			set
			{
				Vector3 pos = parentMatrixInv.MultiplyPoint(value);
				localPosition = pos;
			}
		}
		public Quaternion rotation
		{
			get => m_rotation;
			set
			{
				localRotation = parentMatrixInv.GetRotation() * value;
				UpdateMatrix();
			}
		}
		public Vector3 right { get; private set; }
		public Vector3 up { get; private set; }
		public Vector3 forward { get; private set; }
		public Vector3 scale { get; private set; }

		Transform m_parent;
		public Transform parent
		{
			get
			{
				return m_parent;
			}
			set
			{
				if (m_parent != value)
				{
					if (m_parent != null) m_parent.childs.Remove(this);

					if (value != this && value != null)
					{
						m_parent = value;
						m_parent.childs.Add(this);
					}
					else m_parent = null;
				}
			}
		}

		Matrix4x4 parentMatrix = Matrix4x4.identity;
		Matrix4x4 parentMatrixInv = Matrix4x4.identity;

		HashSet<Transform> childs = new HashSet<Transform>();

		void UpdateMatrix()
		{
			UpdateMatrix(parent == null ? Matrix4x4.identity : parent.localToWorld);
		}

		void UpdateMatrix(Matrix4x4 parentToWorld)
		{
			Matrix4x4 local = Matrix4x4.identity;
			var rotationMatrix = localRotation.ToMatrix();
			local.column_0 = rotationMatrix.column_0.normalized * m_localScale.x;
			local.column_1 = rotationMatrix.column_1.normalized * m_localScale.y;
			local.column_2 = rotationMatrix.column_2.normalized * m_localScale.z;
			local.column_3 = new Vector4(m_localPosition, 1f);

			Matrix4x4 localToWorld;
			this.localToWorld = localToWorld = local * parentToWorld;

			m_position = (Vector3)localToWorld.column_3;
			m_rotation = localToWorld.GetRotation();

			Vector3 scale = new Vector3(localToWorld.column_0.length, localToWorld.column_1.length, localToWorld.column_2.length);
			right = ((Vector3)localToWorld.column_0) / (scale.x == 0f ? 1f : scale.x);
			up = ((Vector3)localToWorld.column_1) / (scale.y == 0f ? 1f : scale.y);
			forward = ((Vector3)localToWorld.column_2) / (scale.z == 0f ? 1f : scale.z);
			this.scale = scale;

			parentMatrix = parentToWorld;
			parentMatrixInv = parentMatrix.GetInversed();

			foreach (var child in childs) child.UpdateMatrix(localToWorld);
		}

		[BehaviourEvent]
		void OnDestroy()
		{
			parent = null;
			foreach (var child in childs) child.gameObject.Destroy();
		}
		public Matrix4x4 localToWorld { get; private set; } = Matrix4x4.identity;
	}
}
