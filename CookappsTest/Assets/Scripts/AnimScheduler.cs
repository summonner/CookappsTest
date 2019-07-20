using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimScheduler : MonoBehaviour {
	public float animTime = 0.1f;
	public IDictionary<Block, Queue<Vector3>> anims = new Dictionary<Block, Queue<Vector3>>();

	public static AnimScheduler instance;
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

	public void Destroy( Block block ) {
		Add( block, Vector3.positiveInfinity );
	}

	public void Move( Block block, Cell moveTo ) {
		Add( block, moveTo.transform.position );
	}

	private void Add( Block block, Vector3 destination ) {
		if ( block == null ) {
			return;
		}

		if ( anims.ContainsKey( block ) == true ) {
			anims[block].Enqueue( destination );
			return;
		}

		var queue = new Queue<Vector3>();
		anims.Add( block, queue );
		queue.Enqueue( destination );
		StartCoroutine( MoveAnim( block, queue ) );
	}

	private IEnumerator MoveAnim( Block block, Queue<Vector3> destinations ) {
		var transform = block.transform;
		var elapsed = 0f;
		while ( destinations.Count > 0 ) {
			var destination = destinations.Peek();
			if ( IsInfinite( destination ) == true ) {
				yield return StartCoroutine( DestroyAnim( transform, elapsed ) );
				break;
			}

			var source = transform.position;
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

	private bool IsInfinite( Vector3 value ) {
		return float.IsInfinity( value.x + value.y + value.z );
	}

	private IEnumerator DestroyAnim( Transform transform, float elapsed ) {
		while ( elapsed < animTime ) {
			transform.localScale = Vector3.Lerp( Vector3.one, Vector3.zero, elapsed / animTime );
			yield return null;
			elapsed += Time.deltaTime;
		}
		transform.localScale = Vector3.zero;
		Destroy( transform.gameObject );
	}
}
