using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace TicTacToeWinForms
{
	public partial class Form1 : Form
	{
		private Game game = Game.Default;
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
}
