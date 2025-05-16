using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackSO", menuName = "AttackSO")]

public class AttackSO : SingletonScriptableObject<AttackSO>
{
    [SerializeField] private List<BaseAttackStrategy> listAttackData = new();
    public List<BaseAttackStrategy> ListAttackData => listAttackData;
    public BaseAttackStrategy GetAttackByType(AttackType type)
    {
        return listAttackData.Find(strategy => strategy.GetAttackType() == type);
    }
}
