using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AmmoCounter : MonoBehaviour {

    public WeaponManager characterWeapon;

    private TMP_Text m_text;

    private void Awake()
    {
        // Get a reference to the text component.
        // Since we are using the base class type <TMP_Text> this component could be either a <TextMeshPro> or <TextMeshProUGUI> component.
        m_text = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update () {
        int cur = characterWeapon.CurrentAmmo;
        int max = characterWeapon.MaxAmmo;
        m_text.text = cur + " / " + max;
	}
}
