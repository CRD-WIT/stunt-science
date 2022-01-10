using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSelector : MonoBehaviour
{
    [SerializeField]
    GameObject[] actionBtn;

    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update() { }

    void Awake() { }

    public GameObject ActionButtonOn(int n)
    {
        return actionBtn[n];
    }
}
