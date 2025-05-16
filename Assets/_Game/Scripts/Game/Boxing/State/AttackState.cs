using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IState
{
    private Fighter fighter;

    public void OnEnter(Fighter fighter)
    {
        Debug.Log(fighter.name + " is now Attack.");
        this.fighter = fighter;
        this.fighter.Attack();
        this.fighter.AttackHitbox.ActivateHitbox();
    }

    public void OnUpdate()
    {
        // Attack behavior
        if (fighter.FighterAnimator.IsAttackAnimationFinished(fighter))
            fighter.ChangeState(new IdleState());
    }

    public void OnExit()
    {
        this.fighter.AttackHitbox.DeactivateAll();
        Debug.Log(fighter.name + " exited Attack state.");
    }


}