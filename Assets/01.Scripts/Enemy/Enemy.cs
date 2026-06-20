using System;
using Game.Core;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    [SerializeField] float hp = 100f;
    public float Hp => hp;
    public Action OnHpDamage;

    [SerializeField] GameObject destroyParticle;
    [SerializeField] Transform model;
    
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

        Vector3 lookDirection = target.position - transform.position;
        lookDirection.y = 0f;
        lookDirection.Normalize();

        model.transform.rotation = Quaternion.Slerp(model.transform.rotation, Quaternion.LookRotation(lookDirection), 5f * Time.deltaTime);
    }

    protected virtual void SetEnemyState()
    {
        
    }

    public void SetEnemy(MapComponent map)
    {
    
        currentMap = map;

        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.angularSpeed = 0f;
        target = GameObject.FindWithTag("Player").transform;

        SetEnemyState();

        // Invoke("DestoryEnemy", 10f);
    }

    public void Damage(float damage)
    {
        hp -= damage;

        OnHpDamage?.Invoke();

        if (hp <= 0)
        {
            Game_Mg.Inst.OnSkillUpdatee(10.0f);
            DestoryEnemy();
        }
    }

    void DestoryEnemy()
    {
        currentMap.EnemyDestroyed();

        OnEnemyDestroyed?.Invoke();

        if(destroyParticle != null) ObjectPoolManager.Instance.Get(destroyParticle, transform.position, quaternion.identity);

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
