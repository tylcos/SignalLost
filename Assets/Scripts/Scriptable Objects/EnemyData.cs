using UnityEngine;



[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy")]
public class EnemyData : ScriptableObject
{
    [Tooltip("The ingame name of the enemy.")]
    public new string name;
    [Tooltip("The prefab for the enemy.")]
    public GameObject enemySprite;
    


    [Space(20)]
    [Tooltip("The movement script for the enemy.")]
    public string moveScript;
}
    