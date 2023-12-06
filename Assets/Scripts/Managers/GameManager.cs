using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public float playerHealth;

    public Image health;

    public int scoreCache;
    public int score = 0;
    public string scoreKey = "Score";

    public delegate void GameOver();
    public static GameOver gameOver;

    public delegate void DeadEnemy();
    public static DeadEnemy deadEnemy;

    //public int amount = 10;

    public TextMeshProUGUI currentScore;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        EnemyBehavior.enemyAttack += DecreaseHealth;
        EnemyProjectile.rangedEnemyAttack += DecreaseHealth;
        EnemyBehavior.enemyDeath1 += IncreaseScore;
        ItemCoin.coinCollected += IncreaseScore;
        RangedEnemyBehavior.enemyDeath2 += IncreaseScore;
        ItemHealth.increaseHealth += IncreaseHealth;
        CubeEnemy.scoreCube += IncreaseScore;
        AreaEffectHealingPool.increaseHealth += IncreaseHealth;

        if (PlayerPrefs.HasKey(scoreKey))
        {
            score = PlayerPrefs.GetInt(scoreKey);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (health != null)
        {
            health.fillAmount = playerHealth;

            if (playerHealth <= 0)
            {
                if (gameOver != null)
                    gameOver();
            }

            currentScore.text = "" + scoreCache;
        }
        
    }

    public void IncreaseScore(int scoreIn)
    {
        if (deadEnemy != null)
            deadEnemy();

        scoreCache += scoreIn;

        if (scoreCache > PlayerPrefs.GetInt(scoreKey))
        {
            PlayerPrefs.SetInt(scoreKey, scoreCache);
            PlayerPrefs.Save();
            score = scoreCache;
        }

        Debug.Log("Score increased! Current score: " + score);
    }

    public void ResetScore()
    {
        // Reset the score to zero
        score = 0;

        // Remove the score key from PlayerPrefs
        PlayerPrefs.DeleteKey(scoreKey);
        PlayerPrefs.Save();

        Debug.Log("Score reset to zero.");
    }

    public void DecreaseHealth()
    {
        playerHealth -= 0.05f;
    }

    public void IncreaseHealth()
    {
        playerHealth += 0.10f;
        if(playerHealth > 1)
        {
            playerHealth = 1;
        }
    }

    //public void IncreaseScoreCube(int scoreAmount)
    //{
    //    if (deadEnemy != null)
    //        deadEnemy();

    //    scoreCache += scoreAmount;

    //    if (scoreCache > PlayerPrefs.GetInt(scoreKey))
    //    {
    //        PlayerPrefs.SetInt(scoreKey, scoreCache);
    //        PlayerPrefs.Save();
    //        score = scoreCache;
    //    }
        
        
    //    Debug.Log("Score increased! Current score: " + score);
    //}
}
