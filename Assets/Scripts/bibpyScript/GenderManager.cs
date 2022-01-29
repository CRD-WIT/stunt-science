using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenderManager : MonoBehaviour
{
    public string gender;
    public GameObject hair;
    // Start is called before the first frame update
    void Start()
    {
        gender = PlayerPrefs.GetString("Gender");
    }

    // Update is called once per frame
    void Update()
    {
        if(gender == "Female")
        {
            hair.SetActive(true);
        }
    }
}
