using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WallObstacleController))]
public class WallObstacleControllerEditor : Editor
{
    private WallObstacleController editorTarget;

    private void OnEnable()
    {
        editorTarget = (WallObstacleController) target;
    }

    private void AdjustPillarsPositions()
    {
        var pillars = editorTarget.GetComponentsInChildren<MeshRenderer>();
        Vector3 leftPillarPosition = editorTarget.transform.position;
        leftPillarPosition.x -= editorTarget.wallWidth / 2 - pillars[0].transform.localScale.x / 2;
        leftPillarPosition.y = pillars[0].transform.localScale.y / 2;

        Vector3 rightPillarPosition = editorTarget.transform.position;
        rightPillarPosition.x += editorTarget.wallWidth / 2 - pillars[0].transform.localScale.x / 2;
        rightPillarPosition.y = pillars[0].transform.localScale.y / 2;

        pillars[0].transform.position = leftPillarPosition;
        pillars[1].transform.position = rightPillarPosition;
    }

    private void AdjustParticlePositions()
    {
        var pillar = editorTarget.GetComponentInChildren<MeshRenderer>();
        var particles = editorTarget.GetComponentsInChildren<ParticleSystem>();

        float perParticleY = pillar.transform.localScale.y / (particles.Length + 1);

        Vector3 particlePosition = editorTarget.transform.position;
        particlePosition.x -= editorTarget.wallWidth / 2;

        for (int i = 0; i < particles.Length; i++)
        {
            particlePosition.y += perParticleY;
            var particle = particles[i];

            particle.transform.position = particlePosition;
            ParticleSystem.MainModule main = particle.main;
            main.startLifetime = editorTarget.wallWidth * 0.2f;
        }
    }

    private void AdjustTriggerColliderSize()
    {
        var collider = editorTarget.GetComponentInChildren<Obstacle>().GetComponent<BoxCollider>();
        var pillar = editorTarget.GetComponentInChildren<MeshRenderer>();

        Vector3 colliderCenter = Vector3.zero;
        colliderCenter.y = pillar.transform.localScale.y / 2;

        Vector3 colliderSize = collider.size;
        colliderSize.x = editorTarget.wallWidth - pillar.transform.localScale.x * 2;

        collider.center = colliderCenter;
        collider.size = colliderSize;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Change Size"))
        {
            AdjustPillarsPositions();
            AdjustParticlePositions();
            AdjustTriggerColliderSize();
        }
    }
}
