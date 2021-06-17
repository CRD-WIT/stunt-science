using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DeviceIDController : MonoBehaviour
{
    public TMP_Text deviceIDPrompt;
    // Start is called before the first frame update
    void Start()
    {
        deviceIDPrompt.SetText($"Sorry, your device <b>{SystemInfo.deviceUniqueIdentifier}</b> is not registered for this game!");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
