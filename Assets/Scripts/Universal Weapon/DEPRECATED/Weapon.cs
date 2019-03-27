
[System.Obsolete("Use EquippedWeapon instead.", true)]
public class Weapon
{
    public int CurrentAmmo;
    public WeaponInfo Info { get; private set; }



    public Weapon (WeaponInfo info)
    {
        Info = info;
        CurrentAmmo = info.clipSize;
    }
}
