using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject playerUnStun;
    public Animator playerAnimator;
    void Start()
    {
        StartCoroutine(FreeFromStunRoutine());
    }

    IEnumerator FreeFromStunRoutine()
    {
        yield return new WaitForSeconds(3.0f);
        playerUnStun.transform.position = new Vector3(transform.position.x, -5.01f, transform.position.z);
        playerAnimator.SetBool("unStun", true);
        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
