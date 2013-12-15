using UnityEngine;
using System.Collections;

public class LevelGenerator : MonoBehaviour
{

	public UnityEngine.GameObject tile;
	public UnityEngine.GameObject tileNC;
	public UnityEngine.GameObject player;
	public UnityEngine.GameObject agent;
	public UnityEngine.GameObject drone;
	public UnityEngine.GameObject machine;

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
		else if(tt == TileType.MACHINE) {
			GameObject go = (GameObject)Instantiate(machine);
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
	const float FROP_PROP = 0.3f;

	// top: 1, right: 2, bottom: 4, left: 8
	// 0 = not initialized

	void CreateBlock(Level lvl, int bx, int by, int type) {
		int[,] blocks = new int[16,BH*BW]
		{
			{
				1,1,1,1,1,1,1,1,1,1,1,1,
				1,0,0,0,0,0,0,0,0,0,0,1,
				1,0,0,0,0,0,0,0,0,0,0,1,
				1,0,0,0,0,0,0,0,0,0,0,1,
				1,0,0,0,0,0,0,0,0,0,0,1,
				1,0,0,0,0,0,0,0,0,0,0,1,
				1,0,0,0,0,0,0,0,0,0,0,1,
				1,1,1,1,1,1,1,1,1,1,1,1
			},
			{
				1,1,1,1,1,0,0,1,1,1,1,1,
				1,0,0,0,0,0,0,0,0,0,0,1,
				1,0,0,0,0,0,0,0,0,0,0,1,
				1,0,0,0,0,0,0,0,0,0,0,1,
				1,0,0,0,0,0,0,0,0,0,0,1,
				1,0,0,0,0,0,0,0,0,0,0,1,
				1,0,0,0,0,0,0,0,0,0,0,1,
				1,1,1,1,1,1,1,1,1,1,1,1
			},
			{
				1,1,1,1,1,1,1,1,1,1,1,1,
				1,0,0,0,0,0,0,0,0,0,0,1,
				1,0,0,0,0,0,0,0,0,0,0,0,
				1,0,0,0,0,0,0,0,0,0,0,0,
				1,0,0,0,0,0,0,0,0,0,0,0,
				1,0,0,0,0,0,0,0,0,0,0,0,
				1,0,0,0,0,0,0,0,0,0,0,1,
				1,1,1,1,1,1,1,1,1,1,1,1
			},
			{
				1,1,1,1,1,0,0,1,1,1,1,1,
				1,0,0,0,0,0,0,0,0,0,0,1,
				1,0,0,0,0,0,0,0,0,0,0,0,
				1,0,0,0,0,0,0,0,0,0,0,0,
				1,0,0,0,0,0,0,0,0,0,0,0,
				1,0,0,0,0,0,0,0,0,0,0,0,
				1,0,0,0,0,0,0,0,0,0,0,1,
				1,1,1,1,1,1,1,1,1,1,1,1
			},
			{
				1,1,1,1,1,1,1,1,1,1,1,1,
				1,0,0,0,0,0,0,0,0,0,0,1,
				1,0,0,0,0,0,0,0,0,0,0,1,
				1,0,0,0,0,0,0,0,0,0,0,1,
				1,0,0,0,0,0,0,0,0,0,0,1,
				1,0,0,0,0,0,0,0,0,0,0,1,
				1,0,0,0,0,0,0,0,0,0,0,1,
				1,1,1,1,1,0,0,1,1,1,1,1
			},
			{
				1,1,1,1,1,0,0,1,1,1,1,1,
				1,0,0,0,0,0,0,0,0,0,0,1,
				1,0,0,0,0,0,0,0,0,0,0,1,
				1,0,0,0,0,0,0,0,0,0,0,1,
				1,0,0,0,0,0,0,0,0,0,0,1,
				1,0,0,0,0,0,0,0,0,0,0,1,
				1,0,0,0,0,0,0,0,0,0,0,1,
				1,1,1,1,1,0,0,1,1,1,1,1
			},
			{
				1,1,1,1,1,1,1,1,1,1,1,1,
				1,0,0,0,0,0,0,0,0,0,0,1,
				1,0,0,0,0,0,0,0,0,0,0,0,
				1,0,0,0,0,0,0,0,0,0,0,0,
				1,0,0,0,0,0,0,0,0,0,0,0,
				1,0,0,0,0,0,0,0,0,0,0,0,
				1,0,0,0,0,0,0,0,0,0,0,1,
				1,1,1,1,1,0,0,1,1,1,1,1
			},
			{
				1,1,1,1,1,0,0,1,1,1,1,1,
				1,0,0,0,0,0,0,0,0,0,0,1,
				1,0,0,0,0,0,0,0,0,0,0,0,
				1,0,0,0,0,0,0,0,0,0,0,0,
				1,0,0,0,0,0,0,0,0,0,0,0,
				1,0,0,0,0,0,0,0,0,0,0,0,
				1,0,0,0,0,0,0,0,0,0,0,1,
				1,1,1,1,1,0,0,1,1,1,1,1
			},
			{
				1,1,1,1,1,1,1,1,1,1,1,1,
				1,0,0,0,0,0,0,0,0,0,0,1,
				0,0,0,0,0,0,0,0,0,0,0,1,
				0,0,0,0,0,0,0,0,0,0,0,1,
				0,0,0,0,0,0,0,0,0,0,0,1,
				0,0,0,0,0,0,0,0,0,0,0,1,
				1,0,0,0,0,0,0,0,0,0,0,1,
				1,1,1,1,1,1,1,1,1,1,1,1
			},
			{
				1,1,1,1,1,0,0,1,1,1,1,1,
				1,0,0,0,0,0,0,0,0,0,0,1,
				0,0,0,0,0,0,0,0,0,0,0,1,
				0,0,0,0,0,0,0,0,0,0,0,1,
				0,0,0,0,0,0,0,0,0,0,0,1,
				0,0,0,0,0,0,0,0,0,0,0,1,
				1,0,0,0,0,0,0,0,0,0,0,1,
				1,1,1,1,1,1,1,1,1,1,1,1
			},
			{
				1,1,1,1,1,1,1,1,1,1,1,1,
				1,0,0,0,0,0,0,0,0,0,0,1,
				0,0,0,0,0,0,0,0,0,0,0,0,
				0,0,0,0,0,0,0,0,0,0,0,0,
				0,0,0,0,0,0,0,0,0,0,0,0,
				0,0,0,0,0,0,0,0,0,0,0,0,
				1,0,0,0,0,0,0,0,0,0,0,1,
				1,1,1,1,1,1,1,1,1,1,1,1
			},
			{
				1,1,1,1,1,0,0,1,1,1,1,1,
				1,0,0,0,0,0,0,0,0,0,0,1,
				0,0,0,0,0,0,0,0,0,0,0,0,
				0,0,0,0,0,0,0,0,0,0,0,0,
				0,0,0,0,0,0,0,0,0,0,0,0,
				0,0,0,0,0,0,0,0,0,0,0,0,
				1,0,0,0,0,0,0,0,0,0,0,1,
				1,1,1,1,1,1,1,1,1,1,1,1
			},
			{
				1,1,1,1,1,1,1,1,1,1,1,1,
				1,0,0,0,0,0,0,0,0,0,0,1,
				0,0,0,0,0,0,0,0,0,0,0,1,
				0,0,0,0,0,0,0,0,0,0,0,1,
				0,0,0,0,0,0,0,0,0,0,0,1,
				0,0,0,0,0,0,0,0,0,0,0,1,
				1,0,0,0,0,0,0,0,0,0,0,1,
				1,1,1,1,1,0,0,1,1,1,1,1
			},
			{
				1,1,1,1,1,0,0,1,1,1,1,1,
				1,0,0,0,0,0,0,0,0,0,0,1,
				0,0,0,0,0,0,0,0,0,0,0,1,
				0,0,0,0,0,0,0,0,0,0,0,1,
				0,0,0,0,0,0,0,0,0,0,0,1,
				0,0,0,0,0,0,0,0,0,0,0,1,
				1,0,0,0,0,0,0,0,0,0,0,1,
				1,1,1,1,1,0,0,1,1,1,1,1
			},
			{
				1,1,1,1,1,1,1,1,1,1,1,1,
				1,0,0,0,0,0,0,0,0,0,0,1,
				0,0,0,0,0,0,0,0,0,0,0,0,
				0,0,0,0,0,0,0,0,0,0,0,0,
				0,0,0,0,0,0,0,0,0,0,0,0,
				0,0,0,0,0,0,0,0,0,0,0,0,
				1,0,0,0,0,0,0,0,0,0,0,1,
				1,1,1,1,1,0,0,1,1,1,1,1
			},
			{
				1,1,1,1,1,0,0,1,1,1,1,1,
				1,0,0,0,0,0,0,0,0,0,0,1,
				0,0,0,0,0,0,0,0,0,0,0,0,
				0,0,0,0,0,0,0,0,0,0,0,0,
				0,0,0,0,0,0,0,0,0,0,0,0,
				0,0,0,0,0,0,0,0,0,0,0,0,
				1,0,0,0,0,0,0,0,0,0,0,1,
				1,1,1,1,1,0,0,1,1,1,1,1
			}
		};
		for(int y=0; y<BH; y++) {
			for(int x=0; x<BW; x++) {
				TileType tt = (blocks[type,x+y*BW] == 1) ? TileType.WALL : TileType.NONE;
				Place(lvl, bx, by, x, y, tt);
			}
		}
	}

	void FillBlock(Level lvl, int bx, int by, int type) {
		const int NUM = 4;
		int[,] fill = new int[NUM,BH*BW]
		{
			{
				1,1,1,1,1,1,1,1,1,1,1,1,
				1,0,0,0,0,0,0,0,0,0,0,1,
				1,0,0,0,0,0,0,0,0,0,0,1,
				1,0,0,0,0,0,0,0,0,0,0,1,
				1,0,0,0,0,0,0,0,0,0,0,1,
				1,0,0,0,0,0,0,0,0,0,0,1,
				1,0,0,0,0,0,0,0,0,0,0,1,
				1,1,1,1,1,1,1,1,1,1,1,1
			},
			{
				1,1,1,1,1,1,1,1,1,1,1,1,
				1,0,0,0,0,0,0,0,0,0,0,1,
				1,0,0,0,0,0,0,0,0,0,0,1,
				1,0,0,0,1,1,1,1,0,0,0,1,
				1,0,0,0,1,1,1,1,0,0,0,1,
				1,0,0,0,0,0,0,0,0,0,0,1,
				1,0,0,0,0,0,0,0,0,0,0,1,
				1,1,1,1,1,1,1,1,1,1,1,1
			},
			{
				1,1,1,1,1,1,1,1,1,1,1,1,
				1,0,0,0,0,0,0,0,0,0,0,1,
				1,0,0,0,0,0,0,0,0,0,0,1,
				1,0,0,0,1,1,1,0,0,0,0,1,
				1,0,0,0,1,1,1,0,0,0,0,1,
				1,0,0,0,1,1,1,1,1,0,0,1,
				1,0,0,0,1,1,1,1,1,0,0,1,
				1,1,1,1,1,1,1,1,1,1,1,1
			},
			{
				1,1,1,1,1,1,1,1,1,1,1,1,
				1,0,0,1,1,0,0,0,0,0,0,1,
				1,0,0,1,1,0,0,0,0,0,0,1,
				1,0,0,1,1,1,1,1,0,0,0,1,
				1,0,0,1,1,1,1,1,0,0,0,1,
				1,0,0,0,0,0,0,0,0,0,0,1,
				1,0,0,0,0,0,0,0,0,0,0,1,
				1,1,1,1,1,1,1,1,1,1,1,1
			}
		};
		// select filling
		int fi = Random.Range(0, NUM);
		// place filling
		for(int y=1; y<BH-1; y++) {
			for(int x=1; x<BW-1; x++) {
				TileType tt = (fill[fi,x+y*BW] == 1) ? TileType.WALL : TileType.NONE;
				Place(lvl, bx, by, x, y, tt);
			}
		}
		// assert room connectivity
		if((type & 1) > 0) {
			// assert top access => delete y=1 and y=2
			SetBlock(lvl, bx, by, 1,BW-1, 1,3, TileType.NONE);
		}
		if((type & 2) > 0) {
			// assert right access => delete x=9 and x=10
			SetBlock(lvl, bx, by, 9,11, 1,BH-1, TileType.NONE);
		}
		if((type & 4) > 0) {
			// assert bottom access => delete y=5 and y=6
			SetBlock(lvl, bx, by, 1,BW-1, 5,7, TileType.NONE);
		}
		if((type & 8) > 0) {
			// assert left access => delete x=1 and x=2
			SetBlock(lvl, bx, by, 1,3, 1,BH-1, TileType.NONE);
		}
	}

	void SetBlock(Level lvl, int bx, int by, int x1, int x2, int y1, int y2, TileType tt) {
		for(int y=y1; y<y2; y++) {
			for(int x=x1; x<x2; x++) {
				Place(lvl, bx, by, x, y, tt);
			}
		}
	}
	
	void Place(Level lvl, int bx, int by, int x, int y, TileType tt) {
		lvl.tiles[BH*NH-1-(by*BH+y),bx*BW+x] = tt;
	}
	
	int BlockARow(int[,] blocks, int x0, int y, bool gotomax) {
		bool goRight = (x0 < NW/2);
		// gamble for mandatory drop down
		int xdrop, x1, x2;
		if(goRight) {
			xdrop = gotomax ? NW - 1 : Random.Range(x0, NW);
			x1 = x0;
			x2 = xdrop;
		}
		else {
			xdrop = gotomax ? 0 : Random.Range(0, x0+1);
			x1 = xdrop;
			x2 = x0;
		}
		for(int x=x1; x<=x2; x++) {
			int v = blocks[y,x];
			if(x < x2)
				v += 2;
			if(x-1 >= x1)
				v += 8;
			if((y < NH-1 && x == xdrop) || (Random.value < DROP_PROP)) {
				v += 4;
			}
			blocks[y,x] = v;
		}
		if(y > 0) {
			for(int x=0; x<NW; x++) {
				if((blocks[y-1,x] & 4) > 0) {
					blocks[y,x] += 1;
				}
				else {
					if(Random.value < DROP_PROP) {
						blocks[y,x] += 1;
						blocks[y-1,x] += 4;
					}
				}
			}
		}
		return xdrop;
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
		int x0y0 = x0;
		for(int y=0; y<NH; y++) {
			x0 = BlockARow(blocks, x0, y, y==1);
		}

		// create blocks
		Level lvl = new Level(NW*BW,NH*BH);
		for(int y=0; y<NH; y++) {
			for(int x=0; x<NW; x++) {
				CreateBlock(lvl, x, y, blocks[y,x]);
				if(!(y == NH-1 && x == x0) && !(y == 0 && x == x0y0)) {
					// do not fill start and goal
					FillBlock(lvl, x, y, blocks[y,x]);
				}
			}
		}

		// place player
		Place(lvl, x0y0,0, 3,5, TileType.PLAYER);

		// place machine
		Place(lvl, x0,NH-1, 6,5, TileType.MACHINE);

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
