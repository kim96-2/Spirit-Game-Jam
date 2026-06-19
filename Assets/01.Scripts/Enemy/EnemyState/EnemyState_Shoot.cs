using UnityEngine;

public class EnemyState_Shoot : EnemyState
{
    EnemyState nextState;

    EnemyShooter shooter;

    float attack_delay;

    float _time;

    public EnemyState_Shoot(Transform target, Enemy machine, EnemyShooter shooter, float attack_delay) : base(target, machine)
    {
        this.attack_delay = attack_delay;

        this.shooter = shooter;
    }

    public void SetState(EnemyState nextState)
    {
        this.nextState = nextState;
    }

    public override void Enter()
    {
        _time = 0f;
    }

    public override void UpdateState()
    {
        if(_time > attack_delay)
        {
            Shoot();
            machine.ChangeState(nextState);
        }

        _time += Time.deltaTime;
    }

    void Shoot()
    {
        Vector3 shootDir = target.position - machine.transform.position;
        shootDir.y = 0f;
        shootDir.Normalize();

        shooter.Shoot(shootDir);
    }
}
