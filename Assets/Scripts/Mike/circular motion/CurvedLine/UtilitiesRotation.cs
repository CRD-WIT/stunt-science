using UnityEngine;

public class UtilitiesRotation : MonoBehaviour
{
    public Transform player;

    void Update()
    {
        this.transform.forward = player.transform.position - this.transform.position;
    }
}