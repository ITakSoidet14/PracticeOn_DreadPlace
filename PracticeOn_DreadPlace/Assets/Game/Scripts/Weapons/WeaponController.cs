using TMPro;
using UnityEngine;
using UnityEngine.Pool;

public class WeaponController : WeaponControllerBase
{
    /*
    [Header("Shooting")]
    public float FireRate = 0.2f;
    private float _nextShotTime;

    [Header("Projectile")]
    public ProjectileBase ProjectilePrefab;
    public Transform FirePoint;
    public float ProjectileSpeed = 30f;

    [Header("Ammo")]
    public int MaxAmmo = 30;
    public int CurrentAmmo;
    public float ReloadTime = 1.5f;
    private bool _isReloading;

    private void Start()
    {
        CurrentAmmo = MaxAmmo;
    }

    public override void TryShoot()
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

    public override void Shoot()
    {
        CurrentAmmo--;
        Debug.Log(CurrentAmmo);
        ProjectileBase bullet = Instantiate(ProjectilePrefab, FirePoint.position, FirePoint.rotation);
        bullet.Initialize(ProjectileSpeed);
    }

    public async override void Reload()
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
    */

    [Header("Shooting")]
    public float FireRate = 0.2f;
    private float _nextShotTime;

    [Header("Projectile")]
    public ProjectileBase ProjectilePrefab;
    public ParticlePool ParticlePool;
    public Transform FirePoint;
    public Transform ParentProjectile;
    public float ProjectileSpeed = 30f;

    [Header("Ammo")]
    public int MaxAmmo = 30;
    public int CurrentAmmo;
    public float ReloadTime = 1.5f;
    public TextMeshProUGUI BulletText;
    private bool _isReloading;

    private IObjectPool<ProjectileBase> _projectilePool;

    private void Awake()
    {
        _projectilePool = new ObjectPool<ProjectileBase>(
            CreateProjectile,
            OnTakeFromPool,
            OnReturnedToPool,
            OnDestroyPooledObject,
            false,
            20,       // default capacity
            200       // max size
        );
    }

    private void Start()
    {
        CurrentAmmo = MaxAmmo;
        BulletText.text = CurrentAmmo.ToString();
    }

    private ProjectileBase CreateProjectile()
    {
        ProjectileBase proj = Instantiate(ProjectilePrefab, ParentProjectile);
        proj.SetPool(_projectilePool);
        return proj;
    }

    private void OnTakeFromPool(ProjectileBase proj)
    {
        proj.gameObject.SetActive(true);
    }

    private void OnReturnedToPool(ProjectileBase proj)
    {
        proj.gameObject.SetActive(false);
    }

    private void OnDestroyPooledObject(ProjectileBase proj)
    {
        Destroy(proj.gameObject);
    }

    public override void TryShoot()
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

    public override void Shoot()
    {
        CurrentAmmo--;
        BulletText.text = CurrentAmmo.ToString();
        ProjectileBase bullet = _projectilePool.Get();
        bullet.transform.position = FirePoint.position;
        bullet.transform.rotation = FirePoint.rotation;
        bullet.Initialize(ProjectileSpeed);
        bullet.SetParticlePool(ParticlePool);
        ParticlePool.Spawn("Flash", FirePoint.position, FirePoint.rotation);
    }

    public async override void Reload()
    {
        if (_isReloading || CurrentAmmo == MaxAmmo) return;

        _isReloading = true;
        print("Reloading start");

        await Awaitable.WaitForSecondsAsync(ReloadTime);

        CurrentAmmo = MaxAmmo;
        BulletText.text = CurrentAmmo.ToString();
        print("Reloading end");
        _isReloading = false;
    }
}
