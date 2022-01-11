using UnityEngine;

public class StageThreePlayerScript : MonoBehaviour
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
