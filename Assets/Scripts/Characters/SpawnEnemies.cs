using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;



public class SpawnEnemies : MonoBehaviour
{
    public Tilemap map;

    public GameObject[] characters;

    public int numberToSpawn;



    private Dictionary<string, int> nameToIndex;



    void Start()
    {
        for (int i = 0; i < characters.Length; i++)
            nameToIndex.Add(characters[i].name, i);

        

        var tiles = map.GetTilesBlock(map.cellBounds).Where(t => t != null);



        TileBase[] differentTiles = new TileBase[characters.Length];
        int numberOfDifferentTiles = map.GetUsedTilesNonAlloc(differentTiles);

        for (int i = 0; i < numberOfDifferentTiles; i++)
        {
            var spawnTiles = tiles.Where(x => x.name == differentTiles[i].name).OrderBy(x => Random.value).Take(numberToSpawn);

            foreach (Tile tile in spawnTiles)
                SpawnCharacter(nameToIndex[tile.name], tile.gameObject.transform);
        }
    }



    public void SpawnCharacter(int index, Transform transform)
    {
        Instantiate(characters[index], transform.position, transform.rotation);
    }
}
