using UnityEngine;

public class Damagable : MonoBehaviour
{

    public float Health = 100f;

    public void TakeDamage(float amount)
    {

        Health -= amount;

        Debug.Log(Health);

        if (Health <= 0f)
        {
            Die();
        }
    }

    private void Die() => Destroy(gameObject);
}
