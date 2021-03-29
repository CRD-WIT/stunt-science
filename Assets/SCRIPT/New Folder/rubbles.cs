using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rubbles : MonoBehaviour
{
    public Rigidbody2D rubblesRB;
    // Start is called before the first frame update
    void Start()
    {
        rubblesRB = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rubblesRB.velocity = new Vector2(rubblesRB.velocity.x, Random.Range(-1, -20));
        StartCoroutine(destroyrub());
    }
    IEnumerator destroyrub()
    {
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }
}
