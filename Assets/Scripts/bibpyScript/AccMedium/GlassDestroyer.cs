using System.Collections;
using UnityEngine;

public class GlassDestroyer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
         StartCoroutine(removeglass());
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    IEnumerator removeglass()
    {
    yield return new WaitForSeconds(2);
    Destroy(gameObject);
    }
}
