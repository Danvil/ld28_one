using UnityEngine;
using UnityEditor;

public class LoadLevel : EditorWindow
{

	[MenuItem("ld28/Load Level")]
	static void Init() {
		LoadLevel window = (LoadLevel)EditorWindow.GetWindow(typeof(LoadLevel));
	}

	UnityEngine.GameObject tile;
	UnityEngine.Texture2D tex;

	void OnGUI() {
//		EditorGUILayout.BeginHorizontal();
		tile = (UnityEngine.GameObject)EditorGUILayout.ObjectField("Tile", tile, typeof(UnityEngine.GameObject));
		tex = (UnityEngine.Texture2D)EditorGUILayout.ObjectField("Level", tex, typeof(UnityEngine.Texture2D));
//		EditorGUILayout.EndHorizontal();
		if(GUILayout.Button("Load")) {
			Execute();
		}
	}

	static int Color32ToInt(Color32 col) {
		return col.r + 256*col.g + 256*256*col.b;
	}

	static int col_white = Color32ToInt(new Color32(255,255,255,255));
	static int col_black = Color32ToInt(new Color32(0,0,0,255));

	static int Color32ToValue(Color32 col) {
		int ic = Color32ToInt(col);
		if(ic == col_white) return 0;
		if(ic == col_black) return 1;
		return 0;
	}

	int GetValue(int x, int y, int[,] values, int width, int height) {
		if(0 <= x && x < width && 0 <= y && y < height) {
			return values[y,x];
		}
		else {
			return 1;
		}
	}

	GameObject CreateTile(int x, int y, int[,] values, int width, int height) {
		int v = GetValue(x,y,values,width,height);
		if(v == 1) {
			GameObject go = (GameObject)Instantiate(tile);
			WallTile wt = go.GetComponent<WallTile>();
			int neighbours =
				+ 1*GetValue(x,y+1,values,width,height)
				+ 2*GetValue(x+1,y,values,width,height)
				+ 4*GetValue(x,y-1,values,width,height)
				+ 8*GetValue(x-1,y,values,width,height);
			wt.SetTile(neighbours);
			return go;
		}
		else {
			return null;
		}
	}

	void Execute() {
		// create parent gameobject
		GameObject go_parent = new GameObject();
		go_parent.transform.position = Vector3.zero;
		// convert color to game data
		int width = tex.width;
		int height = tex.height;
		Color32[] pixels = tex.GetPixels32();
		int[,] values = new int[height,width];
		for(int y=0; y<height; y++) {
			for(int x=0; x<width; x++) {
				values[y,x] = Color32ToValue(pixels[x+y*width]);
			}
		}
		// create level
		for(int y=0; y<height; y++) {
			for(int x=0; x<width; x++) {
				GameObject go = CreateTile(x, y, values, width, height);
				if(go != null) {
					go.transform.parent = go_parent.transform;
					go.transform.localPosition = new Vector3(x, y);
				}
			}
		}
	}
}
