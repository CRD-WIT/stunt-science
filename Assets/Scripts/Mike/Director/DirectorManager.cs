using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectorManager : MonoBehaviour
{
    public Transform target;
    public To directorIsFacingTo, platformIsOn;
    float platformScale;

    void Update()
    {
        if (platformIsOn == To.Left)
        {
            this.transform.localScale = new Vector2(1, 1);

            if (Mathf.Abs(GetComponentInChildren<SpotlightManager>().angle) >= 90)
                directorIsFacingTo = To.Left;
            else
                directorIsFacingTo = To.Right;
        }
        else
        {
            this.transform.localScale = new Vector2(-1, 1);

            if ((Mathf.Abs(GetComponentInChildren<SpotlightManager>().angle) >= 270) || (Mathf.Abs(GetComponentInChildren<SpotlightManager>().angle) <= 90))
                directorIsFacingTo = To.Left;
            else
                directorIsFacingTo = To.Right;
        }


        platformScale = this.transform.localScale.x;


        if (directorIsFacingTo == To.Right)
            this.transform.Find("Director").localScale = new Vector2(platformScale, 1);
        else
            this.transform.Find("Director").localScale = new Vector2(-platformScale, 1);
    }
    public enum To : byte
    {
        Right = 0,
        Left = 1
    }
}
