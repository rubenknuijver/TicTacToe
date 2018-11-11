using System;

namespace TicTacToeWinForms
{
	public class Game
	{
		private Game(string playerX, string playerO, GameTurn turn)
		{
			this.Cells = new TicTacToe.Cell[3 * 3];
			this.PlayerX = playerX;
			this.PlayerO = playerO;
			this.Turn = turn;
		}

		public TicTacToe.Cell[] Cells { get; private set; }

		public string PlayerO { get; }

		public string PlayerX { get; }

		public GameTurn Turn { get; private set; }

		public static Game NewGame(string playerX, string playerO) => new Game(playerX, playerO, GameTurn.Empty);

		public Game RestartGame() => new Game(this.PlayerX, this.PlayerO, this.Turn);
		public Game SwitchTurns() => this.Update(new Game(this.PlayerX, this.PlayerO, this.Turn.Switch()), map => map.Cells = this.Cells);

		private Game Update(Game game, Action<Game> map)
		{
			map(game);
			return game;
		}

		public static readonly Game Default = NewGame("X", "O");
	}
}
