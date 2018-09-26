using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;



public class SpawnEnemies : MonoBehaviour
{
    public Tilemap map;

    public Dictionary<string, GameObject> characters;

    public int numberToSpawn;



    void Start()
    {
        var tiles = map.GetTilesBlock(map.cellBounds).Where(t => t != null)
            .OrderBy(x => Random.value).Take(numberToSpawn);



        foreach (TileBase tile in tiles)
        {
            SpawnCharacter(characters[tile.name]);
        }
    }



    public void SpawnCharacter(GameObject a)
    {

    }
}
