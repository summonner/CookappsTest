using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockMover : MonoBehaviour {
	public float animTime = 0.1f;
	public IDictionary<Block, Queue<Vector3>> anims = new Dictionary<Block, Queue<Vector3>>();

	public static BlockMover instance;
	private void Awake() {
		Debug.Assert( instance == null );
		instance = this;
	}

	private void OnDestroy() {
		Debug.Assert( instance != null );
		instance = null;
	}

	public bool isPlaying {
		get {
			return anims.Count > 0;
		}
	}

	public void Move( Block block, Cell moveTo ) {
		if ( block == null ) {
			return;
		}

		if ( anims.ContainsKey( block ) == true ) {
			anims[block].Enqueue( moveTo.transform.position );
			return;
		}

		var queue = new Queue<Vector3>();
		anims.Add( block, queue );
		queue.Enqueue( moveTo.transform.position );
		StartCoroutine( MoveAnim( block, queue ) );
	}

	private IEnumerator MoveAnim( Block block, Queue<Vector3> destinations ) {
		var transform = block.transform;
		var elapsed = 0f;
		while ( destinations.Count > 0 ) {
			var source = transform.position;
			var destination = destinations.Peek();
			while ( elapsed < animTime ) {
				transform.position = Vector3.Lerp( source, destination, elapsed / animTime );
				yield return null;
				elapsed += Time.deltaTime;
			}
			transform.position = destination;
			destinations.Dequeue();
			elapsed -= animTime;
		}

		anims.Remove( block );
	}
}
