using System.Linq;
using UnityEngine;



public class SpawnEnemies : MonoBehaviour
{
    public int numberToSpawn;



    private DungeonGameManager dgm;



    void Start()
    {
        dgm = GameObject.Find("DungeonGameManager").GetComponent<DungeonGameManager>();

        foreach (int i in RandomHelper.RandomRangeNoRepeat((byte)transform.childCount, (byte)numberToSpawn))
        {
            Transform child = transform.GetChild(i);
            SpawnCharacter(dgm.Enemies.Shuffle().First(), child);

            DungeonGameManager.NumberOfEnemies++;
        }
    }



    public void SpawnCharacter(GameObject spawnObject, Transform spawnTransform)
    {
        if (spawnObject != null)
            Instantiate(spawnObject, spawnTransform.position, Quaternion.identity, transform);
    }
}
