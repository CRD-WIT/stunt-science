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
    void Start() {
        dSpeech = FindObjectOfType<TMP_Text>();
    }

    void Update()
    {
        if (!target.gameObject.activeSelf)
            target = GameObject.FindWithTag("Player").GetComponent<Transform>();

        if (platformIsOn == To.Left)
        {
            this.transform.localScale = new Vector2(0.25f, 0.25f);
            dSpeech.rectTransform.localScale = new Vector2(-1,1);

            if (Mathf.Abs(GetComponentInChildren<SpotlightManager>().angle) >= 90)
                directorIsFacingTo = To.Left;
            else
                directorIsFacingTo = To.Right;
        }
        else
        {
            this.transform.localScale = new Vector2(-0.25f, 0.25f);
            dSpeech.rectTransform.localScale = new Vector2(1,1);

            if ((Mathf.Abs(GetComponentInChildren<SpotlightManager>().angle) >= 270) || (Mathf.Abs(GetComponentInChildren<SpotlightManager>().angle) <= 90))
                directorIsFacingTo = To.Left;
            else
                directorIsFacingTo = To.Right;
        }


        platformScale = this.transform.localScale.x;


        if (directorIsFacingTo == To.Right)
            this.transform.Find("Director").localScale = new Vector2(platformScale/platformScale, 1);
        else
            this.transform.Find("Director").localScale = new Vector2(platformScale/platformScale, 1);
    }
    public enum To : byte
    {
        Right = 0,
        Left = 1
    }
}
