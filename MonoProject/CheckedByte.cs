using System;
using System.Runtime.CompilerServices;

namespace MonoProject {
	public struct CheckedByte : IFormattable, IConvertible, IComparable, IComparable<CheckedByte>, IComparable<byte>, IEquatable<CheckedByte>, IEquatable<byte> {
		byte _value;

		public CheckedByte(byte value) {
			_value = value;
		}

		// Actual code
		
		public static CheckedByte operator +(CheckedByte b1, CheckedByte b2) {
			var result = b1._value + b2._value;
			if (result > byte.MaxValue) {
				throw new OverflowException();
			}
			return new CheckedByte((byte)result);
		}
		
		public static CheckedByte operator -(CheckedByte b1, CheckedByte b2) {
			var result = b1._value - b2._value;
			if (result < byte.MinValue) {
				throw new OverflowException();
			}
			return new CheckedByte((byte)result);
		}

		public static implicit operator CheckedByte(byte b) {
			return new CheckedByte(b);
		}
		
		// Boilerplate for compatibility
		
		public int CompareTo(CheckedByte other) {
			return _value.CompareTo(other);
		}

		public int CompareTo(byte other) {
			return _value.CompareTo(other);
		}

		public bool Equals(CheckedByte other) {
			return _value.Equals(other._value);
		}

		public bool Equals(byte other) {
			return _value.Equals(other);
		}

		public override string ToString() {
			return _value.ToString();
		}

		public int CompareTo(object obj) {
			return _value.CompareTo(obj);
		}

		public string ToString(string format, IFormatProvider formatProvider) {
			return _value.ToString(format, formatProvider);
		}

		public TypeCode GetTypeCode() {
			return TypeCode.Byte;
		}

		public bool ToBoolean(IFormatProvider provider) {
			return ((IConvertible) _value).ToBoolean(provider);
		}

		public byte ToByte(IFormatProvider provider) {
			return ((IConvertible) _value).ToByte(provider);
		}

		public char ToChar(IFormatProvider provider) {
			return ((IConvertible) _value).ToChar(provider);
		}

		public DateTime ToDateTime(IFormatProvider provider) {
			return ((IConvertible) _value).ToDateTime(provider);
		}

		public decimal ToDecimal(IFormatProvider provider) {
			return ((IConvertible) _value).ToDecimal(provider);
		}

		public double ToDouble(IFormatProvider provider) {
			return ((IConvertible) _value).ToDouble(provider);
		}

		public short ToInt16(IFormatProvider provider) {
			return ((IConvertible) _value).ToInt16(provider);
		}

		public int ToInt32(IFormatProvider provider) {
			return ((IConvertible) _value).ToInt32(provider);
		}

		public long ToInt64(IFormatProvider provider) {
			return ((IConvertible) _value).ToInt64(provider);
		}

		public sbyte ToSByte(IFormatProvider provider) {
			return ((IConvertible) _value).ToSByte(provider);
		}

		public float ToSingle(IFormatProvider provider) {
			return ((IConvertible) _value).ToSingle(provider);
		}

		public string ToString(IFormatProvider provider) {
			return ((IConvertible) _value).ToString(provider);
		}

		public object ToType(Type conversionType, IFormatProvider provider) {
			return ((IConvertible) _value).ToType(conversionType, provider);
		}

		public ushort ToUInt16(IFormatProvider provider) {
			return ((IConvertible) _value).ToUInt16(provider);
		}

		public uint ToUInt32(IFormatProvider provider) {
			return ((IConvertible) _value).ToUInt32(provider);
		}

		public ulong ToUInt64(IFormatProvider provider) {
			return ((IConvertible) _value).ToUInt64(provider);
		}
	}
}