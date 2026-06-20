using UnityEngine;

public class EnemyState_Stop : EnemyState
{
    EnemyState nextState;

    float duration;

    float _time;

    public EnemyState_Stop(Transform target, Enemy machine, 
        float duration) : base(target, machine)
    {    

        this.duration = duration;
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

        if(_time > duration) 
        {
            machine.ChangeState(nextState);
            return;
        }
        _time += Time.deltaTime;

    }

}
