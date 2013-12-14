using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections.Generic;

public class AutoWallSprites : EditorWindow
{
	
	[MenuItem("ld28/Auto Wall Sprites")]
	static void Init() {
		AutoWallSprites window = (AutoWallSprites)EditorWindow.GetWindow(typeof(AutoWallSprites));
	}
	
	UnityEngine.GameObject obj;
	string spriteName;
	
	void OnGUI() {
		obj = (UnityEngine.GameObject)EditorGUILayout.ObjectField("Wall Tile Object", obj, typeof(UnityEngine.GameObject));
		spriteName = EditorGUILayout.TextField("Base name of sprite", spriteName);
		if(GUILayout.Button("Apply")) {
			Execute();
		}
	}

	void Execute() {
		UnityEngine.Object[] allSprites = AssetDatabase.LoadAllAssetRepresentationsAtPath(
			"Assets/sprites/" + spriteName + ".png");
		Sprite[] sprites = allSprites.Cast<Sprite>().ToArray();

		WallTile wt = obj.GetComponent<WallTile>();
		if(wt != null) wt.sprites = sprites;

		Number num = obj.GetComponent<Number>();
		if(num != null) num.sprites = sprites;
	}
}

