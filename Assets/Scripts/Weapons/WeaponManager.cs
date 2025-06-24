using UnityEngine;
using System.Collections.Generic;

public class WeaponManager : MonoBehaviour
{
    public Transform weaponHolder;
    public Transform weaponHolderTPS;
    public List<GameObject> weaponPrefabs;
    private IWeapon currentWeapon;

    public GameObject currentWeaponGO;
    public GameObject currentWeaponGO_TPS;

    public bool isFirstPersonView = true;

    void Awake()
    {
        // Corrigé : chemins hiérarchiques exacts
        if (weaponHolder == null)
            weaponHolder = transform.Find("CameraHolder/Player_Camera/weaponHolder");

        if (weaponHolderTPS == null)
        {
            var tpsCam = transform.Find("CameraThirdHolder/Third_person_cam");
            if (tpsCam != null)
                weaponHolderTPS = tpsCam.Find("weaponHolder_TPS");
        }

        if (weaponHolder == null)
            Debug.LogError("❌ weaponHolder (FPS) introuvable !");
        if (weaponHolderTPS == null)
            Debug.LogError("❌ weaponHolderTPS (TPS) introuvable !");
    }

    void Update()
    {
        // Sécurité : s'assurer que Switch_Camera.Instance existe
        if (Switch_Camera.Instance != null)
            isFirstPersonView = !Switch_Camera.Instance.isThirdPerson;

        // Active/désactive les armes selon la vue
        if (currentWeaponGO != null)
            currentWeaponGO.SetActive(isFirstPersonView);

        if (currentWeaponGO_TPS != null)
            currentWeaponGO_TPS.SetActive(!isFirstPersonView);

        // Tir
        if (Input.GetButtonDown("Fire1") && currentWeapon != null)
            currentWeapon.Shoot();

        // Changement d’arme
        if (Input.GetKeyDown(KeyCode.Alpha1)) EquipWeapon(0);
        if (Input.GetKeyDown(KeyCode.Alpha2) && weaponPrefabs.Count > 1) EquipWeapon(1);
    }

    public void EquipWeapon(int index)
    {
        if (weaponHolder == null || weaponHolderTPS == null)
        {
            Debug.LogError("⛔ Impossible d'équiper une arme : holders manquants");
            return;
        }

        // Détruire anciens objets
        if (currentWeaponGO != null)
        {
            if (currentWeapon != null)
                currentWeapon.Unequip();

            Destroy(currentWeaponGO);
            currentWeapon = null;
        }

        if (currentWeaponGO_TPS != null)
            Destroy(currentWeaponGO_TPS);

        // Instancier les deux versions
        GameObject itemInstance = Instantiate(weaponPrefabs[index], weaponHolder);
        GameObject weaponTPS = Instantiate(weaponPrefabs[index], weaponHolderTPS);

        currentWeaponGO = itemInstance;
        currentWeaponGO_TPS = weaponTPS;

        // Config FPS
        currentWeapon = itemInstance.GetComponent<IWeapon>();
        if (currentWeapon != null)
        {
            currentWeapon.Equip(weaponHolder);
        }

        itemInstance.transform.localPosition = new Vector3(0f, 0f, 0f);
        itemInstance.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);

        // Config TPS
        weaponTPS.transform.localPosition = new Vector3(0f, 0.6f, 0f);
        weaponTPS.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);

        // Pour tester :
        Debug.Log("✅ Arme équipée : " + weaponPrefabs[index].name);
    }

    public void UnEquipWeapon()
    {
        if (currentWeaponGO != null || currentWeaponGO_TPS != null)
        {
            if (currentWeapon != null)
            {
                currentWeapon.Unequip();
                currentWeapon = null;
            }

            Destroy(currentWeaponGO);
            Destroy(currentWeaponGO_TPS);
            currentWeaponGO = null;
            currentWeaponGO_TPS = null;

            Debug.Log("🚫 Objet déséquipé !");
        }
        else
        {
            Debug.Log("ℹ️ Aucun objet à déséquiper.");
        }
    }
}
