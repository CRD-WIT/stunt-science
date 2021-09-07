using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class golem : MonoBehaviour
{
    public float moveSpeed, accelaration;
    public Rigidbody2D myRigidbody;
    private Animator myAnimator;
    public ProjSimulationManager theSimulate;
    public bool throwing, damage, standUp;
    public GameObject hpBar, head, hpBarParent, smallBlast;
    public float fullHp, currentHp;
    public bool reduceHp, accelarating;
    
    // Start is called before the first frame update
    void Start()
    {
        myAnimator = GetComponent<Animator>();
        myRigidbody = GetComponent<Rigidbody2D>();
        fullHp = hpBar.transform.localScale.x;
        currentHp = fullHp;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        hpBarParent.transform.position = head.transform.position;
        hpBar.transform.localScale = new Vector2(currentHp, hpBar.transform.localScale.y);
        if (reduceHp)
        {
            currentHp -= Time.deltaTime * (fullHp * 0.1f);

        }
        if (theSimulate.stage == 1)
        {
            myRigidbody.velocity = new Vector2(-moveSpeed, myRigidbody.velocity.y);
        }
        if (theSimulate.stage == 2)
        {
            myRigidbody.velocity = new Vector2(moveSpeed, myRigidbody.velocity.y);
        }
        if (theSimulate.stage == 3)
        {
            myRigidbody.velocity = new Vector2(-moveSpeed, myRigidbody.velocity.y);
        }
        if(accelarating)
        {
            moveSpeed += accelaration * Time.fixedDeltaTime;
        }

        myAnimator.SetFloat("speed", moveSpeed);
        myAnimator.SetBool("throw", throwing);
        myAnimator.SetBool("damage", damage);
        myAnimator.SetBool("standUp", standUp);
    }
    public IEnumerator takeDamaged()
    {
        reduceHp = true;
        yield return new WaitForSeconds(3.33f);
        reduceHp = false;
        smallBlast.SetActive(false);
    }
}
