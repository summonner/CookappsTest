using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour {
	public Board board;
	private Input received;
	private bool waitForInput = false;

	IEnumerator Start () {
		while ( true ) {
			do {
				yield return WaitForAnimation();
				yield return null;
				yield return board.Fill();
			} while ( board.Match() == true );

			yield return WaitForAnimation();
			yield return StartCoroutine( WaitForInput() );

			yield return Swap();
			if ( board.Match() == false ) {
				yield return Swap();
				continue;
			}
		}
	}

	private CustomYieldInstruction Swap() {
		board.Swap( received );
		return WaitForAnimation();
	}

	private CustomYieldInstruction WaitForAnimation() {
		return new WaitWhile( () => AnimScheduler.instance.isPlaying );
	}

	private IEnumerator WaitForInput() {
		waitForInput = true;
		Board.OnInput += OnInput;
		yield return new WaitWhile( () => waitForInput );
		Board.OnInput -= OnInput;
	}

	private void OnInput( Input input ) {
		received = input;
		waitForInput = false;
	}
}
