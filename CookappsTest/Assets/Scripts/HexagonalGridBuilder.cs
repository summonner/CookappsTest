using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HexagonalGridBuilder : MonoBehaviour {
	public GameObject template;
	public float cellRadius = 0.5f;
	public float gap = 0.1f;

	public int mapRadius = 5;
	public int xLimit = 3;

	[ContextMenu( "Build" )]
	void Build() {
		RemoveChildren();
		foreach ( var position in Traverser.Fan( new CubeCoordinate( 0, 0 ), mapRadius ) ) {
			var isOutOfRange = position.x > xLimit
							|| position.x < -xLimit;
			if ( isOutOfRange == true ) {
				continue;
			}

			var trans = Spawn( position );
			trans.name = position.ToString();
			var cell = trans.gameObject.AddComponent<Cell>();
			cell.position = position;
		}
	}

	private void RemoveChildren() {
		while ( transform.childCount > 0 ) {
			var child = transform.GetChild( 0 );
			DestroyImmediate( child.gameObject );
		}
	}

	private Transform Spawn( CubeCoordinate hexPosition ) {
		var mesh = Instantiate<GameObject>( template );
		mesh.name = hexPosition.ToString();
		var trans = mesh.transform;
		trans.parent = transform;
		trans.localPosition = Hex2World( hexPosition );
		trans.localRotation = Quaternion.identity;
		trans.localScale = Vector3.one;
		return trans;
	}

	private Vector2 spacing {
		get {
			return new Vector2(
				(cellRadius * 1.5f + gap),
				cellRadius * Mathf.Sqrt( 3 ) + gap
			);
		}
	}

	public Vector3 Hex2World( CubeCoordinate hex ) {
		var spacing = this.spacing;
		Vector3 position;
		position.y = spacing.y * (hex.z + 0.5f * hex.x);
		position.x = spacing.x * hex.x;
		position.z = 0;
		return position;
	}

	public CubeCoordinate World2Hex( Vector3 world ) {
		var spacing = this.spacing;
		var x = world.x / spacing.x;
		var z = world.y / spacing.y - 0.5f * x;
		return CubeCoordinate.CubeRound( x, z );
	}
}
