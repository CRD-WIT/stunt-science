using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakSpotManager : MonoBehaviour
{
    public GameObject smallBlast;
    public golem theGolem;
    public ProjHardSimulation theSimulate;
    public AudioSource swordSlash;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
     void OnTriggerEnter2D(Collider2D other)
    {


        if (other.gameObject.tag == ("weapon"))
        {  
            swordSlash.Play();
            smallBlast.SetActive(true);
            theGolem.damage = true;
            StartCoroutine(theGolem.takeDamaged());
            GetComponent<SpriteRenderer>().color = new Color32(0, 241, 10, 255);
        }
    }
}
