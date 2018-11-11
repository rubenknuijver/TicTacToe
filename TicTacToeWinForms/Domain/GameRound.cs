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
}
