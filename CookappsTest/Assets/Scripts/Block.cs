﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour {
	public void Set( Cell cell ) {
		cell.block = this;
		transform.position = cell.transform.position;
	}
}