using Unity.Mathematics;
using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform shootPos;

    public virtual void Shoot(Vector3 dir)
    {
        GameObject bullet = Instantiate(bulletPrefab,shootPos.position, Quaternion.identity);

        bullet.GetComponent<Rigidbody>().AddForce(dir * 20f, ForceMode.Impulse);
    }
}
