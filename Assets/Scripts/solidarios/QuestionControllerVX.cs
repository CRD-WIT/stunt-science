using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using GameConfig;

public class QuestionControllerVX : MonoBehaviour
{
    [SerializeField] string actionButtonText;
    float playerAnswer;
    public bool answerIsCorrect = false;
    public bool isModalOpen = true;
    public Camera renderCamera;
    public Color correctAnswerColor;
    public Color givenColor;
    public Color wrongAnswerColor;
    public Difficulty levelDifficulty;
    public int levelNumber;
    public int stageNumber;
    public Orientation orientation;
    public string levelName;
    public string modalTitle;
    public string question;
    public TextColorMode colorMode;
    public UnitOf unitOf;

    string answerUnit;
    [SerializeField] TMP_InputField answerFieldHorizontal;
    [SerializeField] TMP_InputField answerFieldVertical;
    [SerializeField] GameObject difficultyName;
    [SerializeField] GameObject stageName;
    public bool isSimulating;
    [SerializeField] string modalText;
    public string errorText;
    public bool popupVisible;

    [Header("Components")]
    [Space(10)]
    [SerializeField] private Transform baseComponent;
    [SerializeField] Transform extraComponent;
    [SerializeField] GameObject levelNameBox;
    [SerializeField] GameObject problemBoxContainer;
    [SerializeField] GameObject levelBadge;
    [SerializeField] GameObject modalComponentHorizontal;
    [SerializeField] GameObject popupComponentHorizontal;
    [SerializeField] GameObject popupComponentVertical;
    [SerializeField] TMP_Text popupTextHorizontal;
    [SerializeField] TMP_Text popupTextVertical;
    [SerializeField] GameObject modalComponentVertical;
    [SerializeField] GameObject playButtonVertical;
    [SerializeField] GameObject playButtonHorizontal;
    [SerializeField] GameObject timerComponentHorizontal;
    [SerializeField] GameObject timerComponentVertical;
    [SerializeField] GameObject problemBoxVertical;
    [SerializeField] GameObject problemBoxHorizontal;
    [SerializeField] GameObject problemTextVertical;
    [SerializeField] GameObject problemTextHorizontal;
    [SerializeField] GameObject modalTitleVertical;
    [SerializeField] GameObject modalTextHorizontal;
    [SerializeField] GameObject modalTextVertical;
    [SerializeField] GameObject modalTitleHorizontal;
    [SerializeField] GameObject wrongIconHorizontal;
    [SerializeField] GameObject correctIconHorizontal;
    [SerializeField] GameObject wrongIconVertical;
    [SerializeField] GameObject correctIconVertical;
    public Transform[] allComponentTransforms;

    // Start is called before the first frame update
    void Start()
    {
        baseComponent.gameObject.GetComponent<Canvas>().worldCamera = renderCamera;

        givenColor = new Color32(0x73, 0x2b, 0xc2, 0xff);
        correctAnswerColor = new Color32(150, 217, 72, 255);
        wrongAnswerColor = new Color32(237, 66, 66, 255);

    }


    public string GetUnit()
    {
        return answerUnit;
    }

    public void ToggleModal(string title, string text, string actionButtonName)
    {
        isModalOpen = !isModalOpen;
        SetModalTitle(title);
        SetModalText(text);
        SetActionButtonName(actionButtonName);
    }

    public void SetActionButtonName(string text)
    {
        actionButtonText = text;
    }
    public void SetModalText(string s)
    {
        modalText = s;
    }

    public void SetModalTitle(string s)
    {
        modalTitle = s;
    }

    public void SetQuestion(string qstn)
    {
        question = qstn;
    }

    public float GetPlayerAnswer()
    {
        return playerAnswer;
    }

