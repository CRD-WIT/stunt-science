using UnityEngine;
using GameConfig;

public class ArrowColorChange : MonoBehaviour
{
    TextColorMode valueIs;
    QuestionControllerVThree qc;
    string Oname;
    // Start is called before the first frame update
    void Start()
    {
        qc = FindObjectOfType<QuestionControllerVThree>();
    }

    // Update is called once per frame
    void Update()
    {

        if (CurvedLineFollower.answerIs != null)
        {
            if (CurvedLineFollower.answerIs == true)
                valueIs = TextColorMode.Correct;
            else // (CurvedLineFollower.answerIs == false)
                valueIs = TextColorMode.Wrong;
        }
        else
            valueIs = TextColorMode.Given;

        this.gameObject.GetComponent<SpriteRenderer>().color = qc.getHexColor(valueIs);
    }
}
