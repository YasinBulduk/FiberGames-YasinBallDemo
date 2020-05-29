using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TurningObstacleController))]
public class TurningObstacleControllerEditor : Editor
{
    private TurningObstacleController editorTarget;

    private void OnEnable()
    {
        editorTarget = (TurningObstacleController) target;
    }

    private void AdjustCylinderRadius()
    {
        var cylinder = editorTarget.GetComponentInChildren<MeshRenderer>();

        Vector3 cylinderSize = cylinder.transform.localScale;
        cylinderSize.x = editorTarget.centerRadius;
        cylinderSize.z = editorTarget.centerRadius;

        cylinder.transform.localScale = cylinderSize;
    }

    private void AdjustParticlePositionsAndLength()
    {
        var particles = editorTarget.GetComponentsInChildren<ParticleSystem>();

        for (int i = 0; i < particles.Length; i++)
        {
            var particle = particles[i];

            ParticleSystem.MainModule main = particle.main;
            main.startLifetime = editorTarget.particleLength * 0.2f + editorTarget.centerRadius / 2 * .2f;
        }
    }

    private void AdjustTriggerColliderSize()
    {
        var collider = editorTarget.GetComponentInChildren<Obstacle>().GetComponent<BoxCollider>();
        var cylinder = editorTarget.GetComponentInChildren<MeshRenderer>();

        Vector3 colliderCenter = Vector3.zero;
        colliderCenter.y = cylinder.transform.position.y;

        Vector3 colliderSize = collider.size;
        colliderSize.x = editorTarget.centerRadius + editorTarget.particleLength * 2;

        collider.center = colliderCenter;
        collider.size = colliderSize;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Change Size"))
        {
            AdjustCylinderRadius();
            AdjustParticlePositionsAndLength();
            AdjustTriggerColliderSize();
        }
    }
}
