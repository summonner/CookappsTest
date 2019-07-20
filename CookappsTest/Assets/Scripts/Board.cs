﻿using System.Collections;
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
		AnimScheduler.instance.Destroy( map[source].block );
		AnimScheduler.instance.Destroy( map[dest].block );
	}

	public bool Match() {
		return true;
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
