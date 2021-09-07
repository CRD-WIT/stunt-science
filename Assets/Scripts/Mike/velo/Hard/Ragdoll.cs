using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    public float moveSpeedforward;
    private Rigidbody2D myRigidbody;
    public GameObject stick;
    public GameObject stickloc;
    public bool destroyRagdoll;
    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        moveSpeedforward -= 2 * Time.deltaTime;
        myRigidbody.velocity = new Vector2(moveSpeedforward, myRigidbody.velocity.y);
        if (moveSpeedforward <= 0)
        {
            moveSpeedforward = 0;
        }
        if (destroyRagdoll)
        {
            StartCoroutine(playerSpawn());
        }
    }
    IEnumerator playerSpawn()
    {
        destroyRagdoll = false;
        yield return new WaitForSeconds(3);
        Destroy(stick.gameObject);
    }
}
