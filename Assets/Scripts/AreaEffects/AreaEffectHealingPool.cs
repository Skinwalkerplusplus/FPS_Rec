using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaEffectHealingPool : AreaEffectGeneric
{
    public delegate void IncreaseHealth();
    public static IncreaseHealth increaseHealth;

    private IEnumerator coroutine;

    void Start()
    {
        coroutine = MachineGunFire();
    }

    protected override void Effect()
    {
        StartCoroutine(coroutine);
    }

    protected override void EffectEnd()
    {
        base.EffectEnd();
        StopCoroutine(coroutine);
    }

    public IEnumerator MachineGunFire()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            if (increaseHealth != null)
                increaseHealth();
        }
    }

}
