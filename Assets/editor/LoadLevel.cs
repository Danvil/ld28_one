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

	static int Color32ToInt(Color32 col) {
		return col.r + 256*col.g + 256*256*col.b;
	}

	static int col_white = Color32ToInt(new Color32(255,255,255,255));
	static int col_black = Color32ToInt(new Color32(0,0,0,255));
	static int col_red = Color32ToInt(new Color32(255,0,0,255));
	static int col_reddark = Color32ToInt(new Color32(128,0,0,255));
	static int col_green = Color32ToInt(new Color32(0,255,0,255));
	static int col_blue = Color32ToInt(new Color32(0,0,255,255));

	static TileType Color32ToValue(Color32 col) {
		int ic = Color32ToInt(col);
		if(ic == col_white) return TileType.NONE;
		if(ic == col_black) return TileType.WALL;
		if(ic == col_red) return TileType.AGENT;
		if(ic == col_reddark) return TileType.DRONE;
		if(ic == col_green) return TileType.PLAYER;
		if(ic == col_blue) return TileType.PORTAL;
		return TileType.UNKNOWN;
	}

	void ExecuteLoad(Texture2D tex) {
		LevelGenerator lvlgen = lvlGenGo.GetComponent<LevelGenerator>();
		// convert color to game data
		int width = tex.width;
		int height = tex.height;
		Color32[] pixels = tex.GetPixels32();
		Level level = new Level(width, height);
		for(int y=0; y<height; y++) {
			for(int x=0; x<width; x++) {
				level.tiles[y,x] = Color32ToValue(pixels[x+y*width]);
			}
		}
		// create level
		lvlgen.CreateGameobjects(level);
	}

	void ExecuteRandom() {
		LevelGenerator lvlgen = lvlGenGo.GetComponent<LevelGenerator>();
		Level level = lvlgen.CreateRandomLevel();
		lvlgen.CreateGameobjects(level);
	}

}
