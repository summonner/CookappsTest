using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {
	public BlockFactory factory;
	public int[] candidates = { 0 };
	public float spawnDelay = 0.5f;
	public Cell cell;

	public Cell Spawn() {
		if ( cell.block != null ) {
			return null;
		}

		var block = factory.Generate( candidates[Random.Range( 0, candidates.Length )] );
		block.Set( cell );
		return cell;
	}
}
