using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Match3;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public enum FighterType 
{ 
    Player,
    Enemy
}

[System.Serializable]
public class TeamData
{   
    public List<Fighter> activeFighters = new();
    public float GetPercentHealth() => CurrentHealth() / MaxHealth();
    private float MaxHealth() => activeFighters.Count * MaxHealthConst;
    private float CurrentHealth()
    {
        float currentHealth = 0;
        for (int i = 0; i < activeFighters.Count; i++)
            currentHealth += activeFighters[i].Health;
        return currentHealth;
    }

    private const float MaxHealthConst = 100.0f;
}

public class BoxingManager : GenericSingleton<BoxingManager>
{
    [Header("Fighters Reference")]
    [SerializeField] private Fighter[] players;
    [SerializeField] private Fighter[] enemies;
    [SerializeField] private Transform[] pointsPlayers;
    [SerializeField] private Transform[] pointsEnemies;

    private BoxingLevelConfig boxingLevelConfig;

    private readonly Dictionary<FighterType, TeamData> activeFighters = new();
    public Dictionary<FighterType, TeamData> ActiveTeam => activeFighters;

    #region Action Event
    public static Action<Fighter> onAnyFighterDie;
    public static Action onAnyFighterTakeDamage;
    public static Action onAnyFighterApplyHealth;
    public static Action onLevelCompleted;
    public static Action onLevelFailed;
    public static Action onLevelStarted;
    #endregion

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    private void OnEnable()
    {
        onAnyFighterDie += OnAnyFighterDie;
        Match3Manager.onMatch += HandleMatch3;
    }

    private void OnDisable()
    {
        onAnyFighterDie -= OnAnyFighterDie;
        Match3Manager.onMatch -= HandleMatch3;
    }

    private void Start() => GenerateLevel();

    public void GenerateLevel()
    {
        ResetFighters();
        SetupLevelConfig();
        SpawnFighter();

        onLevelStarted?.Invoke();
    }

    private void SetupLevelConfig()
    {
        int level = DataManager.Instance.LevelGame;
        boxingLevelConfig = BoxingSO.Instance.GetBoxingLevelConfig(level);
    }

    private void ResetFighters()
    {
        activeFighters[FighterType.Player] = new TeamData();
        activeFighters[FighterType.Enemy] = new TeamData();

        foreach (var player in players) player.HideFighter();
        foreach (var enemy in enemies) enemy.HideFighter();
    }

    private void SpawnFighter()
    {
        SpawnByType(FighterType.Player, players, pointsPlayers, boxingLevelConfig.PlayersCount);
        SpawnByType(FighterType.Enemy, enemies, pointsEnemies, boxingLevelConfig.EnemiesCount);
    }

    private void SpawnByType(FighterType type, Fighter[] fighterArray, Transform[] points, int count)
    {
        for (int i = 0; i < count; i++)
        {
            fighterArray[i].ActiveFighter(points[i], ActiveTeam[type], type);
            activeFighters[type].activeFighters.Add(fighterArray[i]);
        }
    }

    public void OnAnyFighterDie(Fighter fighter)
    {
        if (activeFighters[FighterType.Enemy].GetPercentHealth() == 0) onLevelCompleted?.Invoke();
        else if (activeFighters[FighterType.Player].GetPercentHealth() == 0) onLevelFailed?.Invoke();
    }

    private void HandleMatch3(CellConfig.CellType cellType, int combo)
    {
        if(CellConfig.CellType.Health == cellType) ApplyHealth(combo);
        else PerformAttack(combo, cellType);
    }

    private void ApplyHealth(int amount)
    {
        for (int i = 0; i < activeFighters[FighterType.Player].activeFighters.Count; i++)
        {
            var player = activeFighters[FighterType.Player].activeFighters[i];
            int _healthAmount = (int)(amount * 2);
            player.ApplyHealth(_healthAmount);
        }
    }

    private void PerformAttack(int combo, CellConfig.CellType cellType)
    {
        for (int i = 0; i < activeFighters[FighterType.Player].activeFighters.Count; i++)
        {
            var player = activeFighters[FighterType.Player].activeFighters[i];
            AttackType attackType = ConvertCellTypeToAttackType(cellType);
            player.SetAttackStrategy(attackType, combo);
            player.ChangeState(new AttackState());
        }
    }

    public static AttackType ConvertCellTypeToAttackType(CellConfig.CellType cellType)
    {
        switch (cellType)
        {
            case CellConfig.CellType.NormalAttack:
                return UnityEngine.Random.value > 0.5f ? AttackType.PunchLeft : AttackType.PunchRight;
            case CellConfig.CellType.HeadAttack:
                return AttackType.Head;
            case CellConfig.CellType.StomachAttack:
                return AttackType.Stomach;
        }

        return AttackType.PunchLeft;
    }

    public Fighter GetClosestFighterTarget(FighterType fighterType, Vector3 currentPosition)
    {
        return ActiveTeam[fighterType].activeFighters
            .Where(fighter => fighter.gameObject.activeSelf)
            .OrderBy(fighter => Vector3.Distance(fighter.transform.position, currentPosition))
            .FirstOrDefault();
    }
}
