using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using GameConfig;
using UnityEngine.UI;

public class QuestionControllerC : MonoBehaviour
{
    
    float playerAnswer;
    public float limit = 0;
    public Transform baseComponent, problemBox, extraComponent, levelBadge;
    public bool answerIsCorrect = false, isModalOpen = true, isSimulating, nextStage, retried;
    public Color correctAnswerColor, givenColor, wrongAnswerColor;
    public Difficulty levelDifficulty;
    public AccMidSimulation simulationManager;
    public int levelNumber, stage;
    public string modalTitle, question, timer;
    public string levelName;
    public TextColorMode colorMode;
    public Settings settingUI;
    public UnitOf unit;
    string answerUnit, difficulty;
    public HeartManager heartManager;
    int passedLevel;
    [SerializeField] bool timerOn = false, loaded = false;
    [SerializeField] TMP_InputField answerFieldHorizontal;
    public Transform difficultyName;
    public TMP_Text stageName;
    public FirebaseManager firebaseManager;
    public string modalText, errorText;
    public bool popupVisible, extraOn;
    public TMP_Text levelNumberText;

    [SerializeField]
    GameObject modalComponentHorizontal, popupComponentHorizontal, playButtonHorizontal, timerComponentHorizontal, problemBoxHorizontal,
            problemTextHorizontal, modalTextHorizontal, modalTitleHorizontal, wrongIconHorizontal, correctIconHorizontal;
    [SerializeField] TMP_Text popupTextHorizontal;
    [SerializeField] Button actionBtn;
    StageManager level = new StageManager();
    HeartManager life;

    string[] gameLevel = { "", "Velocity", "Acceleration", "Free Fall", "Projectile Motion", "Circular Motion", "Forces", "Work", "Energy", "Power", "Momemtum" };
    // Start is called before the first frame update
    public void SetGameLevel(int level)
    {
        PlayerPrefs.SetString("Level", gameLevel[level]);
    }
    void Start()
    {
        Transform[] components = { baseComponent, modalComponentHorizontal.transform, extraComponent };

        givenColor = new Color32(0x73, 0x2b, 0xc2, 0xff);
        correctAnswerColor = new Color32(150, 217, 72, 255);
        wrongAnswerColor = Color.red;

        levelName = level.GetGameLevel();
        switch (levelDifficulty)
        {
            case Difficulty.Easy:
                difficulty = level.GetDifficulty();
                break;
            case Difficulty.Medium:
                difficulty = level.GetDifficulty();
                break;
            case Difficulty.Hard:
                difficulty = level.GetDifficulty();
                break;
        }
        difficultyName.GetComponent<TMP_Text>().text = difficulty;

        life = FindObjectOfType<HeartManager>();

        // Set global gameplay stats for data logging.
        PlayerPrefs.SetString("LevelNumber", levelNumber.ToString());
        PlayerPrefs.SetString("DifficultyName", difficulty);
        PlayerPrefs.SetString("Stage", stage.ToString());

    }
    public void ActionBtn(bool endLevel)
    {
        if (endLevel)
        {

        }
        else
        {            
            if (answerIsCorrect)
                Next();
            else
            {
                StartCoroutine(Retry());
                firebaseManager.GameLogMutation(levelNumber, stage, difficulty, Actions.Retried, 0);                                
            }
                
            isModalOpen = false;
            isSimulating = false;
        }

    }

    public void EndLevel()
    {

    }

