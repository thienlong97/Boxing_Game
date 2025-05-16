using Match3;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTask
{
    public CellConfig.CellType cellType;
    public int combo;

    public EnemyTask (CellConfig.CellType cellType, int combo)
    {
        this.cellType = cellType;
        this.combo = combo;
    }
}

public class Enemy : Fighter
{
    private Queue<EnemyTask> taskQueue = new();

    private void Start()
    {
        ChangeState(new IdleState());
    }

    private void OnEnable()
    {
        Match3Manager.onMatch += HandleMatch3;
        StartCoroutine(C_SimpleAI());
    }

    private void OnDisable()
    {
        Match3Manager.onMatch -= HandleMatch3;
    }

    protected override void Refresh()
    {
        base.Refresh();
        taskQueue.Clear();
    }

    private void HandleMatch3(CellConfig.CellType cellType, int combo)
    {       
        EnemyTask _enemyTask = new EnemyTask(cellType,combo);
        taskQueue.Enqueue(_enemyTask);
        Debug.Log("ADDTASk" + taskQueue.Count);
    }

    private IEnumerator C_SimpleAI()
    {
        while (true)
        {
            if (taskQueue.Count == 0)
            {
                yield return null;
                continue;
            }

            float _randomTime = Random.Range(1.0f, 3.0f);
            yield return new WaitForSeconds(_randomTime);

            EnemyTask _enemyTask = taskQueue.Dequeue();
            CellConfig.CellType cellType = _enemyTask.cellType;
            if (CellConfig.CellType.Health == cellType) /*ApplyHealth(_enemyTask.combo)*/;  
            else
            {
                if (!canAttack()) continue;
                AttackType attackType = BoxingManager.ConvertCellTypeToAttackType(cellType);
                SetAttackStrategy(attackType, _enemyTask.combo);
                ChangeState(new AttackState());
            }
        }
    }

    private bool canAttack()
    {
        int _myTeamCount = BoxingManager.Instance.ActiveTeam[FighterType.Enemy].activeFighters.Count;
        int _playerTeamCount = BoxingManager.Instance.ActiveTeam[FighterType.Player].activeFighters.Count;
        int _count = _myTeamCount - _playerTeamCount;
        if (_count == 0) return true;
        else
        {
            float _percent = 1.0f / (_count + 1);
            return Random.value < _percent;
        }
    }
}
