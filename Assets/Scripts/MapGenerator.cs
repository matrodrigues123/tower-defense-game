using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public GameObject mapTilePrefab;

    public static GridCell startTile;

    public static int mapWidth;
    public static int mapHeight;
    // color selection
    [SerializeField] Color WALKABLE_COLOR = Color.white;
    [SerializeField] Color NON_WALKABLE_COLOR = Color.black;
    

    public static GridCell[,] mapTiles;

    public void Start()
    {
        generateMap(10, 10);
    }

    private void generateMap(int h, int w)
    {
        mapHeight = h;
        mapWidth = w;
        mapTiles = new GridCell[mapHeight, mapWidth];
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                GameObject newTileObj = Instantiate(mapTilePrefab);
                newTileObj.transform.position = new Vector2(x,y); 
                GridCell newCell = new GridCell(newTileObj);
                mapTiles[y, x] = newCell;       
            }
        }
    }

    

}

public class GridCell
{
    public GameObject gameObj {set; get;}
    public bool isWalkable {set;get;}

    public GridCell (GameObject mapTile)
    {
        this.gameObj = mapTile;
        this.isWalkable = true;
    }
}