    public void SetAnswer()
    {
        SetUnit();
        if (orientation == Orientation.Horizontal)
        {
            if (answerFieldHorizontal.text == "")
                StartCoroutine(IsEmpty());
            else
            {
                playerAnswer = float.Parse(answerFieldHorizontal.text.Split(new string[] { answerUnit }, System.StringSplitOptions.None)[0]);
                answerFieldHorizontal.text = playerAnswer + answerUnit;
            }
        }
        else
        {
            if (answerFieldVertical.text == "")
            {
                StartCoroutine(IsEmpty());
            }
            else if (playerAnswer <= 0)
            {
                errorText = $"Invalid answer! Answer must be a greater than 0.";
                StartCoroutine(IsEmpty());
            }
            else
            {
                playerAnswer = float.Parse(answerFieldVertical.text.Split(new string[] { answerUnit }, System.StringSplitOptions.None)[0]);
                answerFieldVertical.text = playerAnswer + answerUnit;
            }
        }

    }

    IEnumerator IsEmpty()
    {
        // warningTxt.text = "Please enter your answer!";
        yield return new WaitForSeconds(1);
        // warningTxt.text = "";
    }

    public string SetUnit()
    {
        //TODO: passed the appropriate unit in the answerFieldHorizontal suffixed to the answer
        switch (unitOf)
        {
            case UnitOf.distance:
                answerUnit = " m";
                break;
            case UnitOf.time:
                answerUnit = " s";
                break;
            case UnitOf.velocity:
                answerUnit = " m/s";
                break;
            case UnitOf.acceleration:
                answerUnit = " m/s??";
                break;
            case UnitOf.angle:
                answerUnit = "??";
                break;
            case UnitOf.angularVelocity:
                answerUnit = "??/s";
                break;
            case UnitOf.force:
                answerUnit = " N";
                break;
            case UnitOf.mass:
                answerUnit = " kg";
                break;
            case UnitOf.work:
                answerUnit = " J";
                break;
            case UnitOf.energy:
                answerUnit = " kW";
                break;
            case UnitOf.power:
                answerUnit = " kWh";
                break;
            case UnitOf.momuntum:
                answerUnit = " kg???m/s";
                break;
        }
        return answerUnit;
    }

    public void Retry()
    {
        modalComponentHorizontal.gameObject.SetActive(false);
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }



    public string Unit()
    {
        //TODO: passed the appropriate unit in the answerFieldHorizontal suffixed to the answer
        switch (unitOf)
        {
            case UnitOf.distance:
                answerUnit = " m";
                break;
            case UnitOf.time:
                answerUnit = " s";
                break;
            case UnitOf.velocity:
                answerUnit = " m/s";
                break;
            case UnitOf.acceleration:
                answerUnit = " m/s??";
                break;
            case UnitOf.angle:
                answerUnit = "??";
                break;
            case UnitOf.angularVelocity:
                answerUnit = "??/s";
                break;
            case UnitOf.force:
                answerUnit = " N";
                break;
            case UnitOf.mass:
                answerUnit = " kg";
                break;
            case UnitOf.work:
                answerUnit = " J";
                break;
            case UnitOf.energy:
                answerUnit = " kW";
                break;
            case UnitOf.power:
                answerUnit = " kWh";
                break;
            case UnitOf.momuntum:
                answerUnit = " kg???m/s";
                break;
        }
        return answerUnit;
    }

    public string getHexColor(TextColorMode mode)
    {
        switch (mode)
        {
            case TextColorMode.Wrong:
                return ColorUtility.ToHtmlStringRGB(wrongAnswerColor);
            case TextColorMode.Correct:
                return ColorUtility.ToHtmlStringRGB(correctAnswerColor);
            default:
                return ColorUtility.ToHtmlStringRGB(givenColor);
        }
    }

    public void SetColor(TMP_Text target, TextColorMode mode)
    {
        switch (mode)
        {
            case TextColorMode.Wrong:
                target.color = wrongAnswerColor;
                break;
            case TextColorMode.Correct:
                target.color = correctAnswerColor;
                break;
            default:
                target.color = givenColor;
                break;
        }
    }

