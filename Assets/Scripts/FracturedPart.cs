using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FracturedPart : MonoBehaviour
{
    public float fadeOutTime = 0.5f;

    private Rigidbody m_Rigidbody;
    private MeshRenderer m_MeshRenderer;
    private Material m_InitialMaterial;

    void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_MeshRenderer = GetComponent<MeshRenderer>();
        gameObject.SetActive(false);
    }

    private IEnumerator FadeOutRoutine()
    {
        Color initialColor = m_InitialMaterial.GetColor("_BaseColor");
        Color targetColor = initialColor;
        targetColor.a = 0f;
        m_MeshRenderer.material.SetColor("_BaseColor", initialColor);

        float delta = 0f;
        while(true)
        {
            float t = delta / fadeOutTime;

            Color currentColor = Color.Lerp(initialColor, targetColor, t);
            m_MeshRenderer.material.SetColor("_BaseColor", currentColor);

            delta += Time.deltaTime;

            if(t >= 1f)
            {
                break;
            }

            yield return null;
        }

        Destroy(gameObject);
    }

    public void Release(Vector3 releaseForce, Material initialMaterial)
    {
        transform.parent = null;
        gameObject.SetActive(true);
        m_InitialMaterial = initialMaterial;
        m_Rigidbody.AddForce(releaseForce, ForceMode.VelocityChange);

        StartCoroutine(FadeOutRoutine());
    }
}
