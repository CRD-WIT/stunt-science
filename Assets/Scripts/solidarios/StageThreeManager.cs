using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StageThreeManager : MonoBehaviour
{
    public Player myPlayer;
    private CeillingGenerator theCeiling;
    private HeartManager HeartManager;
    public TMP_Text playerNameText, messageText, timer;
    public float distance, gameTime, Speed, elapsed, currentPos;
    string pronoun, pPronoun, pNoun, playerName, playerGender;
    public GameObject slidePlatform, lowerGround, AfterStuntMessage, safeZone, rubblesStopper, dimensionLine;

    StageManager sm = new StageManager();
    
    // Start is called before the first frame update
    string levelName = "Free Fall";
    float height = 5.0f;
    float targetTime = 0f;
    string question = $"[name] is hanging from a rope and [pronoun] is instructed to let go of it, drop down, and hang again to the horizontal pole below. If [name] will let go ang grab the pole after exactly [t] seconds, at what distance should [pronoun] hands above the pole before letting go?";
    public float elapsed;
    public TMP_Text playerNameText, messageText, timerText, questionText;
    public GameObject AfterStuntMessage;
    public GameObject dimensionLine;
    Animator thePlayerAnimation;
    bool isSimulating = false;
    public GameObject playerHingeJoint;
    public GameObject thePlayer;
    public GameObject playerHangingFixed;
    public GameObject FirstCamera;
    public GameObject SecondCamera;
    Vector3 thePlayer_position;
    public GameObject accurateCollider;

    public GameObject platformBar;

    Vector3 platformBar_position;

    void Start()
    {
        theCeiling = FindObjectOfType<CeillingGenerator>();           
        myPlayer = FindObjectOfType<Player>();
        HeartManager = FindObjectOfType<HeartManager>();
        playerName = PlayerPrefs.GetString("Name");
        playerGender = PlayerPrefs.GetString("Gender");
        Stage3SetUp();
    }

    void FixedUpdate()
    {
        Stage3SetUp();
    }
    public void Stage3SetUp(){
        dimensionLine.SetActive(true);
        rubblesStopper.SetActive(true);
        AfterStuntMessage.SetActive(false);
        rubblesStopper.SetActive(true);
        slidePlatform.SetActive(true);
        lowerGround.SetActive(false);
        //groundPlatform.transform.localScale = new Vector3(76.95f, groundPlatform.transform.localScale.y, 1);
        Speed = 0;
        if(playerGender == "Male"){
            pronoun = "he";
            pPronoun = "him";
            pNoun = "his";
        }else{
            pronoun = "she";
            pPronoun = "her";
            pNoun = "her";}
        while((distance < 9f)||(distance > 10f)){
            float v = Random.Range(9f, 10f);
            Speed = (float)System.Math.Round(v,2);
            float t = Random.Range(3f, 3.5f);
            gameTime = (float)System.Math.Round(t,2);
            distance = (float)System.Math.Round((Speed*gameTime), 2);
        } 
        myPlayer.lost = false;
        myPlayer.standup = false;
        RumblingManager.shakeON = true;
        DimensionManager.dimensionLength = distance;
        theCeiling.createQuadtilemap(); 
        safeZone.transform.position = new Vector2(distance, -2);
        timer.text = "0.00s";
        myPlayer.transform.position = new Vector2(0f, -3);   
        elapsed=0;  
        SimulationManager.isSimulating =false;
        SimulationManager.question = "The entire ceiling is now crumbling and the only safe way out is for "+playerName+" to jump and slide down the manhole. If "+pronoun+" runs at constant velocity of <color=purple>"+Speed.ToString()+" meters per second for exactly <color=#006400>"+gameTime.ToString()+" seconds, how </color></color> <color=red>far</color> from the center of the manhole should "+playerName+" start running so that "+pronoun+" will fall down in it when "+pronoun+" stops?";
    }
    IEnumerator StuntResult()
    {
        //messageFlag = false;
        yield return new WaitForSeconds(4f);
        AfterStuntMessage.SetActive(true);
    }
}
