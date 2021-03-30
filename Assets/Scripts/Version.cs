using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Version : MonoBehaviour
{
    public TextMeshProUGUI versionTextGUI;
    // Start is called before the first frame update
    void Start()
    {
        versionTextGUI.SetText($"v{Application.version}");
    }
}
