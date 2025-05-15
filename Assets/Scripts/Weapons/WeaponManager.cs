using UnityEngine;
using System.Collections.Generic;

public class WeaponManager : MonoBehaviour
{
    private Transform weaponHolder;
    public List<GameObject> weaponPrefabs;
    private IWeapon currentWeapon;

    public GameObject currentWeaponGO;
    
    void Awake()
    {
        if (weaponHolder == null)
        {
            weaponHolder = transform.Find("CameraHolder/Player_Camera/weaponHolder");
        }
    }
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && currentWeapon != null)
            currentWeapon.Shoot();

        if (Input.GetKeyDown(KeyCode.Alpha1)) EquipWeapon(0);
        if (Input.GetKeyDown(KeyCode.Alpha2) && weaponPrefabs.Count > 1) EquipWeapon(1);
    }

    public void EquipWeapon(int index)
    {
        // Détruire l’ancien objet
        if (currentWeaponGO != null)
        {
            if (currentWeapon != null)
                currentWeapon.Unequip(); // uniquement si c’est une arme

            Destroy(currentWeaponGO);
            currentWeapon = null;
        }

        // Instancier le nouvel objet
        GameObject itemInstance = Instantiate(weaponPrefabs[index], weaponHolder.position, weaponHolder.rotation, weaponHolder);
        currentWeaponGO = itemInstance;

        // Vérifie si c’est une arme ou juste un objet
        currentWeapon = itemInstance.GetComponent<IWeapon>();
        if (currentWeapon != null)
        {
            currentWeapon.Equip(weaponHolder);
        }
        else
        {
            // Si ce n’est pas une arme, juste le placer dans la main
            itemInstance.transform.SetParent(weaponHolder);
            itemInstance.transform.localPosition = Vector3.zero;
            itemInstance.transform.localRotation = Quaternion.identity;
        }
    }

    public void UnEquipWeapon()
    {
        if (currentWeaponGO != null)
        {
            if (currentWeapon != null)
            {
                currentWeapon.Unequip();
                currentWeapon = null;
            }

            Destroy(currentWeaponGO);
            currentWeaponGO = null;

            Debug.Log("🚫 Objet déséquipé !");
        }
        else
        {
            Debug.Log("ℹ️ Aucun objet à déséquiper.");
        }
    }

}
