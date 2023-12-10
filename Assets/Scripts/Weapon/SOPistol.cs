using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon Data", menuName = "ScriptableObjects/Weapon Data", order = 0)]
public class SOPistol : ScriptableObject
{
    [Header("Weapon Stats")]
    public float damage;
    public float weaponRange;
    //public int maxAmmo;
    public float bulletsPerSecond;
    //public int currentMaxAmmo;
    public float forceToApply;
    //public int maxBulletsPerShoot;
    [Header("Weapon Type")]
    public bool isAMachineGun;
    public bool isARocketLauncher;
    public bool isAGrenadeLauncher;
    //[Header("Weapon Customs")]
    //public Texture2D crosshairTexture;
    //public AudioClip fireSound;


}
