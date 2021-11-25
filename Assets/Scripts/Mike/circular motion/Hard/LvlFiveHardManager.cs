using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LvlFiveHardManager : MonoBehaviour
{
    [SerializeField]float distance, velocity, aVelocity, radius, time;
    MechaManager mm;
    
    // Start is called before the first frame update
    void Start()
    {
        mm = FindObjectOfType<MechaManager>();
    }

    // Update is called once per frame
    void Update()
    {
        mm.SetMechaVelocity(aVelocity, time);
    }
    //1st gear d =1.18, 2nd gear d= 1.55, 3rd gear d =1.15
}
