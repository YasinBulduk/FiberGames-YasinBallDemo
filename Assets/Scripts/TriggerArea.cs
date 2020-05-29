using UnityEngine;
using UnityEditor;

public class TriggerArea : AbstractInteractableTrigger
{
    public Material triggerMaterial;

    private MeshRenderer m_MeshRenderer;

    void Awake()
    {
        m_MeshRenderer = GetComponent<MeshRenderer>();
    }

    void Start()
    {
        ChangeColor(triggerMaterial.GetColor("_BaseColor"));
    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        Handles.color = Color.red;
        for (int i = 0; i < triggerList.Count; i++)
        {
            Vector3 centerToTriggerable = transform.position - triggerList[i].transform.position;
            Vector3 center1 = Quaternion.AngleAxis(1f, Vector3.up) * centerToTriggerable;
            Vector3 center2 = Quaternion.AngleAxis(-1f, Vector3.up) * centerToTriggerable;
            
            Handles.DrawAAPolyLine(triggerList[i].transform.position + center1, triggerList[i].transform.position, triggerList[i].transform.position + center2);
        }
    }
#endif

    private void ChangeColor(Color newColor)
    {
        m_MeshRenderer.material.SetColor("_BaseColor", newColor);
    }

    public override void Interact(GameObject other)
    {
        BallController ball = other.GetComponent<BallController>();

        if (!ball) return;

        if(ball.CurrentMaterial == triggerMaterial)
        {
            Trigger();
        }
    }

    public override void Trigger()
    {
        if (triggerList.Count > 0)
        {
            foreach (var triggerable in triggerList)
            {
                triggerable.Triggered();
            }
        }
    }
}
