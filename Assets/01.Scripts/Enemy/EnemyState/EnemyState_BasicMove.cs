using UnityEngine;
using UnityEngine.AI;

public class EnemyState_BasicMove : EnemyState
{
    EnemyState nextState;

    NavMeshAgent agent;

    float duration;
    float maxRange;

    float _time;

    public EnemyState_BasicMove(Transform target, Enemy machine, 
        NavMeshAgent agent, float duration, float maxRange) : base(target, machine)
    {    

        this.agent = agent;
        this.duration = duration;
        this.maxRange = maxRange;
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
        // Debug.Log("In");

        float sqrDistance = Vector3.SqrMagnitude(target.position - agent.transform.position);

        if(sqrDistance > maxRange * maxRange) agent.SetDestination(target.position);

        if(_time > duration) 
        {
            machine.ChangeState(nextState);
            return;
        }
        _time += Time.deltaTime;

    }
}
