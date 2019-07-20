using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class Cell : MonoBehaviour {
	public enum InitialBlockType {
		Top = 0,
		Red,
		Yellow,
		Green,
		Blue,
		Magenta,
	}

	public CubeCoordinate position;
	public InitialBlockType initialBlock;

	public Block block;

	public void SwapBlock( Cell cell ) {
		var temp = cell.block;
		cell.block = block;
		AnimScheduler.instance.Move( cell.block, cell );

		this.block = temp;
		AnimScheduler.instance.Move( block, this );
	}
}
