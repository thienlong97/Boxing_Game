using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieState : IState
{
    private Fighter fighter;

    public void OnEnter(Fighter fighter)
    {
        this.fighter = fighter;
        fighter.FighterAnimator.PlayDie();
        fighter.StopAllCoroutines();
        fighter.Invoke(nameof(fighter.HideFighter), 1.5f);
    }

    public void OnUpdate() 
    {

    }

    public void OnExit() 
    { 

    }
}
