using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour {

    // Ammo, health, and death updates should be performed from the player class

    struct HealthbarSettings
    {
        public float maximum;
        public float health;
        public Transform healthbarTransform;

        public void Rescale()
        {
            healthbarTransform.localScale = new Vector3(health / maximum, healthbarTransform.localScale.y, healthbarTransform.localScale.z);
        }
    }

    HealthbarSettings _healthbarSettings;
    public PlayerController player;
    private GameObject healthbar;
    private GameObject ammo;
    private GameObject deathMessage;


    private void Awake()
    {
        foreach (Transform child in transform)
        {
            switch (child.tag)
            {
                case "UI Healthbar":
                    if(healthbar != null)
                    {
                        Debug.LogError("There are multiple objects tagged as 'UI Healthbar' in the canvas!");
                    } else
                    {
                        healthbar = child.gameObject;
                        _healthbarSettings.healthbarTransform = healthbar.transform;
                    }
                    break;
                case "UI Ammo Counter":
                    if (ammo != null)
                    {
                        Debug.LogError("There are multiple objects tagged as 'UI Ammo Counter' in the canvas!");
                    }
                    else
                    {
                        ammo = child.gameObject;
                    }
                    break;
                case "UI Death Message":
                    if (deathMessage != null)
                    {
                        Debug.LogError("There are multiple objects tagged as 'UI Death Message' in the canvas!");
                    }
                    else
                    {
                        deathMessage = child.gameObject;
                    }
                    break;
                default:
                    Debug.LogError("There is an untagged object in the canvas!");
                    break;
            }
        }
    }

    public void DeathSequence()
    {
        //this should fade in death message and fade out the rest somehow
        deathMessage.SetActive(true);
    }

    public void UpdateHealthbar()
    {
        _healthbarSettings.maximum = player.MaxHitPoints;
        _healthbarSettings.health = player.CurrentHitPoints;
        _healthbarSettings.Rescale();
    }
}
