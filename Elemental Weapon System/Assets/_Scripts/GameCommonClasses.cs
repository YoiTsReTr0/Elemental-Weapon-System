namespace Elemental.Main
{
    public enum WeaponType
    {
        AssaultRifle,
        Shotgun,
        RocketLauncher,
        Pistol,
        DefaultMelee
    }

    public enum WeaponSlotType
    {
        Primary,
        Secondary,
        Sidearm,
        Melee
    }

    public enum WeaponPowerType
    {
        Fire,
        Ice,
        Lightning
    }

    public enum ProjectileType
    {
        // Assault Rifle, SMG and pistol
        _9mm,
        _7pt62,
        _5pt56,

        //Shotgun Ammo
        _buckshot
    }

    /*public enum UserWeaponSlot
    {
        Primary,
        Secondary,
        Sidearm,
        Melee
    }*/

    /// <summary>
    /// Following the concept - is picked up by a shooting capable entity
    /// </summary>
    public enum WeaponOwner
    {
        Bearer,
        OnGround
    }

    public enum WeaponFireMode
    {
        FullAuto,
        BurstFire, // Burst fire functionality has not yet been added
        SingleShot
    }
}