using UnityEngine;


public interface IWeapon
{
    void Shoot();
    void Equip(Transform handtransform);
    void Unequip();
}
