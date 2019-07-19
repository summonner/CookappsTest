using UnityEngine;
using System.Collections;

[System.Serializable]
public struct CubeCoordinate {
	[SerializeField] private int _x;
	[SerializeField] private int _z;

	public int x {
		get { 
			return _x;
		}
		private set {
			_x = value;
		}
	}

	public int y {
		get {
			return (x + z) * -1;
		}
	}

	public int z {
		get {
			return _z;
		}
		private set {
			_z = value;
		}
	}

	public int q { get { return x; } }
	public int r { get { return z; } }

	[System.Diagnostics.Conditional( "UNITY_EDITOR" )]
	private static void AssertArgument( float x, float y, float z ) {
		const float threshold = 0.001f;
		if ( x + y + z > threshold ) {
			throw new System.ArgumentException( "Invalid argument : " + x + " + " + y + " + " + z + " = " + (x + y + z) );
		}
	}

	private CubeCoordinate( int x, int y, int z ) {
		AssertArgument( x, y, z );
		this._x = x;
		this._z = z;
	}

	public CubeCoordinate( int q, int r ) {
		_x = q;
		_z = r;
	}

	public override string ToString() {
		return string.Format( "CubeCoord({0}, {1}, {2})", x, y, z );
	}
	
	private static CubeCoordinate CubeRound( float q, float r ) {
		return CubeRound( q, -(q + r), r );
	}

	private static CubeCoordinate CubeRound( float x, float y, float z ) {
		AssertArgument( x, y, z );
		var rx = Mathf.RoundToInt( x );
		var ry = Mathf.RoundToInt( y );
		var rz = Mathf.RoundToInt( z );

		var dx = Mathf.Abs( rx - x );
		var dy = Mathf.Abs( ry - y );
		var dz = Mathf.Abs( rz - z );
		var largeDiff = Mathf.Max( dx, dy, dz );

		if ( largeDiff == dx ) {
			rx = -(ry + rz);
		}
		else if ( largeDiff == dy ) {
			ry = -(rx + rz);
		}
		else {	// largeDiff == dz
			rz = -(rx + ry);
		}

		return new CubeCoordinate( rx, ry, rz );
	}

	public int Distance( CubeCoordinate subject ) {
		return Distance( this, subject );
	}

	public static int Distance( CubeCoordinate left, CubeCoordinate right ) {
		var x = Mathf.Abs( left.x - right.x );
		var y = Mathf.Abs( left.y - right.y );
		var z = Mathf.Abs( left.z - right.z );
		return Mathf.Max( x, y, z );
	}

	public static CubeCoordinate operator+ ( CubeCoordinate left, CubeCoordinate right ) {
		var x = left.x + right.x;
		var y = left.y + right.y;
		var z = left.z + right.z;
		return new CubeCoordinate( x, y, z );
	}

	public static CubeCoordinate operator- ( CubeCoordinate left, CubeCoordinate right ) {
		var x = left.x - right.x;
		var y = left.y - right.y;
		var z = left.z - right.z;
		return new CubeCoordinate( x, y, z );
	}

	public static CubeCoordinate operator* ( CubeCoordinate left, int scala ) {
		var x = left.x * scala;
		var y = left.y * scala;
		var z = left.z * scala;
		return new CubeCoordinate( x, y, z );
	}

	public static CubeCoordinate operator* ( int scala, CubeCoordinate right ) {
		return right * scala;
	}

	public static CubeCoordinate operator* ( CubeCoordinate left, float scala ) {
		var x = left.x * scala;
		var y = left.y * scala;
		var z = left.z * scala;
		return CubeRound( x, y, z );
	}

	public static CubeCoordinate operator* ( float scala, CubeCoordinate right ) {
		return right * scala;
	}

	public static CubeCoordinate operator/ ( CubeCoordinate left, float scala ) {
		return left * (1 / scala);
	}

	public static CubeCoordinate operator/ ( float scala, CubeCoordinate right ) {
		return right * (1 / scala);
	}
}
