using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponWheel : MonoBehaviour
{
    public int[] maxAmmo = new int[3];
    public int[] currentAmmo = new int[3];
    public TextMeshProUGUI[] ammoTexts = new TextMeshProUGUI[3];

    private int currentWeaponIndex = 0;

    public GameObject[] weapons;

    void Start()
    {
        SwitchToWeapon(currentWeaponIndex);

        // Initialize ammo counts for each weapon
        for (int i = 0; i < 3; i++)
        {
            currentAmmo[i] = maxAmmo[i];
            UpdateAmmoText(i);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchWeapon(0);
            SwitchToWeapon(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchWeapon(1);
            SwitchToWeapon(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SwitchWeapon(2);
            SwitchToWeapon(2);
        }

        if (Input.GetMouseButtonDown(0))
        {
            FireWeapon();
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
        }
    }

    void SwitchWeapon(int weaponIndex)
    {
        // Update the current weapon index
        currentWeaponIndex = weaponIndex;

        // Update ammo text for all weapons
        for (int i = 0; i < 3; i++)
        {
            UpdateAmmoText(i);
        }
    }

    void FireWeapon()
    {
        // Example: Reduce ammo count for the current weapon
        if (currentAmmo[currentWeaponIndex] > 0)
        {
            currentAmmo[currentWeaponIndex]--;
            UpdateAmmoText(currentWeaponIndex);
            Debug.Log("Weapon " + (currentWeaponIndex + 1) + " fired! Ammo remaining: " + currentAmmo[currentWeaponIndex]);
        }
        else
        {
            Debug.Log("Weapon " + (currentWeaponIndex + 1) + " is out of ammo!");
        }


    }

    void UpdateAmmoText(int weaponIndex)
    {
        // Update the UI ammo text for the specified weapon
        ammoTexts[weaponIndex].text = "Ammo: " + currentAmmo[weaponIndex] + "/" + maxAmmo[weaponIndex];
    }
}
