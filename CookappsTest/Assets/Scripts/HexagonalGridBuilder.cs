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

	public Vector3 Hex2World( CubeCoordinate hex ) {
		var horizontalSpacing = (cellRadius * 1.5f + gap);
		var verticalSpacing = cellRadius * Mathf.Sqrt( 3 ) + gap;
		Vector3 position;
		position.y = verticalSpacing * (hex.z + 0.5f * hex.x);
		position.x = horizontalSpacing * hex.x;
		position.z = 0;
		return position;
	}

}
