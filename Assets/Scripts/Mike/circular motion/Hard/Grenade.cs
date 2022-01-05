 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField]Transform parent, playerHand;
    float flightTime = 0;
    Vector2 initState;
    public bool isThrown = false, explode = false;
    bool willExplode = false;
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.GetComponent<Transform>().SetParent(playerHand);
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        initState = rb.velocity;
        rb.isKinematic = true;

    }

    // Update is called once per frame
    void Update()
    {
        if(isThrown){
            rb.bodyType = RigidbodyType2D.Dynamic;
            flightTime += Time.deltaTime;
            rb.velocity = new Vector2(-6f, (15f)*(1f-flightTime));
        }
        else
        {
            // rb.bodyType = RigidbodyType2D.Kenimatic;
            // rb.isKinematic = true;
            flightTime = 0;
            rb.velocity = initState;
        }
        if(willExplode){
            StartCoroutine(Explode());
        }
    }
    IEnumerator Explode(){
        willExplode = false;
        yield return new WaitForSeconds(0.5f);
        explode = true;
    }
    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.name == "head"){
            isThrown = false;
            this.gameObject.GetComponent<Transform>().SetParent(parent);
            willExplode = true;
        }
    }
}
