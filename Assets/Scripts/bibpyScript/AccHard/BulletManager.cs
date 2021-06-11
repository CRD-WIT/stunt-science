using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(disolve());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == ("wall"))
        {
            Destroy(gameObject);
        }
    }
    IEnumerator disolve()
    {
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }
}
