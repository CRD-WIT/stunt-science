using UnityEngine;

public class PlayerColliderEvent : MonoBehaviour
{
    public bool isCollided;
    void Start()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "PlatformBar")
        {
            isCollided = true;
        }

    }

}
