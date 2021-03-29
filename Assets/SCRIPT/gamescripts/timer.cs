using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class timer : MonoBehaviour
{
    public float msec;
    public float sec;
    public float min;
    public Text timertxt;
    public float distanceTimer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timeStart();
    }
    public void timeStart()
    {
        StartCoroutine("StopWatch");
    }
    IEnumerator StopWatch()
    {
        while (true)
        {
            distanceTimer -= Time.deltaTime;
            msec = (int)((distanceTimer + (int)distanceTimer) * 100);
            sec = (int)(distanceTimer % 60);
            min = (int)(distanceTimer / 60 % 60);
            timertxt.text = string.Format("{0:00}:{1:00}:{2:00}", min, sec, msec);

            yield return null;
        }
    }
}
