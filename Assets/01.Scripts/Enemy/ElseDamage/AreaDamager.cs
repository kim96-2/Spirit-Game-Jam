using System.Collections;
using Game.Core;
using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(PoolObject))]
public class AreaDamager : MonoBehaviour
{
    [SerializeField] float damageRange = 4f;
    [SerializeField] float tickDamage = 5f;
    [SerializeField] float damageDuration = 5f;

    [SerializeField] Transform damageAreaObject;
    [SerializeField] ParticleSystem areaParticle;

    PlayerCtrl player;

    PoolObject poolObject;

    float _time = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerCtrl>();

        poolObject = GetComponent<PoolObject>();
        poolObject.OnGet += SetDamager;
    }

    void SetDamager()
    {
        _time = 0f;

        Vector3 rangeScale = new(damageRange, damageAreaObject.localScale.y, damageRange);
        Vector3 position = transform.position;
        position.y = -1.41f;

        damageAreaObject.localScale = rangeScale;
        damageAreaObject.position = position;

        areaParticle.transform.position = position;
        
        rangeScale.y = damageRange / 2f;
        rangeScale.x = damageRange / 2f;
        rangeScale.z = damageRange / 2f;
        var shape = areaParticle.shape;
        shape.scale = rangeScale;

        StartCoroutine(Damager());
    }

    void DestroyDamager()
    {
        poolObject.Release();
    }

    IEnumerator Damager()
    {
        WaitForSeconds wait = new WaitForSeconds(1f);

        for(int i = 0; i < damageDuration; i++)
        {
            yield return wait;

            Vector3 direction = player.transform.position - transform.position;
            direction.y = 0f;

            if(direction.sqrMagnitude < damageRange)
            {
                player.Damage(tickDamage);
            }

        }

        yield return wait;

        DestroyDamager();
    }
}
