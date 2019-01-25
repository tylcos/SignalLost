using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour {

    // Ammo, health, and death updates should be performed from the player class

    struct HealthbarSettings
    {
        public float maximum;
        public float health;
        public Transform healthbarTransform;

        public void Redraw()
        {
            healthbarTransform.localScale = new Vector3(health / maximum, healthbarTransform.localScale.y, healthbarTransform.localScale.z);
        }
    }

    struct AmmoSettings
    {
        public WeaponManager weapon;
        public TMP_Text text;

        public void Redraw()
        {
            int cur = weapon.CurrentAmmo;
            int max = weapon.MaxAmmo;
            text.text = cur + " / " + max;
        }
    }

    HealthbarSettings _healthbarSettings;
    AmmoSettings _ammoSettings;
    public PlayerController player;
    [SerializeField]
    private GameObject healthbar = null;
    [SerializeField]
    private GameObject ammo = null;
    [SerializeField]
    private GameObject deathMessage = null;


    private void OnEnable()
    {
        try
        {
            _healthbarSettings.healthbarTransform = healthbar.GetComponentsInChildren<Transform>()[2];
            player.DamageTaken += OnTakeDamage;
        }
        catch (UnassignedReferenceException)
        {
            Debug.LogError("The healthbar was not assigned in the UI canvas! Disabling updates!");
        }

        try
        {
            _ammoSettings.text = ammo.gameObject.GetComponent<TMP_Text>();
            _ammoSettings.weapon = player.GetComponentInChildren<WeaponManager>();
            _ammoSettings.weapon.WeaponDataChanged += OnWeaponUpdate;
        }
        catch (UnassignedReferenceException)
        {
            Debug.LogError("The ammo counter was not assigned in the UI canvas! Disabling updates!");
        }

        if (deathMessage != null)
        {
            player.Died += OnDeath;
        }
        else
        {
            Debug.LogError("The death message was not assigned in the UI canvas! Disabling updates!");
        }

        UpdateAmmo();
        UpdateHealthbar();
    }

    private void DeathSequence()
    {
        //this should fade in death message and fade out the rest somehow
        deathMessage.SetActive(true);
        if(healthbar != null)
            UpdateHealthbar();
        if(ammo != null)
            UpdateAmmo();
    }

    // called when player takes damage
    private void OnTakeDamage(float damageReceived)
    {
        UpdateHealthbar();
    }

    // redraw healthbar
    private void UpdateHealthbar()
    {
        _healthbarSettings.maximum = player.MaxHitPoints;
        _healthbarSettings.health = player.CurrentHitPoints;
        _healthbarSettings.Redraw();
    }

    private void OnDeath()
    {
        DeathSequence();
    }

    private void UpdateAmmo()
    {
        _ammoSettings.Redraw();
    }

    private void OnWeaponUpdate()
    {
        UpdateAmmo();
    }
}
