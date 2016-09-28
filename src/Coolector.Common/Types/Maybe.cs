using System;

namespace Coolector.Common.Types
{
    public struct Maybe<T> : IEquatable<Maybe<T>> where T : class
    {
        private readonly T _value;
        public T Value
        {
            get
            {
                if (HasNoValue)
                    throw new InvalidOperationException();

                return _value;
            }
        }

        public bool HasValue => _value != null;
        public bool HasNoValue => !HasValue;

        private Maybe(T value)
        {
            _value = value;
        }

        public static implicit operator Maybe<T>(T value) => new Maybe<T>(value);

        public static bool operator ==(Maybe<T> maybe, T value) => !maybe.HasNoValue && maybe.Value.Equals(value);

        public static bool operator !=(Maybe<T> maybe, T value) => !(maybe == value);

        public static bool operator ==(Maybe<T> first, Maybe<T> second) => first.Equals(second);

        public static bool operator !=(Maybe<T> first, Maybe<T> second) => !(first == second);

        public override bool Equals(object obj)
        {
            if (obj is T)
                obj = new Maybe<T>((T)obj);

            if (!(obj is Maybe<T>))
                return false;

            var other = (Maybe<T>)obj;

            return Equals(other);
        }

        public bool Equals(Maybe<T> other)
        {
            if (HasNoValue && other.HasNoValue)
                return true;

            if (HasNoValue || other.HasNoValue)
                return false;

            return _value.Equals(other._value);
        }

        public override int GetHashCode() => HasNoValue ? 0 : _value.GetHashCode();

        public override string ToString() => HasNoValue ? "No value" : Value.ToString();
    }
}