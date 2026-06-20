using UnityEngine;


public class Enemy_Boss : Enemy
{
    EnemyState_Stop state_firstStop;
    
    EnemyState_BasicMove state_move;

    EnemyState_MultipleChoice state_multipleChoice;

    EnemyState_Shoot state_shoot1;
    EnemyState_Shoot state_shoot2;
    EnemyState_Shoot state_shoot3;

    [SerializeField] EnemyShooter shooter1;
    [SerializeField] EnemyShooter shooter2;
    [SerializeField] EnemyShooter shooter3;

    protected override void SetEnemyState()
    {
        state_firstStop = new(target, this, 4f);

        state_move = new(target, this, navMeshAgent, 3f, 10f);
        state_multipleChoice = new(target, this, 0.5f);

        state_shoot1 = new(target, this, shooter1, 0.5f);
        state_shoot2 = new(target, this, shooter2, 0.2f);
        state_shoot3 = new(target, this, shooter3, 1f);

        state_firstStop.SetState(state_move);

        state_move.SetState(state_multipleChoice);

        state_shoot1.SetState(state_move);
        state_shoot2.SetState(state_move);
        state_shoot3.SetState(state_move);

        EnemyState[] choiceStates = new[]{state_shoot1, state_shoot2, state_shoot3};
        state_multipleChoice.SetState(choiceStates);

        ChangeState(state_firstStop);
    }
}
