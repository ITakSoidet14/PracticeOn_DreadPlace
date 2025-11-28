using UnityEngine;
using UnityEngine.Pool;

public abstract class ProjectileBase : MonoBehaviour
{
    public abstract void Initialize(float speed);
    public abstract void MoveForward();
    public abstract void SetPool(IObjectPool<ProjectileBase> pool);
}
