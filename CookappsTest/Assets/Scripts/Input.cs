using UnityEngine;

public struct Input {
	public readonly CubeCoordinate selected;
	public readonly CubeCoordinate direction;
	public CubeCoordinate second {
		get {
			return selected + direction;
		}
	}

	public Input( CubeCoordinate selected, CubeCoordinate direction ) {
		this.selected = selected;
		this.direction = direction;
	}
}
