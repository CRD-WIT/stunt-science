using System.Collections;
using UnityEngine;

public class ZombieManager : MonoBehaviour
{
    public GameObject zombie;
    private ForceSimulation theSimulate;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(destroyZombies());
        theSimulate = FindObjectOfType<ForceSimulation>();
        if(theSimulate.stage == 1 || theSimulate.stage == 3)
        {
            GetComponent<Rigidbody2D>().velocity = transform.right * Random.Range(15, 25);
        }
         if(theSimulate.stage == 2)
        {
            GetComponent<Rigidbody2D>().velocity = transform.right *  Random.Range(-15, -25);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator destroyZombies()
    {
        yield return new WaitForSeconds(5);
        Destroy(zombie);
    }
}
