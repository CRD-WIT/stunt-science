using System.Collections;
using UnityEngine;
using TMPro;

public class CameraScript : MonoBehaviour
{
    GameObject cameraImage;
    GameObject spotLight;
    public GameObject target;
    [SerializeField] bool isCalloutOpen;
    [SerializeField] GameObject callout;
    [SerializeField] TMP_Text calloutText;
    [SerializeField] QuestionController questionController;

    // Start is called before the first frame update
    void Start()
    {
        cameraImage = transform.Find("CameraTop").gameObject;
        spotLight = transform.Find("SpotLight").gameObject;

        cameraImage.GetComponent<LookAtConstraint2D>().SetTarget(target);
        spotLight.GetComponent<LookAtConstraint2D>().SetTarget(target);
    }

    public IEnumerator DirectorsCall()
    {
        if (questionController.isSimulating)
        {
            calloutText.SetText("Lights!");
            yield return new WaitForSeconds(0.75f);
            calloutText.SetText("Camera!");
            yield return new WaitForSeconds(0.75f);
            calloutText.SetText("Action!");
            yield return new WaitForSeconds(0.75f);
            calloutText.SetText("");
        }
        else
        {
            calloutText.SetText("Cut!");
            yield return new WaitForSeconds(0.75f);
            isCalloutOpen = false;
        }
    }

    void Update()
    {
        if (isCalloutOpen)
        {
            callout.SetActive(true);
            StartCoroutine(DirectorsCall());
        }
        else
        {
            callout.SetActive(false);
        }
    }
}
