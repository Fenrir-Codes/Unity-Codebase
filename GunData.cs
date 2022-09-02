using UnityEngine;

[CreateAssetMenu(fileName = "Gun", menuName = "Weapon/Gun")]
public class GunData : ScriptableObject
{
    [Header("Type of the gun")]
    public new string name;

    [Header("Settings")]
    [SerializeField] public float damage = 0f;
    [SerializeField, Range(0, 1000)] public int range = 200; //Range is the distance
    public int currentAmmo;
    public int magSize;
    public int ammoReserves;
    public float fireRate;
    public float reloadTimePistol = 1.8f;
    public float reloadTimeAR = 3.000f;
    public float reloadTimeShotgun = 2.6f;
    public float reloadTimeLMG = 4.300f;
    public float reloadTimeMinigun = 2.267f;
    //[HideInInspector]
    //public bool isShooting;
    //public bool isReloading;


}
