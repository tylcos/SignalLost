using UnityEngine;



public class SpawnEnemies : MonoBehaviour
{
    public int numberToSpawn;



    void Start()
    {        
        foreach (int i in RandomHelper.RandomRangeNoRepeat((byte)transform.childCount, (byte)numberToSpawn))
        {
            Transform child = transform.GetChild(i);
            SpawnCharacter(child.GetComponent<SpawnPoint>().spawnCharacter, child);
        }
    }



    public void SpawnCharacter(GameObject spawnObject, Transform spawnTransform)
    {
        if (spawnObject != null)
            Instantiate(spawnObject, spawnTransform.position, Quaternion.identity, transform);
    }
}