    void ToggleOrientation(Orientation o)
    {
        bool condition = o == Orientation.Horizontal;
        problemBoxHorizontal.SetActive(condition);
        problemTextHorizontal.SetActive(condition);
        problemBoxVertical.SetActive(!condition);
        problemTextVertical.SetActive(!condition);
    }

    void Update()
    {

        ToggleOrientation(orientation);

        if (orientation == Orientation.Horizontal)
        {
            popupComponentHorizontal.SetActive(popupVisible);
            popupComponentVertical.SetActive(false);
            popupTextHorizontal.SetText(errorText);
            modalComponentHorizontal.gameObject.SetActive(isModalOpen);
            problemTextHorizontal.SetActive(!isModalOpen);
            modalComponentVertical.SetActive(false);
            modalTitleHorizontal.SetActive(true);
            modalTitleVertical.SetActive(false);
            modalTitleHorizontal.GetComponent<TMP_Text>().SetText(modalTitle);
            playButtonHorizontal.SetActive(!isSimulating);
            answerFieldHorizontal.gameObject.SetActive(!isModalOpen);
            problemTextHorizontal.GetComponent<TMP_Text>().SetText(question);
            modalTextHorizontal.GetComponent<TMP_Text>().SetText(modalText);
            if (answerIsCorrect)
            {
                wrongIconHorizontal.SetActive(false);
                correctIconHorizontal.SetActive(true);
                SetColor(modalTitleHorizontal.GetComponent<TMP_Text>(), TextColorMode.Correct);
            }
            else
            {
                wrongIconHorizontal.SetActive(true);
                correctIconHorizontal.SetActive(false);
                SetColor(modalTitleHorizontal.GetComponent<TMP_Text>(), TextColorMode.Wrong);
            }
        }
        else
        {
            popupComponentVertical.SetActive(popupVisible);
            popupComponentHorizontal.SetActive(false);
            popupTextVertical.SetText(errorText);
            modalComponentVertical.gameObject.SetActive(isModalOpen);
            problemTextVertical.SetActive(!isModalOpen);
            modalComponentHorizontal.SetActive(false);
            modalTitleHorizontal.SetActive(false);
            modalTitleVertical.SetActive(true);
            playButtonVertical.SetActive(!isSimulating);
            playButtonVertical.SetActive(!isModalOpen);
            answerFieldVertical.gameObject.SetActive(!isModalOpen);
            problemTextVertical.GetComponent<TMP_Text>().SetText(question);
            modalTitleVertical.GetComponent<TMP_Text>().SetText(modalTitle);
            modalTextVertical.GetComponent<TMP_Text>().SetText(modalText);
            if (answerIsCorrect)
            {
                wrongIconVertical.SetActive(false);
                correctIconVertical.SetActive(true);
                SetColor(modalTitleVertical.GetComponent<TMP_Text>(), TextColorMode.Correct);
            }
            else
            {
                wrongIconVertical.SetActive(true);
                correctIconVertical.SetActive(false);
                SetColor(modalTitleVertical.GetComponent<TMP_Text>(), TextColorMode.Wrong);
            }
        }

        timerComponentHorizontal.SetActive(isSimulating && (orientation == Orientation.Horizontal));
        timerComponentVertical.SetActive(isSimulating && (orientation == Orientation.Vertical));

        // correctIconHorizontal.SetActive(!answerIsCorrect);
        // wrongIconHorizontal.SetActive(answerIsCorrect);

        levelNameBox.GetComponent<TMP_Text>().SetText($"{levelName}");
        extraComponent.Find("LevelNumber").GetComponent<TMP_Text>().SetText($"{levelNumber}");
        stageName.GetComponent<TMP_Text>().SetText($"Stage {stageNumber}");
        difficultyName.GetComponent<TMP_Text>().SetText($"{levelDifficulty}");
    }
}
