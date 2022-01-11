using TMPro;
using UnityEngine;

public class SafeZoneIndicator : MonoBehaviour
{
    // Start is called before the first frame update
    public string text;
    public TMP_Text container;
    // Update is called once per frame
    void Update()
    {
        container.SetText(text);
    }
}
