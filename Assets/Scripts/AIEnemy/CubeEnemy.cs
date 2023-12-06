using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeEnemy : MonoBehaviour, IEnemyBasic
{
    public float enemyHealth;
    public int scoreAwarded;

    public delegate void ScoreCube(int score);
    public static ScoreCube scoreCube;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(enemyHealth <= 0)
        {
            EnemyDeath();
        }
    }

    public void RecieveDamage(float damageTaken)
    {
        enemyHealth -= damageTaken;
    }

    protected virtual void EnemyDeath()
    {
        if (scoreCube != null)
            scoreCube(scoreAwarded);
        Destroy(this.gameObject);
    }
}
