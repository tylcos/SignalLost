using System.Linq;
using System.Collections.Generic;
using UnityEngine;



public class SpawnEnemies : MonoBehaviour
{
    void Start()
    {
        /*
        List<TilePos> tiles = TilePos.GetAllTiles(map);



        TileBase[] differentTiles = new TileBase[characters.Length];
        map.GetUsedTilesNonAlloc(differentTiles);

        for (int i = 0; i < differentTiles.Length; i++)
        {
            Stopwatch sss = Stopwatch.StartNew();
            var spawnTiles = tiles.Where(x => x.Tile.name == differentTiles[i].name).OrderBy(x => Random.value).Take(numberToSpawn);
            UnityEngine.Debug.Log("Inner   " + sss.Elapsed);
            foreach (TilePos tile in spawnTiles)
                SpawnCharacter(nameToIndex[tile.Tile.name], map.CellToWorld(tile.Pos));

            UnityEngine.Debug.Log("Inner   " + sss.Elapsed);
        } */



        foreach (Transform child in transform)
        {
            CharacterSpawner sp = child.GetComponent<CharacterSpawner>();

            var randomList = RandomRangeNoRepeat(0, child.childCount, sp.numberToSpawn);
            foreach (int i in randomList)
                SpawnCharacter(sp.spawnCharacter, child.GetChild(i), transform.parent); // Spawn character with the room as the parent
        }
    }



    public void SpawnCharacter(GameObject spawnObject, Transform spawnTransform, Transform parent)
    {
        Instantiate(spawnObject, spawnTransform.position, spawnTransform.rotation, parent);
    }



    public IEnumerable<int> RandomRangeNoRepeat(int start, int length, int number)
    {
        if (number > length)
            throw new System.ArgumentOutOfRangeException("Number to return must not be greater than the length of the range.");
        else if (number == 0)
            return Enumerable.Empty<int>();



        int[] intList = Enumerable.Range(start, length).ToArray();
        
        for (int i = 0; i < number; i++)
        {
            int random = Random.Range(i, length);

            int swap = intList[i];
            intList[i] = intList[random];
            intList[random] = swap;
        }

        return intList.Take(number);
    }


    /*
    public Dictionary<string, int> ParseNumberToSpawn()
    {
        Dictionary<string, int> characters = new Dictionary<string, int>();
        
        foreach (string line in charactersToSpawn.Split('\n'))
        {
            string[] splitLine = line.Split(' ');
            int charactersToSpawn;

            bool validParse = int.TryParse(splitLine[1], out charactersToSpawn);
            if (!validParse || splitLine.Length > 2)
                throw new System.ArgumentException("Invalid room spawn arguments.");

            characters.Add(splitLine[0], charactersToSpawn);
        }

        return characters;
    } */
}


/*
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

        foreach (Vector3Int pos in map.cellBounds.allPositionsWithin)
        {
            TileBase tile = map.GetTile(pos);

            if (tile != null)
                tiles.Add(new TilePos(tile, pos));
        }

        return tiles;
    }
}
*/