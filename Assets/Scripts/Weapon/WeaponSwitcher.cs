using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WeaponSwitcher : MonoBehaviour
{
    public GameObject[] weapons;
    public GameObject projectile;
    public GameObject projectileGrenade;
    public GameObject[] bulletsRenderer;

    public GameManager gm;

    private int currentWeaponIndex = 0;
    private int mouseWeapon;
    public bool isPaused;

    public int[] maxAmmo;
    public int[] currentAmmo;
    public int[] currentMaxAmmo;

    //public TextMeshProUGUI[] ammoTexts;
    public TextMeshProUGUI[] maxAmmoTexts;

    public AudioClip shot;

    public ParticleSystem[] muzzleFlash;

    public Camera playerCamera;

    private float raycastDistance = 100f;
    public float mgCadence;
    private float damage;
    private float forceApplied;

    public bool isARocketLauncher;
    public bool isAMachineGun;
    public bool isAGrenadeLauncher;
    private bool canReload;
    private bool outOfBullets;

    public Transform spawnPoint;
    public Transform spawnPointGen;

    private IEnumerator coroutine;

    public delegate void WeaponFired();
    public static WeaponFired weaponFired;

    public delegate void WeaponFiredStop();
    public static WeaponFiredStop weaponFiredStop;

    public SOPistol[] weaponData;

    void Start()
    {
        GetWeaponData();
        SwitchToWeapon(currentWeaponIndex);
        coroutine = MachineGunFire();
        PauseMenu.gameIsPaused += GamePaused;
        gm = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        UpdateAmmoText(currentWeaponIndex);

        float scrollWheel = Input.GetAxis("Mouse ScrollWheel");

        if (scrollWheel != 0f)
        {
            if ((scrollWheel > 0f) && (mouseWeapon <= 4))
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
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {

            SwitchToWeapon(4);
            //isAMachineGun = false;
            //isARocketLauncher = true;
        }
        if (Input.GetMouseButtonDown(0))
        {
            if(isPaused == false)
            {
                if (isAMachineGun)
                {
                    StartCoroutine(coroutine);
                }

                else
                {
                    Debug.Log("fireWeapon");
                    FireWeapon();
                }
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

            if (!isARocketLauncher && !isAGrenadeLauncher)
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

            if (isAGrenadeLauncher)
            {
                Instantiate(projectileGrenade, spawnPointGen.position, spawnPointGen.rotation);
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

        gm.ammoTexts[weaponIndex].text = "Ammo: " + currentAmmo[weaponIndex] + "/" + currentMaxAmmo[weaponIndex];

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
        isAGrenadeLauncher = weaponData[currentWeaponIndex].isAGrenadeLauncher;
        //crosshairTexture = weaponData.crosshairTexture;
        //fireSound = weaponData.fireSound;
    }

    void GamePaused(bool paused)
    {
        isPaused = paused;
    }
}
