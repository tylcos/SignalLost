using UnityEngine;



[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy")]
public class EnemyData : ScriptableObject
{
    [Tooltip("The prefab for the enemy.")]
    public GameObject enemyPrefab;
    


    [Space(20)]
    public float health;
    public float aggroRange;
}
    