using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RumblingManager : MonoBehaviour
{
    // Transform of the camera to shake. Grabs the gameObject's transform
    // if null.
    RockDestroyer reset;
    public Transform camTransform;
    public float shakeDuration = 0f, shakeAmount = 0.8f, decreaseFactor = 1.0f, debrisRange = 1.4f;
    public bool rubbleON = true, collapsing = true;
    public static bool isCrumbling, shakeON, fallingBouder;
    bool isThisFirst = true;
    public GameObject rumbling;
    public GameObject[] rubbles, debris;
    Vector3 originalPos;
    void Awake()
    {
        if (camTransform == null)
        {
            camTransform = GetComponent(typeof(Transform)) as Transform;
        }
    }

    void OnEnable()
    {
        originalPos = camTransform.localPosition;
    }

    void Update()
    {
        if (shakeON)
        {
            StartCoroutine(camshake());
            if (rubbleON)
            {
                //FallingBoulders.isRumbling = true;
                StartCoroutine(fallingrubble());
            }
            // if (isThisFirst)
            //     FirstRun();
            // else                
            //     reset.isDestroyed = false;
        }
        if (isCrumbling)
        {
            StartCoroutine(Crumbling());
        }
    }
    void FirstRun()
    {
        isThisFirst = false;
        // reset = FindObjectOfType<RockDestroyer>();
        // reset.isDestroyed = false;
    }
    IEnumerator Crumbling()
    {
        if (shakeDuration > 0)
        {
            rumbling.SetActive(true);
            camTransform.localPosition = originalPos + Random.insideUnitSphere * (shakeAmount * 3);
            shakeDuration -= Time.deltaTime * (decreaseFactor * 2);
        }
        else
        {
            shakeDuration = 0f;
            camTransform.localPosition = originalPos;
        }
        yield return new WaitForSeconds(0.75f);
        shakeDuration = 0f;
        shakeON = false;
    }
    IEnumerator camshake()
    {
        if (shakeDuration > 0)
        {
            rumbling.SetActive(true);
            camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;
            shakeDuration -= Time.deltaTime * decreaseFactor;
        }
        else
        {
            shakeDuration = 0f;
            camTransform.localPosition = originalPos;
        }
        yield return new WaitForSeconds(2);
        shakeON = false;
        rumbling.SetActive(false);
        yield return new WaitForSeconds(5);
        if (isCrumbling)
        {
            yield break;
        }
        shakeON = true;
        shakeDuration = 0.5f;
    }
    IEnumerator fallingrubble()
    {
        //StartCoroutine(BoulderDrop());
        GameObject TempGo2 = Instantiate(rubbles[Random.Range(0, 1)]);
        TempGo2.transform.position = new Vector2(Random.Range(-3f, 40f), Random.Range(9f, 11f));
        rubbleON = false;
        yield return new WaitForSeconds(1);
        fallingBouder = true;
        rubbleON = true;
        yield return new WaitForSeconds(0.1f);
        rubbleON = false;
        yield return new WaitForSeconds(6);
        rubbleON = true;
    }
    IEnumerator debrisFall()
    {
        yield return new WaitForSeconds(0.3f);
        moreCollapse();
        yield return new WaitForSeconds(0.3f);
        moreCollapse();
        yield return new WaitForSeconds(0.3f);
        moreCollapse();
    }
    void moreCollapse()
    {
        GameObject TempGo2 = Instantiate(debris[Random.Range(0, 2)]);
        TempGo2.transform.position = new Vector2(Random.Range(-3, debrisRange), Random.Range(9f, 11f));
    }
    public void collapse()
    {
        if (collapsing)
        {
            StartCoroutine(debrisFall());
            collapsing = false;
        }
    }
    IEnumerator BoulderDrop()
    {
        FallingBoulders.isRumbling = true;
        yield return new WaitUntil(() => FallingBoulders.boulderDrop);
        FallingBoulders.isRumbling = false;
        FallingBoulders.boulderDrop = false;
    }
}
