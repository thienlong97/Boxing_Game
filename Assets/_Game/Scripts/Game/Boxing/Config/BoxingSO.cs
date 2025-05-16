using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BoxingLevelConfig
{
    [Range(1, 3)]
    public int PlayersCount;
    [Range(1, 3)]
    public int EnemiesCount;

}

[CreateAssetMenu(fileName = "BoxingSO", menuName = "BoxingSO")]
public class BoxingSO : SingletonScriptableObject<BoxingSO>
{
    [SerializeField] private List<BoxingLevelConfig> listBoxingLevelConfig = new();
    public List<BoxingLevelConfig> ListBoxingLevelConfig => listBoxingLevelConfig;

    public BoxingLevelConfig GetBoxingLevelConfig(int level)
    {
        if (level >= ListBoxingLevelConfig.Count) level = ListBoxingLevelConfig.Count - 1;
        return listBoxingLevelConfig[level];
    }
}
