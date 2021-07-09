using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStar : MonoBehaviour
{
    public GameObject activeStar;
    public GameObject inactiveStar;
    public bool active;

    // Update is called once per frame
    void Update()
    {
        activeStar.SetActive(active);
        inactiveStar.SetActive(!active);
    }

    public void ToggleStar(bool value){
        active = value;
    }
}


