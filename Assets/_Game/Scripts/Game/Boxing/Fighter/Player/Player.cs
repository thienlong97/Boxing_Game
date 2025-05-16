using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Fighter
{
    private void Start()
    {
        ChangeState(new IdleState());
    }

    protected override void Update()
    {
        base.Update();
    }
}
