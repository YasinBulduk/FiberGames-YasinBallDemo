using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(SlidingStick))]
public class BallController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float groundDistance = 0.51f;

    public bool IsPlayerDied { get; private set; }
    public bool PlayerControl
    {
        get { return m_PlayerControl; }
        set { m_PlayerControl = value; m_InputStick.enabled = value; }
    }

    public Material CurrentMaterial
    {
        get { return m_CurrentMaterial; }
        set { m_CurrentMaterial = value; UpdateColor(value); }
    }

    [SerializeField] private ParticleSystem m_TrailParticle;
    [SerializeField] private FracturedPart[] m_FracturedParts;

    private Rigidbody m_Rigidbody;
    private MeshRenderer m_MeshRenderer;
    private Material m_CurrentMaterial;
    private SlidingStick m_InputStick;
    private bool m_PlayerControl = true;
    private bool m_IsGrounded = false;
    private bool m_IsDieRoutineRunning = false;

    void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        //Saniyede maksimum 800 derece dönüş
        m_Rigidbody.maxAngularVelocity = 14f;
        m_MeshRenderer = GetComponent<MeshRenderer>();
        CurrentMaterial = m_MeshRenderer.sharedMaterial;
        m_InputStick = GetComponent<SlidingStick>();

#if UNITY_EDITOR
        if (!m_TrailParticle)
        {
            Debug.LogError($"[{gameObject.name}] Trail particle is not setted.");
        }
#endif
    }

    void Update()
    {
        CheckIsGrounded();

        if (m_IsGrounded)
        {
            if (!m_TrailParticle.isPlaying)
                m_TrailParticle.Play();
        }
    }

    void FixedUpdate()
    {
        if (!PlayerControl) return;

        Vector3 torqueAxis = new Vector3(m_InputStick.Direction.y, 0f, -m_InputStick.Direction.x);
        Vector3 forceDirection = new Vector3(m_InputStick.Direction.x, 0f, m_InputStick.Direction.y);
        m_Rigidbody.AddTorque(torqueAxis * moveSpeed, ForceMode.VelocityChange);
        m_Rigidbody.AddForce(forceDirection * moveSpeed * 2f, ForceMode.Acceleration);
    }

    void OnTriggerEnter(Collider other)
    {
        ICanInteractable interaction = other.GetComponent<ICanInteractable>();
        
        if (interaction != null)
        {
            interaction.Interact(this.gameObject);
        }
    }

    private void CheckIsGrounded()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, groundDistance))
        {
            if (hit.collider.CompareTag("Ground"))
            {
                m_IsGrounded = true;
            }
            else
            {
                m_IsGrounded = false;
            }
        }
        else
        {
            m_IsGrounded = false;
        }
    }

    private void UpdateColor(Material newMaterial)
    {
        m_MeshRenderer.material = newMaterial;
        ParticleSystem.MainModule main = m_TrailParticle.main;
        main.startColor = newMaterial.GetColor("_BaseColor");
    }

    private IEnumerator DieRoutine()
    {
        m_IsDieRoutineRunning = true;
        m_MeshRenderer.enabled = false;

        Vector3 appliedForce = m_Rigidbody.velocity;

        for (int i = 0; i < m_FracturedParts.Length; i++)
        {
            m_FracturedParts[i].Release(appliedForce, CurrentMaterial);
        }

        m_TrailParticle.transform.parent = null;
        m_TrailParticle.Stop();
        ParticleSystem.MainModule main = m_TrailParticle.main;
        main.stopAction = ParticleSystemStopAction.Destroy;

        PlayerControl = false;

        yield return new WaitForSeconds(0.3f);

        IsPlayerDied = true;
        m_IsDieRoutineRunning = false;

        Destroy(gameObject);
    }

    public void Die()
    {
        if (m_IsDieRoutineRunning) return;

        StartCoroutine(DieRoutine());
    }
}
