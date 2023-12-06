using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCoin : ItemGeneric
{
    public delegate void CoinCollected(int score);
    public static CoinCollected coinCollected;

    protected override void ExecuteAction()
    {
        base.ExecuteAction();
        if (coinCollected != null)
            coinCollected(10);
        Destroy(this.gameObject);
    }

}
