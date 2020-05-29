using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : Singleton<UIManager>
{
    public Button startGameButton;
    public Button nextLevelButton;
    public TextMeshProUGUI levelNameText;
    public float levelNameDisplayTime = 1f;
    public float levelNameScaleTime = 0.5f;

    private bool m_IsLevelNameDisplaying = false;

    private IEnumerator LevelNameDisplayRoutine(string levelName)
    {
        m_IsLevelNameDisplaying = true;

        levelNameText.text = levelName;
        levelNameText.transform.localScale = Vector3.zero;
        levelNameText.gameObject.SetActive(true);
        float fadeTime = 0f;
        while(true)
        {
            float t = fadeTime / levelNameScaleTime;

            levelNameText.transform.localScale = Vector3.one * t;

            fadeTime += Time.deltaTime;

            if(t >= 1f)
            {
                break;
            }

            yield return null;
        }

        yield return new WaitForSeconds(levelNameDisplayTime);

        while (true)
        {
            float t = fadeTime / levelNameScaleTime;

            levelNameText.transform.localScale = Vector3.one * t;

            fadeTime -= Time.deltaTime;

            if (t <= 0f)
            {
                break;
            }

            yield return null;
        }

        levelNameText.gameObject.SetActive(false);
        m_IsLevelNameDisplaying = false;
    }

    private void DisableLevelName()
    {
        levelNameText.gameObject.SetActive(false);
    }

    public void DisplayLevelName(string levelName)
    {
        if (m_IsLevelNameDisplaying) return;

        StartCoroutine(LevelNameDisplayRoutine(levelName));
    }

    public void DisplayStartGameButton()
    {
        startGameButton.gameObject.SetActive(true);
    }

    public void DisableStartGameButton()
    {
        startGameButton.gameObject.SetActive(false);
    }

    public void DisplayNextLevelButton()
    {
        nextLevelButton.gameObject.SetActive(true);
    }
    
    public void DisableNextLevelButton()
    {
        nextLevelButton.gameObject.SetActive(false);
    }

    public void DisableButtons()
    {
        DisableStartGameButton();
        DisableNextLevelButton();
        DisableLevelName();
    }
}
