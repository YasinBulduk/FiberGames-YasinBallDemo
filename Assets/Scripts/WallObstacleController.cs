using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct WallObstacleState
{
    public bool isEnabled;
    public float timeInSeconds;
}

public class WallObstacleController : AbstractTriggerable
{
    [Tooltip("If obstacle is constant running. States is not more important.")]
    public bool isConstant = true;
    [Tooltip("Infinite loop array over time")]
    public WallObstacleState[] states;

    [SerializeField] private List<ParticleSystem> particles;
    [SerializeField] private List<Collider> triggerColliders;

    [Header("Editor Only")]
    public float wallWidth = 1f;

    private bool m_IsRunning = false;

    void Start()
    {
        if (!isConstant)
        {
            StartCoroutine(StateRoutine());
        }
        else
        {
            StartObstacle();
        }
    }

    private IEnumerator StateRoutine()
    {
        m_IsRunning = true;
        int length = states.Length;
        int currentIndex = -1;
        while (true)
        {
            WallObstacleState currentState = states[++currentIndex % length];
            if(currentState.isEnabled)
            {
                StartObstacle();
            }
            else
            {
                StopObstacle();
            }
            
            float waitTime = currentState.timeInSeconds;
            float delta = 0f;

            while(delta < waitTime)
            {
                delta += Time.deltaTime;
                if (!m_IsRunning)
                {
                    yield break;
                }

                yield return null;
            }
        }
    }

    public void StopObstacle()
    {
        particles.ForEach(p => p.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear));
        triggerColliders.ForEach(c => c.enabled = false);
    }

    public void StartObstacle()
    {
        particles.ForEach(p => p.Play());
        triggerColliders.ForEach(c => c.enabled = true);
    }

    public override void Triggered()
    {
        m_IsRunning = false;
        StopObstacle();
    }
}
