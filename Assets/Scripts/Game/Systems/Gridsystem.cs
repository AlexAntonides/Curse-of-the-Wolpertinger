using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Gridsystem : MonoBehaviour {

    public Material tileMaterial;

    private static List<Gridtile> gridTiles = new List<Gridtile>();

    private Gridmap _gridClass;
    private int[,,] _gridMap;
    
    void Awake()
    {
        initializeClasses();
        initializeGridmap();
    }

    void initializeClasses()
    {
        _gridClass = transform.GetComponent<Gridmap>();
        _gridMap = _gridClass.gridMap;
    }

    void initializeGridmap()
    {
        int lengthX = _gridMap.GetLength(0);
        for (int x = 0; x < lengthX; x++)
        {
            int lengthY = _gridMap.GetLength(1);

            for (int y = 0; y < lengthY; y++)
            {
                int lengthZ = _gridMap.GetLength(2);

                for (int z = 0; z < lengthZ; z++)
                {
                    Vector3 location = new Vector3(x, y, z);
                    int type = _gridMap[x, y, z];
                    spawnTile(location, type);
                }
            }
        }
    }

    void spawnTile(Vector3 location, int type)
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.parent = this.transform;
        cube.transform.name = Constants.NAME_TILE;

        Gridtile tileComp = cube.AddComponent<Gridtile>();
        tileComp.type = (Gridtile.TileType)type;
        gridTiles.Add(tileComp);

        cube.GetComponent<Renderer>().material = tileMaterial;

        cube.transform.position = location;
    }

    public void ShowAllTiles(int type = 1)
    {
        foreach (Gridtile tile in gridTiles)
        {
            if ((int)tile.type == type)
            {
                tile.Switch(true);
            }
        }
    }

    public void HideAllTiles(int type = 1)
    {
        foreach (Gridtile tile in gridTiles)
        {
            tile.Switch(false);
        }
    }

    public static void DisableAllRays()
    {
        for (int i = 0; i < gridTiles.Count; i++)
        {
            gridTiles[i].gameObject.layer = 2;
        }
    }

    public static void EnableAllRays()
    {
        for (int i = 0; i < gridTiles.Count; i++)
        {
            gridTiles[i].gameObject.layer = 0;
        }
    }

    public static void ResetAllGridTiles()
    {
        for (int tileCount = 0; tileCount < gridTiles.Count; tileCount++)
        {
            gridTiles[tileCount].f = 0;
            gridTiles[tileCount].g = 0;
            gridTiles[tileCount].h = 0;
            gridTiles[tileCount].closed = false;
            gridTiles[tileCount].open = false;
            gridTiles[tileCount].parent = null;
        }
    }

    public static Gridtile getGridTile(Vector3 position)
    {
        Gridtile result = null;

        for (int tileCount = 0; tileCount < gridTiles.Count; tileCount++)
        {
            if (gridTiles[tileCount].transform.position == new Vector3(position.x, gridTiles[tileCount].transform.position.y, position.z))
            {
                result = gridTiles[tileCount];
            }
        }

        return result;
    }

    public static List<Gridtile> GetGridTiles()
    {
        return gridTiles;
    }
}
