
using UnityEngine;

public class HitState : IState
{
    private Fighter fighter;
    private AttackType hitFromAttackType;

    public HitState(AttackType hitFromAttackType)
    {
        this.hitFromAttackType = hitFromAttackType;
    }

    public void OnEnter(Fighter fighter)
    {
        this.fighter = fighter;
        this.fighter.FighterAnimator.PlayHit(hitFromAttackType);
    }

    public void OnExit()
    {
        
    }

    public void OnUpdate()
    {
        if (fighter.FighterAnimator.IsAttackAnimationFinished(fighter))
            fighter.ChangeState(new IdleState());
    }
}
