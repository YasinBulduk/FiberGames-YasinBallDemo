using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    //Game Based Systems
    [HideInInspector] public AbstractLevelController levelController;

    //Systems
    [HideInInspector] public LevelManager levelManager;
    [HideInInspector] public UIManager uiManager;

    [Header("System Prefabs")]
    [SerializeField] private GameObject m_UIManagerPrefab;
    [SerializeField] private GameObject m_LevelManagerPrefab;

    private AsyncOperation currentLevelOperation;
    private bool m_IsGameStart;
    private bool m_SceneLoading = false;
    private static int m_GameLoopCounter = 0;

    void Start()
    {
        Application.targetFrameRate = 60;

        InitializeSystems();

        SceneManager.LoadScene(levelManager.GetCurrentLevel());
        m_SceneLoading = true;

        StartCoroutine(GameLoop());
    }

    private void InitializeSystems()
    {
#if UNITY_EDITOR
        if (!m_UIManagerPrefab)
        {
            Debug.LogErrorFormat("[{0}] UI manager prefab is not setted. Game execution stopped.", this.gameObject.name);
            return;
        }
#endif
        uiManager = Instantiate(m_UIManagerPrefab).GetComponent<UIManager>();
        uiManager.DisableButtons();
        uiManager.startGameButton.onClick.AddListener(() => m_IsGameStart = true);
        uiManager.nextLevelButton.onClick.AddListener(LoadLevel);

#if UNITY_EDITOR
        if (!m_LevelManagerPrefab)
        {
            Debug.LogErrorFormat("[{0}] Level manager prefab is not setted. Game execution stopped.", this.gameObject.name);
            return;
        }
#endif
        levelManager = Instantiate(m_LevelManagerPrefab).GetComponent<LevelManager>();
    }

    #region Scene Management
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene loadedScene, LoadSceneMode sceneLoadMode)
    {
        if (loadedScene.name.Equals("BOOT")) return;

        levelController = AbstractLevelController.Instance;
        levelController.SetupLevel(this);
        m_SceneLoading = false;

        DataManager.SetLastPlayedLevel(loadedScene.name);

#if UNITY_EDITOR
        Debug.LogFormat("[{0}]Scene loaded: {1}", this.gameObject.name, loadedScene.name);
#endif
    }

    private void LoadSceneAsync(string sceneName)
    {
        currentLevelOperation = SceneManager.LoadSceneAsync(sceneName);
        currentLevelOperation.allowSceneActivation = false;

        m_SceneLoading = true;
    }

    public void LoadLevel()
    {
        if (currentLevelOperation != null)
        {
            currentLevelOperation.allowSceneActivation = true;
        }
        else
        {
#if UNITY_EDITOR
            Debug.LogErrorFormat("[{0}] Async operation is null. Loading CurrentLevel...", this.gameObject.name);
#endif

            //If Async operation fails. Load CurrentLevel
            SceneManager.LoadScene(levelManager.GetCurrentLevel());
        }
    }
    #endregion

    private void ResetGameBasedVariables()
    {
        m_IsGameStart = false;
        m_GameLoopCounter++;
    }

    private IEnumerator GameLoop()
    {
        while (true)
        {
            while (m_SceneLoading) yield return null; //wait player to push load level button

            //Start butonunu göster
            uiManager.DisplayStartGameButton();

            //Butona basılana kadar bekle
            while (!m_IsGameStart)
            {
                yield return null;
            }

            //Aktif level ismini göster
            uiManager.DisplayLevelName(levelManager.GetCurrentLevel());

            //Oyuncu bölümü bitirene kadar bekle
            yield return StartCoroutine(levelController.LevelLoopRoutine());

            //Yeni bölümü yükle
            LoadSceneAsync(levelManager.GetNextLevel());

            //Next Level Buttonunu göster
            uiManager.DisplayNextLevelButton();

            //Bölüm başı değişkenleri sıfırla
            ResetGameBasedVariables();
        }
    }
}
