using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockFactory : MonoBehaviour {
	public Block[] templates;

	public Block Generate( int type ) {
		var isOutOfRange = type < 0
						|| type >= templates.Length;
		if ( isOutOfRange == true ) {
			return null;
		}

		return Generate( templates[type] );
	}

	private static Block Generate( Block blockTemplate ) {
		var block = Instantiate( blockTemplate );
		var transform = block.transform;
		transform.position = Vector3.zero;
		transform.rotation = Quaternion.identity;
		transform.localScale = Vector3.one;
		return block;
	}
}
