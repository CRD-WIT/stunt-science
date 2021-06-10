using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameConfig;
public class MediumManager : MonoBehaviour
{
    PlayerV2 myPlayer;
    Transform conveyor;
    GameObject conveyorWheel1, conveyorWheel2;
    Rigidbody2D conveyorWheel1RB, conveyorWheel2RB;

    // Start is called before the first frame update
    void Start()
    {
        myPlayer = FindObjectOfType<PlayerV2>();
        conveyorWheel1 = conveyor.Find("Wheel1").gameObject;
        conveyorWheel2 = conveyor.Find("Wheel2").gameObject;
        conveyorWheel1RB = conveyorWheel1.GetComponent<Rigidbody2D>();
        conveyorWheel2RB = conveyorWheel2.GetComponent<Rigidbody2D>();
    }
    // void SetConveyorSpeed(){
    //     float circumferenceOfWheel = 2 * (float)(Mathf.PI * 1.15),
    //         speed, ;
    // }

    // Update is called once per frame
    void Update()
    {
    }
}
