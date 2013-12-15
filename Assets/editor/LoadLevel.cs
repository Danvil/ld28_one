using UnityEngine;
using UnityEditor;

public class LoadLevel : EditorWindow
{

	[MenuItem("ld28/Load Level")]
	static void Init() {
		LoadLevel window = (LoadLevel)EditorWindow.GetWindow(typeof(LoadLevel));
	}

	GameObject lvlGenGo;
	Texture2D tex;

	void OnGUI() {
		lvlGenGo = (GameObject)EditorGUILayout.ObjectField("Generator", lvlGenGo, typeof(GameObject));
		tex = (Texture2D)EditorGUILayout.ObjectField("Level", tex, typeof(Texture2D));
		if(GUILayout.Button("Load")) {
			ExecuteLoad(tex);
		}
		if(GUILayout.Button("Random")) {
			ExecuteRandom();
		}
	}

	void ExecuteLoad(Texture2D tex) {
		LevelGenerator lvlgen = lvlGenGo.GetComponent<LevelGenerator>();
		Level level = lvlgen.CreateLevelFromTex(tex);
		lvlgen.CreateGameobjects(level);
	}

	void ExecuteRandom() {
		LevelGenerator lvlgen = lvlGenGo.GetComponent<LevelGenerator>();
		Level level = lvlgen.CreateRandomLevel();
		lvlgen.CreateGameobjects(level);
	}

}
