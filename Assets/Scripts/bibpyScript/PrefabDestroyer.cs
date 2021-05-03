using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabDestroyer : MonoBehaviour
{
    public GameObject debriPrefab;
    private SimulationManager theSimulate;
    Rigidbody2D rubbleRb;
    PolygonCollider2D rubbleCol;
    // Start is called before the first frame update
    void Start()
    {
        rubbleRb = this.gameObject.GetComponent<Rigidbody2D>();
        rubbleCol = this.gameObject.GetComponent<PolygonCollider2D>();
        theSimulate = FindObjectOfType<SimulationManager>();
        rubbleRb.bodyType = RigidbodyType2D.Dynamic;
        rubbleCol.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (theSimulate.destroyPrefab)
        {
            StartCoroutine(OffRubbleCollider());
        }
    }
    IEnumerator OffRubbleCollider()
    {
        rubbleRb.bodyType = RigidbodyType2D.Kinematic;
        rubbleCol.enabled = false;
        yield return new WaitForSeconds(4);
        Destroy(this.debriPrefab);
        Start();
    }
}
