using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    public string initialLevel;
    public string[] levels;

    public string currentLevelName;
    public int currentLevelIndex;

    protected override void Awake()
    {
        base.Awake();

        currentLevelName = DataManager.GetLastPlayedLevel();
        currentLevelIndex = Array.IndexOf(levels, currentLevelName);
    }

    public string GetNextLevel()
    {
        currentLevelIndex = (currentLevelIndex + 1) % levels.Length;

        currentLevelName = levels[currentLevelIndex];

        return currentLevelName;
    }

    public string GetCurrentLevel()
    {
        return currentLevelName;
    }
}