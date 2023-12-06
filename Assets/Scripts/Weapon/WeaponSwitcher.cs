using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WeaponSwitcher : MonoBehaviour
{
    public GameObject[] weapons;
    public GameObject projectile;
    public GameObject[] bulletsRenderer;

    private int currentWeaponIndex = 0;
    private int mouseWeapon;

    public int[] maxAmmo = new int[4];
    public int[] currentAmmo = new int[4];
    public int[] currentMaxAmmo = new int[4];

    public TextMeshProUGUI[] ammoTexts = new TextMeshProUGUI[4];
    public TextMeshProUGUI[] maxAmmoTexts = new TextMeshProUGUI[4];

    public AudioClip shot;

    public ParticleSystem[] muzzleFlash = new ParticleSystem[4];

    public Camera playerCamera;

    private float raycastDistance = 100f;
    public float mgCadence;
    private float damage;
    private float forceApplied;

    public bool isARocketLauncher;
    public bool isAMachineGun;
    private bool canReload;
    private bool outOfBullets;

    public Transform spawnPoint;

    private IEnumerator coroutine;

    public delegate void WeaponFired();
    public static WeaponFired weaponFired;

    public delegate void WeaponFiredStop();
    public static WeaponFiredStop weaponFiredStop;

    public SOPistol[] weaponData = new SOPistol[4];




    void Start()
    {
        GetWeaponData();
        SwitchToWeapon(currentWeaponIndex);
        coroutine = MachineGunFire();
    }

    void Update()
    {
        UpdateAmmoText(currentWeaponIndex);

        float scrollWheel = Input.GetAxis("Mouse ScrollWheel");

        if (scrollWheel != 0f)
        {
            if ((scrollWheel > 0f) && (mouseWeapon <= 3))
            {
                
                mouseWeapon++;
                SwitchToWeapon(mouseWeapon);
            }
            else if ((scrollWheel < 0f) && (mouseWeapon >= 0))
            {
                
                mouseWeapon--;
                SwitchToWeapon(mouseWeapon);
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            
            SwitchToWeapon(0);
            //isAMachineGun = false;
            //isARocketLauncher = false;

        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            
            SwitchToWeapon(1);
            //isAMachineGun = false;
            //isARocketLauncher = false;

        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            
            SwitchToWeapon(2);
            //isAMachineGun = true;
            //isARocketLauncher = false;

        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            
            SwitchToWeapon(3);
            //isAMachineGun = false;
            //isARocketLauncher = true;
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (isAMachineGun)
            {
                StartCoroutine(coroutine);
            }

            else
            {
                FireWeapon();
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (weaponFiredStop != null)
                weaponFiredStop();

            StopCoroutine(coroutine);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {         
            if (canReload)
            {
                int cacheAmmo = maxAmmo[currentWeaponIndex] - currentAmmo[currentWeaponIndex];

                if (0 > (currentMaxAmmo[currentWeaponIndex] - cacheAmmo))
                {
                    currentAmmo[currentWeaponIndex] = currentAmmo[currentWeaponIndex] + currentMaxAmmo[currentWeaponIndex];

                    currentMaxAmmo[currentWeaponIndex] = 0; 
                }

                else
                {
                    currentMaxAmmo[currentWeaponIndex] = currentMaxAmmo[currentWeaponIndex] - cacheAmmo;

                    currentAmmo[currentWeaponIndex] = currentAmmo[currentWeaponIndex] + cacheAmmo;
                }
            }
        }

        //Have to deprecate

        if (currentAmmo[currentWeaponIndex] <= 0)
        {
            bulletsRenderer[currentWeaponIndex].SetActive(false);
        }

        if (currentAmmo[currentWeaponIndex] > 0)
        {
            bulletsRenderer[currentWeaponIndex].SetActive(true);
        }
    }

    void SwitchToWeapon(int weaponIndex)
    {
        if (weaponIndex >= 0 && weaponIndex < weapons.Length)
        {
            // Disable the currently active weapon
            weapons[currentWeaponIndex].SetActive(false);

            // Enable the selected weapon
            weapons[weaponIndex].SetActive(true);

            // Update the current weapon index
            currentWeaponIndex = weaponIndex;

            GetWeaponData();
        }
    }

    

    void FireWeapon()
    {
        if ((currentAmmo[currentWeaponIndex] > 0))
        {
            if (weaponFired != null)
                weaponFired();

            outOfBullets = false;

            currentAmmo[currentWeaponIndex]--;
            UpdateAmmoText(currentWeaponIndex);
            Debug.Log("Weapon " + (currentWeaponIndex + 1) + " fired! Ammo remaining: " + currentAmmo[currentWeaponIndex]);
            AudioSource.PlayClipAtPoint(shot, transform.position);

            if (muzzleFlash != null)
            {
                muzzleFlash[currentWeaponIndex].Play();
            }

            if (!isARocketLauncher)
            {
                Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, raycastDistance))
                {
                    Debug.Log("Hit target: " + hit.collider.gameObject.name);

                    if (hit.collider.CompareTag("Enemy"))
                    {
                        hit.collider.GetComponent<IEnemyBasic>()?.RecieveDamage(damage);

                        if(hit.collider.GetComponent<Rigidbody>() != null)
                        {
                            hit.collider.GetComponent<Rigidbody>()?.AddForce(ray.direction * forceApplied);
                        }
                    }
                }
                else
                {
                    Debug.Log("Missed the target!");
                }
            }

            if (isARocketLauncher)
            {
                Instantiate(projectile, spawnPoint.position, spawnPoint.rotation);
            }

        }
        else
        {
            outOfBullets = true;
            Debug.Log("Weapon " + (currentWeaponIndex + 1) + " is out of ammo!");
        }

        if (currentMaxAmmo[currentWeaponIndex] <= 0)
        {
            canReload = false;
            currentMaxAmmo[currentWeaponIndex] = 0;
        }

        else
        {
            canReload = true;
        }
    }

    void UpdateAmmoText(int weaponIndex)
    {

        ammoTexts[weaponIndex].text = "Ammo: " + currentAmmo[weaponIndex] + "/" + currentMaxAmmo[weaponIndex];

    }

    public IEnumerator MachineGunFire()
    {
        while (true)
        {
            yield return new WaitForSeconds(mgCadence);
            FireWeapon();
            yield return new WaitForSeconds(mgCadence);
            if (weaponFiredStop != null)
                weaponFiredStop();
        }
    }

    private void GetWeaponData()
    {
        damage = weaponData[currentWeaponIndex].damage;
        raycastDistance = weaponData[currentWeaponIndex].weaponRange;
        //maxAmmo = weaponData.maxAmmo;
        mgCadence = weaponData[currentWeaponIndex].bulletsPerSecond;
        forceApplied = weaponData[currentWeaponIndex].forceToApply;
        //maxBulletsPerShoot = weaponData.maxBulletsPerShoot;
        //accuracyDropPerShot = weaponData.accuracyDropPerShot;
        //accuracyRecoveryPerSecond = weaponData.accuracyRecoveryPerSecond;
        //accuracyMax = weaponData.accuracyMax;
        isAMachineGun = weaponData[currentWeaponIndex].isAMachineGun;
        isARocketLauncher = weaponData[currentWeaponIndex].isARocketLauncher;
        //crosshairTexture = weaponData.crosshairTexture;
        //fireSound = weaponData.fireSound;
    }
}
