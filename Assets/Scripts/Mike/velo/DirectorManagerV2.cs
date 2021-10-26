using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DirectorManagerV2 : MonoBehaviour
{
    public Transform target;
    Transform initTarget, tempTarget;
    public ToThe directorIsFacingTo, platformIsOn;
    float platformScale;
    TMP_Text dSpeech;
    void Start()
    {
        // dSpeech = FindObjectOfType<TMP_Text>();
        dSpeech = transform.Find("SpeechBubble").Find("DirectorsSpeech").GetComponent<TMP_Text>();
        initTarget = target;
    }

    void Update()
    {
        platformScale = this.transform.localScale.x;

        if (GameObject.Find("stonePrefab(Clone)") != null)
        {
            Debug.Log("active");
            tempTarget = GameObject.FindWithTag("Player").GetComponent<Transform>();
        }
        else
            tempTarget = initTarget;

        target = tempTarget;

        if (platformIsOn == ToThe.Left)
        {
            this.transform.localScale = new Vector2(0.22f, 0.22f);
            dSpeech.rectTransform.localScale = new Vector2(-1, 1);

            if (Mathf.Abs(GetComponentInChildren<SpotlightManagerV1>().angle) >= 90)
                directorIsFacingTo = ToThe.Left;
            else
                directorIsFacingTo = ToThe.Right;

            if (directorIsFacingTo == ToThe.Right)
                this.transform.Find("Director").localScale = new Vector2(platformScale / platformScale, 1);
            else
                this.transform.Find("Director").localScale = new Vector2(-platformScale / platformScale, 1);
        }
        else
        {
            this.transform.localScale = new Vector2(-0.22f, 0.22f);
            dSpeech.rectTransform.localScale = new Vector2(1, 1);

            if ((Mathf.Abs(GetComponentInChildren<SpotlightManagerV1>().angle) >= 270) || (Mathf.Abs(GetComponentInChildren<SpotlightManagerV1>().angle) <= 90))
                directorIsFacingTo = ToThe.Left;
            else
                directorIsFacingTo = ToThe.Right;

            if (directorIsFacingTo == ToThe.Right)
                this.transform.Find("Director").localScale = new Vector2(-platformScale / platformScale, 1);
            else
                this.transform.Find("Director").localScale = new Vector2(platformScale / platformScale, 1);
        }
    }
    public enum ToThe : byte
    {
        Right = 0,
        Left = 1
    }
}
