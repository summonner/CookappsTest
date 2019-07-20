using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Top : Block {
	private int life = 2;
	private float spinSpeed = 1f;
	public Transform spinner;

	public override void Splash() {
		life -= 1;
		if ( life <= 0 ) {
			Destroy();
		}
	}

	private void Update() {
		if ( life >= 2 ) {
			return;
		}

		spinner.Rotate( Vector3.up, spinSpeed * 360f * Time.deltaTime );
	}
}
