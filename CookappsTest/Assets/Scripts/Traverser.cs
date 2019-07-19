using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Traverser {
	private static readonly CubeCoordinate[] around = FlatTopDirection.around;
	private static readonly CubeCoordinate[] half = new [] {
		FlatTopDirection.NE, FlatTopDirection.SE,
	};

	public static IEnumerable<CubeCoordinate> Ring( CubeCoordinate center, int radius ) {
		if ( radius <= 0f ) {
			yield return center;
			yield break;
		}

		var position = center + (around[4] * radius);
		foreach ( var dir in around ) {
			for ( var i = 0; i < radius; ++i ) {
				yield return position;
				position += dir;
			}
		}
	}

	public static IEnumerable<CubeCoordinate> Spiral( CubeCoordinate center, int radius ) {
		for ( ; radius >= 0; --radius ) {
			foreach ( var position in Ring( center, radius ) ) {
				yield return position;
			}
		}
	}

	public static IEnumerable<CubeCoordinate> Arc( CubeCoordinate center, int radius ) {
		if ( radius <= 0f ) {
			yield return center;
			yield break;
		}

		var position = center + (around[4] * radius);
		foreach ( var dir in half ) {
			for ( var i = 0; i < radius; ++i ) {
				yield return position;
				position += dir;
			}
		}

		yield return position;
	}

	public static IEnumerable<CubeCoordinate> Fan( CubeCoordinate center, int radius ) {
		for ( ; radius >= 0; --radius ) {
			foreach ( var position in Arc( center, radius ) ) {
				yield return position;
			}
		}
	}
}

public class FlatTopDirection {
	public static readonly CubeCoordinate N = new CubeCoordinate( 0, 1 );
	public static readonly CubeCoordinate NE = new CubeCoordinate( 1, 0 );
	public static readonly CubeCoordinate SE = new CubeCoordinate( 1, -1 );
	public static readonly CubeCoordinate S = new CubeCoordinate( 0, -1 );
	public static readonly CubeCoordinate SW = new CubeCoordinate( -1, 0 );
	public static readonly CubeCoordinate NW = new CubeCoordinate( -1, 1 );

	public static readonly CubeCoordinate[] around = new [] { NE, SE, S, SW, NW, N };
}