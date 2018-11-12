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
		public static Maybe<T> None = new Maybe<T>();

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

		public Maybe<T> DefaultIfEmpty(T defaultValue)
		{
			return this.HasValue ? Some(this.Value) : Some(defaultValue);
		}

		public B Match<B>(Func<T, B> Some, Func<B> None)
		{
			return HasValue ? Some(Value) : None();
		}
	}

	public static class MaybeExtensions
	{
		public static Maybe<B> Select<A, B>(this Maybe<A> self, Func<A, B> map) =>
			self.Match(
				Some: a => Maybe<B>.Some(map(a)),
				None: () => Maybe<B>.None
			);

		public static Maybe<C> SelectMany<A, B, C>(
			this Maybe<A> self,
			Func<A, Maybe<B>> bind,
			Func<A, B, C> project) =>
			self.Match(
				Some: a => bind(a).Match(
								Some: b => Maybe<C>.Some(project(a, b)),
								None: () => Maybe<C>.None),
				None: () => Maybe<C>.None
			);
	}
}
