using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HighScoreManager : MonoBehaviour
{
    public GameManager gm;

    public TextMeshProUGUI highScore;
    public TextMeshProUGUI thisScore;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        highScore.text = "" + gm.score;
        Debug.Log(gm.score);
        thisScore.text = "" + gm.scoreCache;
    }
}
