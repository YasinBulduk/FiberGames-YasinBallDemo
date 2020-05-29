using System;
using System.Collections.Generic;
using UnityEngine;

public class PainterValve : AbstractInteractableTriggerable
{
    public ColorInteractTable colorTable;

    private MeshRenderer m_MeshRenderer;
    private PainterParticleController m_ParticleController;
    private PainterColorTable m_CurrentColorTable;
    private int m_CurrentTableIndex = 0;

    void Awake()
    {
        m_MeshRenderer = GetComponentInChildren<MeshRenderer>();
        m_ParticleController = GetComponent<PainterParticleController>();
    }

    void Start()
    {
#if UNITY_EDITOR
        if(!colorTable)
        {
            Debug.LogError($"[{gameObject.name}] Color table is not setted.");
            return;
        }
        else if (colorTable.interactTable.Length <= 0)
        {
            Debug.LogError($"[{gameObject.name}] Interact Color Table is must have at least 1 material.");
            return;
        }
#endif

        m_CurrentColorTable = colorTable.interactTable[0];
        UpdateColor();
    }

    private void UpdateColor()
    {
        Color newColor = m_CurrentColorTable.selfMaterial.GetColor("_BaseColor");

        m_MeshRenderer.material.SetColor("_BaseColor", newColor);
        m_ParticleController.SetColor(newColor);
    }

    public override void Interact(GameObject other)
    {
        BallController ball = other.GetComponent<BallController>();

        if (!ball) return;

        if(m_CurrentColorTable.inMaterial == ball.CurrentMaterial)
        {
            ball.CurrentMaterial = m_CurrentColorTable.outMaterial;
        }
        else
        {
            ball.CurrentMaterial = m_CurrentColorTable.selfMaterial;
        }
    }

    public override void Triggered()
    {
        if (colorTable.interactTable.Length <= 1 || m_CurrentTableIndex == colorTable.interactTable.Length - 1) return;

        m_CurrentColorTable = colorTable.interactTable[++m_CurrentTableIndex];
        UpdateColor();
    }
}
