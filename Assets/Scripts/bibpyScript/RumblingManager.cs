using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RumblingManager : MonoBehaviour
{
    // Transform of the camera to shake. Grabs the gameObject's transform
	// if null.
	public Transform camTransform;
	
	// How long the object should shake for.
	public float shakeDuration = 0f;
	
	// Amplitude of the shake. A larger value shakes the camera harder.
	public float shakeAmount = 0.8f;
	public float decreaseFactor = 1.0f;
    public bool shakeON = true;
    public bool rubbleON = true;
    
    public GameObject rumbling;
    public GameObject[] rubbles;
    public GameObject[] debris;
    public float debrisRange = 1.4f;
    
	
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
        
        
        if(shakeON == true)
        {
           
	    	StartCoroutine(camshake());
            if (rubbleON == true)
            {
                 StartCoroutine(fallingrubble());
            }
            
        }
        
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
        shakeON = true;
        
        
        shakeDuration = 0.5f;
        
    }
    IEnumerator fallingrubble()
    {
        
        GameObject TempGo2 = Instantiate(rubbles[Random.Range(0, 1)]);
        TempGo2.transform.position = new Vector2(Random.Range(-3, 20), 5f);
         GameObject TempGo3 = Instantiate(rubbles[Random.Range(0, 1)]);
        TempGo3.transform.position = new Vector2(Random.Range(-3, 20), 6f);
         GameObject TempGo = Instantiate(rubbles[Random.Range(0, 1)]);
        TempGo.transform.position = new Vector2(Random.Range(-3, 20), 8f);
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
        GameObject TempGo2 = Instantiate(debris[Random.Range(0, 2)]);
        TempGo2.transform.position = new Vector2(Random.Range(-3, debrisRange), 4);
        GameObject TempGo7 = Instantiate(debris[Random.Range(0, 2)]);
        TempGo7.transform.position = new Vector2(Random.Range(-3, debrisRange), 5);
        GameObject TempGo6 = Instantiate(debris[Random.Range(0, 2)]);
        TempGo6.transform.position = new Vector2(Random.Range(-3, debrisRange), 6);
        yield return new WaitForSeconds(0.3f);
         GameObject TempGo3 = Instantiate(debris[Random.Range(0, 2)]);
        TempGo3.transform.position = new Vector2(Random.Range(-3, debrisRange), 4);
        GameObject TempGo5 = Instantiate(debris[Random.Range(0, 2)]);
        TempGo5.transform.position = new Vector2(Random.Range(-3, debrisRange), 5);
        yield return new WaitForSeconds(0.3f);
         GameObject TempGo = Instantiate(debris[Random.Range(0, 2)]);
        TempGo.transform.position = new Vector2(Random.Range(-3, debrisRange), 4);
        GameObject TempGo4 = Instantiate(debris[Random.Range(0, 2)]);
        TempGo4.transform.position = new Vector2(Random.Range(-3, debrisRange), 5);
        
      
        
         
         
    }
    public void collapse()
    {
        StartCoroutine(debrisFall());
    }
}
