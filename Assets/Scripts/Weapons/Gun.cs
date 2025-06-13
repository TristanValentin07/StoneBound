using UnityEngine;

public class SimplePistol : MonoBehaviour, IWeapon
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletForce = 100f;
    public float cooldownTime = 1f; // temps entre les tirs (en secondes)

    private float lastShootTime = -Mathf.Infinity;
    private Transform hand;

    public void Shoot()
    {
        if (Time.time - lastShootTime < cooldownTime)
        {
            return;
        }

        lastShootTime = Time.time;

        if (bulletPrefab == null || firePoint == null)
        {
            Debug.LogWarning("BulletPrefab ou FirePoint non assigné !");
            return;
        }

        Debug.DrawRay(firePoint.position, firePoint.forward * 2f, Color.red, 2f);

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        bullet.transform.position = firePoint.position;
        bullet.transform.forward = firePoint.forward;
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 direction = (transform.forward + new Vector3(0, 0.55f, 0)).normalized;
            rb.AddForce(direction * bulletForce, ForceMode.Impulse);
        }
        Destroy(bullet, 3f);
    }

    public void Equip(Transform handTransform)
    {
        hand = handTransform;
        transform.SetParent(handTransform);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(30, -5, 90);
    }

    public void Unequip()
    {
        transform.SetParent(null);
        hand = null;
    }
}