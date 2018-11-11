using System;

namespace TicTacToeWinForms
{
	public struct Maybe<T>
	{
		private Maybe(T value)
		{
			this.Value = value;
			this.HasValue = true;
		}

		public bool HasValue { get; }
		public T Value { get; }

		public static Maybe<T> Some(T value) => new Maybe<T>(value);
		public static Maybe<T> Nome = new Maybe<T>();

		public void When(Action<T> some, Action none)
		{
			if (this.HasValue)
			{
				some(this.Value);
			}
			else
			{
				none();
			}
		}
	}
}
