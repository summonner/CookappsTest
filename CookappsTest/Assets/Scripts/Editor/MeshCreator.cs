using UnityEngine;
using UnityEditor;
using System.Collections;

public static class MeshCreator {

	[MenuItem( "My Editor/Create Hexa Cell Mesh" )]
	public static void Create() {
		var vertices = new Vector3[6];
		for ( int i=0; i < 6; ++i ) {
			vertices[i] = GetHexaCorner( Vector3.zero, .5f, i );
		}

		var indices = new int[12];
		for ( int i=0; i < 4; ++i ) {
			indices[i * 3 + 0] = 5;
			indices[i * 3 + 1] = 4 - i;
			indices[i * 3 + 2] = 3 - i;
		}

		var mesh = new Mesh();
		mesh.vertices = vertices;
		mesh.triangles = indices;
		MeshUtility.Optimize( mesh );
		mesh.RecalculateNormals();

		AssetDatabase.CreateAsset( mesh, "Assets/hexagon.asset" );
	}

	private static Vector3 GetHexaCorner( Vector3 center, float size, int index ) {
		var deg = 60 * index + 0;
		var rad = Mathf.Deg2Rad * deg;
		var x = center.x + size * Mathf.Cos( rad );
		var y = center.y + size * Mathf.Sin( rad );
		var z = 0;
		return new Vector3( x, y, z );
	}
}
