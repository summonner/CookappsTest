using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour {
	private IDictionary<CubeCoordinate, Cell> map = new Dictionary<CubeCoordinate, Cell>( 30 );
	public Spawner spawner;

	void Awake() {
		foreach ( var child in GetComponentsInChildren<Cell>() ) {
			map.Add( child.position, child );
		}
	}

	private void Start() {
		StartCoroutine( Fill() );
	}

	private IEnumerator Fill() {
		var current = spawner.Spawn();
		while ( current != null ) {
			while ( true ) {
				var next = FindEmptyCell( current.position );
				if ( next == null ) {
					break;
				}

				current.SwapBlock( next );
				current = next;
			}

			yield return new WaitForSeconds( spawner.spawnDelay );
			current = spawner.Spawn();
		}
	}

	private Cell FindEmptyCell( CubeCoordinate position ) {
		foreach ( var dir in dropDirections ) {
			var next = position + dir;
			if ( map.ContainsKey( next ) == false ) {
				continue;
			}

			if ( map[next].block != null ) {
				continue;
			}

			return map[next];
		}

		return null;
	}

	private IEnumerable<CubeCoordinate> dropDirections {
		get {
			yield return FlatTopDirection.S;
			if ( Random.value > 0.5f ) {
				yield return FlatTopDirection.SE;
				yield return FlatTopDirection.SW;
			}
			else {
				yield return FlatTopDirection.SW;
				yield return FlatTopDirection.SE;
			}
		}
	}
}
