using UnityEngine;
using System;

public class DataManager : GenericSingleton<DataManager>
{
    private const string LevelKey = "LevelGame";

    public static event Action<int> OnLevelChanged;

    private int levelGame
    {
        get => PlayerPrefs.GetInt(LevelKey, 0);
        set
        {
            PlayerPrefs.SetInt(LevelKey, value);
            OnLevelChanged?.Invoke(value);
        }
    }

    public int LevelGame => levelGame;
    public void LevelUp() => levelGame++;

    private void OnEnable() => BoxingManager.onLevelCompleted += LevelUp;
    private void OnDisable() => BoxingManager.onLevelCompleted -= LevelUp;
}
