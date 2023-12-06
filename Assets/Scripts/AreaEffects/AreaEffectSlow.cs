using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaEffectSlow : AreaEffectGeneric
{
    public PlayerMovement playerMovement;

    protected override void Effect()
    {
        playerMovement.movSpeed = 3f;
        playerMovement.canSprint = false;
    }

    protected override void EffectEnd()
    {
        base.EffectEnd();
        playerMovement.movSpeed = 5f;
        playerMovement.canSprint = true;
    }
}
