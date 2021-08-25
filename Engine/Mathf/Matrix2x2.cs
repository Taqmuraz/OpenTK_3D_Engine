namespace Engine
{
	public struct Matrix2x2
	{
		private Vector2 row_0;
		private Vector2 row_1;

		public Matrix2x2(Vector2 row_0, Vector2 row_1)
		{
			this.row_0 = row_0;
			this.row_1 = row_1;
		}

		public float GetDeterminant()
		{
			return row_0.x * row_1.y - row_0.y * row_1.x;
		}

		public Vector2 this[int index]
		{
			get
			{
				switch (index)
				{
					case 0: return row_0;
					case 1: return row_1;
					default: throw new System.ArgumentException("Row index out of matrix2x2 range");
				}
			}
			set
			{
				switch (index)
				{
					case 0: row_0 = value; return;
					case 1: row_1 = value; return;
					default: throw new System.ArgumentException("Row index out of matrix2x2 range");
				}
			}
		}
		public float this[int index, int element]
		{
			get
			{
				switch (index)
				{
					case 0: return row_0[element];
					case 1: return row_1[element];
					default: throw new System.ArgumentException("Row index out of matrix2x2 range");
				}
			}
			set
			{
				switch (index)
				{
					case 0: row_0[element] = value; return;
					case 1: row_1[element] = value; return;
					default: throw new System.ArgumentException("Row index out of matrix2x2 range");
				}
			}
		}
		public override string ToString()
		{
			return $"{row_0}\n{row_1}\n";
		}
	}
}