    public void EvaluatePlayerScore()
    {
        string playerPrefsName = "";
        switch (levelDifficulty)
        {
            case Difficulty.Easy:
                playerPrefsName = ($"level{levelName}Easy");
                break;
            case Difficulty.Medium:
                playerPrefsName = ($"level{levelName}Medium");
                break;
            case Difficulty.Hard:
                playerPrefsName = ($"level{levelName}Hard");
                break;
        }
        Debug.Log($"Player Score: {playerPrefsName}:{heartManager.life}");
        PlayerPrefs.SetInt(playerPrefsName, heartManager.life);
        settingUI.ToggleLevelFinished();
        //SceneManager.LoadScene("LevelSelectV2");

    }
    public void ActivateResult(string message, bool isCorrect, bool isComplete = false)
    {
        Debug.Log("ActivateResult Triggered");
        answerFieldHorizontal.text = "";
        answerIsCorrect = isCorrect;
        isModalOpen = true;
        if (isCorrect)
        {
            // NOTE: Use this template when ending levels.
            if (isComplete)
            {
                actionBtn.GetComponent<Button>().onClick.RemoveAllListeners();
                actionBtn.GetComponent<Button>().onClick.AddListener(EvaluatePlayerScore);

                actionBtn.transform.Find("BtnName").GetComponent<TMP_Text>().text = "Finish";
                modalTitle = "Stunts Completed!";
                modalText = message;
                SetColor(modalTitleHorizontal.GetComponent<TMP_Text>(), TextColorMode.Correct);
                firebaseManager.GameLogMutation(levelNumber, stage, difficulty, Actions.Completed, 0);                                
            }
            else
            {
                //TODO: Check current life points. If life == 0, hide the action button.
                
                actionBtn.transform.Find("BtnName").GetComponent<TMP_Text>().text = "Next";
                modalTitle = "Stunt Success!";
                modalText = message;
                SetColor(modalTitleHorizontal.GetComponent<TMP_Text>(), TextColorMode.Correct);
                firebaseManager.GameLogMutation(levelNumber, stage, difficulty, Actions.NextStage, 0);                                
            }
        }
        else
        {
            actionBtn.transform.Find("BtnName").GetComponent<TMP_Text>().text = "Retry";
            modalTitle = "Stunt Failed!";
            modalText = message;
            SetColor(modalTitleHorizontal.GetComponent<TMP_Text>(), TextColorMode.Wrong);
            firebaseManager.GameLogMutation(levelNumber, stage, difficulty, Actions.Failed, 0);                            
        }
        actionBtn.interactable = true;
        isSimulating = false;
    }
    public void SetQuestion(string qstn)
    {
        Debug.Log($"Setting question...{qstn}");
        question = qstn;
    }

    public float GetPlayerAnswer()
    {
        return this.playerAnswer;
    }

    public void AnswerLimiter()
    {
        string value = answerFieldHorizontal.text;
        // Bug when using whole number
        string[] splitted = value.Split('.');
        answerFieldHorizontal.characterLimit = splitted[0].Length + 3;
    }
    public void SetAnswer()
    {        
        firebaseManager.GameLogMutation(levelNumber, stage, difficulty, Actions.Started, 0);                
        playerAnswer = float.Parse(answerFieldHorizontal.text);
        if (answerFieldHorizontal.text == "")
        {
            StartCoroutine(IsEmpty());
        }
        else
        {
            answerFieldHorizontal.text = playerAnswer + answerUnit;        
        }
        extraOn = true;
    }
    public void Next()
    {
        simulationManager.next();
        if (stage == 1)
        {
            stage = 2;
            nextStage = true;
        }
        else if (stage == 2)
        {
            stage = 3;
            nextStage = true;
        }
        firebaseManager.GameLogMutation(levelNumber, stage, difficulty, Actions.NextStage, 0);                        
    }
    IEnumerator Retry()
    {

        // TODO: Fix delay for background intro.
        extraOn = false;
        answerFieldHorizontal.text = "";
        retried = true;
        if (stage == 1)
            stage = 1;
        else if (stage == 2)
            stage = 2;
        else
            stage = 3;
        yield return new WaitForEndOfFrame();
        isModalOpen = false;
    }

    IEnumerator IsEmpty()
    {
        popupVisible = true;
        errorText = "Answer box is empty!";
        yield return new WaitForSeconds(3);
        popupVisible = false;
        errorText = "";
    }

