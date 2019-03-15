using TMPro;
using UnityEngine;



public class UIController : MonoBehaviour
{
    // Ammo, health, and death updates should be performed from the player class

    private struct HealthbarSettings
    {
        public float maximum;
        public float health;
        public Transform healthbarTransform;

        public void Redraw()
        {
            healthbarTransform.localScale = new Vector3(health / maximum, healthbarTransform.localScale.y, healthbarTransform.localScale.z);
        }
    }

    public struct AmmoSettings
    {
        public EquippedWeapon wep;
        public int mode;
        public int cur;
        public int max;
        public TMP_Text text;
        public bool reload;
        public float reloadPercent;
        public GameObject uIndic;
        public Transform uFill;
        public float uFillFull;

        public void Redraw()
        {
            if (mode == WeaponController.COMBATMODE_GUN)
            {
                cur = wep.CurrentAmmo;
                max = wep.MaxAmmo;
                reload = wep.reloading;
                reloadPercent = wep.reloadProgress;
                text.text = cur + " / " + max;
                uIndic.SetActive(reload);
                uFill.localScale = new Vector3(uFillFull * reloadPercent, uFill.localScale.y, uFill.localScale.z);
                // turn on reload indicator and update it or turn it off
                // need a reload bar to use
            }
            else if (mode == WeaponController.COMBATMODE_MELEE)
            {
                uIndic.SetActive(false);
                text.text = "melee";
            }
        }
    }

    private struct ScoreSettings
    {
        public 
    }



    private HealthbarSettings _healthbarSettings;
    private AmmoSettings _ammoSettings;
    [SerializeField]
    private GameObject uiReloadIndicator = null;
    [SerializeField]
    private GameObject uiReloadIndicatorMask = null;
    [SerializeField]
    private PlayerWeaponController PWC = null;
    [SerializeField]
    private PlayerController player = null;
    [SerializeField]
    private GameObject healthbar = null;
    [SerializeField]
    private GameObject ammo = null;
    [SerializeField]
    private GameObject deathMessage = null;
    [SerializeField]
    private TextMeshProUGUI score;


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
            _ammoSettings.wep = PWC.GetEquippedWeapon();
            _ammoSettings.uIndic = uiReloadIndicator;
            _ammoSettings.uFill = uiReloadIndicatorMask.transform;
            _ammoSettings.uFillFull = uiReloadIndicatorMask.transform.localScale.x;
            EquippedWeapon.WeaponAmmoChanged += OnWeaponUpdate;
            EquippedWeapon.WeaponSwapped += OnWeaponSwap;
            PWC.ReloadUpdate += OnReloadUpdate;

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
        if (healthbar != null)
            UpdateHealthbar();
        if (ammo != null)
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

    private void OnWeaponSwap(EquippedWeapon wep, int combatMode)
    {
        _ammoSettings.wep = wep;
        _ammoSettings.mode = combatMode;
        UpdateAmmo();
    }

    private void OnReloadUpdate(bool reloading, float progress)
    {
        UpdateAmmo();
    }
}
