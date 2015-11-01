using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AStar : MonoBehaviour {

    private static float horizontalScore = 1f;
    private static float diagonalScore = 1.414f;

    public static List<Gridtile> Search(Gridtile start, Gridtile end)
    {
        if (start && end)
        {
            Gridsystem.ResetAllGridTiles();

            List<Gridtile> openList = new List<Gridtile>();
            List<Gridtile> closedList = new List<Gridtile>();

            Gridtile currentTile;
            Gridtile neighbour;

            float gScore;
            bool gScoreIsBest;

            openList.Add(start);
            while (openList.Count > 0)
            {
                if (openList[0] != null)
                {
                    openList.Sort(sortOnF);

                    currentTile = openList[0];

                    if (currentTile == end)
                    {
                        return getPathToTile(currentTile);
                    }

                    openList.Remove(currentTile);
                    closedList.Add(currentTile);
                    currentTile.closed = true;
                    currentTile.open = false;

                    for (int tileCount = 0; tileCount < currentTile.neighbours.Count; tileCount++)
                    {
                        neighbour = currentTile.neighbours[tileCount];

                        if (neighbour.closed && neighbour.type == Gridtile.TileType.FRIENDLY || neighbour.closed && neighbour.type == Gridtile.TileType.ENEMY || neighbour.closed && neighbour.type == Gridtile.TileType.FORBIDDEN)
                        {
                            continue;
                        }

                        if (isDiagonal(currentTile, neighbour))
                        {
                            gScore = currentTile.g + diagonalScore;
                        }
                        else
                        {
                            gScore = currentTile.g + horizontalScore;
                        }

                        gScoreIsBest = false;

                        if (!neighbour.open && !neighbour.closed && neighbour.type == Gridtile.TileType.PATH || !neighbour.open && !neighbour.closed && neighbour.type == Gridtile.TileType.ENDPATH || !neighbour.open && !neighbour.closed && neighbour.type == Gridtile.TileType.STARTPATH)
                        {
                            gScoreIsBest = true;

                            neighbour.h = heuristic(neighbour.transform.position, end.transform.position);

                            openList.Add(neighbour);
                            neighbour.open = true;
                        }
                        else if (gScore < neighbour.g)
                        {
                            gScoreIsBest = true;
                        }

                        if (gScoreIsBest)
                        {
                            neighbour.parent = currentTile;
                            neighbour.g = gScore;
                            neighbour.f = neighbour.g + neighbour.h;
                        }
                    }
                }
                else
                {
                    openList.RemoveAt(0);
                }
            }
        }

        return new List<Gridtile>();
    }

    private static bool isDiagonal(Gridtile center, Gridtile neighbour)
    {
        if (center.transform.position.x != neighbour.transform.position.x && center.transform.position.z != center.transform.position.z)
        {
            return true;
        }
        return false;
    }

    private static List<Gridtile> getPathToTile(Gridtile tile)
    {
        List<Gridtile> path = new List<Gridtile>();

        while (tile.parent)
        {
            if (!path.Contains(tile))
            {
                path.Add(tile);
                tile = tile.parent;
            }
        }

        path.Reverse();
        return path;
    }

    private static int sortOnF(Gridtile a, Gridtile b)
    {
        if (a.f > b.f || a.f == b.f && a.h > b.h)
        {
            return 1;
        }
        else
        {
            return -1;
        }
    }

    private static int heuristic(Vector3 pos0, Vector3 pos1)
    {
        // Manhattan distance.
        int d1 = Mathf.Abs((int)pos1.x - (int)pos0.x);
        int d2 = Mathf.Abs((int)pos1.z - (int)pos0.z);
        return d1 + d2;
    }
}
