using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float Lifetime = 3f;
    public int Damage = 25;

    private Vector3 _velocity;

    public void Initialize(float speed)
    {
        _velocity = transform.forward * speed;
        Destroy(gameObject, Lifetime);
    }

    void Update()
    {
        MoveForward();
    }

    private void MoveForward()
    {
        transform.position += _velocity * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.TryGetComponent(out Damagable damagable))
            damagable.TakeDamage(Damage);

        Destroy(gameObject);
    }
}
