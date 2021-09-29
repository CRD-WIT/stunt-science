using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DirectorManager : MonoBehaviour
{
    public Transform target;
    public To directorIsFacingTo, platformIsOn;
    float platformScale;
    TMP_Text dSpeech;
    void Start()
    {
        // dSpeech = FindObjectOfType<TMP_Text>();
        dSpeech = transform.Find("SpeechBubble").Find("DirectorsSpeech").GetComponent<TMP_Text>();
    }

    void Update()
    {
        platformScale = this.transform.localScale.x;

        if (!target.parent.parent.gameObject.activeSelf)
            target = GameObject.FindWithTag("Player").GetComponent<Transform>();

        if (platformIsOn == To.Left)
        {
            this.transform.localScale = new Vector2(0.22f, 0.22f);
            dSpeech.rectTransform.localScale = new Vector2(-1, 1);

            if (Mathf.Abs(GetComponentInChildren<SpotlightManager>().angle) >= 90)
                directorIsFacingTo = To.Left;
            else
                directorIsFacingTo = To.Right;

            if (directorIsFacingTo == To.Right)
                this.transform.Find("Director").localScale = new Vector2(platformScale / platformScale, 1);
            else
                this.transform.Find("Director").localScale = new Vector2(-platformScale / platformScale, 1);
        }
        else
        {
            this.transform.localScale = new Vector2(-0.22f, 0.22f);
            dSpeech.rectTransform.localScale = new Vector2(1, 1);

            if ((Mathf.Abs(GetComponentInChildren<SpotlightManager>().angle) >= 270) || (Mathf.Abs(GetComponentInChildren<SpotlightManager>().angle) <= 90))
                directorIsFacingTo = To.Left;
            else
                directorIsFacingTo = To.Right;

            if (directorIsFacingTo == To.Right)
                this.transform.Find("Director").localScale = new Vector2(-platformScale / platformScale, 1);
            else
                this.transform.Find("Director").localScale = new Vector2(platformScale / platformScale, 1);
        }
    }
    public enum To : byte
    {
        Right = 0,
        Left = 1
    }
}
