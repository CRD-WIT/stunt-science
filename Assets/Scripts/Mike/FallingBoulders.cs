using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingBoulders : MonoBehaviour
{    [SerializeField] GameObject[] boulders;
    public static bool isRumbling, boulderDrop;
    bool first = true;
    float rotateTime;
    public static float dropPoint;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        rotateTime += Time.deltaTime;
        if (boulderDrop)
        {
            StartCoroutine(Dropping());
        }
        if (isRumbling)
            boulderDrop = RumblingManager.shakeON;
        else if (!isRumbling)
            boulderDrop = false;
    }
    IEnumerator Dropping()
    {
        // isRumbling = false;
        if (RumblingManager.fallingBouder)
        {
            RumblingManager.fallingBouder = false;
            for (int i = 0; i < (Random.Range(3, 5)); i++)
            {
                GameObject boulder = Instantiate(boulders[Random.Range(0, 2)]);
                boulder.transform.position = new Vector2(Random.Range(-5, dropPoint), Random.Range(11.5f, 14));
                boulder.transform.localScale = new Vector2(this.transform.localScale.x * Random.Range(0.05f, 0.3f), this.transform.localScale.y * Random.Range(0.2f, 0.5f));
                Rigidbody2D boulderRB = boulder.GetComponent<Rigidbody2D>();
                boulderRB.gravityScale = Random.Range(1f, 2f);
                boulderRB.mass = Random.Range(225, 300);
                boulderRB.rotation = 90 * rotateTime;
            }
            // boulderRB.rotation = 90 * rotateTime;
            // boulderDrop = false;
            yield return new WaitForSeconds(0.3f);
        }
    }
}