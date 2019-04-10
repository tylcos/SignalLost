using UnityEngine;

public abstract class Gun : MonoBehaviour
{
    public abstract void Initialize(EquippedWeapon wep);

    public abstract void Fire(Vector2 direction);

    public abstract void Reload(float time);

    public abstract void CancelReload();
}
