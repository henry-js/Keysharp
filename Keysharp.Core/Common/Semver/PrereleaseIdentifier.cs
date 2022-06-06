﻿using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using Semver.Utility;

namespace Semver
{
	/// <summary>
	/// An individual prerelease identifier for a semantic version.
	/// </summary>
	/// <remarks>
	/// <para>The prerelease portion of a semantic version is composed of dot ('<c>.</c>') separated identifiers.
	/// A prerelease identifier is either an alphanumeric or numeric identifier. A valid numeric
	/// identifier is composed of ASCII digits (<c>[0-9]</c>) without leading zeros. A valid
	/// alphanumeric identifier is a non-empty string of ASCII alphanumeric and hyphen characters
	/// (<c>[0-9A-Za-z-]</c>) with at least one non-digit character. Prerelease identifiers are
	/// compared first by whether they are numeric or alphanumeric. Numeric identifiers have lower
	/// precedence than alphanumeric identifiers. Numeric identifiers are compared to each other
	/// numerically. Alphanumeric identifiers are compared to each other lexically in ASCII sort
	/// order.</para>
	///
	/// <para>Because <see cref="PrereleaseIdentifier"/> is a struct, the default value is a
	/// <see cref="PrereleaseIdentifier"/> with a <see langword="null"/> value. However, the
	/// <see cref="Semver"/> namespace types do not accept and will not return such a
	/// <see cref="PrereleaseIdentifier"/>.</para>
	///
	/// <para>Invalid prerelease identifiers including arbitrary Unicode characters, empty string,
	/// and numeric identifiers with leading zero can currently be produced by the
	/// <see cref="SemVersion(int, int, int, string, string)"/> constructor and the obsolete
	/// <see cref="SemVersion.Parse(string,bool)"/> and
	/// <see cref="SemVersion.TryParse(string,out SemVersion,bool)"/> methods. Such alphanumeric
	/// identifiers are compared via an ordinal string comparision. Numeric identifiers with
	/// leading zeros are considered equal (e.g. '<c>15</c>' is equal to '<c>015</c>').
	/// </para>
	/// </remarks>
	public readonly struct PrereleaseIdentifier : IEquatable<PrereleaseIdentifier>, IComparable<PrereleaseIdentifier>, IComparable
	{
		/// <summary>
		/// The numeric value of the prerelease identifier if it is a numeric identifier, otherwise
		/// <see langword="null"/>.
		/// </summary>
		/// <value>The numeric value of the prerelease identifier if it is a numeric identifier,
		/// otherwise <see langword="null"/>.</value>
		/// <remarks>The numeric value of a prerelease identifier will never be negative.</remarks>
		public int? NumericValue { get; }

		/// <summary>
		/// The string value of the prerelease identifier even if it is a numeric identifier.
		/// </summary>
		/// <value>The string value of this prerelease identifier even if it is a numeric identifier
		/// or <see langword="null"/> if this is a default <see cref="PrereleaseIdentifier"/>.</value>
		/// <remarks>Invalid numeric prerelease identifiers with leading zeros will have a string
		/// value including the leading zeros. This can be used to distinguish invalid numeric
		/// identifiers with different numbers of leading zeros.</remarks>
		public string Value { get; }

		/// <summary>
		/// Constructs a valid <see cref="PrereleaseIdentifier"/>.
		/// </summary>
		/// <param name="value">The string value of this prerelease identifier.</param>
		/// <param name="allowLeadingZeros">Whether to allow leading zeros in the <paramref name="value"/>
		/// parameter. If <see langword="true"/>, leading zeros will be allowed on numeric identifiers
		/// but will be removed.</param>
		/// <exception cref="ArgumentNullException">The <paramref name="value"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentException">The <paramref name="value"/> is empty or contains invalid characters
		/// (i.e. characters that are not ASCII alphanumerics or hyphens) or has leading zeros for
		/// a numeric identifier when <paramref name="allowLeadingZeros"/> is <see langword="false"/>.</exception>
		/// <exception cref="OverflowException">The numeric identifier value is too large for <see cref="Int32"/>.</exception>
		/// <remarks>Because a valid numeric identifier does not have leading zeros, this constructor
		/// will never create a <see cref="PrereleaseIdentifier"/> with leading zeros even if
		/// <paramref name="allowLeadingZeros"/> is <see langword="true"/>. Any leading zeros will
		/// be removed.</remarks>
		public PrereleaseIdentifier(string value, bool allowLeadingZeros = false)
			: this(value, allowLeadingZeros, nameof(value))
		{
		}

		/// <summary>
		/// Construct a valid numeric <see cref="PrereleaseIdentifier"/> from an integer value.
		/// </summary>
		/// <exception cref="ArgumentOutOfRangeException">The <paramref name="value"/> is negative.</exception>
		/// <param name="value">The non-negative value of this identifier.</param>
		public PrereleaseIdentifier(int value)
		{
			if (value < 0)
				throw new ArgumentOutOfRangeException(nameof(value), $"Numeric prerelease identifiers can't be negative: {value}.");

			Value = value.ToString(CultureInfo.InvariantCulture);
			NumericValue = value;
		}

		/// <summary>
		/// Constructs a valid <see cref="PrereleaseIdentifier"/>.
		/// </summary>
		/// <remarks>
		/// Internal constructor allows changing the parameter name to enable methods using this
		/// as part of their prerelease identifier validation to match the parameter name to their
		/// parameter name.
		/// </remarks>
		internal PrereleaseIdentifier(string value, bool allowLeadingZeros, string paramName)
		{
			if (value is null)
				throw new ArgumentNullException(paramName);

			if (value.Length == 0)
				throw new ArgumentException("Prerelease identifier cannot be empty.", paramName);

			if (value.IsDigits())
			{
				if (value.Length > 1 && value[0] == '0')
				{
					if (allowLeadingZeros)
						value = value.TrimLeadingZeros();
					else
						throw new ArgumentException($"Leading zeros are not allowed on numeric prerelease identifiers '{value}'.", paramName);
				}

				try
				{
					NumericValue = int.Parse(value, NumberStyles.None, CultureInfo.InvariantCulture);
				}
				catch (OverflowException)
				{
					// Remake the overflow exception to give better message
					throw new OverflowException($"Prerelease identifier '{value}' was too large for {nameof(Int32)}.");
				}
			}
			else
			{
				if (!value.IsAlphanumericOrHyphens())
					throw new ArgumentException($"A prerelease identifier can contain only ASCII alphanumeric characters and hyphens '{value}'.", paramName);

				NumericValue = null;
			}

			Value = value;
		}

		/// <summary>
		/// Private constructor used by <see cref="CreateUnsafe"/>.
		/// </summary>
		private PrereleaseIdentifier(string value, int? numericValue)
		{
			Value = value;
			NumericValue = numericValue;
		}

		/// <summary>
		/// Converts this identifier into an equivalent string value.
		/// </summary>
		/// <returns>The string value of this identifier or <see langword="null"/> if this is
		/// a default <see cref="PrereleaseIdentifier"/></returns>
		public static implicit operator string(PrereleaseIdentifier prereleaseIdentifier)
		=> prereleaseIdentifier.Value;

		/// <summary>
		/// Determines whether two identifiers are <em>not</em> equal.
		/// </summary>
		/// <returns><see langword="true"/> if the value of <paramref name="left"/> is different
		/// from the value of <paramref name="right"/>; otherwise <see langword="false"/>.</returns>
		/// <remarks>Numeric identifiers with leading zeros are considered equal (e.g. '<c>15</c>'
		/// is equal to '<c>015</c>').</remarks>
		public static bool operator !=(PrereleaseIdentifier left, PrereleaseIdentifier right)
		=> !left.Equals(right);

		/// <summary>
		/// Determines whether two identifiers are equal.
		/// </summary>
		/// <returns><see langword="true"/> if the value of <paramref name="left"/> is the same as
		/// the value of <paramref name="right"/>; otherwise <see langword="false"/>.</returns>
		/// <remarks>Numeric identifiers with leading zeros are considered equal (e.g. '<c>15</c>'
		/// is equal to '<c>015</c>').</remarks>
		public static bool operator ==(PrereleaseIdentifier left, PrereleaseIdentifier right)
		=> left.Equals(right);

		/// <summary>
		/// Compares two identifiers and indicates whether this instance precedes, follows, or is
		/// equal to the other in precedence order.
		/// </summary>
		/// <returns>
		/// An integer that indicates whether this instance precedes, follows, or is equal to
		/// <paramref name="value"/> in precedence order.
		/// <list type="table">
		///     <listheader>
		///         <term>Value</term>
		///         <description>Condition</description>
		///     </listheader>
		///     <item>
		///          <term>-1</term>
		///          <description>This instance precedes <paramref name="value"/>.</description>
		///     </item>
		///     <item>
		///          <term>0</term>
		///          <description>This instance is equal to <paramref name="value"/>.</description>
		///     </item>
		///     <item>
		///          <term>1</term>
		///          <description>This instance follows <paramref name="value"/>.</description>
		///     </item>
		/// </list>
		/// </returns>
		/// <remarks>Numeric identifiers have lower precedence than alphanumeric identifiers.
		/// Numeric identifiers are compared numerically. Numeric identifiers with leading zeros are
		/// considered equal (e.g. '<c>15</c>' is equal to '<c>015</c>'). Alphanumeric identifiers are
		/// compared lexically in ASCII sort order. Invalid alphanumeric identifiers are
		/// compared via an ordinal string comparision.</remarks>
		public int CompareTo(PrereleaseIdentifier value)
		{
			// Handle the fact that numeric identifiers are always less than alphanumeric
			// and numeric identifiers are compared equal even with leading zeros.
			if (NumericValue is int numericValue)
			{
				if (value.NumericValue is int otherNumericValue)
					return numericValue.CompareTo(otherNumericValue);

				return -1;
			}

			if (value.NumericValue != null)
				return 1;

			return IdentifierString.Compare(Value, value.Value);
		}

		/// <summary>
		/// Compares this identifier to an <see cref="Object"/> and indicates whether this instance
		/// precedes, follows, or is equal to the object in precedence order.
		/// </summary>
		/// <returns>
		/// An integer that indicates whether this instance precedes, follows, or is equal to
		/// <paramref name="value"/> in precedence order.
		/// <list type="table">
		///     <listheader>
		///         <term>Value</term>
		///         <description>Condition</description>
		///     </listheader>
		///     <item>
		///          <term>-1</term>
		///          <description>This instance precedes <paramref name="value"/>.</description>
		///     </item>
		///     <item>
		///          <term>0</term>
		///          <description>This instance is equal to <paramref name="value"/>.</description>
		///     </item>
		///     <item>
		///          <term>1</term>
		///          <description>This instance follows <paramref name="value"/> or <paramref name="value"/>
		///                         is <see langword="null"/>.</description>
		///     </item>
		/// </list>
		/// </returns>
		/// <exception cref="ArgumentException"><paramref name="value"/> is not a <see cref="PrereleaseIdentifier"/>.</exception>
		/// <remarks>Numeric identifiers have lower precedence than alphanumeric identifiers.
		/// Numeric identifiers are compared numerically. Numeric identifiers with leading zeros are
		/// considered equal (e.g. '<c>15</c>' is equal to '<c>015</c>'). Alphanumeric identifiers are
		/// compared lexically in ASCII sort order. Invalid alphanumeric identifiers are
		/// compared via an ordinal string comparision.</remarks>
		public int CompareTo(object value)
		{
			if (value is null) return 1;

			return value is PrereleaseIdentifier other
				   ? CompareTo(other)
				   : throw new ArgumentException($"Object must be of type {nameof(PrereleaseIdentifier)}.", nameof(value));
		}

		/// <summary>
		/// Determines whether two identifiers are equal.
		/// </summary>
		/// <returns><see langword="true"/> if <paramref name="value"/> is equal to the this identifier;
		/// otherwise <see langword="false"/>.</returns>
		/// <remarks>Numeric identifiers with leading zeros are considered equal (e.g. '<c>15</c>'
		/// is equal to '<c>015</c>').</remarks>
		public bool Equals(PrereleaseIdentifier value)
		{
			if (NumericValue is int numericValue) return numericValue == value.NumericValue;

			return Value == value.Value;
		}

		/// <summary>Determines whether the given object is equal to this identifier.</summary>
		/// <returns><see langword="true"/> if <paramref name="value"/> is equal to the this identifier;
		/// otherwise <see langword="false"/>.</returns>
		/// <remarks>Numeric identifiers with leading zeros are considered equal (e.g. '<c>15</c>'
		/// is equal to '<c>015</c>').</remarks>
		public override bool Equals(object value)
		=> value is PrereleaseIdentifier other&& Equals(other);

		/// <summary>Gets a hash code for this identifier.</summary>
		/// <returns>A hash code for this identifier.</returns>
		/// <remarks>Numeric identifiers with leading zeros are have the same hash code (e.g.
		/// '<c>15</c>' has the same hash code as '<c>015</c>').</remarks>
		public override int GetHashCode()
		{
			if (NumericValue is int numericValue) return CombinedHashCode.Create(numericValue);

			return CombinedHashCode.Create(Value);
		}

		/// <summary>
		/// Converts this identifier into an equivalent string value.
		/// </summary>
		/// <returns>The string value of this identifier or <see langword="null"/> if this is
		/// a default <see cref="PrereleaseIdentifier"/></returns>
		public override string ToString() => Value;

		/// <summary>
		/// Construct a potentially invalid <see cref="PrereleaseIdentifier"/>.
		/// </summary>
		/// <exception cref="ArgumentNullException">The <paramref name="value"/> parameter is <see langword="null"/>.</exception>
		/// <remarks>This should be used only by the <see cref="SemVersion"/> constructor that
		/// still accepts illegal values.</remarks>
		[Obsolete]
		internal static PrereleaseIdentifier CreateLoose(string value)
		{
			// Avoid parsing some non-ASCII digits as a number by checking that they are all digits
			if (value.IsDigits() && int.TryParse(value, NumberStyles.None, CultureInfo.InvariantCulture, out var numericValue))
				return new PrereleaseIdentifier(value, numericValue);

			return new PrereleaseIdentifier(value, null);
		}

		/// <summary>
		/// Construct a <see cref="PrereleaseIdentifier"/> without checking that any of the invariants
		/// hold. Used by the parser for performance.
		/// </summary>
		/// <remarks>This is a create method rather than a constructor to clearly indicate uses
		/// of it. The other constructors have not been hidden behind create methods because only
		/// constructors are visible to the package users. So they see a class consistently
		/// using constructors without any create methods.</remarks>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static PrereleaseIdentifier CreateUnsafe(string value, int? numericValue) => new PrereleaseIdentifier(value, numericValue);
	}
}