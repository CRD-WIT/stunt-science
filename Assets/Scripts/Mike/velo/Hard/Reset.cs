using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reset : MonoBehaviour
{
    public int stage;
    void OnEnable()
    {
        stage = FindObjectOfType<HardManager>().stage;
        FindObjectOfType<HardManager>().gameObject.SetActive(false);
    }
}
