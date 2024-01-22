using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public float playerHealth;

    public Image health;
    public GameObject lowHealth;
    public GameObject HUD;
    public GameObject finalScore;

    public int scoreCache;
    public int score = 0;
    public string scoreKey = "Score";

    public delegate void GameOver(string gameOverScene);
    public static GameOver gameOver;

    public delegate void DeadEnemy();
    public static DeadEnemy deadEnemy;

    //public int amount = 10;

    public TextMeshProUGUI currentScore;
    public TextMeshProUGUI[] ammoTexts;

    private static GameManager instance;
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

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
        SceneManager.sceneLoaded += SceneLoaded;

        if (PlayerPrefs.HasKey(scoreKey))
        {
            score = PlayerPrefs.GetInt(scoreKey);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        
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
        if (health != null)
        {
            health.fillAmount = playerHealth;

            if (playerHealth <= 0)
            {
                if (gameOver != null)
                    gameOver("GameOver");
            }

            if (playerHealth <= 0.3)
            {
                lowHealth.SetActive(true);
            }

            else
            {
                lowHealth.SetActive(false);
            }

            currentScore.text = "" + scoreCache;
        }
    }

    public void IncreaseHealth()
    {
        playerHealth += 0.10f;
        if(playerHealth > 1)
        {
            playerHealth = 1;
        }
    }

    public void SceneLoaded(Scene scene, LoadSceneMode mode)
    {
        playerHealth = 1;

        Debug.Log("Scene loaded: " + scene);
        if(scene.name == "SampleScene")
        {
            HUD.SetActive(true);
            finalScore.SetActive(false);

        }

        else
        {
            HUD.SetActive(false);
            finalScore.SetActive(true);
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
