using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombManager : MonoBehaviour
{
    private ForceSimulation theSimulate;
    private ColliderManager theCollide;
    private ForceManagerOne theManagerOne;
    private ForceManagerTwo theManagerTwo;
    private ForceManagerThree theManagerThree;
    public GameObject bomb, explosionPrefab;
    public GameObject[] glassHolder;
    public GameObject[] otherGlassHolder;
    private Rigidbody2D bombRigidbody;
    public bool followRagdoll, explodebomb;
    // Start is called before the first frame update
    void Start()
    {
        theCollide = FindObjectOfType<ColliderManager>();
        theManagerOne = FindObjectOfType<ForceManagerOne>();
        theManagerTwo = FindObjectOfType<ForceManagerTwo>();
        theManagerThree = FindObjectOfType<ForceManagerThree>();
        bombRigidbody = bomb.gameObject.GetComponent<Rigidbody2D>();
        theSimulate = FindObjectOfType<ForceSimulation>();
    }

    // Update is called once per frame
    void Update()
    {
        if (theSimulate.stage == 1)
        {
            if (theCollide.collide == true)
            {
                StartCoroutine(explode());
                if (theManagerOne.playerAnswer > theManagerOne.correctAnswer || theManagerOne.playerAnswer < theManagerOne.correctAnswer)
                {
                    followRagdoll = true;
                }

            }
            if (theManagerOne.throwBomb == true)
            {
                if (theManagerOne.playerAnswer == theManagerOne.correctAnswer)
                {
                    bombRigidbody.velocity = new Vector2(12, bombRigidbody.velocity.y);
                }
            }
        }
        if (theSimulate.stage == 2)
        {
            if (theCollide.collide == true)
            {
                StartCoroutine(explode());
                if (theManagerTwo.playerAnswer > theManagerTwo.correctAnswer || theManagerTwo.playerAnswer < theManagerTwo.correctAnswer)
                {
                    followRagdoll = true;
                }

            }
            if (theManagerTwo.throwBomb == true)
            {
                if (theManagerTwo.playerAnswer == theManagerTwo.correctAnswer)
                {
                    bombRigidbody.velocity = new Vector2(-12, bombRigidbody.velocity.y);
                }
            }
        }
        if (theSimulate.stage == 3)
        {

            if (explodebomb)
            {
                explodebomb = false;
                StartCoroutine(explode());

            }

        }
    }
    IEnumerator explode()
    {
        if (theSimulate.stage == 1)
        {
            if (theManagerOne.playerAnswer <= theManagerOne.correctAnswer)
            {
                yield return new WaitForSeconds(2f);
                GameObject explosion = Instantiate(explosionPrefab);
                explosion.transform.position = bomb.transform.position;
                bomb.SetActive(false);

                yield return new WaitForSeconds(.1f);
                if (theManagerOne.tooWeak == true)
                {
                    glassHolder[0].SetActive(false);
                    otherGlassHolder[0].SetActive(false);
                }
                yield return new WaitForSeconds(.4f);
                Destroy(explosion);
            }
            if (theManagerOne.playerAnswer > theManagerOne.correctAnswer)
            {
                theManagerOne.throwBomb = false;
                yield return new WaitForSeconds(1.2f);
                GameObject explosion = Instantiate(explosionPrefab);
                explosion.transform.position = bomb.transform.position;
                bomb.SetActive(false);

                yield return new WaitForSeconds(.1f);
                if (theManagerOne.tooWeak == true)
                {
                    glassHolder[0].SetActive(false);
                    otherGlassHolder[0].SetActive(false);
                }
                yield return new WaitForSeconds(.4f);
                Destroy(explosion);
            }
        }
        if (theSimulate.stage == 2)
        {
            if (theManagerTwo.playerAnswer <= theManagerTwo.correctAnswer)
            {
                yield return new WaitForSeconds(2.1f);
                GameObject explosion = Instantiate(explosionPrefab);
                explosion.transform.position = bomb.transform.position;
                bomb.SetActive(false);

                yield return new WaitForSeconds(.1f);
                if (theManagerTwo.tooWeak == true)
                {
                    glassHolder[1].SetActive(false);
                    otherGlassHolder[1].SetActive(false);
                }
                yield return new WaitForSeconds(.4f);
                Destroy(explosion);
            }
            if (theManagerTwo.playerAnswer > theManagerTwo.correctAnswer)
            {
                theManagerTwo.throwBomb = false;
                yield return new WaitForSeconds(1.2f);
                GameObject explosion = Instantiate(explosionPrefab);
                explosion.transform.position = bomb.transform.position;
                bomb.SetActive(false);

                yield return new WaitForSeconds(.1f);
                if (theManagerTwo.tooWeak == true)
                {
                    glassHolder[1].SetActive(false);
                    otherGlassHolder[1].SetActive(false);
                }
                yield return new WaitForSeconds(.4f);
                Destroy(explosion);
            }

        }
        if (theSimulate.stage == 3)
        {
            yield return new WaitForSeconds(3);
            GameObject explosion = Instantiate(explosionPrefab);
            explosion.transform.position = bomb.transform.position;
            bomb.SetActive(false);
            yield return new WaitForSeconds(1);
            otherGlassHolder[2].SetActive(false);
        }

        ForceSimulation.simulate = false;

    }
}
