using UnityEngine;
using System.Collections;

public class LevelGenerator : MonoBehaviour
{

	public UnityEngine.GameObject tile;
	public UnityEngine.GameObject tileNC;
	public UnityEngine.GameObject player;
	public UnityEngine.GameObject agent;
	public UnityEngine.GameObject drone;
	
	void Start() {
	}

	void Update() {
	}

	GameObject CreateTile(int x, int y, Level level) {
		TileType tt = level.GetTileType(x,y);
		if(tt == TileType.WALL) {
			GameObject go;
			if(level.IsWallTileContained(x,y)) {
				// all neighbours are walls -> no collision required
				go = (GameObject)Instantiate(tileNC);
			}
			else {
				go = (GameObject)Instantiate(tile);
			}
			WallTile wt = go.GetComponent<WallTile>();
			wt.SetTile(level.GetWallTileIndex(x,y));
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

	const int BW = 12;
	const int BH = 8;
	const int NW = 4;
	const int NH = 3;
	const float DROP_PROP = 0.3f;

	// top: 1, right: 2, bottom: 4, left: 8
	// 0 = not initialized

	void CreateBlock(Level lvl, int bx, int by, int type) {
		int[,] blocks = new int[16,BH*BW]
		{
			{
				1,1,1,1,1,1,1,1,1,1,1,1,
				1,1,1,1,1,1,1,1,1,1,1,1,
				1,0,0,0,0,0,0,0,0,0,0,1,
				1,0,0,0,0,0,0,0,0,0,0,1,
				1,0,0,0,0,0,0,0,0,0,0,1,
				1,0,0,0,0,0,0,0,0,0,0,1,
				1,1,1,1,1,1,1,1,1,1,1,1,
				1,1,1,1,1,1,1,1,1,1,1,1
			},
			{
				1,1,1,1,1,0,0,1,1,1,1,1,
				1,1,1,1,1,0,0,1,1,1,1,1,
				1,0,0,0,0,0,0,0,0,0,0,1,
				1,0,0,0,0,0,0,0,0,0,0,1,
				1,0,0,0,0,0,0,0,0,0,0,1,
				1,0,0,0,0,0,0,0,0,0,0,1,
				1,1,1,1,1,1,1,1,1,1,1,1,
				1,1,1,1,1,1,1,1,1,1,1,1
			},
			{
				1,1,1,1,1,1,1,1,1,1,1,1,
				1,1,1,1,1,1,1,1,1,1,1,1,
				1,0,0,0,0,0,0,0,0,0,0,0,
				1,0,0,0,0,0,0,0,0,0,0,0,
				1,0,0,0,0,0,0,0,0,0,0,0,
				1,0,0,0,0,0,0,0,0,0,0,0,
				1,1,1,1,1,1,1,1,1,1,1,1,
				1,1,1,1,1,1,1,1,1,1,1,1
			},
			{
				1,1,1,1,1,0,0,1,1,1,1,1,
				1,1,1,1,1,0,0,1,1,1,1,1,
				1,0,0,0,0,0,0,0,0,0,0,0,
				1,0,0,0,0,0,0,0,0,0,0,0,
				1,0,0,0,0,0,0,0,0,0,0,0,
				1,0,0,0,0,0,0,0,0,0,0,0,
				1,1,1,1,1,1,1,1,1,1,1,1,
				1,1,1,1,1,1,1,1,1,1,1,1
			},
			{
				1,1,1,1,1,1,1,1,1,1,1,1,
				1,1,1,1,1,1,1,1,1,1,1,1,
				1,0,0,0,0,0,0,0,0,0,0,1,
				1,0,0,0,0,0,0,0,0,0,0,1,
				1,0,0,0,0,0,0,0,0,0,0,1,
				1,0,0,0,0,0,0,0,0,0,0,1,
				1,1,1,1,1,0,0,1,1,1,1,1,
				1,1,1,1,1,0,0,1,1,1,1,1
			},
			{
				1,1,1,1,1,0,0,1,1,1,1,1,
				1,1,1,1,1,0,0,1,1,1,1,1,
				1,0,0,0,0,0,0,0,0,0,0,1,
				1,0,0,0,0,0,0,0,0,0,0,1,
				1,0,0,0,0,0,0,0,0,0,0,1,
				1,0,0,0,0,0,0,0,0,0,0,1,
				1,1,1,1,1,0,0,1,1,1,1,1,
				1,1,1,1,1,0,0,1,1,1,1,1
			},
			{
				1,1,1,1,1,1,1,1,1,1,1,1,
				1,1,1,1,1,1,1,1,1,1,1,1,
				1,0,0,0,0,0,0,0,0,0,0,0,
				1,0,0,0,0,0,0,0,0,0,0,0,
				1,0,0,0,0,0,0,0,0,0,0,0,
				1,0,0,0,0,0,0,0,0,0,0,0,
				1,1,1,1,1,0,0,1,1,1,1,1,
				1,1,1,1,1,0,0,1,1,1,1,1
			},
			{
				1,1,1,1,1,0,0,1,1,1,1,1,
				1,1,1,1,1,0,0,1,1,1,1,1,
				1,0,0,0,0,0,0,0,0,0,0,0,
				1,0,0,0,0,0,0,0,0,0,0,0,
				1,0,0,0,0,0,0,0,0,0,0,0,
				1,0,0,0,0,0,0,0,0,0,0,0,
				1,1,1,1,1,0,0,1,1,1,1,1,
				1,1,1,1,1,0,0,1,1,1,1,1
			},
			{
				1,1,1,1,1,1,1,1,1,1,1,1,
				1,1,1,1,1,1,1,1,1,1,1,1,
				0,0,0,0,0,0,0,0,0,0,0,1,
				0,0,0,0,0,0,0,0,0,0,0,1,
				0,0,0,0,0,0,0,0,0,0,0,1,
				0,0,0,0,0,0,0,0,0,0,0,1,
				1,1,1,1,1,1,1,1,1,1,1,1,
				1,1,1,1,1,1,1,1,1,1,1,1
			},
			{
				1,1,1,1,1,0,0,1,1,1,1,1,
				1,1,1,1,1,0,0,1,1,1,1,1,
				0,0,0,0,0,0,0,0,0,0,0,1,
				0,0,0,0,0,0,0,0,0,0,0,1,
				0,0,0,0,0,0,0,0,0,0,0,1,
				0,0,0,0,0,0,0,0,0,0,0,1,
				1,1,1,1,1,1,1,1,1,1,1,1,
				1,1,1,1,1,1,1,1,1,1,1,1
			},
			{
				1,1,1,1,1,1,1,1,1,1,1,1,
				1,1,1,1,1,1,1,1,1,1,1,1,
				0,0,0,0,0,0,0,0,0,0,0,0,
				0,0,0,0,0,0,0,0,0,0,0,0,
				0,0,0,0,0,0,0,0,0,0,0,0,
				0,0,0,0,0,0,0,0,0,0,0,0,
				1,1,1,1,1,1,1,1,1,1,1,1,
				1,1,1,1,1,1,1,1,1,1,1,1
			},
			{
				1,1,1,1,1,0,0,1,1,1,1,1,
				1,1,1,1,1,0,0,1,1,1,1,1,
				0,0,0,0,0,0,0,0,0,0,0,0,
				0,0,0,0,0,0,0,0,0,0,0,0,
				0,0,0,0,0,0,0,0,0,0,0,0,
				0,0,0,0,0,0,0,0,0,0,0,0,
				1,1,1,1,1,1,1,1,1,1,1,1,
				1,1,1,1,1,1,1,1,1,1,1,1
			},
			{
				1,1,1,1,1,1,1,1,1,1,1,1,
				1,1,1,1,1,1,1,1,1,1,1,1,
				0,0,0,0,0,0,0,0,0,0,0,1,
				0,0,0,0,0,0,0,0,0,0,0,1,
				0,0,0,0,0,0,0,0,0,0,0,1,
				0,0,0,0,0,0,0,0,0,0,0,1,
				1,1,1,1,1,0,0,1,1,1,1,1,
				1,1,1,1,1,0,0,1,1,1,1,1
			},
			{
				1,1,1,1,1,0,0,1,1,1,1,1,
				1,1,1,1,1,0,0,1,1,1,1,1,
				0,0,0,0,0,0,0,0,0,0,0,1,
				0,0,0,0,0,0,0,0,0,0,0,1,
				0,0,0,0,0,0,0,0,0,0,0,1,
				0,0,0,0,0,0,0,0,0,0,0,1,
				1,1,1,1,1,0,0,1,1,1,1,1,
				1,1,1,1,1,0,0,1,1,1,1,1
			},
			{
				1,1,1,1,1,1,1,1,1,1,1,1,
				1,1,1,1,1,1,1,1,1,1,1,1,
				0,0,0,0,0,0,0,0,0,0,0,0,
				0,0,0,0,0,0,0,0,0,0,0,0,
				0,0,0,0,0,0,0,0,0,0,0,0,
				0,0,0,0,0,0,0,0,0,0,0,0,
				1,1,1,1,1,0,0,1,1,1,1,1,
				1,1,1,1,1,0,0,1,1,1,1,1
			},
			{
				1,1,1,1,1,0,0,1,1,1,1,1,
				1,1,1,1,1,0,0,1,1,1,1,1,
				0,0,0,0,0,0,0,0,0,0,0,0,
				0,0,0,0,0,0,0,0,0,0,0,0,
				0,0,0,0,0,0,0,0,0,0,0,0,
				0,0,0,0,0,0,0,0,0,0,0,0,
				1,1,1,1,1,0,0,1,1,1,1,1,
				1,1,1,1,1,0,0,1,1,1,1,1
			}
		};
		for(int y=0; y<BH; y++) {
			for(int x=0; x<BW; x++) {
				TileType tt = (blocks[type,x+y*BW] == 1) ? TileType.WALL : TileType.NONE;
				lvl.tiles[BH*NH-1-(y+by*BH),x+bx*BW] = tt;
			}
		}
	}

	public Level CreateRandomLevel() {
		// init blocks
		int[,] blocks = new int[NH,NW];
		for(int y=0; y<NH; y++) {
			for(int x=0; x<NW; x++) {
				blocks[y,x] = 0;
			}
		}
		// choose random block in first row
		int x0 = Random.Range(0, NW);
		bool goRight = (x0 <= NW/2);
		// gamble for mandatory drop down
		int xdrop;
		if(goRight) {
			xdrop = Random.Range(x0, NW);
			for(int x=x0; x<=xdrop; x++) {
				int v = blocks[0,x];
				if(x < xdrop) v += 2;
				if(x-1 >= x0) v += 8;
				if((x == xdrop) || (Random.value < DROP_PROP)) {
					v += 4;
				}
				blocks[0,x] = v;
			}
		}
		else {
			xdrop = Random.Range(0, x0+1);
			for(int x=x0; x>=xdrop; x--) {
				int v = blocks[0,x];
				if(x > xdrop) v += 8;
				if(x+1 <= x0) v += 2;
				if((x == xdrop) || (Random.value < DROP_PROP)) {
					v += 4;
				}
				blocks[0,x] = v;
			}
		}
		// create blocks
		Level lvl = new Level(NW*BW,NH*BH);
		for(int y=0; y<NH; y++) {
			for(int x=0; x<NW; x++) {
				CreateBlock(lvl, x, y, blocks[y,x]);
			}
		}
		// place player
		lvl.tiles[BH*NH-1-(0*BH+3),x0*BW+3] = TileType.PLAYER;
		return lvl;
	}
	
	public void CreateGameobjects(Level level) {
		// create parent gameobject
		GameObject go_parent = new GameObject();
		go_parent.name = "Level";
		go_parent.transform.position = Vector3.zero;
		// create level
		for(int y=0; y<level.height; y++) {
			for(int x=0; x<level.width; x++) {
				GameObject go = CreateTile(x, y, level);
				if(go != null) {
					go.transform.parent = go_parent.transform;
					go.transform.localPosition = new Vector3(x, y);
				}
			}
		}
	}

}
