using System.Security;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DataManager
{
    private readonly static string LAST_PLAYED_LEVEL = "LPL";

    public static void SetLastPlayedLevel(string levelName)
    {
        PlayerPrefs.SetString(LAST_PLAYED_LEVEL, levelName);
        PlayerPrefs.Save();
    }

    public static string GetLastPlayedLevel()
    {
        return PlayerPrefs.GetString(LAST_PLAYED_LEVEL, LevelManager.Instance.initialLevel);
    }
}
