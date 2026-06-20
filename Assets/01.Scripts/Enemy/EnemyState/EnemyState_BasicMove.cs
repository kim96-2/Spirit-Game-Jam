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

        agent.isStopped = false;

        // agent.speed = 0.01f;    
    }

    public override void UpdateState()
    {
        // Debug.Log("In");

        float sqrDistance = Vector3.SqrMagnitude(target.position - agent.transform.position);

        if(sqrDistance > maxRange * maxRange) 
        {
            agent.SetDestination(target.position);
            agent.isStopped = false;
        }
        else
        {
            agent.isStopped = true;
        }

        if(_time > duration) 
        {
            machine.ChangeState(nextState);
            return;
        }
        _time += Time.deltaTime;

    }

    public override void Exit()
    {
        agent.isStopped = true;
    }
}
