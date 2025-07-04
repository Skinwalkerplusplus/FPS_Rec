using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerSpeed : MonoBehaviour
{
    public float mult = 10f;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = mult;
        Time.fixedDeltaTime = 0.02f / Time.timeScale;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
