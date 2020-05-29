using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractLevelController : AbstractTriggerable
{
    [HideInInspector] public bool isLevelSuccess;

    [SerializeField] protected Transform m_SpawnPoint;
    [SerializeField] protected BallController m_BallPrefab;

    protected GameManager m_GameManager;
    protected BallController m_BallController;

    #region Singleton
    protected static AbstractLevelController m_Instance;
    public static AbstractLevelController Instance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = FindObjectOfType<AbstractLevelController>();
                if (m_Instance == null)
                {
                    throw new System.NullReferenceException("Sahnede " + typeof(AbstractLevelController).Name + " türden nesne bulunmuyor.");
                }
            }

            return m_Instance;
        }
    }

    protected virtual void OnDestroy()
    {
        m_Instance = null;
    }
    #endregion

    public virtual void SetupLevel(GameManager manager)
    {
        this.m_GameManager = manager;

        isLevelSuccess = false;
    }

    public abstract IEnumerator LevelLoopRoutine();
}
