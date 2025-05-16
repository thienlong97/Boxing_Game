using System;
using System.Collections.Generic;
using UnityEngine;

public class FighterAnimator
{
    [SerializeField] private Animator animator;
    public Animator Animator => animator;

    public FighterAnimator (Animator animator)
    {
        this.animator = animator;
    }

    private static readonly int AttackLayer = 1;

    private static readonly int Idle = Animator.StringToHash("Idle");
    private static readonly int Die  = Animator.StringToHash("Die");
    private static readonly int Win  = Animator.StringToHash("Win");

    private static readonly Dictionary<AttackType, int> hitHashes = new Dictionary<AttackType, int>
    {
        { AttackType.Head, Animator.StringToHash("Head_Hit") },
        { AttackType.Stomach, Animator.StringToHash("Stomach_Hit") },
        { AttackType.PunchLeft, Animator.StringToHash("Punch_Hit") },
        { AttackType.PunchRight, Animator.StringToHash("Punch_Hit") }
    };

    private static readonly Dictionary<AttackType, int> attackHashes = new Dictionary<AttackType, int>
    {
        { AttackType.Head, Animator.StringToHash("Head_Attack") },
        { AttackType.Stomach, Animator.StringToHash("Stomach_Attack") },
        { AttackType.PunchLeft, Animator.StringToHash("PunchLeft_Attack") },
        { AttackType.PunchRight, Animator.StringToHash("PunchRight_Attack") }
    };

    public void PlayIdle() => animator.SetTrigger(Idle);
    public void PlayDie() => animator.SetTrigger(Die);
    public void PlayWin() => animator.SetTrigger(Win);
    public void PlayAttack(AttackType attackType) => PlayAnimation(attackType, attackHashes, "Attack");
    public void PlayHit(AttackType attackType) => PlayAnimation(attackType, hitHashes, "Hit");

    private void PlayAnimation(AttackType type, Dictionary<AttackType, int> hashDictionary, string action)
    {
        if (hashDictionary.TryGetValue(type, out int hash))
        {
            animator.SetTrigger(hash);
        }
        else
        {
            Debug.LogWarning($"Animation for {action} Type {type} not found!");
        }
    }

    public bool IsAttackAnimationFinished(Fighter fighter)
    {
        AnimatorStateInfo stateInfo = fighter.FighterAnimator.Animator.GetCurrentAnimatorStateInfo(AttackLayer);
        return stateInfo.IsTag("Attack") && stateInfo.normalizedTime >= 1f;
    }
}
