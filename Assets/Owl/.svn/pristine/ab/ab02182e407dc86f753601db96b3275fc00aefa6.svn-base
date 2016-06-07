//// ----------------------------------------------------------------------------
//// Tuple structs for use in .NET Not-Quite-3.5 (e.g. Unity3D).
////
//// Used Chapter 3 in http://functional-programming.net/ as a starting point.
////
//// Notes by Thomas: This class was not made by me, but edited by me to support
//// Tuples of triple types. I find myself wishing that this class existed often,
//// so I made a dummy one
//// ----------------------------------------------------------------------------
//
//
using System;

public struct Tuple <T1, T2>
{
	T1 _a;
	T2 _b;
	
	/// <summary>
	/// Retyurns the first element of the tuple
	/// </summary>
	public T1 a
	{
		get { return _a; }
		set { _a = value; }
	}
	
	/// <summary>
	/// Returns the second element of the tuple
	/// </summary>
	public T2 b
	{
		get { return _b; }
		set { _b = value; }
	}
	
	/// <summary>
	/// Create a new tuple value
	/// </summary>
	/// <param name="item1">First element of the tuple</param>
	/// <param name="second">Second element of the tuple</param>
	public Tuple(T1 item1, T2 item2)
	{
		this._a = item1;
		this._b = item2;
	}

	public override string ToString()
	{
		return string.Format("Tuple({0}, {1})", a, b);
	}
	
	public override int GetHashCode()
	{
		int hash = 17;
		hash *= 23 + (_a == null ? 0 : _a.GetHashCode());
		hash *= 23 + (_b == null ? 0 : _b.GetHashCode());
		return hash;
	}
	
	public override bool Equals(object o)
	{
		var t = o.GetType();
		if (!(t.IsValueType))
			return false;
		
		var other = (Tuple<T1, T2>) o;
		
		return this == other;
	}
	
	public bool Equals(Tuple<T1, T2> other)
	{
		return this == other;
	}
	
	public static bool operator==(Tuple<T1, T2> a, Tuple<T1, T2> b)
	{
		if (object.ReferenceEquals(a, null)) {
			return object.ReferenceEquals(b, null);
		}
		if (a._a == null && b._a != null) return false;
		if (a._b == null && b._b != null) return false;
		return
			a._a.Equals(b._a) &&
				a._b.Equals(b._b);
	}
	
	public static bool operator!=(Tuple<T1, T2> a, Tuple<T1, T2> b)
	{
		return !(a == b);
	}
}


public struct Tuple<T1, T2, T3>
{
	private readonly T1 item1;
	private readonly T2 item2;
	private readonly T3 item3;

	/// <summary>
	/// Returns the first element of the tuple
	/// </summary>
	public T1 Item1
	{
		get { return item1; }
	}
	
	/// <summary>
	/// Returns the second element of the tuple
	/// </summary>
	public T2 Item2
	{
		get { return item2; }
	}

	/// <summary>
	/// Returns the third element of the tuple
	/// </summary>
	/// <value>The item3.</value>
	public T3 Item3 { get { return item3; } }

	public Tuple (T1 item1, T2 item2, T3 item3) {
		this.item1 = item1;
		this.item2 = item2;
		this.item3 = item3;
	}

	public override string ToString()
	{
		return string.Format("Tuple({0}, {1}, {2})", Item1, Item2, Item3);
	}
	
	public override bool Equals(object o)
	{
		if (!(o is Tuple<T1, T2, T3>)) {
			return false;
		}
		
		var other = (Tuple<T1, T2, T3>) o;
		
		return this == other;
	}
	
	public bool Equals(Tuple<T1, T2, T3> other)
	{
		return this == other;
	}

	public override int GetHashCode()
	{
		int hash = 17;
		hash *= 23 + (item1 == null ? 0 : item1.GetHashCode());
		hash *= 23 + (item2 == null ? 0 : item2.GetHashCode());
		hash *= 23 + (item3 == null ? 0 : item3.GetHashCode());
		return hash;
	}
	
	public static bool operator==(Tuple<T1, T2, T3> a, Tuple<T1, T2, T3> b)
	{
		if (object.ReferenceEquals(a, null)) {
			return object.ReferenceEquals(b, null);
		}

		if (a.item1 == null && b.item1 != null) return false;
		if (a.item2 == null && b.item2 != null) return false;
		if (a.item3 == null && b.item3 != null) return false;
		return
			a.item1.Equals(b.item1) && a.item2.Equals(b.item2) && a.item3.Equals(b.item3);
	}
	
	public static bool operator!=(Tuple<T1, T2, T3> a, Tuple<T1, T2, T3> b)
	{
		return !(a == b);
	}
}


