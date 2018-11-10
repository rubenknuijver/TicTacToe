using System;
using System.Drawing;
using System.Windows.Forms;

namespace TicTacToeWinForms
{
	public partial class GameStartingForm : Form
	{
		private readonly Action<string, string, int> play;

		public GameStartingForm(Action<string, string, int> play)
		{
			this.play = play;
			this.InitializeComponent();
		}

		private void numericRounds_ValueChanged(object sender, EventArgs e)
		{
			if (sender is NumericUpDown numeric)
			{
				var invalid = (numeric.Value % 2 == 0);

				numeric.ForeColor = invalid
					? Color.Red
					: Color.Black;
			}
		}

		private void PlayButton_Clicked(object sender, EventArgs e)
		{
			var invalid = (numericRounds.Value % 2 == 0);
			if (invalid)
			{
				MessageBox.Show("number of round should be an uneven number");
				return;
			}

			if (string.IsNullOrWhiteSpace(textPlayerX.Text))
			{
				MessageBox.Show("Player X has no name.");
				return;
			}

			if (string.IsNullOrWhiteSpace(textPlayerO.Text))
			{
				MessageBox.Show("Player O has no name.");
				return;
			}

			this.DialogResult = DialogResult.OK;

			play(textPlayerX.Text, textPlayerO.Text, (int)numericRounds.Value);

			this.Close();
		}
	}
}
