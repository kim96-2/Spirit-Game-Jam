using UnityEngine;

public class EnemyState
{
    protected Enemy machine;

    protected Transform target;

    public EnemyState(Transform target, Enemy machine)
    {
        this.machine = machine;

        this.target = target;
    }

    public virtual void Enter()
    {
        
    }
	public virtual void Exit()
    {
        
    }
	public virtual void UpdateState()
    {
        
    }
}
