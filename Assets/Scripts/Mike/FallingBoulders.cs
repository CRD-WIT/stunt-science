using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingBoulders : MonoBehaviour
{
    [SerializeField] Transform playerPos;
    [SerializeField] GameObject[] boulders;
    public static bool isRumbling, boulderDrop;
    float rotateTime;
    // Start is called before the first frame update
    void Start()
    {
        isRumbling =true;
    }

    // Update is called once per frame
    void Update()
    {
        rotateTime += Time.deltaTime;
        if (isRumbling)
        {
            StartCoroutine(Dropping());
        }

        else
            boulderDrop = true;
    }
    IEnumerator Dropping()
    {
        isRumbling = false;
        if (boulderDrop)
        {
            float dropPoint = playerPos.position.x - 0.5f;
            GameObject boulder = Instantiate(boulders[Random.Range(0, 2)]);
            boulder.transform.position = new Vector2(Random.Range(-20, dropPoint), Random.Range(11.5f, 14));
            Rigidbody2D boulderRB = boulder.GetComponent<Rigidbody2D>();
            boulderRB.mass = Random.Range(225, 300);
            // boulderRB.rotation = 90 * rotateTime;
            boulderDrop = false;
            yield return new WaitForSeconds(0.3f);
            isRumbling =true;
        }
    }

}
