using Game.Core;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(PoolObject))]
[RequireComponent(typeof(Rigidbody))]
public class EnemyBullet_Follow : EnemyBullet
{
    [SerializeField] float secondTargetGoTime = 4f;

    Vector3 firstTarget;
    Vector3 secondTarget;

    float _moveTime = 0f;

    public override void Shoot(Vector3 dir, Enemy enemy, float damage)
    {
        base.Shoot(dir, enemy, damage);

        firstTarget = transform.position + dir * moveSpeed * 4f;

        secondTarget = enemy.transform.position;

        enemy.OnEnemyDestroyed += EnemyDestroyed;
    }

    void EnemyDestroyed()
    {
        enemy = null;
    }

    protected override void MoveBullet()
    {
        float secondTargetAmount = Mathf.Clamp01(_moveTime / secondTargetGoTime - 1f);
        // float secondTargetAmount = 1f;

        Vector3 targetDir = secondTarget - transform.position;
        targetDir.Normalize();

        if(enemy != null)
        {
            secondTarget = enemy.transform.position;
        }

        if((secondTarget - transform.position).sqrMagnitude < 0.1f)
        {
            DestroyBullet();
            return;
        }

        shootDir += targetDir * secondTargetAmount * moveSpeed * 2f * Time.deltaTime;

        Debug.DrawLine(transform.position, shootDir);

        

        shootDir.y = 0f;
        if(shootDir.sqrMagnitude > 1f) shootDir.Normalize();

        rigidbody.MovePosition(transform.position + shootDir * moveSpeed * Time.deltaTime);
        _moveTime += Time.deltaTime;
    }
}
