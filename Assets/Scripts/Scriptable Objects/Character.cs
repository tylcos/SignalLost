using UnityEngine;



[CreateAssetMenu(fileName = "New Character", menuName = "Character")]
public class Character : ScriptableObject
{
    [Tooltip("The prefab for the enemy.")]
    public GameObject characterPrefab;



    [Space(20)]
    [HideInInspector]
    public float Health;
    public float MaxHealth;



    public void OnEnable()
    {
        Health = MaxHealth;
    }
}
    