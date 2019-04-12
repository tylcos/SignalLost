using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;



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
            if (health < 0)
            {
                health = 0;
            }
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
            if (mode == WeaponController.COMBATMODE_GUN && !DungeonGameManager.LoadingNewLevel)
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
        public TMP_Text text;
        public int score;

        public void Redraw()
        {
            text.text = "Score: " + score;
        }
    }



    private HealthbarSettings _healthbarSettings;
    private AmmoSettings _ammoSettings;
    private ScoreSettings _scoreSettings;
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
    private GameObject score = null;

    [SerializeField]
    private Image fadeOut;



    private void OnEnable()
    {
        try
        {
            _healthbarSettings.healthbarTransform = healthbar.GetComponentsInChildren<Transform>()[2];
            player.DamageTaken += OnTakeDamage;
        }
        catch (UnassignedReferenceException)
        {
            Debug.LogError("The healthbar and related objects were not assigned in the UI canvas! Disabling updates!");
        }

        try
        {
            _ammoSettings.text = ammo.GetComponent<TMP_Text>();
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
            Debug.LogError("The ammo counter and related objects were not assigned in the UI canvas! Disabling updates!");
        }

        if (score != null)
        {
            _scoreSettings.text = score.GetComponent<TMP_Text>();
        }
        else
        {
            Debug.LogError("The score was not assigned in the UI canvas! Disabling Updates!");
        }

        if (deathMessage != null)
        {
            player.Died += OnDeath;
        }
        else
        {
            Debug.LogError("The death message was not assigned in the UI canvas! Disabling updates!");
        }

        DungeonGameManager.ScoreChanged += OnScoreChanged;

        UpdateAmmo();
        UpdateHealthbar();
        UpdateScore();
        fadeOut.gameObject.SetActive(true);
    }

    private void Start()
    {
        StartFadeBlind(1f, 0f, 2f, true);
    }


    /// <summary>
    /// Linearly changes the alpha channel over time of the blind over the UI 
    /// </summary>
    /// <param name="alphaStart">Starting aplha value</param>
    /// <param name="alphaFinish">Ending aplha value</param>
    /// <param name="time">Time taken to transition from the start value to the end value</param>
    /// <param name="enableCursor">Turns the cursor on or off after the method is done</param>
    public void StartFadeBlind(float alphaStart, float alphaFinish, float time, bool enableCursor)
    {
        StartCoroutine(FadeBlind(alphaStart, alphaFinish, time, enableCursor));
    }

    private IEnumerator<WaitForEndOfFrame> FadeBlind(float alphaStart, float alphaFinish, float time, bool enableCursor)
    {
        Cursor.visible = false;

        float elapsedTime = 0;
        Color color = fadeOut.color;

        while (elapsedTime < time)
        {
            color.a = Mathf.Lerp(alphaStart, alphaFinish, elapsedTime / time);
            fadeOut.color = color;

            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        Cursor.visible = enableCursor;
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
        _ammoSettings.wep = PWC.GetEquippedWeapon();
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

    private void OnScoreChanged()
    {
        UpdateScore();
    }

    private void UpdateScore()
    {
        _scoreSettings.score = DungeonGameManager.CurrentScore;
        _scoreSettings.Redraw();
    }
}