    public string Unit(UnitOf unitOf)
    {
        string unit = "";
        switch (unitOf)
        {
            case UnitOf.distance:
                unit = "m";
                break;
            case UnitOf.time:
                unit = "s";
                break;
            case UnitOf.velocity:
                unit = "m/s";
                break;
            case UnitOf.acceleration:
                unit = "m/s²";
                break;
            case UnitOf.angle:
                unit = "°";
                break;
            case UnitOf.angularVelocity:
                unit = "°/s";
                break;
            case UnitOf.force:
                unit = "N";
                break;
            case UnitOf.mass:
                unit = "kg";
                break;
            case UnitOf.work:
                unit = "J";
                break;
            case UnitOf.energy:
                unit = "kW";
                break;
            case UnitOf.power:
                unit = "kWh";
                break;
            case UnitOf.momuntum:
                unit = "kg•m/s";
                break;
        }
        return unit;
    }
    public void SetUnitTo(UnitOf unitOf)
    {
        switch (unitOf)
        {
            case UnitOf.distance:
                answerUnit = "m";
                break;
            case UnitOf.time:
                answerUnit = "s";
                break;
            case UnitOf.velocity:
                answerUnit = "m/s";
                break;
            case UnitOf.acceleration:
                answerUnit = "m/s²";
                break;
            case UnitOf.angle:
                answerUnit = "°";
                break;
            case UnitOf.angularVelocity:
                answerUnit = "°/s";
                break;
            case UnitOf.force:
                answerUnit = "N";
                break;
            case UnitOf.mass:
                answerUnit = "kg";
                break;
            case UnitOf.work:
                answerUnit = "J";
                break;
            case UnitOf.energy:
                answerUnit = "kW";
                break;
            case UnitOf.power:
                answerUnit = "kWh";
                break;
            case UnitOf.momuntum:
                answerUnit = "kg•m/s";
                break;
        }
    }
    public Color getHexColor(TextColorMode mode)
    {
        switch (mode)
        {
            case TextColorMode.Wrong:
                return wrongAnswerColor;
            case TextColorMode.Correct:
                return correctAnswerColor;
            default:
                return givenColor;
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
    void Update()
    {
        correctIconHorizontal.SetActive(answerIsCorrect);
        wrongIconHorizontal.SetActive(!answerIsCorrect);
        popupComponentHorizontal.SetActive(popupVisible);
        popupTextHorizontal.SetText(errorText);
        modalComponentHorizontal.gameObject.SetActive(isModalOpen);
        modalTitleHorizontal.GetComponent<TMP_Text>().SetText(modalTitle);
        Debug.Log(question);
        problemTextHorizontal.GetComponent<TMP_Text>().SetText(question);
        modalTextHorizontal.GetComponent<TMP_Text>().SetText(modalText);
        problemTextHorizontal.SetActive(!isModalOpen);
        // playButtonHorizontal.SetActive(!isModalOpen);
        answerFieldHorizontal.gameObject.SetActive(!isModalOpen);

        extraComponent.gameObject.SetActive(isSimulating || popupVisible);
        playButtonHorizontal.SetActive(!isSimulating);
        //timerComponentHorizontal.gameObject.SetActive(timerOn);
        timerComponentHorizontal.GetComponent<TMP_Text>().SetText(timer);

        problemBox.Find("StageBar1").Find("LevelName").GetComponent<TMP_Text>().SetText($"{levelName}");
        levelNumberText.SetText($"{levelNumber}");
        stageName.SetText($"Stage {stage}");
        difficultyName.GetComponent<TMP_Text>().SetText($"{levelDifficulty}");
    }
}
/*That means that Bolt's speed during his world-record run was 10.44 meters per second.
Since many people are more familiar with automobiles and speed limits, it might be more
useful to think of this in terms of kilometers per hour or miles per hour: 37.58 or 23.35, respectively
Usain Bolt of Jamaica, set at the 2009 World Athletics Championships final in Berlin, Germany on 16 August 2009,

Average human speed is 1.4m/s.*/


// TODO: Implement limiter