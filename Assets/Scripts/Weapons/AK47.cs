using UnityEngine;

public class AK47 : MonoBehaviour, IWeapon
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletForce = 100f;
    private Transform hand;

    public void Shoot()
    {
        if (bulletPrefab == null || firePoint == null)
        {
            Debug.LogWarning("BulletPrefab ou FirePoint non assigné !");
            return;
        }
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        bullet.transform.position = firePoint.position;
        bullet.transform.forward = firePoint.forward;
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 direction = firePoint.forward;
            rb.AddForce(direction * bulletForce, ForceMode.Impulse);
        }
        Destroy(bullet, 3f);
    }

    public void Equip(Transform handTransform)
    {
        hand = handTransform;
        transform.SetParent(handTransform);
        transform.localPosition = new Vector3(0.0f, 0.15f, 1f);
        transform.localRotation = Quaternion.Euler(90, 0, 0);
    }

    public void Unequip()
    {
        transform.SetParent(null);
        hand = null;
    }
}