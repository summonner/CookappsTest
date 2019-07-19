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
}
