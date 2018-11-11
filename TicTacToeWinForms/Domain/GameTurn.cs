using System;
using System.Linq;

namespace TicTacToeWinForms
{
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
}
