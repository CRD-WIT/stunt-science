using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorManager : MonoBehaviour
{
    public GameObject[] indicatorSprite;
    public GameObject targetPos, indicator;
    public QuestionControllerB theQuestion;
    public bool showReady;
    int option;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (theQuestion.answerIsCorrect == true)
        {
            option = 1;
        }
        else
        {
            option = 0;
        }
    }
    public IEnumerator showIndicator()
    {
        if (showReady)
        {
            indicatorSprite[option].SetActive(true);
            indicator.transform.position = new Vector2(targetPos.transform.position.x + 1.5f, targetPos.transform.position.y + 1.5f);
            showReady = false;
        }
        yield return new WaitForSeconds(2);
        indicatorSprite[option].SetActive(false);

    }
}
