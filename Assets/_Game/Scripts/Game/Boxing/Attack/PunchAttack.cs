using UnityEngine;

[CreateAssetMenu(menuName = "Attack/PunchAttack")]
public class PunchAttack : BaseAttackStrategy
{
    [SerializeField] private AttackType attackType;

    public override AttackType GetAttackType()
    {
        return attackType;
    }
}
