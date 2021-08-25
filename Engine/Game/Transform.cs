using System.Collections.Generic;

namespace Engine.Game
{
	public sealed class Transform : Component
	{
		public Vector3 localPosition {
			get
			{
				return new Vector3(local.column_3.x, local.column_3.y, local.column_3.z);
			}
			set
			{
				local.column_3 = value;
				UpdateMatrix(UpdateState.PositionChanged);
			}
		}
		Quaternion m_localRotation = Quaternion.identity;
		public Quaternion localRotation
		{
			get
			{
				return m_localRotation;
			}
			set
			{
				Matrix4x4 rotation = value.ToMatrix();
				local.column_0 = rotation.column_0 * local.column_0.length;
				local.column_1 = rotation.column_1 * local.column_1.length;
				local.column_2 = rotation.column_2 * local.column_2.length;
				m_localRotation = value;
				UpdateMatrix(UpdateState.RotationChanged);
			}
		}
		public Vector3 localScale
		{
			get
			{
				return new Vector3(local.column_0.length, local.column_1.length, local.column_2.length);
			}
			set
			{
				local.column_0 = local.column_0.normalized * value.x;
				local.column_1 = local.column_1.normalized * value.y;
				local.column_2 = local.column_2.normalized * value.z;
				UpdateMatrix(UpdateState.ScaleChanged);
			}
		}
		Vector3 m_position;
		public Vector3 position
		{
			get => m_position;
			set
			{
				Vector3 pos = parentMatrixInv.MultiplyPoint(value);
				local.column_3 = new Vector4(pos.x, pos.y, pos.z, 1f);
				
				UpdateMatrix(UpdateState.PositionChanged);
			}
		}
		Quaternion m_rotation;
		public Quaternion rotation
		{
			get => m_rotation;
			set
			{
				localRotation = parentMatrixInv.GetRotation() * value;
				UpdateMatrix(UpdateState.RotationChanged);
			}
		}
		public Vector3 right { get; private set; }
		public Vector3 up { get; private set; }
		public Vector3 forward { get; private set; }
		public Vector3 scale { get; private set; }

		[System.Flags]
		enum UpdateState : byte
		{
			None = 0,
			Position = 1,
			Rotation = 2,
			Scale = 4,
			Right = 8,
			Up = 16,
			Forward = 32,
			All = 255,

			PositionChanged = None,
			RotationChanged = Rotation | Right | Up | Forward,
			ScaleChanged = Scale
		}

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
			UpdateMatrix(UpdateState.All);
		}

		void UpdateMatrix(UpdateState state)
		{
			UpdateMatrix(parent == null ? Matrix4x4.identity : parent.localToWorld, state);
		}

		void UpdateMatrix(Matrix4x4 parentToWorld, UpdateState updateState)
		{
			Matrix4x4 localToWorld;
			this.localToWorld = localToWorld = local * parentToWorld;

			parentMatrix = parentToWorld;
			parentMatrixInv = parentMatrix.GetInversed();

			if (updateState.HasFlag(UpdateState.Rotation)) m_rotation = localToWorld.GetRotation();
			if (updateState.HasFlag(UpdateState.Position)) m_position = (Vector3)localToWorld.column_3;

			if (updateState.HasFlag(UpdateState.Right)) right = (Vector3)localToWorld.column_0;
			if (updateState.HasFlag(UpdateState.Up)) up = (Vector3)localToWorld.column_1;
			if (updateState.HasFlag(UpdateState.Forward)) forward = (Vector3)localToWorld.column_2;

			if (updateState.HasFlag(UpdateState.Scale)) scale = new Vector3(localToWorld.column_0.length, localToWorld.column_1.length, localToWorld.column_2.length);

			foreach (var child in childs) child.UpdateMatrix(localToWorld, UpdateState.All);
		}

		[BehaviourEvent]
		void OnDestroy()
		{
			parent = null;
			foreach (var child in childs) child.gameObject.Destroy();
		}

		Matrix4x4 local = Matrix4x4.identity;
		public Matrix4x4 localToWorld { get; private set; } = Matrix4x4.identity;
	}
}
