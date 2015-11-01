using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Gridtile : MonoBehaviour {

    /* This is the class every tile has. It switches between visible or invisible when the mouse hovers over the tile. */
    public enum TileType
    {
        ENEMY = 0,
        FRIENDLY = 1,
        PATH = 2,
        FORBIDDEN = 3,

        STARTPATH = 21,
        ENDPATH = 22
    }

    public TileType type = TileType.FORBIDDEN;

    public List<Gridtile> neighbours = new List<Gridtile>();

    public float g;
    public float h;
    public float f;

    public bool closed = false;
    public bool open = false;
    public bool occupied = false;

    public Gridtile parent;

    public Material startMaterial;
    public Material movementMaterial;

    void Start()
    {
        Switch(false);
        ChangeTransparency(0.50f);
        CheckNeighbours();

        startMaterial = GetComponent<Renderer>().material;
        movementMaterial = Resources.Load("Tile_Move") as Material;

        if (type == TileType.STARTPATH)
            transform.tag = Constants.TAG_STARTTILE;
        else if (type == TileType.ENDPATH)
            transform.tag = Constants.TAG_ENDTTILE;
    }

    void OnMouseOver()
    {
        if (Gamesystem.gameState == GameState.OUT_OF_WAVE)
        {
            ChangeTransparency(1f);
        }
    }

    void OnMouseExit()
    {
        if (Gamesystem.gameState == GameState.OUT_OF_WAVE) 
        {
            ChangeTransparency(0.50f);
        }
    }

    public void ChangeTransparency(float value)
    {
        Color color = new Color(GetComponent<Renderer>().material.color.r, GetComponent<Renderer>().material.color.g, GetComponent<Renderer>().material.color.b, value);
        GetComponent<Renderer>().material.color = color;
    }

    public void Switch(bool value)
    {
        /* Enable or Disable the renderer. */
        GetComponent<Renderer>().enabled = value;
    }

    private void CheckNeighbours()
    {
        for (int x = -1; x <= 1; x++)
        {
            Vector3 pos = new Vector3(transform.position.x + x, transform.position.y, transform.position.z);

            if (transform.position != pos)
            {
                if (Gridsystem.getGridTile(pos))
                {
                    neighbours.Add(Gridsystem.getGridTile(pos));
                }
            }
        }

        for (int z = -1; z <= 1; z++)
        {
            Vector3 pos = new Vector3(transform.position.x, transform.position.y, transform.position.z + z);

            if (transform.position != pos)
            {
                if (Gridsystem.getGridTile(pos))
                {
                    neighbours.Add(Gridsystem.getGridTile(pos));
                }
            }
        }
    }
}
