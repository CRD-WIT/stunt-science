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
    [SerializeField] QuestionController questionController;
    public bool directorIsCalling;

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
        if (questionController.isSimulating)
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
            yield return new WaitForSeconds(0.75f);
            isCalloutOpen = false;
        }
    }

    void FixedUpdate()
    {
        if (directorIsCalling)
        {
            StartCoroutine(DirectorsCall());
        }
        else
        {
            directorIsCalling = false;
        }
    }
}
