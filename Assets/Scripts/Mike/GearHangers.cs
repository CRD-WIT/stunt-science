using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearHangers : MonoBehaviour
{
    private CapsuleCollider2D hangers;
    [SerializeField]
    private HingeJoint2D playerHanger;
    // Start is called before the first frame update
    bool isHangerOn, isHangerNumerator;
    string hangerName;
    void Start()
    {
        hangers = GetComponent<CapsuleCollider2D>();
        playerHanger = GetComponent<HingeJoint2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(hangerName == this.gameObject.name){
            this.gameObject.SetActive(true);
        }else if(hangerName.Length == 0){
            this.gameObject.SetActive(true);
        }else{
            this.gameObject.SetActive(false);
        }
        Debug.Log(hangerName);
        if (Level5EasyManager.isHanging)
        {
            this.playerHanger.enabled = true;
            isHangerNumerator = true;
        }
        else
        {
            this.playerHanger.enabled = false;
            isHangerOn = false;
        }
        if (!isHangerOn)
        {
            StartCoroutine(HangerChange());
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "gearHangers")
        {
            other.gameObject.SetActive(false);
            hangerName = this.gameObject.name;
            this.playerHanger.enabled = true;
            Level5EasyManager.isHanging = true;
        }
    }
    IEnumerator HangerChange()
    {
        isHangerOn = true;
        if (isHangerNumerator)
        {
            hangers.enabled =false;
            yield return new WaitForSeconds(1);
            hangers.enabled =true;
            isHangerNumerator = false;
        }
    }
}
