using System.Collections;
using UnityEngine;
using TMPro;

public class CameraScript : MonoBehaviour
{
    GameObject cameraImage;
    GameObject spotLight;
    public GameObject target;
    public bool isCalloutOpen;
    [SerializeField] GameObject callout;
    [SerializeField] TMP_Text calloutText;
    [SerializeField] QuestionContProjMed questionController;
    public bool directorIsCalling;
    public bool isStartOfStunt;

    // Start is called before the first frame update
    void Start()
    {
        cameraImage = transform.Find("CameraTop").gameObject;
        spotLight = transform.Find("SpotLight").gameObject;
        callout.SetActive(false);
        cameraImage.GetComponent<LookAtConstraint2D>().SetTarget(target);
        spotLight.GetComponent<LookAtConstraint2D>().SetTarget(target);
    }

    public IEnumerator DirectorsCall()
    {
        if (isStartOfStunt)
        {
            callout.SetActive(true);
            calloutText.SetText("Lights!");
            yield return new WaitForSeconds(0.75f);
            calloutText.SetText("Camera!");
            yield return new WaitForSeconds(0.75f);
            calloutText.SetText("Action!");
            yield return new WaitForSeconds(0.75f);
            calloutText.SetText("");
            callout.SetActive(false);
            questionController.isSimulating = true;
        }
        else
        {
            calloutText.SetText("Cut!");
            callout.SetActive(true);            
            yield return new WaitForSeconds(0.75f);
            callout.SetActive(false); 
        }
    }

    void FixedUpdate()
    {
        if (directorIsCalling)
        {
            StartCoroutine(DirectorsCall());
            directorIsCalling = false;
        }
    }
}
