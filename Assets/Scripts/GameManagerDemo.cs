using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManagerDemo : MonoBehaviour
{
    public GameObject player;
    public GameObject playerName;
    public TextMeshProUGUI timerTextGUI;
    public TextMeshProUGUI problemTextGUI;
    public TextMeshProUGUI timerTextFinalGUI;
    public TMP_InputField playerAnswer;
    public TextMeshProUGUI problemText;
    public GameObject readyUI;
    public GameObject resultUI;
    private float gameTime = 0.0f;
    private Rigidbody2D playerRigidbody2D;
    private Animator playerAnimator;
    TimeSpan duration;
    private float timeLastFrame;
    public TextMeshProUGUI readyText;
    public GameObject xpFeedback;
    //Test SafeZone Object
    public GameObject safeZone;
    bool isSimulating = false;

    private GameObject[] debris;
    private CoinsManager coinBar;
    private Vector3 safeZoneDemoPosition;
    public GameObject actionClosed;
    public Slider levelProgress;
    public GameObject actionOpen;
    public TextMeshProUGUI resultTitle;
    public TextMeshProUGUI resultFeedback;
    public GameObject earthShaker;
    public TextMeshProUGUI greeterText;
    public GameObject timelyShaker;
    bool isCorrect = false;
    // Start is called before the first frame update
    void Start()
    {
        playerRigidbody2D = player.GetComponent<Rigidbody2D>();
        playerAnimator = player.GetComponent<Animator>();
        playerRigidbody2D.MovePosition(new Vector2(0, 0));
        timeLastFrame = Time.realtimeSinceStartup;
        debris = GameObject.FindGameObjectsWithTag("Debri");
        coinBar = GameObject.FindGameObjectWithTag("CoinBar").GetComponent<CoinsManager>();
        safeZoneDemoPosition = safeZone.transform.position;        
        timelyShaker.SetActive(true);
        string p_name = PlayerPrefs.GetString("playerName");
        problemText.SetText($"The ceiling is about to crumble down and the only safe spot is <color=red><b>8m</b></color> away from <color=orange><b>{p_name}</b></color>. If <color=orange><b>{p_name}</b></color> have exactly <color=green><b>4sec</b></color> to run into the safe spot, what should be his <b><color=purple>velocity</color></b> so that he will not get hit by the crumbling debris of the ceiling?");
        greeterText.SetText($"Howdy {p_name}! Here's your stunt script:");
    }

    public void ToggleSimulation(bool value)
    {
        StartCoroutine(ReadyRoutine());
    }

    IEnumerator ReadyRoutine()
    {
        actionOpen.SetActive(true);
        readyUI.SetActive(true);
        readyText.SetText("Lights");
        yield return new WaitForSeconds(0.7f);
        readyText.SetText("Camera");
        yield return new WaitForSeconds(0.5f);
        readyText.SetText("Action!");
        actionOpen.SetActive(false);
        actionClosed.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        readyUI.SetActive(false);
        isSimulating = true;
    }

    IEnumerator ResultRoutine()
    {
        yield return new WaitForSeconds(3.0f);
        resultUI.SetActive(true);

    }

    IEnumerator XPAddRoutine(float levelUp)
    {
        yield return new WaitForSeconds(1.0f);
        xpFeedback.SetActive(true);
        yield return new WaitForSeconds(1.50f);
        levelProgress.value = levelUp;
    }


    public void ContinueNextSubLevel()
    {
        resultUI.SetActive(false);
        if (isCorrect)
        {
            StartCoroutine(XPAddRoutine(0.4f));
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float playerAnswerValue = float.Parse(playerAnswer.text != "" ? playerAnswer.text : "0", System.Globalization.CultureInfo.InvariantCulture);
        if (isSimulating)
        {
            timelyShaker.SetActive(false);
            gameTime += Time.fixedDeltaTime;

            playerAnimator.SetBool("isRunning", true);

            duration = TimeSpan.FromMilliseconds(gameTime * 1000);

            int milliseconds = Convert.ToInt32(duration.ToString(@"ff"));
            int seconds = Convert.ToInt32(duration.ToString(@"ss"));

            timerTextGUI.SetText($"{seconds}:{milliseconds} sec");

            // Correct
            if ($"{seconds}:{milliseconds}" == "4:0" && playerAnswerValue == 2)
            {
                resultTitle.SetText("Good Take!");
                resultFeedback.SetText("You have successfully solved the problem! Press continue to receive your StuntXP award.");
                isSimulating = false;
                foreach (GameObject debri in debris)
                {
                    debri.transform.tag = "Untagged";
                    debri.GetComponent<Rigidbody2D>().bodyType = UnityEngine.RigidbodyType2D.Dynamic;
                    
                }
                earthShaker.SetActive(true);
                coinBar.AddCoins(safeZoneDemoPosition, 150);
                safeZone.SetActive(false);
                earthShaker.SetActive(false);
                isCorrect = true;
                StartCoroutine(ResultRoutine());
            }

            //More than
            if ($"{seconds}:{milliseconds}" == "3:0" && playerAnswerValue > 2)
            {
                resultTitle.SetText("Cut!");
                resultFeedback.SetText("You're too fast! Try a different answer.");
                isSimulating = false;
                foreach (GameObject debri in debris)
                {
                    debri.GetComponent<Rigidbody2D>().bodyType = UnityEngine.RigidbodyType2D.Dynamic;
                }
                earthShaker.SetActive(true);
                isCorrect = false;
                StartCoroutine(ResultRoutine());
            }
            // Less than
            if ($"{seconds}:{milliseconds}" == "3:0" && playerAnswerValue < 2)
            {
                resultTitle.SetText("Cut!");
                resultFeedback.SetText("You're too slow! You can't reach the safe zone in time! Please try again!");
                isSimulating = false;
                foreach (GameObject debri in debris)
                {
                    debri.GetComponent<Rigidbody2D>().bodyType = UnityEngine.RigidbodyType2D.Dynamic;
                }
                earthShaker.SetActive(true);
                isCorrect = false;
                StartCoroutine(ResultRoutine());
            }

            playerRigidbody2D.velocity = new Vector2(playerAnswerValue, playerRigidbody2D.velocity.y);

        }
        else
        {
            duration = TimeSpan.FromMilliseconds(gameTime * 1000);
            timerTextGUI.SetText(duration.ToString(@"ss\.ff") + " sec");
            timerTextFinalGUI.SetText(duration.ToString(@"ss\.ff") + " sec");
            playerAnimator.SetBool("isRunning", false);
            playerRigidbody2D.velocity = new Vector2(0, playerRigidbody2D.velocity.y);
        }
    }
}

