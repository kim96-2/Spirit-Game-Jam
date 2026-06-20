using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    [SerializeField] float hp = 100f;
    
    protected NavMeshAgent navMeshAgent;
    protected Transform target;

    MapComponent currentMap;

    EnemyState currentState;

    public event Action OnEnemyDestroyed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentState?.UpdateState();
    }

    protected virtual void SetEnemyState()
    {
        
    }

    public void SetEnemy(MapComponent map)
    {
    
        currentMap = map;

        navMeshAgent = GetComponent<NavMeshAgent>();
        target = GameObject.FindWithTag("Player").transform;

        SetEnemyState();

        // Invoke("DestoryEnemy", 10f);
    }

    public void Damage(float damage)
    {
        hp -= damage;

        if(hp < 0) DestoryEnemy();
    }

    void DestoryEnemy()
    {
        currentMap.EnemyDestroyed();

        OnEnemyDestroyed?.Invoke();

        Destroy(this.gameObject);
    }

    #region Enemy State

    public void ChangeState(EnemyState to)
    {
        currentState?.Exit();
        to.Enter();

        currentState = to;
    }

    #endregion
}
