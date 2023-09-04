
using UnityEngine;

[CreateAssetMenu(fileName = "GunData", menuName = "Weapon/New GunData")]
public class GunData : ScriptableObject
{
    [Header("Type of the gun")]
    public new string name;
    public WeaponType TypeOfWeapon;
    public Sprite GunImage;
    [Header("Settings")]
    [SerializeField] public float damage = 0f;
    [SerializeField, Range(0, 1000)] public int range = 100; //Range is the distance
    public float inAccuracy = 1f;
    public int currentAmmo;
    public int magazineSize;
    public int ammoReserves;
    public int maxAmmoReserves = 120;
    public float fireRate;
    public float reloadTime = 0.0f;
    public float hitForce = 500f;
    [HideInInspector]
    public float timeBetweenLastShot;
    [Space]
    [Header("The sounds of DryFire")]
    public AudioClip[] dryFireClip;
    [Header("The sounds of shots")]
    public AudioClip[] fireClip;
    [Header("The sounds of reload")]
    public AudioClip reloadClip;

    [Space]
    [Header("Weapon hip recoil")]
    public float recoilX;
    public float recoilY;
    public float recoilZ;
    [Space]
    [Header("Weapon aim recoil")]
    public float aimrecoilX;
    public float aimrecoilY;
    public float aimrecoilZ;

    [HideInInspector]
    public bool isShooting;
    [HideInInspector]
    public bool isReloading = false;

    public enum WeaponType
    {
        EmptyHands,
        Pistol,
        AssaultRifle,
        Smg,
        Shotgun,
        Minigun,
        lmg,
        Sniper
    }
}
