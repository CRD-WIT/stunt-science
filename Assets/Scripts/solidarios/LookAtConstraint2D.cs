using UnityEngine;

public class LookAtConstraint2D : MonoBehaviour
{
    public bool invert;
    public GameObject target;
    public bool flipped;
    // Start is called before the first frame update
    public void SetTarget(GameObject o)
    {
        this.target = o;
    }

    void LookConstraint()
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector2 direction;

        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        if (invert)
        {
            if (flipped)
            {
                direction = new Vector2(((target.transform.position.x) - transform.position.x) * -1, (target.transform.position.y - transform.position.y) * -1);
                transform.up = direction;
            }
            else
            {
                direction = new Vector2(((target.transform.position.x) - transform.position.x), (target.transform.position.y - transform.position.y));
                transform.up = direction;
            }
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
