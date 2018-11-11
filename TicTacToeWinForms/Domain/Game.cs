using System;

namespace TicTacToeWinForms
{
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

		public static readonly Game Default = NewGame("X", "O");
	}
}
