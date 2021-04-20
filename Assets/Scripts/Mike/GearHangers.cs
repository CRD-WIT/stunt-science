using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearHangers : MonoBehaviour
{
    [SerializeField]
    private CapsuleCollider2D hangers;
    [SerializeField]
    private HingeJoint2D playerHanger;
    // Start is called before the first frame update
    void Start()
    {
        hangers = GetComponent<CapsuleCollider2D>();
        playerHanger = GetComponent<HingeJoint2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Level5EasyManager.isHanging)
        {
            this.playerHanger.enabled = true;
        }else{
            this.playerHanger.enabled = false;
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "gearHangers")
        {
            other.gameObject.SetActive(false);
            Level5EasyManager.isHanging = true;
        }
    }
}
