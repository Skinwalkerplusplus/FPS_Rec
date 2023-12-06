using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaEffectGeneric : MonoBehaviour, IAreaEffectPlayer
{
    public void PlayerEnter()
    {
        Effect();
    }

    public void PlayerExit()
    {
        EffectEnd();
    }

    protected virtual void Effect()
    {

    }

    protected virtual void EffectEnd()
    {

    }
}
