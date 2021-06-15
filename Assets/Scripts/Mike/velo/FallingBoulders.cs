using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingBoulders : MonoBehaviour
{
    [SerializeField] GameObject[] boulders;
    public static bool isRumbling, boulderDrop;
    bool first = true;
    float rotateTime;
    public static float dropPoint, moreDropPoint;
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
        else
            boulderDrop = false;
    }
    IEnumerator Dropping()
    {
        if (RumblingManager.fallingBouder)
        {
            RumblingManager.fallingBouder = false;
            for (int i = 0; i < (Random.Range(3, 5)); i++)
            {
                GameObject boulder = Instantiate(boulders[Random.Range(0, 2)]);
                boulder.transform.position = new Vector2(Random.Range(-5f, dropPoint), Random.Range(11.5f, 14));
                boulder.transform.localScale = new Vector2(this.transform.localScale.x * Random.Range(0.05f, 0.3f), this.transform.localScale.y * Random.Range(0.2f, 0.5f));
                Rigidbody2D boulderRB = boulder.GetComponent<Rigidbody2D>();
                boulderRB.gravityScale = Random.Range(1f, 2f);
                boulderRB.mass = Random.Range(225, 300);
                boulderRB.rotation = 90 * rotateTime;
                yield return new WaitForSeconds(0.3f);
            } 
            if (VelocityMediumManager.stage == 3)
                StartCoroutine(DropMore());
        }
    }
    IEnumerator DropMore()
    {
        for (int i = 0; i < (Random.Range(3, 5)); i++)
        {
            GameObject moreBoulders = Instantiate(boulders[Random.Range(0, 2)]);
            moreBoulders.transform.position = new Vector2(Random.Range(moreDropPoint, 35f), Random.Range(11.5f, 14));
            moreBoulders.transform.localScale = new Vector2(this.transform.localScale.x * Random.Range(0.05f, 0.3f), this.transform.localScale.y * Random.Range(0.2f, 0.5f));
            Rigidbody2D moreBoulderRB = moreBoulders.GetComponent<Rigidbody2D>();
            moreBoulderRB.gravityScale = Random.Range(1f, 2f);
            moreBoulderRB.mass = Random.Range(225, 300);
            moreBoulderRB.rotation = 90 * rotateTime;
        }
        yield return new WaitForSeconds(0.3f);
    }
}