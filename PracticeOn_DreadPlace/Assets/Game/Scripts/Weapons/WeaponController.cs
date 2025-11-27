using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [Header("Shooting")]
    public float FireRate = 0.2f;
    private float _nextShotTime;

    [Header("Projectile")]
    public Projectile ProjectilePrefab;
    public Transform FirePoint;
    public float ProjectileSpeed = 30f;
    private float _nextFireTime;

    [Header("Ammo")]
    public int MaxAmmo = 30;
    public int CurrentAmmo;
    public float ReloadTime = 1.5f;
    private bool _isReloading;

    private void Start()
    {
        CurrentAmmo = MaxAmmo;
    }

    public void TryShoot()
    {
        if (Time.time < _nextShotTime || _isReloading) return;

        if (CurrentAmmo <= 0)
        {
            Reload();
            return;
        }

        Shoot();
        _nextShotTime = Time.time + FireRate;
    }

    private void Shoot()
    {
        CurrentAmmo--;
        Debug.Log(CurrentAmmo);
        Projectile bullet = Instantiate(ProjectilePrefab, FirePoint.position, FirePoint.rotation);
        bullet.Initialize(ProjectileSpeed);
    }

    public async void Reload()
    {
        if (_isReloading || CurrentAmmo == MaxAmmo) return;

        print("Reloading start");
        _isReloading = true;

        // ожидание времени перезарядки, модификатор await показывает что метод нужно ожидать
        await Awaitable.WaitForSecondsAsync(ReloadTime);

        CurrentAmmo = MaxAmmo;
        print("Reloading end");
        _isReloading = false;
    }
}
