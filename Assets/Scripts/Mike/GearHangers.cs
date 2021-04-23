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
    public static float gameTime;
    void Start()
    {
        hangers = GetComponent<CapsuleCollider2D>();
        playerHanger = GetComponent<HingeJoint2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(hangerName);
        if (Level5EasyManager.isHanging)
        {
            isHangerNumerator = true;
            if (hangerName == this.gameObject.name)
            {
                this.playerHanger.enabled = true;
            }
            else
            {
                this.playerHanger.enabled = false;
            }
        }
        else
        {
            playerHanger.enabled = false;
            isHangerOn = false;
        }
        if (hangerName == "")
        {
            playerHanger.enabled = false;
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
            StartCoroutine(HangerName());
            this.playerHanger.enabled = true;
            Level5EasyManager.isHanging = true;
        }
    }
    IEnumerator HangerChange()
    {
        isHangerOn = true;
        if (isHangerNumerator)
        {
            hangers.enabled = false;
            yield return new WaitForSeconds(2);
            hangers.enabled = true;
            isHangerNumerator = false;
        }
    }
    IEnumerator HangerName()
    {
        hangerName = this.gameObject.name;
        yield return new WaitForSeconds(gameTime);
        hangerName = "";
    }
}
