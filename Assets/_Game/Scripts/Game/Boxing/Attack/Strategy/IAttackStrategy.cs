using UnityEngine;

public enum AttackType
{
    Head,
    Stomach,
    PunchLeft,
    PunchRight
}

public interface IAttackStrategy
{
    void Attack(Fighter fighter);
}