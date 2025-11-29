using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ParticlePool : MonoBehaviour
{
    [System.Serializable]
    public class PoolItem
    {
        public string key;
        public ParticleSystem prefab;
        public int defaultCapacity = 10;
        public int maxSize = 50;
    }

    [Header("Список пулов")]
    public List<PoolItem> items;

    private Dictionary<string, ObjectPool<ParticleSystem>> pools;

    private void Awake()
    {
        pools = new Dictionary<string, ObjectPool<ParticleSystem>>();

        foreach (var item in items)
        {
            var newPool = new ObjectPool<ParticleSystem>(
                createFunc: () => CreateParticle(item),
                actionOnGet: OnGet,
                actionOnRelease: OnRelease,
                actionOnDestroy: DestroyParticle,
                collectionCheck: false,
                defaultCapacity: item.defaultCapacity,
                maxSize: item.maxSize
            );

            pools.Add(item.key, newPool);
        }
    }

    private ParticleSystem CreateParticle(PoolItem item)
    {
        var ps = Instantiate(item.prefab, transform);
        ps.gameObject.SetActive(false);
        return ps;
    }

    private void OnGet(ParticleSystem ps)
    {
        ps.gameObject.SetActive(true);
        ps.Play(true);
    }

    private void OnRelease(ParticleSystem ps)
    {
        ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        ps.gameObject.SetActive(false);
    }

    private void DestroyParticle(ParticleSystem ps)
    {
        Destroy(ps.gameObject);
    }

    /// <summary>
    /// Спавн партикла по ключу.
    /// </summary>
    public void Spawn(string key, Vector3 position, Quaternion rotation)
    {
        if (!pools.TryGetValue(key, out var pool))
        {
            Debug.LogWarning($"ParticleMultiPool: ключ '{key}' не найден");
            return;
        }

        var ps = pool.Get();

        ps.transform.SetPositionAndRotation(position, rotation);

        StartCoroutine(ReturnAfterLifetime(ps, pool));
    }

    private System.Collections.IEnumerator ReturnAfterLifetime(ParticleSystem ps, ObjectPool<ParticleSystem> pool)
    {
        yield return new WaitUntil(() => !ps.IsAlive(true));
        pool.Release(ps);
    }
}
