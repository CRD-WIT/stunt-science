using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptForTesting : MonoBehaviour
{
    public NewConveyorManager cm;
    public float av;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        cm.SetConveyorSpeed(av, 2, 2.2f);
    }
}
