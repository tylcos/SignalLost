using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CQC : MonoBehaviour
{
    public abstract void Initialize(EquippedWeapon wep, MovementController source, int layer);

    public abstract void Attack();

    public abstract void CancelAttack();
}
