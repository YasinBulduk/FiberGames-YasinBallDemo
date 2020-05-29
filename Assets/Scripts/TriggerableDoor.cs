using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TriggerableDoor : AbstractTriggerable
{
    [SerializeField] private Transform openedPosition;
    [Tooltip("In Seconds")]
    [SerializeField] private float doorOpenTime;


    private MeshRenderer m_MeshRenderer;
    private Transform m_Door;
    private bool m_IsDoorOpened = false;

    void Awake()
    {
        m_MeshRenderer = GetComponentInChildren<MeshRenderer>();
        m_Door = m_MeshRenderer.transform;
    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        if(openedPosition)
        {
            Handles.color = Color.blue;
            Handles.DrawLine(transform.position, openedPosition.position);
        }
    }
#endif

    private IEnumerator OpenDoorRoutine()
    {
        float startDistance = Vector3.Distance(m_Door.position, openedPosition.position);
        float distanceInTime = startDistance / doorOpenTime; 
        while (true)
        {
            float distanceDelta = distanceInTime * Time.deltaTime;
            Vector3 newPosition = Vector3.MoveTowards(m_Door.position, openedPosition.position, distanceDelta);
            m_Door.position = newPosition;

            if(Vector3.Distance(m_Door.position, openedPosition.position) <= distanceDelta)
            {
                yield break;
            }

            yield return null;
        }
    }

    public override void Triggered()
    {
        if(!m_IsDoorOpened)
        {
            m_IsDoorOpened = true;
            StartCoroutine(OpenDoorRoutine());
        }
    }
}
