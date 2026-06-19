using UnityEngine;

public class Enemy_Shooter : Enemy
{
    EnemyState_BasicMove state_move;
    EnemyState_Shoot state_shoot;

    [SerializeField] EnemyShooter shooter;

    protected override void SetEnemyState()
    {
        state_move = new(target, this, navMeshAgent, 3f, 8f);
        state_shoot = new(target, this, shooter, 1f);

        state_move.SetState(state_shoot);
        state_shoot.SetState(state_move);

        ChangeState(state_move);
    }
}
