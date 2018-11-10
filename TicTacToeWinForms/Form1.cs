using System;
using System.Linq;
using System.Windows.Forms;

namespace TicTacToeWinForms
{
	public partial class Form1 : Form
	{
		private bool turn = false;
		private Cell[] cells = new Cell[3 * 3];

		public Form1()
		{
			this.InitializeComponent();
		}

		private void TurnButton_Clicked(object sender, EventArgs e)
		{
			if (sender is Button button)
			{
				var pos = Convert.ToInt32(button.Tag) - 1;
				if (!this.cells[pos].IsEmpty)
					return;

				button.Enabled = false;
				this.cells[pos].Occupant = button.Text = this.turn
					? "O"
					: "X";

				var winner = TicTacToe.DoWeHaveAWinner(this.cells);
				var draw = this.cells.All(p => !p.IsEmpty) && (winner == null); // <-- no winner, check to show intent.

				if (winner == null)
				{
					if (draw) MessageBox.Show("It's a draw");
					else this.turn = !this.turn;

					return;
				}

				MessageBox.Show("The winner is " + winner);

				this.Controls
					.OfType<Button>()
					.ToList()
					.ForEach(control =>
					{
						control.Enabled = false;
					});
			}
		}

		private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.turn = false;
			this.cells = new Cell[3 * 3];
			foreach (var button in this.Controls.OfType<Button>())
			{
				button.Text = string.Empty;
				button.Enabled = true;
			}
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.Close();
			Application.Exit();
		}

		private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			MessageBox.Show("written by r.knuijver aka Prime");
		}
	}

	public struct Cell
	{
		public bool IsEmpty => this.Occupant == null;

		public object Occupant { get; internal set; }
	}

	public static class TicTacToe
	{
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

		private static readonly int[][][] _goodStart = {
			new int[][] { new int[] { 4 }, new int[] { 0, 2, 6, 8 } },
			new int[][] { new int[] { 1 }, new int[] { 6, 8 }, new int[] { 0, 2 } },
			new int[][] { new int[] { 7 }, new int[] { 0, 2 }, new int[] { 6, 8 } },
			new int[][] { new int[] { 0 }, new int[] { 4, 8, 6, 2 } }
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
	}
}
