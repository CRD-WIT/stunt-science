using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearHangers : MonoBehaviour
{
    private CapsuleCollider2D hangers;
    [SerializeField] private HingeJoint2D playerHanger;
    PlayerCM2 myPlayer;
    bool isHangerOn, isHangerNumerator;
    string hangerName;
    public static float hangTime;
    float startTime;
    public static bool isHanging;
    void Start()
    {
        myPlayer = FindObjectOfType<PlayerCM2>();
        hangers = GetComponent<CapsuleCollider2D>();
        playerHanger = GetComponent<HingeJoint2D>();
    }
    void Update()
    {
        if (Level5EasyManager.isHanging)
        {
            myPlayer.isHanging =true;
            isHangerNumerator = true;
            if (hangerName == this.gameObject.name)
            {
                hangTime = Time.realtimeSinceStartup - startTime;
                this.playerHanger.enabled = true;
            }
            else
            {
                this.playerHanger.enabled = false;
            }
        }
        else
        {
            myPlayer.isHanging = false;
            playerHanger.enabled = false;
            isHangerOn = false;
        }
        if (!isHangerOn)
        {
            StartCoroutine(HangerChange());
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        Level5EasyManager.isHanging = true;
        other.gameObject.SetActive(false);
        hangerName = this.gameObject.name;
        startTime = Time.realtimeSinceStartup;
    }
    IEnumerator HangerChange()
    {
        isHangerOn = true;
        if (isHangerNumerator)
        {
            startTime = 0;
            hangTime = 0;
            hangerName = "";
            hangers.enabled = false;
            yield return new WaitForSeconds(2);
            hangers.enabled = true;
            isHangerNumerator = false;
        }
    }
}
