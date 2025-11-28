using UnityEngine;

public abstract class WeaponControllerBase : MonoBehaviour
{
    public abstract void TryShoot();
    public abstract void Shoot();
    public abstract void Reload();
}
