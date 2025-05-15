using UnityEngine;

public class Railgun : MonoBehaviour, IWeapon
{
    public Transform firePoint;
    public float range = 100f;
    public float cooldown = 1f;
    public LineRenderer laserLine;
    public LayerMask hitLayers;

    private bool canShoot = true;

    public void Shoot()
    {
        if (!canShoot) return;

        canShoot = false;
        Invoke(nameof(ResetCooldown), cooldown);

        if (Physics.Raycast(firePoint.position, firePoint.forward, out RaycastHit hit, range, hitLayers))
        {
            Debug.Log("Touché : " + hit.collider.name);

            ShowLaser(firePoint.position, hit.point);

            EnemyAI enemy = hit.collider.GetComponent<EnemyAI>();
            if (enemy != null)
            {
                enemy.TakeDamage(100);
            }
            else
            {
                Debug.LogWarning("Objet touché, mais pas d'EnemyAI dessus : " + hit.collider.name);
            }
        }
        else
        {
            ShowLaser(firePoint.position, firePoint.position + firePoint.forward * range);
        }
    }
    
    void ShowLaser(Vector3 start, Vector3 end)
    {
        if (laserLine == null) return;

        laserLine.SetPosition(0, start);
        laserLine.SetPosition(1, end);
        laserLine.enabled = true;
        Invoke(nameof(HideLaser), 0.05f);
    }

    void HideLaser()
    {
        if (laserLine != null)
            laserLine.enabled = false;
    }

    public void Equip(Transform handTransform)
    {
        transform.SetParent(handTransform);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    public void Unequip()
    {
        gameObject.SetActive(false);
        transform.SetParent(null);
    }

    private void ResetCooldown()
    {
        canShoot = true;
    }
}