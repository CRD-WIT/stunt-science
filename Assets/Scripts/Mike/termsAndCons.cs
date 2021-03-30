using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// this is unused
/// </summary>
public class termsAndCons : MonoBehaviour
{
    public Toggle TandC;
    public static bool goodToGo;
    public bool check;
    // Start is called before the first frame update
    void Start()
    {
        TandC = GetComponent<Toggle>();
    }
    private void Update() {
        termAndCon();
        check = goodToGo;
    }
    public void termAndCon(){
        if(TandC.isOn){
            goodToGo = true;
        }
        else{
            goodToGo = false;
        }
    }
}
