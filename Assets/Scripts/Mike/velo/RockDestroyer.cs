using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockDestroyer : MonoBehaviour
{
    // Start is called before the first frame update
    public bool isDestroyed;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!isDestroyed){
            StartCoroutine(DestroyObj());
        }
    }
    public IEnumerator DestroyObj(){
        yield return new WaitForSeconds(2);
        Destroy(this.gameObject);
        isDestroyed = true;    
    }
}
