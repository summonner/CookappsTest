using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class Cell : MonoBehaviour {
	public enum InitialBlockType {
		Normal = 0,
		Top,
	}

	public CubeCoordinate position;
	public InitialBlockType initialBlock;

	public Block block;

	public void SwapBlock( Cell cell ) {
		var temp = cell.block;
		cell.block = block;
		BlockMover.instance.Move( cell.block, cell );

		this.block = temp;
		BlockMover.instance.Move( block, cell );
	}
}
