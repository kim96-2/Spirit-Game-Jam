using Game.Core;
using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Potan_Mgr : MonoBehaviour
{
    [Header("투사체 설정")]
    public float speed = 15.0f;
    public float damage = 20.0f;

    public float m_SlowDuration = 1.5f;
    public float m_SlowTimeScale = 0.8f;

    [SerializeField] GameObject destroyParticle;

    Rigidbody rigidbody;

    private bool isDestroyed = false;

    void Start()
    {
        Destroy(gameObject, 3.0f);
        rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (isDestroyed) return;

        rigidbody.MovePosition(transform.position + transform.forward * speed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter(Collider coll)
    {
        if (isDestroyed) return;

        if (coll.CompareTag("Enemy"))
        {
            Enemy enemy = coll.gameObject.GetComponent<Enemy>();

            if (enemy != null)
            {
                isDestroyed = true;

                if (CameraEffect.Inst != null)
                {
                    CameraEffect.Inst.PlayHitEffect(m_SlowDuration, m_SlowTimeScale, 55f);
                }

                enemy.Damage(damage);

                if (destroyParticle != null)
                {
                    if (ObjectPoolManager.Instance != null)
                    {
                        ObjectPoolManager.Instance.Get(destroyParticle, transform.position, quaternion.identity);
                    }
                    else
                    {
                        GameObject backupEffect = Instantiate(destroyParticle, transform.position, Quaternion.identity);

                        Destroy(backupEffect, 0.7f);
                    }
                }

                Destroy(gameObject);
            }
        }
    }
}