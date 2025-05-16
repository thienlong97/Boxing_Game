using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IState
{
    private Fighter fighter;

    public void OnEnter(Fighter fighter)
    {
        this.fighter = fighter;
        this.fighter.FighterAnimator.PlayIdle();
        Debug.Log(fighter.name + " is now Idle.");
    }

    public void OnUpdate()
    {
        fighter.MoveLookTarget();
    }

    public void OnExit()
    {
        Debug.Log(fighter.name + " exited Idle state.");
    }
}