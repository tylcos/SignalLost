using UnityEngine;



public class SpawnEnemies : MonoBehaviour
{
    public int numberToSpawn;



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

		
        
        foreach (int i in RandomHelper.RandomRangeNoRepeat(0, transform.childCount, numberToSpawn))
        {
            Transform child = transform.GetChild(i);

            SpawnCharacter(child.GetComponent<SpawnPoint>().spawnCharacter, child, transform.parent);
        }
    }



    public void SpawnCharacter(GameObject spawnObject, Transform spawnTransform, Transform parent)
    {
        Instantiate(spawnObject, spawnTransform.position, spawnTransform.rotation, parent);
    }
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
