using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VersionController : MonoBehaviour
{
    public TMP_Text versionText;
    void Start()
    {
        versionText.SetText($"v{Application.version}");
    }
}
