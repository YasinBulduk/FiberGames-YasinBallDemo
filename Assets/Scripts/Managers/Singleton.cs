using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    #region Variables
    private static T instance;
    #endregion


    #region Properties
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<T>();
                if (instance == null)
                {
                    throw new System.NullReferenceException("Sahnede " + typeof(T).Name + " türden nesne bulunmuyor.");
                }
            }
            return instance;
        }
    }
    #endregion


    #region Builtin Methods
    protected virtual void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        if (instance == null) instance = this as T;
        else DestroyImmediate(gameObject);
    }

    protected virtual void OnDestroy()
    {
        instance = null;
    }
    #endregion
}