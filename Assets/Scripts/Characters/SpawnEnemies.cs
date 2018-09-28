using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;



public class SpawnEnemies : MonoBehaviour
{
    public Tilemap map;

    public GameObject[] characters;

    public int numberToSpawn;



    private Dictionary<string, int> nameToIndex = new Dictionary<string, int>();



    void Start()
    {
        for (int i = 0; i < characters.Length; i++)
            nameToIndex.Add(characters[i].name, i);
        


        var tiles = TilePos.GetAllTiles(map);



        TileBase[] differentTiles = new TileBase[characters.Length];
        int numberOfDifferentTiles = map.GetUsedTilesNonAlloc(differentTiles);
            
        for (int i = 0; i < numberOfDifferentTiles; i++)
        {
            var spawnTiles = tiles.Where(x => x.Tile.name == differentTiles[i].name).OrderBy(x => Random.value).Take(numberToSpawn);

            foreach (TilePos tile in spawnTiles)
                SpawnCharacter(nameToIndex[tile.Tile.name], map.CellToWorld(tile.Pos));
        }
    }



    public void SpawnCharacter(int index, Vector3 pos)
    {
        Instantiate(characters[index], pos, transform.rotation);
    }
}



public class TilePos
{
    public TileBase Tile;
    public Vector3Int Pos;



    public TilePos(TileBase tile, Vector3Int pos)
    {
        Tile = tile;
        Pos = pos;
    }



    public static List<TilePos> GetAllTiles(Tilemap map)
    {
        var tiles = new List<TilePos>();

        foreach (var pos in map.cellBounds.allPositionsWithin)
        {
            TileBase tile = map.GetTile(pos);

            if (tile != null)
                tiles.Add(new TilePos(tile, pos));
        }

        return tiles;
    }
}
