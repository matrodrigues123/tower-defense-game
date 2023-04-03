using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Summoner : Enemy
{
    public GameObject basicEnemy;
    protected override void DestroyEnemy()
    {
        // spawn 4 normal enemies when dead
        foreach (Vector2 neighborPos in GetEmptyNeighbors(gameObject.transform.position))
        {
            GameObject newEnemy = Instantiate(basicEnemy, neighborPos, Quaternion.identity);
        }
        Enemies.enemies.Remove(gameObject);
        Destroy(gameObject);


    }

    private List<Vector2> GetEmptyNeighbors(Vector2 summonerPos)
    {
        List<Vector2> emptyNeighbors = new List<Vector2>();
        int[,] directions = { { 1, 0 }, { -1, 0 }, { 0, 1 }, { 0, -1 } };
        for (int i = 0; i < 4; i++)
        {
            Vector2 neighborPos = summonerPos + new Vector2(directions[i, 0], directions[i, 1]);
            if (neighborPos.x >= 0 && neighborPos.x < MapGenerator.mapWidth && neighborPos.y >= 0 && neighborPos.y < MapGenerator.mapHeight)
            {
                GridCell neighborCell = MapGenerator.mapTiles[(int)neighborPos.y, (int)neighborPos.x];
                if (neighborCell.isWalkable)
                {
                    emptyNeighbors.Add(neighborPos);
                }
            }
        }
        return emptyNeighbors;
    }

}
