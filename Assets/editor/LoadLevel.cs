using UnityEngine;
using UnityEditor;

public class LoadLevel : EditorWindow
{

	[MenuItem("ld28/Load Level")]
	static void Init() {
		LoadLevel window = (LoadLevel)EditorWindow.GetWindow(typeof(LoadLevel));
	}

	UnityEngine.GameObject tile;
	UnityEngine.GameObject tileNC;
	UnityEngine.GameObject player;
	UnityEngine.GameObject agent;
	UnityEngine.GameObject drone;
	UnityEngine.Texture2D tex;

	void OnGUI() {
//		EditorGUILayout.BeginHorizontal();
		tile = (UnityEngine.GameObject)EditorGUILayout.ObjectField("Tile", tile, typeof(UnityEngine.GameObject));
		tileNC = (UnityEngine.GameObject)EditorGUILayout.ObjectField("Tile No Collider", tileNC, typeof(UnityEngine.GameObject));
		player = (UnityEngine.GameObject)EditorGUILayout.ObjectField("Player", player, typeof(UnityEngine.GameObject));
		agent = (UnityEngine.GameObject)EditorGUILayout.ObjectField("Agent", agent, typeof(UnityEngine.GameObject));
		drone = (UnityEngine.GameObject)EditorGUILayout.ObjectField("Drone", drone, typeof(UnityEngine.GameObject));
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
	static int col_red = Color32ToInt(new Color32(255,0,0,255));
	static int col_reddark = Color32ToInt(new Color32(128,0,0,255));
	static int col_green = Color32ToInt(new Color32(0,255,0,255));

	enum TileType {
		NONE,
		WALL,
		PLAYER,
		AGENT,
		DRONE,
		UNKNOWN
	}

	static TileType Color32ToValue(Color32 col) {
		int ic = Color32ToInt(col);
		if(ic == col_white) return TileType.NONE;
		if(ic == col_black) return TileType.WALL;
		if(ic == col_red) return TileType.AGENT;
		if(ic == col_reddark) return TileType.DRONE;
		if(ic == col_green) return TileType.PLAYER;
		return TileType.UNKNOWN;
	}

	TileType GetTileType(int x, int y, TileType[,] values, int width, int height) {
		if(0 <= x && x < width && 0 <= y && y < height) {
			return values[y,x];
		}
		else {
			return TileType.WALL;
		}
	}

	int GetWall(int x, int y, TileType[,] values, int width, int height) {
		TileType tt = GetTileType(x,y,values,width,height);
		if(tt == TileType.WALL) {
			return 1;
		}
		else {
			return 0;
		}
	}

	GameObject CreateTile(int x, int y, TileType[,] values, int width, int height) {
		TileType tt = GetTileType(x,y,values,width,height);
		if(tt == TileType.WALL) {
			int neighbours =
				+ 1*GetWall(x,y+1,values,width,height)
				+ 2*GetWall(x+1,y,values,width,height)
				+ 4*GetWall(x,y-1,values,width,height)
				+ 8*GetWall(x-1,y,values,width,height);
			int neighboursDiag = 
				+ GetWall(x+1,y+1,values,width,height)
				+ GetWall(x+1,y-1,values,width,height)
				+ GetWall(x-1,y-1,values,width,height)
				+ GetWall(x-1,y+1,values,width,height);
			GameObject go;
			if(neighbours == 0 && neighboursDiag == 0) {
				// all neighbours are walls -> no collision required
				go = (GameObject)Instantiate(tileNC);
			}
			else {
				go = (GameObject)Instantiate(tile);
			}
			WallTile wt = go.GetComponent<WallTile>();
			wt.SetTile(neighbours);
			return go;
		}
		else if(tt == TileType.PLAYER) {
			return (GameObject)Instantiate(player);
		}
		else if(tt == TileType.AGENT) {
			GameObject go = (GameObject)Instantiate(agent);
			AgentAI ai = go.GetComponent<AgentAI>();
			ai.desiredX = x;
			return go;
		}
		else if(tt == TileType.DRONE) {
			GameObject go = (GameObject)Instantiate(drone);
			Drone ai = go.GetComponent<Drone>();
			ai.Home = new Vector2(x,y);
			return go;
		}
		else {
			return null;
		}
	}

	void Execute() {
		// create parent gameobject
		GameObject go_parent = new GameObject();
		go_parent.name = "Level";
		go_parent.transform.position = Vector3.zero;
		// convert color to game data
		int width = tex.width;
		int height = tex.height;
		Color32[] pixels = tex.GetPixels32();
		TileType[,] values = new TileType[height,width];
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
