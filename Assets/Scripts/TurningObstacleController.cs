using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct TurningObstacleState
{
    public bool isEnabled;
    public float timeInSeconds;
    public bool isTurning;
}

public class TurningObstacleController : AbstractTriggerable
{
    [Tooltip("Degrees per second")]
    public float turnSpeed = 360f;
    [Tooltip("If obstacle is constant running. States is not more important.")]
    public bool isConstant = true;
    [Tooltip("Infinite loop array over time")]
    public TurningObstacleState[] states;

    [SerializeField] private List<ParticleSystem> particles;
    [SerializeField] private List<Collider> triggerColliders;

    //Editor scripti için
    [Header("Editor Only")]
    public float centerRadius = 0.5f;
    public float particleLength = 1f;

    private bool m_IsRunning = false;
    private bool m_IsStarted = false;

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
            TurningObstacleState currentState = states[++currentIndex % length];

            if (currentState.isEnabled)
            {
                StartObstacle();
            }
            else
            {
                StopObstacle();
            }

            float waitTime = currentState.timeInSeconds;
            float delta = Time.deltaTime;

            while (delta < waitTime)
            {
                delta += Time.deltaTime;

                if (currentState.isTurning)
                {
                    transform.rotation *= Quaternion.AngleAxis(turnSpeed * Time.deltaTime, Vector3.up);
                }

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
        if (!m_IsStarted) return;
        m_IsStarted = false;
        particles.ForEach(p => p.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear));
        triggerColliders.ForEach(c => c.enabled = false);
    }

    public void StartObstacle()
    {
        if (m_IsStarted) return;
        m_IsStarted = true;
        particles.ForEach(p => p.Play());
        triggerColliders.ForEach(c => c.enabled = true);
    }

    public override void Triggered()
    {
        m_IsRunning = false;
        StopObstacle();
    }
}
