using System.Collections;
using UnityEngine;

public class fallingBoulderSound : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.GetComponent<AudioSource>().enabled =false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.tag == "ground"){
            StartCoroutine(PlaySound());
        }
    }
    IEnumerator PlaySound(){
        this.gameObject.GetComponent<AudioSource>().enabled =true;
        yield return new WaitForSeconds(0.5f);
        this.gameObject.GetComponent<AudioSource>().enabled =false;
    }
}
