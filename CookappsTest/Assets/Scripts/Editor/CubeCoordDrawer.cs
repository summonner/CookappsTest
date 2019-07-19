using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer( typeof(CubeCoordinate) )]
public class CubeCoordDrawer : PropertyDrawer {
	public override void OnGUI( Rect position, SerializedProperty property, GUIContent label ) {
		int x = property.FindPropertyRelative( "_x" ).intValue;
		int z = property.FindPropertyRelative( "_z" ).intValue;
		int y = (x + z) * -1;
		EditorGUI.LabelField( position, label, new GUIContent( "( " + x + ", " + y + ", " + z + " )" ) );
	}
}
