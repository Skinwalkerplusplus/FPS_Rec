using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHealth : ItemGeneric
{
    public delegate void IncreaseHealth();
    public static IncreaseHealth increaseHealth;

    protected override void ExecuteAction()
    {
        base.ExecuteAction();
        if (increaseHealth != null)
            increaseHealth();
    }
}
