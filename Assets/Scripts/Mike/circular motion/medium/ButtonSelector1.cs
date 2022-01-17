using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSelector1 : MonoBehaviour
{
    [SerializeField]
    GameObject[] endBtn;

    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update() { }

    void Awake() { }

    public GameObject ActionButtonOn(int n)
    {
        return endBtn[n];
    }
}
