using UnityEngine;
using System.Collections;

public class RagdollV2 : MonoBehaviour
{
    public float moveSpeedforward;
    private Rigidbody2D myRigidbody;
    public GameObject stick;
    public GameObject stickloc;
    public static bool ragdollEnabled, disableRagdoll;
    public static PlayerV2 myPlayer;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        moveSpeedforward -= 2 * Time.deltaTime;
        myRigidbody.velocity = new Vector2(moveSpeedforward, myRigidbody.velocity.y);
        if (moveSpeedforward <= 0)
        {
            moveSpeedforward = 0;
        }
        if (this.gameObject.activeSelf)
        {
            disableRagdoll = true;
        }
        if (disableRagdoll)
        {
            // ragdollEnabled = true;
            StartCoroutine(playerSpawn());
        }
    }
    IEnumerator playerSpawn()
    {
        disableRagdoll = false;
        yield return new WaitForSeconds(3);
        Destroy(stick.gameObject);
        myPlayer.moveSpeed = 0;
        myPlayer.gameObject.transform.position = stickloc.transform.position;
        myPlayer.gameObject.SetActive(true);
    }
}