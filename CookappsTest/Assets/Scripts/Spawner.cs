using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {
	public Block[] templates;
	public float spawnDelay = 0.5f;
	public Cell cell;

	public Cell Spawn() {
		if ( cell.block != null ) {
			return null;
		}

		var block = NewBlock( templates[Random.Range( 0, templates.Length )] );
		block.Set( cell );
		return cell;
	}

	private static Block NewBlock( Block blockTemplate ) {
		var block = Instantiate( blockTemplate );
		var transform = block.transform;
		transform.position = Vector3.zero;
		transform.rotation = Quaternion.identity;
		transform.localScale = Vector3.one;
		return block;
	}

}
