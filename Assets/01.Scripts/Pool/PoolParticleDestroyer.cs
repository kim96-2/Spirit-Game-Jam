using Game.Core;
using UnityEngine;

[RequireComponent(typeof(PoolObject))]
[RequireComponent(typeof(ParticleSystem))]
public class PoolParticleDestroyer : MonoBehaviour
{
    PoolObject poolObject;
    ParticleSystem particleSystem;

    void Awake()
    {
        poolObject = GetComponent<PoolObject>();
        particleSystem = GetComponent<ParticleSystem>();

        var main = particleSystem.main;
        main.stopAction = ParticleSystemStopAction.Callback;

    }

    void OnParticleSystemStopped()
    {
        poolObject.Release();
    }
}
