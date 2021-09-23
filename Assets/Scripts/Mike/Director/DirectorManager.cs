using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectorManager : MonoBehaviour
{
    public FacingTo facingTo, platformIsOn;
    float platformScale;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (platformIsOn == FacingTo.Left)
            this.transform.parent.localScale = new Vector2(1, 1);
        else
            this.transform.parent.localScale = new Vector2(-1, 1);
        platformScale = this.transform.parent.localScale.x;
        if (facingTo == FacingTo.Right)
            this.transform.localScale = new Vector2(platformScale, 1);
        else
            this.transform.localScale = new Vector2(-platformScale, 1);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "LowerBody")
        {
            Debug.Log(other.gameObject.name);
            if (facingTo == FacingTo.Right)
                facingTo = FacingTo.Left;
            else
                facingTo = FacingTo.Right;
        }
    }
    public enum FacingTo : byte
    {
        Right = 0,
        Left = 1
    }
}
