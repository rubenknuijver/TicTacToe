using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace TicTacToeWinForms
{

	public struct GameRound
	{
		private readonly int total;

		public GameRound(int rounds, int current)
			: this()
		{
			this.total = rounds;
			this.Current = current;
		}

		public int Current { get; }
		public bool IsFinal { get => this.total == this.Current; }
		public int RoundsLeft { get => this.total - this.Current; }

		public GameRound Next() => new GameRound(this.total, this.Current + 1);
	}

	public struct GameTurn
	{
		public readonly bool Value;

		public GameTurn(bool value)
			: this()
		{
			this.Value = value;
		}

		public GameTurn Switch() => new GameTurn(!this.Value);
	}

	public partial class Form1 : Form
	{
		private Game game = Game.None;
		private GameRound round;
		private IDictionary<GameRound, Game> score;

		public Form1()
		{
			this.InitializeComponent();
			this.GameEndView();
		}

		private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			MessageBox.Show("written by r.knuijver aka Prime");
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.Close();
			Application.Exit();
		}

		private void GameEndView()
		{
			this.Controls
				.OfType<Button>()
				.ToList()
				.ForEach(control =>
				{
					control.Enabled = false;
				});
		}

		private void GameStartView()
		{
			foreach (var button in this.Controls.OfType<Button>())
			{
				button.Text = string.Empty;
				button.Enabled = true;
			}
		}

		private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
		{
			using (var dialog = new GameStartingForm(this.Play, game.PlayerX, game.PlayerO))
			{
				dialog.ShowDialog();
			}
		}

		private void Play(string playerX, string playerO, int rounds)
		{
			this.score = new Dictionary<GameRound, Game>();
			this.game = Game.NewGame(playerX, playerO);
			this.round = new GameRound(rounds, 1);
			this.GameStartView();
		}

		private void SwitchTurns()
		{
			this.game = this.game.SwitchTurns();
		}

		private void TurnButton_Clicked(object sender, EventArgs e)
		{
			if (sender is Button button)
			{
				var pos = Convert.ToInt32(button.Tag) - 1;
				if (!this.game.IsEmpty(pos))
					return;

				button.Enabled = false;

				this.game.MarkSpace(pos, g =>
				{
					button.Text = g.Turn.Marker();

					var winner = g.DoWeHaveAWinner();
					var draw = (!g.HasEmptySpaces()) && (winner == null); // <-- no winner, check to show intent.

					if (winner == null)
					{
						if (draw) MessageBox.Show("It's a draw");
						else this.SwitchTurns();

						return;
					}

					MessageBox.Show($"The winner of round {this.round.Current} is {winner}");
					this.score.Add(this.round, g);
					if (this.round.RoundsLeft > 0)
					{
						this.round = this.round.Next();
						this.game = g.RestartGame();
						if (this.round.IsFinal) MessageBox.Show($"Final round.");
						this.GameStartView();
					}
					else
					{
						this.GameEndView();
					}
				});
			}
		}
	}

	public class Game
	{
		private Game(string playerX, string playerO)
		{
			this.Cells = new TicTacToe.Cell[3 * 3];
			this.PlayerX = playerX;
			this.PlayerO = playerO;
		}

		public TicTacToe.Cell[] Cells { get; private set; }

		public string PlayerO { get; }

		public string PlayerX { get; }

		public GameTurn Turn { get; private set; }

		public static Game NewGame(string playerX, string playerO)
			=> new Game(playerX, playerO);

		public Game RestartGame()
			=> this.Update(new Game(this.PlayerX, this.PlayerO), map =>
			{
				map.Turn = this.Turn;
			});

		public Game SwitchTurns()
			=> this.Update(new Game(this.PlayerX, this.PlayerO), map =>
			{
				map.Cells = this.Cells;
				map.Turn = this.Turn.Switch();
			});

		private Game Update(Game game, Action<Game> map)
		{
			map(game);
			return game;
		}

		public static readonly Game None = NewGame(null, null);
	}

	public static class GameExtensions
	{
		public static object CurrentPlayer(this Game game) => game.Turn.Value ? game.PlayerO : game.PlayerX;

		public static object DoWeHaveAWinner(this Game game) => TicTacToe.DoWeHaveAWinner(game.Cells);
		public static bool HasEmptySpaces(this Game game) => game.Cells.Any(p => p.IsEmpty);
		public static bool IsEmpty(this Game game, int pos) => game.Cells[pos].IsEmpty;
		public static string Marker(this GameTurn turn) => turn.Value ? "O" : "X";

		public static void MarkSpace(this Game game, int pos, Action<Game> mark)
		{
			if (game.IsEmpty(pos))
			{
				game.Occupy(pos, game.CurrentPlayer());
				mark(game);
			}
		}
		public static T Occupy<T>(this Game game, int pos, T player) => (T)(game.Cells[pos].Occupant = player as object);
		public static string TurnMarker(this Game game) => game.Turn.Marker();
	}

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
