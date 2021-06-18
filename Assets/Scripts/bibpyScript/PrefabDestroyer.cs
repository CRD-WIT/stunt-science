using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabDestroyer : MonoBehaviour
{
    // public GameObject debriPrefab;
    // private SimulationManager theSimulate;
    Rigidbody2D rubbleRb;
    PolygonCollider2D rubbleCol;
    public static bool destroyPrefab, end;
    // Start is called before the first frame update
    void Start()
    {
        rubbleRb = this.gameObject.GetComponent<Rigidbody2D>();
        rubbleCol = this.gameObject.GetComponent<PolygonCollider2D>();
        // theSimulate = FindObjectOfType<SimulationManager>();
        this.rubbleRb.bodyType = RigidbodyType2D.Dynamic;
        this.rubbleCol.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (destroyPrefab)
        {
            StartCoroutine(OffRubbleCollider());
        }
        if (end)
        {
            StartCoroutine(DestroyRubbles());
        }
    }
    IEnumerator OffRubbleCollider()
    {
        this.rubbleRb.bodyType = RigidbodyType2D.Kinematic;
        this.rubbleCol.enabled = false;
        yield return new WaitForSeconds(3);
        StartCoroutine(DestroyRubbles());
    }
    IEnumerator DestroyRubbles()
    {
        destroyPrefab = false;
        yield return new WaitForEndOfFrame();
        Destroy(this.gameObject);
        end = false;
    }
}
