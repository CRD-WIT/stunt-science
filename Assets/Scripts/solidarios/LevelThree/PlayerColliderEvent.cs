using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerColliderEvent : MonoBehaviour
{
    public bool isCollided;
    void Start()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "PlatformBar")
        {
            isCollided = true;
        }

    }

}
