using UnityEngine;
using TMPro;

public class VersionController : MonoBehaviour
{
    public TMP_Text versionText;
    void Start()
    {
        versionText.SetText($"v{Application.version}");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
