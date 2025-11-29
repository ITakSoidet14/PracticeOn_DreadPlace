using UnityEngine;
using UnityEngine.Pool;

public class Projectile : ProjectileBase
{
    /*
    public float Lifetime = 3f;
    public int Damage = 25;

    private Vector3 _velocity;

    public override void Initialize(float speed)
    {
        _velocity = transform.forward * speed;
        Destroy(gameObject, Lifetime);
    }

    void Update()
    {
        MoveForward();
    }

    public override void MoveForward()
    {
        transform.position += _velocity * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.TryGetComponent(out Damagable damagable))
            damagable.TakeDamage(Damage);

        Destroy(gameObject);
    }
    */

    public float Lifetime = 3f;
    public int Damage = 25;
    public TrailRenderer Trail;

    private ParticlePool _particlePool;
    private Vector3 _velocity;
    private float _lifeTimer;

    private IObjectPool<ProjectileBase> _pool;

    public override void SetParticlePool(ParticlePool particlePool)
    {
        _particlePool = particlePool;
    }

    public override void SetPool(IObjectPool<ProjectileBase> pool)
    {
        _pool = pool;
    }

    public override void Initialize(float speed)
    {
        _velocity = transform.forward * speed;
        _lifeTimer = Lifetime;
    }

    void Update()
    {
        MoveForward();

        _lifeTimer -= Time.deltaTime;
        if (_lifeTimer <= 0f)
            ReturnToPool();
    }

    public override void MoveForward()
    {
        transform.position += _velocity * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.TryGetComponent(out Damagable damagable))
        {
            damagable.TakeDamage(Damage);
            _particlePool.Spawn("Blood", transform.position, transform.rotation);
        }
        else
        {
            _particlePool.Spawn("Hit", transform.position, transform.rotation);
        }

        ReturnToPool();
    }

    public void ReturnToPool()
    {
        Trail.Clear();

        if (_pool != null)
            _pool.Release(this);
        else
            Destroy(gameObject); // fallback safety
    }
}
