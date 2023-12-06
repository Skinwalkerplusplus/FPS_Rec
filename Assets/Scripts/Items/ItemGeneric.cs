using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGeneric : MonoBehaviour, IItemExecutable
{
    public AudioSource audioSource;

    [SerializeField]
    private string itemName;

    [SerializeField]
    private float itemExpireTime;

    

    //protected FPSPlayer FPSplayer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnteredByPlayer()
    {
        //FPSplayer = player;

        audioSource.Play();

        //GetComponent<MeshRenderer>().enabled = false;
        GetComponent<Collider>().enabled = false;

        itemExpireTime = 0;
        GetComponentInChildren<MeshRenderer>().enabled = false;
        ExecuteAction();
    }

    protected virtual void ExecuteAction()
    {

    }
}
