using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RumblingManager : MonoBehaviour
{
    // Transform of the camera to shake. Grabs the gameObject's transform
    // if null.
    public Transform camTransform;
    public float shakeDuration = 0f, shakeAmount = 0.8f, decreaseFactor = 1.0f, debrisRange = 1.4f;
    public bool rubbleON = true, collapsing = true;
    public static bool isCrumbling, shakeON;
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
        if (shakeON == true)
        {
            StartCoroutine(camshake());
            if (rubbleON == true)
            {
                StartCoroutine(fallingrubble());
            }
        }
        if (isCrumbling)
        {
            StartCoroutine(Crumbling());
        }
    }
    IEnumerator Crumbling()
    {        
        if (shakeDuration > 0)
        {
            rumbling.SetActive(true);
            camTransform.localPosition = originalPos + Random.insideUnitSphere * (shakeAmount * 3);
            shakeDuration -= Time.deltaTime * (decreaseFactor*2);
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
        GameObject TempGo2 = Instantiate(rubbles[Random.Range(0, 1)]);
        TempGo2.transform.position = new Vector2(Random.Range(-3f, 40f), 9f);
        GameObject TempGo3 = Instantiate(rubbles[Random.Range(0, 1)]);
        TempGo3.transform.position = new Vector2(Random.Range(-3f, 40f), 10f);
        GameObject TempGo = Instantiate(rubbles[Random.Range(0, 1)]);
        TempGo.transform.position = new Vector2(Random.Range(-3f, 40f), 11f);
        rubbleON = false;
        yield return new WaitForSeconds(1);
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
        TempGo2.transform.position = new Vector2(Random.Range(-3, debrisRange), 9);
        GameObject TempGo7 = Instantiate(debris[Random.Range(0, 2)]);
        TempGo7.transform.position = new Vector2(Random.Range(-3, debrisRange), 10);
        GameObject TempGo6 = Instantiate(debris[Random.Range(0, 2)]);
        TempGo6.transform.position = new Vector2(Random.Range(-3, debrisRange), 11);
        GameObject TempGo1 = Instantiate(debris[Random.Range(0, 2)]);
        TempGo1.transform.position = new Vector2(Random.Range(-3, debrisRange), 9);
        GameObject TempGo3 = Instantiate(debris[Random.Range(0, 2)]);
        TempGo3.transform.position = new Vector2(Random.Range(-3, debrisRange), 10);
        GameObject TempGo4 = Instantiate(debris[Random.Range(0, 2)]);
        TempGo4.transform.position = new Vector2(Random.Range(-3, debrisRange), 11);
    }
    public void collapse()
    {
        if (collapsing)
        {
            StartCoroutine(debrisFall());
            collapsing = false;
        }
    }
}
