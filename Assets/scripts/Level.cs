using UnityEngine;
using System.Collections;

public enum TileType
{
	NONE,
	WALL,
	PLAYER,
	AGENT,
	DRONE,
	MACHINE,
	PORTAL,
	UNKNOWN
}

public class Level
{
	public TileType[,] tiles;
	public int width, height;

	public Level(int width, int height) {
		this.width = width;
		this.height = height;
		this.tiles = new TileType[height,width];
	}

	public TileType GetTileType(int x, int y) {
		if(0 <= x && x < width && 0 <= y && y < height) {
			return tiles[y,x];
		}
		else {
			return TileType.WALL;
		}
	}
	
	public int GetWall(int x, int y) {
		if(GetTileType(x,y) == TileType.WALL) {
			return 1;
		}
		else {
			return 0;
		}
	}

	public int GetWallTileIndex(int x, int y) {
		return
			+ 1*GetWall(x,y+1)
			+ 2*GetWall(x+1,y)
			+ 4*GetWall(x,y-1)
			+ 8*GetWall(x-1,y);
	}

	public bool IsWallTileContained(int x, int y) {
		int neighboursDiag = 
			+ GetWall(x+1,y+1)
			+ GetWall(x+1,y-1)
			+ GetWall(x-1,y-1)
			+ GetWall(x-1,y+1);
		return neighboursDiag == 0 && GetWallTileIndex(x,y) == 0;
	}
	

}
