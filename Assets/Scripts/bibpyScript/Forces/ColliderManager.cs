using UnityEngine;

public class ColliderManager : MonoBehaviour
{
     public bool collide;
     private PlayerB thePlayer;
     public GameObject braker; 
     private ForceManagerThree theManagerThree;
     

    // Start is called before the first frame update
    void Start()
    {
        thePlayer = FindObjectOfType<PlayerB>();
        
    }
    // Update is called once per frame
    void Update()
    {
        theManagerThree = FindObjectOfType<ForceManagerThree>();
    }
        
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == ("wall"))
        {
            
           collide = true;   
        }
        if (collision.gameObject.tag == ("braker"))
        {
           thePlayer.moveSpeed = 0; 
           thePlayer.addweights = true;
           braker.SetActive(false);
           ForceSimulation.simulate = false;
           theManagerThree.addingWeight = true;
        }

    }
}
