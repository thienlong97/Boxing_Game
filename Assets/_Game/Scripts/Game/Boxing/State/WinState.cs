using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinState : IState
{
    private Fighter fighter;
    public void OnEnter(Fighter fighter)
    {
        this.fighter = fighter;
        fighter.FighterAnimator.PlayWin();
    }

    public void OnExit()
    {
        
    }

    public void OnUpdate()
    {
        
    }

}
