using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // public GameObject ragdollBody;
    public Animator playerAnimator;
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Debri")
        {
            playerAnimator.SetBool("didFail", true);
        }
    }

    void Start()
    {

    }

}
