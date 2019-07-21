using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Board : MonoBehaviour, IBeginDragHandler, IDragHandler {
	private IDictionary<CubeCoordinate, Cell> map = new SortedList<CubeCoordinate, Cell>( 30, new OrderByHeight() );
	public HexagonalGridBuilder converter;
	public BlockFactory factory;
	public Spawner spawner;

	public delegate void OnInputEvent( Input input );
	public static event OnInputEvent OnInput = delegate { };

	void Awake() {
		foreach ( var child in GetComponentsInChildren<Cell>() ) {
			map.Add( child.position, child );
			Init( child );
		}
	}

	private void Init( Cell cell ) {
		var block = factory.Generate( (int)cell.initialBlock );
		if ( block == null ) {
			return;
		}

		block.Set( cell );
	}

	public void Swap( Input input ) {
		var source = input.selected;
		var dest = input.second;

		var isValid = map.ContainsKey( source )
				   && map.ContainsKey( dest );
		if ( isValid == false ) {
			return;
		}

		map[source].SwapBlock( map[dest] );
	}

	public bool Match() {
		var matched = new List<CubeCoordinate>();
		foreach ( var cell in map.Values ) {
			var skip = cell.block == null
					|| cell.block.color <= 0;
			if ( skip == true ) {
				continue;
			}

			foreach ( var direction in pullDirection ) {
				matched.AddRange( Match( cell, GetLine( direction ), 3 ) );
			}

			foreach ( var cluster in clusters ) {
				matched.AddRange( Match( cell, cluster, 4 ) );
			}
		}

		var splashes = new List<Cell>();
		matched = matched.Distinct().ToList();
		foreach ( var position in matched ) {
			map[position].block.Destroy();
			splashes.AddRange( FindSplashArea( position ) );
		}

		foreach ( var cell in splashes.Distinct() ) {
			cell.block.Splash();
		}

		return matched.Count > 0;
	}

	private IEnumerable<CubeCoordinate> GetLine( CubeCoordinate direction ) {
		for ( var i=1; ; ++i ) {
			yield return direction * i;
		}
	}

	private CubeCoordinate[][] clusters = new [] {
		new [] { FlatTopDirection.NW, FlatTopDirection.N, FlatTopDirection.NE },
		new [] { FlatTopDirection.N, FlatTopDirection.NE, FlatTopDirection.SE },
		new [] { FlatTopDirection.SW, FlatTopDirection.NW, FlatTopDirection.N },
	};

	private IEnumerable<CubeCoordinate> Match( Cell origin, IEnumerable<CubeCoordinate> diffs, int minimumMatchCount ) {
		var color = origin.block.color;
		var matched = new List<CubeCoordinate>( minimumMatchCount );
		matched.Add( origin.position );

		foreach ( var diff in diffs ) {
			var current = origin.position + diff;
			Cell cell = null;
			if ( map.TryGetValue( current, out cell ) == false ) {
				break;
			}

			if ( cell.block.color != color ) {
				break;
			}

			matched.Add( current );
		}

		if ( matched.Count >= minimumMatchCount ) {
			return matched;
		}
		else {
			return new CubeCoordinate[0];
		}
	}

	private IEnumerable<Cell> FindSplashArea( CubeCoordinate target ) {
		Cell cell = null;
		foreach ( var direction in FlatTopDirection.around ) {
			if ( map.TryGetValue( target + direction, out cell ) == false ) {
				continue;
			}

			if ( cell.block == null ) {
				continue;
			}

			yield return cell;
		}
	}

	public Coroutine Fill() {
		return StartCoroutine( FillAux() );
	}

	private IEnumerator FillAux() {
		do {
			Pull();
			yield return new WaitForSeconds( spawner.spawnDelay );
		} while ( spawner.Spawn() != null );
	}

	private void Pull() {
		var hasMove = true;
		while ( hasMove ) {
			hasMove = false;
			foreach ( var cell in map ) {
				hasMove |= (Pull( cell.Value ) != null);
			}
		}
	}

	private Cell Pull( Cell current ) {
		if ( current.block != null ) {
			return null;
		}

		var next = FindCellHasBlock( current.position, pullDirection );
		if ( next != null ) {
			current.SwapBlock( next );
		}

		return next;
	}

	private Cell FindCellHasBlock( CubeCoordinate position, IEnumerable<CubeCoordinate> directions ) {
		foreach ( var dir in directions ) {
			var next = position + dir;
			if ( map.ContainsKey( next ) == false ) {
				continue;
			}

			if ( map[next].block == null ) {
				continue;
			}

			return map[next];
		}

		return null;
	}

	public void OnBeginDrag( PointerEventData eventData ) {
		var selected = converter.World2Hex( eventData.pointerPressRaycast.worldPosition - transform.position );
		var drag = Vector3.Normalize( eventData.pointerCurrentRaycast.worldPosition - eventData.pointerPressRaycast.worldPosition );
		var direction = converter.World2Hex( drag );
		OnInput( new Input( selected, direction ) );
	}

	public void OnDrag( PointerEventData eventData ) {
		// do nothing
	}

	private static IEnumerable<CubeCoordinate> pullDirection {
		get {
			yield return FlatTopDirection.N;
			if ( Random.value > 0.5f ) {
				yield return FlatTopDirection.NE;
				yield return FlatTopDirection.NW;
			}
			else {
				yield return FlatTopDirection.NW;
				yield return FlatTopDirection.NE;
			}

		}
	}

	private class OrderByHeight : IComparer<CubeCoordinate> {
		public int Compare( CubeCoordinate left, CubeCoordinate right ) {
			var heightDiff = Height( left ) - Height( right );
			if ( heightDiff != 0 ) {
				return heightDiff;
			}

			return left.x - right.x;
		}

		private int Height( CubeCoordinate coord ) {
			return coord.z - coord.y;
		}
	}
}
