using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameConfig;
public class MediumManager : MonoBehaviour
{
    PlayerV2 myPlayer;

    public float angularVelocity, time;
    ConveyorManager conveyor;

    // Start is called before the first frame update
    void Start()
    {
        myPlayer = FindObjectOfType<PlayerV2>();
        conveyor = FindObjectOfType<ConveyorManager>();
    }

    // Update is called once per frame
    void Update()
    {
        conveyor.SetConveyorSpeed(angularVelocity, time);
    }
}
