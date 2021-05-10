using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtConstraint2D : MonoBehaviour
{
    public bool invert;
    public GameObject target;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void SetTarget(GameObject o){
        this.target = o;
    }

    void LookConstraint()
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector2 direction;

        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        if (invert)
        {
            direction = new Vector2(((target.transform.position.x - 1.5f) - transform.position.x) * -1, (target.transform.position.y - transform.position.y) * -1);
            transform.up = direction;
        }
        else
        {
            direction = new Vector2((target.transform.position.x - transform.position.x), (transform.position.y - transform.position.y));
            transform.up = direction;
        }
    }

    // Update is called once per frame
    void Update()
    {
        LookConstraint();
    }
}
