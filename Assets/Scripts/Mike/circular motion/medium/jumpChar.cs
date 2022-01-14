using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jumpChar : MonoBehaviour
{
    public GameObject ragdoll, ragdollSpantPnt;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == ("stickmanspawn"))
        {
            ragdollspawn();
            RagdollV2.disableRagdoll = true;
        }
    }
    
    public void ragdollspawn()
    {
        this.gameObject.SetActive(false);
        ragdoll.SetActive(true);
        GameObject stick = ragdoll;
        stick.transform.position = ragdollSpantPnt.transform.position;
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == ("stickmanspawn"))
        {
            ragdollspawn();
            RagdollV2.disableRagdoll = true;
        }
    }
}
