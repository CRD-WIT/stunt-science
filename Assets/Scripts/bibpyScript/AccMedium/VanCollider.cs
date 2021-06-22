using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VanCollider : MonoBehaviour
{
    public GameObject ragdollPrefab, stickmanPoint,player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == ("wall"))
        {
            ragdollSpawn();
            player.SetActive(false);
            AccMidSimulation.playerDead = true;
            
        }
    }
     public void ragdollSpawn()
    {
        GameObject stick = Instantiate(ragdollPrefab);
        stick.transform.position = stickmanPoint.transform.position;
    }
}
