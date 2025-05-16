using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Match3LevelConfig
{
    [Header("Grid Settings")]
    [Range(6, 8)]
    public int gridWidth;
    [Range(4, 6)]
    public int gridHeight;
    [Range(3, 4)]
    public int colorMax;
}

[Serializable]
public class CellConfig
{
    public CellType cellType;
    public Color colorType;
    public Color colorHighlight;
    public Sprite iconSprite;

    public enum CellType
    {
        Health          = 0,
        NormalAttack    = 1,
        HeadAttack      = 2,
        StomachAttack   = 3
    }
}

[CreateAssetMenu(fileName = "Match3LevelSO", menuName = "Match3LevelSO")]
public class Match3LevelSO : SingletonScriptableObject<Match3LevelSO>
{
    [SerializeField] private List<CellConfig> listCellConfig  = new();
    public List<CellConfig> ListCellConfig => listCellConfig;

    [SerializeField] private List<Match3LevelConfig> listMath3LevelConfig = new();
    public List<Match3LevelConfig> ListMath3LevelConfig => listMath3LevelConfig;

    public CellConfig GetRandomNewCellConfig => ListCellConfig[UnityEngine.Random.Range(0, ListCellConfig.Count)];

}
