using UnityEngine;

public class EnemyState_MultipleChoice : EnemyState
{
    EnemyState[] nextStates;

    float attack_delay;

    float _time;

    int randIndex;

    public EnemyState_MultipleChoice(Transform target, Enemy machine, float attack_delay) : base(target, machine)
    {
        this.attack_delay = attack_delay;
    }

    public void SetState(EnemyState[] nextStates)
    {
        this.nextStates = nextStates;

        randIndex = Random.Range(0, nextStates.Length);
    }

    public override void Enter()
    {
        _time = 0f;

        randIndex += Random.Range(1, nextStates.Length);
    }

    public override void UpdateState()
    {
        if(_time > attack_delay)
        {
            machine.ChangeState(nextStates[randIndex % nextStates.Length]);
        }

        _time += Time.deltaTime;
    }

}
