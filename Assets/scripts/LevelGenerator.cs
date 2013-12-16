using UnityEngine;
using System.Collections;

public class LevelGenerator : MonoBehaviour
{

	public UnityEngine.GameObject tile;
	public UnityEngine.GameObject tileNC;
//	public UnityEngine.GameObject player;
	public UnityEngine.GameObject agent;
	public UnityEngine.GameObject drone;
	public UnityEngine.GameObject machine;
	public UnityEngine.GameObject portal;

	void Awake() {
		Globals.lvlGen = this;
	}

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
			//return (GameObject)Instantiate(player);
			if(Globals.player != null) {
				Globals.player.transform.position = new Vector3(x,y,0);
			}
			return null;
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
			go.transform.localPosition = new Vector2(0.5f,1.0f);
			return go;
		}
		else if(tt == TileType.PORTAL_0) {
			GameObject go = (GameObject)Instantiate(portal);
			go.transform.localPosition = new Vector2(2.0f,2.0f);
			go.GetComponent<Portal>().level = 0;
			return go;
		}
		else if(tt == TileType.PORTAL_1) {
			GameObject go = (GameObject)Instantiate(portal);
			go.transform.localPosition = new Vector2(2.0f,2.0f);
			go.GetComponent<Portal>().level = 1;
			return go;
		}
		else if(tt == TileType.PORTAL_2) {
			GameObject go = (GameObject)Instantiate(portal);
			go.transform.localPosition = new Vector2(2.0f,2.0f);
			go.GetComponent<Portal>().level = 2;
			return go;
		}
		else if(tt == TileType.PORTAL_3) {
			GameObject go = (GameObject)Instantiate(portal);
			go.transform.localPosition = new Vector2(2.0f,2.0f);
			go.GetComponent<Portal>().level = 3;
			return go;
		}
		else if(tt == TileType.PORTAL_4) {
			GameObject go = (GameObject)Instantiate(portal);
			go.transform.localPosition = new Vector2(2.0f,2.0f);
			go.GetComponent<Portal>().level = 4;
			return go;
		}
		else if(tt == TileType.PORTAL_5) {
			GameObject go = (GameObject)Instantiate(portal);
			go.transform.localPosition = new Vector2(2.0f,2.0f);
			go.GetComponent<Portal>().level = 5;
			return go;
		}
		else if(tt == TileType.PORTAL_6) {
			GameObject go = (GameObject)Instantiate(portal);
			go.transform.localPosition = new Vector2(2.0f,2.0f);
			go.GetComponent<Portal>().level = 6;
			return go;
		}
		else if(tt == TileType.PORTAL_7) {
			GameObject go = (GameObject)Instantiate(portal);
			go.transform.localPosition = new Vector2(2.0f,2.0f);
			go.GetComponent<Portal>().level = 7;
			return go;
		}
		else {
			return null;
		}
	}

	const int BW = 12;
	const int BH = 8;

	public struct Parameters
	{
		public int nw, nh;
		public float droneProp;
		public float agentProp;

		public static Parameters Defaults() {
			Parameters p = new Parameters();
			p.nw = 4;
			p.nh = 3;
			p.agentProp = 0.15f;
			p.droneProp = 0.01f;
			return p;
		}
	}
	
	Parameters parameters;
	
	int NW { get { return parameters.nw; } }
	int NH { get { return parameters.nh; } }

	const float DROP_PROP = 0.2f;
	const float FROP_PROP = 0.1f;

	float AGENT_PROP { get { return parameters.agentProp; } }
	float DRONE_PROP { get { return parameters.droneProp; } }

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
		const int NUM = 11;
		int[] fill_flip = new int[NUM] {
			0,0,0,0,0,1,1,1,1,1,1
		};
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
				1,0,0,0,0,1,1,0,0,0,0,1,
				1,0,0,0,0,1,1,0,0,0,0,1,
				1,0,0,0,0,0,0,0,0,0,0,1,
				1,0,0,0,0,0,0,0,0,0,0,1,
				1,1,1,1,1,1,1,1,1,1,1,1
			},
			{
				1,1,1,1,1,1,1,1,1,1,1,1,
				1,0,0,0,0,0,0,0,0,0,0,1,
				1,0,0,0,0,0,0,0,0,0,0,1,
				1,0,0,0,0,0,0,0,0,0,0,1,
				1,0,0,0,0,1,1,0,0,0,0,1,
				1,0,0,0,1,1,1,1,0,0,0,1,
				1,0,0,1,1,1,1,1,1,0,0,1,
				1,1,1,1,1,1,1,1,1,1,1,1
			},
			{
				1,1,1,1,1,1,1,1,1,1,1,1,
				1,0,0,0,0,0,0,0,0,0,0,1,
				1,0,0,1,1,1,1,1,1,0,0,1,
				1,0,1,1,0,0,0,0,1,1,0,1,
				1,0,1,1,0,0,0,0,1,1,0,1,
				1,0,0,1,1,1,1,1,1,0,0,1,
				1,0,0,0,0,0,0,0,0,0,0,1,
				1,1,1,1,1,1,1,1,1,1,1,1
			},
			{
				1,1,1,1,1,1,1,1,1,1,1,1,
				1,0,0,0,0,0,0,0,0,0,0,1,
				1,0,0,1,0,0,0,0,1,0,0,1,
				1,0,1,1,0,1,1,0,1,1,0,1,
				1,0,1,1,0,1,1,0,1,1,0,1,
				1,0,0,1,0,0,0,0,1,0,0,1,
				1,0,0,0,0,0,0,0,0,0,0,1,
				1,1,1,1,1,1,1,1,1,1,1,1
			},
			{
				1,1,1,1,1,1,1,1,1,1,1,1,
				1,0,0,1,0,0,0,0,0,0,0,1,
				1,0,0,1,0,0,0,0,0,0,0,1,
				1,0,0,0,0,0,0,0,0,0,0,1,
				1,0,0,1,0,0,0,0,0,0,0,1,
				1,0,1,1,1,0,0,0,0,0,0,1,
				1,1,1,1,1,1,0,0,0,0,0,1,
				1,1,1,1,1,1,1,1,1,1,1,1
			},
			{
				1,1,1,1,1,1,1,1,1,1,1,1,
				1,0,0,0,0,0,0,0,0,0,0,1,
				1,0,0,0,0,0,0,0,0,0,0,1,
				1,0,0,0,0,0,0,1,1,1,0,1,
				1,0,0,0,0,0,0,0,0,0,0,1,
				1,0,0,0,0,0,0,0,0,0,0,1,
				1,0,0,0,0,0,0,0,0,0,0,1,
				1,1,1,1,1,1,1,1,1,1,1,1
			},
			{
				1,1,1,1,1,1,1,1,1,1,1,1,
				1,0,0,0,0,0,0,0,0,0,0,1,
				1,0,0,0,0,0,0,0,0,0,0,1,
				1,1,1,1,1,1,0,0,0,0,0,1,
				1,1,1,1,1,1,0,0,0,0,0,1,
				1,0,0,0,0,0,0,0,0,0,0,1,
				1,0,0,0,0,0,0,0,0,0,0,1,
				1,1,1,1,1,1,1,1,1,1,1,1
			},
			{
				1,1,1,1,1,1,1,1,1,1,1,1,
				1,0,0,0,0,0,0,0,0,0,0,1,
				1,0,0,0,0,0,0,0,0,0,0,1,
				1,0,0,0,1,1,0,0,0,0,0,1,
				1,0,0,0,1,1,1,0,0,0,0,1,
				1,0,0,0,1,1,1,1,0,0,0,1,
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
			},
			{
				1,1,1,1,1,1,1,1,1,1,1,1,
				1,0,0,0,0,0,0,0,0,0,0,1,
				1,0,0,0,0,0,0,0,0,0,0,1,
				1,0,0,1,1,1,1,1,0,0,0,1,
				1,0,0,1,1,1,1,1,0,0,0,1,
				1,0,0,1,1,0,0,0,0,0,0,1,
				1,0,0,1,1,0,0,0,0,0,0,1,
				1,1,1,1,1,1,1,1,1,1,1,1
			}
		};
		// select filling
		int fi = Random.Range(0, NUM);
		// do flip?
		bool fiFlip = (fill_flip[fi] != 0 && Random.Range(0,2) == 1);
		// place filling
		for(int y=1; y<BH-1; y++) {
			for(int x=1; x<BW-1; x++) {
				int v = (fiFlip ? fill[fi,(BW-1-x)+y*BW] : fill[fi,x+y*BW]);
				TileType tt = (v == 1 ? TileType.WALL : TileType.NONE);
				Place(lvl, bx, by, x, y, tt);
			}
		}
		// assert room connectivity
		if((type & 1) > 0) {
			// assert top access => delete y=1 and y=2
//			FillRegion(lvl, bx, by, 1,BW-1, 1,3, TileType.NONE);
			FillRegion(lvl, bx, by, 1,BW-1, 1,2, TileType.NONE);
		}
		if((type & 2) > 0) {
			// assert right access => delete x=9 and x=10
//			FillRegion(lvl, bx, by, 9,11, 1,BH-1, TileType.NONE);
			FillRegion(lvl, bx, by, 10,11, 1,BH-1, TileType.NONE);
		}
		if((type & 4) > 0) {
			// assert bottom access => delete y=5 and y=6
//			FillRegion(lvl, bx, by, 1,BW-1, 5,7, TileType.NONE);
			FillRegion(lvl, bx, by, 1,BW-1, 6,7, TileType.NONE);
		}
		if((type & 8) > 0) {
			// assert left access => delete x=1 and x=2
//			FillRegion(lvl, bx, by, 1,3, 1,BH-1, TileType.NONE);
			FillRegion(lvl, bx, by, 1,2, 1,BH-1, TileType.NONE);
		}
	}

	void FillRegion(Level lvl, int bx, int by, int x1, int x2, int y1, int y2, TileType tt) {
		for(int y=y1; y<y2; y++) {
			for(int x=x1; x<x2; x++) {
				Place(lvl, bx, by, x, y, tt);
			}
		}
	}
	
	int BlockARow(int[,] blocks, int x0, int y, bool gotomax) {
		bool goRight = (x0 < NW/2);
		// gamble for mandatory drop down
		int xdrop, x1, x2;
		if(goRight) {
			xdrop = gotomax ? NW - 1 : Random.Range(y == 0 ? x0+1 : x0, NW);
			x1 = x0;
			x2 = xdrop;
		}
		else {
			xdrop = gotomax ? 0 : Random.Range(y == 0 ? 1 : 0, x0);
			x1 = xdrop;
			x2 = x0;
		}
		for(int x=x1; x<=x2; x++) {
			int v = blocks[y,x];
			if(x < x2)
				v += 2;
			if(x-1 >= x1)
				v += 8;
			if(y < NH-1 && (x == xdrop || Random.value < DROP_PROP)) {
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
					if(Random.value < FROP_PROP) {
						blocks[y,x] += 1;
						blocks[y-1,x] += 4;
					}
				}
			}
		}
		return xdrop;
	}

	void Place(Level lvl, int bx, int by, int x, int y, TileType tt) {
		lvl.tiles[BH*NH-1-(by*BH+y),bx*BW+x] = tt;
	}
	
	TileType LvlGet(Level lvl, int bx, int by, int x, int y) {
		return lvl.tiles[BH*NH-1-(by*BH+y),bx*BW+x];
	}

	bool IsFree(Level lvl, int bx, int by, int x, int y) {
		if(0 <= x && x < BW && 0 <= y && y < BH) {
			return LvlGet(lvl, bx, by, x, y) == TileType.NONE;
		}
		else {
			return false;
		}
	}
	
	bool IsSupport(Level lvl, int bx, int by, int x, int y) {
		if(0 <= x && x < BW && 0 <= y && y < BH) {
			return LvlGet(lvl, bx, by, x, y) == TileType.WALL;
		}
		else {
			return false;
		}
	}
	
	void PopulateBlock(Level lvl, int bx, int by) {
		// place agents
		// candidate field: filled below, free above
		for(int y=0; y<BH; y++) {
			for(int x=0; x<BW; x++) {
				// check if free
				if(IsFree(lvl,bx,by,x,y)
				   && IsFree(lvl,bx,by,x,y-1)
				   && IsSupport(lvl,bx,by,x,y+1)
				){
					// gamble
					if(Random.value < AGENT_PROP)
						Place(lvl, bx, by, x, y, TileType.AGENT);
				}
			}
		}
		// place drones
		// candidate field: minimum height, free around
		for(int y=0; y<BH/2; y++) {
			for(int x=0; x<BW; x++) {
				// check if free
				if(   IsFree(lvl,bx,by,x  ,y-1)
				   && IsFree(lvl,bx,by,x  ,y  )
				   && IsFree(lvl,bx,by,x  ,y+1)
				   && IsFree(lvl,bx,by,x+1,y-1)
				   && IsFree(lvl,bx,by,x+1,y  )
				   && IsFree(lvl,bx,by,x+1,y+1)
				   && IsFree(lvl,bx,by,x-1,y-1)
				   && IsFree(lvl,bx,by,x-1,y  )
				   && IsFree(lvl,bx,by,x-1,y+1)
				   ){
					// gamble
					if(Random.value < DROP_PROP)
						Place(lvl, bx, by, x, y, TileType.DRONE);
				}
			}
		}
	}

	public Level CreateRandomLevel(Parameters par) {
		parameters = par;
		// init blocks
		int[,] blocks = new int[NH,NW];
		for(int y=0; y<NH; y++) {
			for(int x=0; x<NW; x++) {
				blocks[y,x] = 0;
			}
		}

		// choose random block in first row
		int x0 = (NH == 1 ? (Random.Range(0,2) == 0 ? 0 : NW-1) : Random.Range(0, NW));
		int x0y0 = x0;
		for(int y=0; y<NH; y++) {
			x0 = BlockARow(blocks, x0, y, y != 0 && y != NH-1);
		}
		// do not drop in portal room
		if((blocks[0,x0y0] & 4) > 0) {
			blocks[0,x0y0] -= 4;
			if(NH > 1) {
				blocks[1,x0y0] -= 1;
			}
		}

		// create blocks
		Level lvl = new Level(NW*BW,NH*BH);
		for(int y=0; y<NH; y++) {
			for(int x=0; x<NW; x++) {
				CreateBlock(lvl, x, y, blocks[y,x]);
				if(!(y == NH-1 && x == x0) && !(y == 0 && x == x0y0)) {
					// do not fill start and goal
					FillBlock(lvl, x, y, blocks[y,x]);
					PopulateBlock(lvl, x, y);
				}
			}
		}

		// place player
		Place(lvl, x0y0,0, 3,6, TileType.PLAYER);

		// place portal
		Place(lvl, x0y0,0, 4,6, TileType.PORTAL_0);
		
		// place machine
		Place(lvl, x0,NH-1, 8,6, TileType.MACHINE);

//		// place portal
//		Place(lvl, x0,NH-1, 3,6, TileType.PORTAL_0);
		
		return lvl;
	}
	
	public GameObject CreateGameobjects(Level level) {
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
					go.transform.localPosition += new Vector3(x, y);
				}
			}
		}
		return go_parent;
	}

	static int Color32ToInt(Color32 col) {
		return col.r + 256*col.g + 256*256*col.b;
	}
	
	static int col_white = Color32ToInt(new Color32(255,255,255,255));
	static int col_black = Color32ToInt(new Color32(0,0,0,255));
	static int col_red = Color32ToInt(new Color32(255,0,0,255));
	static int col_reddark = Color32ToInt(new Color32(128,0,0,255));
	static int col_green = Color32ToInt(new Color32(0,255,0,255));
	static int col_blue_0 = Color32ToInt(new Color32(0,0,255,255));
	static int col_blue_1 = Color32ToInt(new Color32(1,0,255,255));
	static int col_blue_2 = Color32ToInt(new Color32(2,0,255,255));
	static int col_blue_3 = Color32ToInt(new Color32(3,0,255,255));
	static int col_blue_4 = Color32ToInt(new Color32(4,0,255,255));
	static int col_blue_5 = Color32ToInt(new Color32(5,0,255,255));
	static int col_blue_6 = Color32ToInt(new Color32(6,0,255,255));
	static int col_blue_7 = Color32ToInt(new Color32(7,0,255,255));

	static TileType Color32ToValue(Color32 col) {
		int ic = Color32ToInt(col);
		if(ic == col_white) return TileType.NONE;
		if(ic == col_black) return TileType.WALL;
		if(ic == col_red) return TileType.AGENT;
		if(ic == col_reddark) return TileType.DRONE;
		if(ic == col_green) return TileType.PLAYER;
		if(ic == col_blue_0) return TileType.PORTAL_0;
		if(ic == col_blue_1) return TileType.PORTAL_1;
		if(ic == col_blue_2) return TileType.PORTAL_2;
		if(ic == col_blue_3) return TileType.PORTAL_3;
		if(ic == col_blue_4) return TileType.PORTAL_4;
		if(ic == col_blue_5) return TileType.PORTAL_5;
		if(ic == col_blue_6) return TileType.PORTAL_6;
		if(ic == col_blue_7) return TileType.PORTAL_7;
		return TileType.UNKNOWN;
	}
	
	public Level CreateLevelFromTex(Texture2D tex) {
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
		return level;
	}


}
