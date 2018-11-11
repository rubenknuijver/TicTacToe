namespace TicTacToeWinForms
{
	public static class TicTacToe
	{
		private static readonly int[][][] _goodStart = {
			new int[][] { new int[] { 4 }, new int[] { 0, 2, 6, 8 } },
			new int[][] { new int[] { 1 }, new int[] { 6, 8 }, new int[] { 0, 2 } },
			new int[][] { new int[] { 7 }, new int[] { 0, 2 }, new int[] { 6, 8 } },
			new int[][] { new int[] { 0 }, new int[] { 4, 8, 6, 2 } }
		};

		private static readonly int[,] _matches = {
			{ 0, 1, 2 },
			{ 3, 4, 5 },
			{ 6, 7, 8 },
			{ 0, 3, 6 },
			{ 1, 4, 7 },
			{ 2, 5, 8 },
			{ 0, 4, 8 },
			{ 2, 4, 6 }
		};

		/// <summary>
		/// We could have winning combinations in de cell collection
		/// </summary>
		/// <param name="cells"></param>
		/// <returns></returns>
		public static object DoWeHaveAWinner(Cell[] cells)
		{
			Cell map(int index, int pos) => cells[_matches[index, pos]];

			for (int i = 0; i < cells.Length - 1; i++)
			{
				var c1 = map(i, 0);
				var c2 = map(i, 1);
				var c3 = map(i, 2);

				if (c1.IsEmpty || c2.IsEmpty || c3.IsEmpty)
				{ // if one is blank
					continue;    // -- no need to go further
				}

				if (c1.Occupant == c2.Occupant && c2.Occupant == c3.Occupant)
				{
					return c1.Occupant;
				}
			}

			return null;
		}

		public struct Cell
		{
			public bool IsEmpty => this.Occupant == null;

			public object Occupant { get; internal set; }
		}
	}
}
