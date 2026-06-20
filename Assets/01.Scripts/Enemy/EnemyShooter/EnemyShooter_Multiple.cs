using Unity.Mathematics;
using UnityEngine;

public class EnemyShooter_Multiple : EnemyShooter
{
    [SerializeField] int shootCount = 4;
    [SerializeField] float angle = 30f;

    public override void Shoot(Vector3 dir)
    {
        shootCount = Mathf.Max(shootCount, 2);

        float3 currentDir;
        for(int i = 0; i < shootCount; i ++)
        {
            float currentAngle = -angle / 2f + angle * i / (float)(shootCount - 1f);
            // currentAngle *= Mathf.Deg2Rad;

            currentDir = Quaternion.Euler(0, currentAngle, 0) * dir;

            ShootBullet(currentDir);
        }
    }
}
