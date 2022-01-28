using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hanger : MonoBehaviour
{
    // private CapsuleCollider2D hangers;
    [SerializeField]
    private HingeJoint2D playerHanger;
    PlayerCM2 myPlayer;
    bool isHangerOn,
        isHangerNumerator;
    string hangerName;
    public static float hangTime;
    float startTime;
    public static bool isHanging;

    void Start()
    {
        playerHanger = GetComponent<HingeJoint2D>();
    }

    void Update()
    {
        if (isHanging)
        {
            this.playerHanger.enabled = true;

            isHanging = false;
        }
        else
        {
            isHangerOn = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        isHanging = true;
    }
}
