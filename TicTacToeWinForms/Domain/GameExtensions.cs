using System;
using System.Linq;

namespace TicTacToeWinForms
{
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
}
