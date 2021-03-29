using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    //public GameObject level2manager;
    public Player thePlayer;
    public Player thePlayer2;
    public Stickman thestick;
    private parallax theparax;
    private camerashake thecamshake;
    

    public actionCount theAct;
   
    public ScoreManager thescorer;
    public cameraman thedirector;
     public camerascript thecamera;
    //public cameracontroller thecam;
    public boardanim theboard;
    public flag theflag;
    private stopperController theStopper;
    float respawntime = 2;
    
    public float respawnposx;
    public float respawnposy;


    private Vector3 PlayerStartPoint;

    public Text timertxt;
    public Text msectxt;
    public Text sectxt;
    
    public Text stage;
    float distanceTimer;
    public float msec;
    private float sec;
    private float min;
    public bool A;
    public bool B;
    public bool simulate;
    public Text playerdistance;


    private float Asec;
    public float Amsec;
    public InputField time;
    private float answers;
    //modified
    public float pInput;
    float correctsec;
    float correctsecRO;
    float correctmsec;
    float FAcorrectmsec;

    private float FAmsec;

    public float playerpos;
    public float currentpos;
    float playerposdeci;

    float distance;
    public bool died;

    float generatetime;
    float generatetimeIN;
    float generatedistance;
    //public Text givenspeed;
   //public Text givendistance;
    public float correctanswer;
    public float correctanswerRO;
    

    public double marginOfError = 0.01;
    public double actualError;
    float speedInput;


    public bool tileDrawn = true;
    public tilemapgenerator thetilegenerator;
    bool ragdollready = true;
    float excesdistance;

    public Button start;
    public Button retry;
    public Button play;
    public GameObject next;
    public GameObject question;
    public GameObject check;
    //public GameObject wrong;
    public GameObject stuntground;
    private Vector3 stuntgroundstartpoint;
    public GameObject distancemarking;
    public GameObject ceilingstopper;
    public Vector3 distancemarkstartpoint;
    public bool forwardstunt;




    public bool destroytiles;
    public GameObject allquestion;
    int selectedchar;
    public AudioSource button1;
    public AudioSource button2;
    public AudioSource wrongsfx;
    public bool buzz;
    public AudioSource bustedsfx;
    public bool busted;
    public AudioSource correctsfx;
    public bool ding;
    
    public bool destroystick;

    public GameObject stuntfailed;
    bool failprompt;
    public Text speedquestion;
    //public Text[] playername;
    //public Text correctanswertxt;
   //public Text fastslowtxt;
    //public Text pronoun;
    int gender;
    public bool playerDead;
    
    public GameObject jumpers;
    public GameObject safewall;
    string pronouns;
    string posessiveP;
    string fastslowtxt;
    public Text stuntsfailtxt;

    public GameObject meter;
    public GameObject meterimage;
    public Text metertxt;
     public GameObject transition;
    





    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(showstartbutton());
        stage.text = ("speed");
        selectedchar = PlayerPrefs.GetInt ("sex");
        PlayerStartPoint = thePlayer.transform.position;
        thetilegenerator = FindObjectOfType<tilemapgenerator>();
        thedirector = FindObjectOfType<cameraman>();
        playerpos = thePlayer.transform.position.x;
        stuntgroundstartpoint = stuntground.transform.position;
        distancemarkstartpoint = distancemarking.transform.position;
        //thecam = FindObjectOfType<cameracontroller>();
        thecamera = FindObjectOfType<camerascript>();
        theboard = FindObjectOfType<boardanim>();
        theflag = FindObjectOfType<flag>();
        thestick = FindObjectOfType<Stickman>();
        theAct = FindObjectOfType<actionCount>();
        thescorer = FindObjectOfType<ScoreManager>();
        theStopper = FindObjectOfType<stopperController>();
        theparax = FindObjectOfType<parallax>();
        gender = PlayerPrefs.GetInt("sex");
        thePlayer.positioning();
        thecamshake = FindObjectOfType<camerashake>();
        
       
        if (selectedchar == 1)
        {
            thePlayer = thePlayer2;
        }
        //playername[0].text = PlayerPrefs.GetString("name");
        //playername[1].text = PlayerPrefs.GetString("name") + (" is");
        
        if (gender == 0)
        {
            pronouns = ("he");
            posessiveP = ("his");
        }
        if (gender == 1)
        {
            pronouns = ("she");
            posessiveP = ("her");
        }
        StartCoroutine(showproblem());
        
       
        
       

    }

    // Update is called once per frame
    void FixedUpdate()
    { 
        
       
        
       
        //playerposdeci = thePlayer.transform.position.x  %1;
        currentpos = thePlayer.transform.position.x;


        //actualError = (float)Mathf.Abs(correctanswer - answers);
       
        distance = pInput * generatetime;

       /* if ((actualError <= marginOfError)&&(actualError!=0))
         {
             answers = correctanswer;

         }
         else
         {
             answers = pInput;
         }*/


        if (simulate == true)
        {
           

            /*if (thePlayer.happy == true)
            {
                playerdistance.text = currentpos.ToString("F2") + "m";
            }
            else
            {
                playerdistance.text = currentpos.ToString("F2") + "m";
            }*/

           

            /*if (currentpos >= generatedistance )
            {
                thePlayer.moveSpeed = 0f;
                StopCoroutine("StopWatch");
            }*/
            if (pInput == correctanswerRO)
            {
                
               
                if (currentpos >= generatedistance)
                {
                    thePlayer.moveSpeed = 0;
                    StopCoroutine("StopWatch");
                    
                    if (thePlayer.moveSpeed == 0)
                    {
                        safewall.SetActive(true);
                        theparax.moveSpeed = 0;
                        //thePlayer.happy = true;
                        msectxt.text = string.Format(":{0:00}", FAcorrectmsec);
                        sectxt.text = ":0" + correctsec;
                        thePlayer.transform.position = new Vector2(generatedistance, -1);                        
                        play.gameObject.SetActive(false);
                        next.gameObject.SetActive(true);
                        thescorer.addingoOneCoin();
                        check.SetActive(true);
                        jumpers.SetActive(true);
                        //theflag.flagAnim = 1;
                        dingling();
                        ceilingstopper.SetActive(false);
                        
                        
                        
                    }
                }
            }
            if (pInput != correctanswerRO)
            {
               
                /*if (currentpos > distance)
                {
                    thePlayer.transform.position = new Vector2(distance, -1);

                }*/
                if (pInput < correctanswerRO)
                {
                    if(currentpos >= distance)
                    {
                        StopCoroutine("StopWatch");
                        thePlayer.moveSpeed = 0f;
                        if (thePlayer.moveSpeed == 0)
                        {
                            wasted();
                            buzzzing();
                            theparax.moveSpeed = 0;
                            msectxt.text = string.Format(":{0:00}", Amsec);
                            sectxt.text = ":0" + Asec;
                            fastslowtxt = ("slow!");
                            thePlayer.lost = true;
                            thePlayer.standup = true;
                           thePlayer.transform.position = new Vector3(distance, -1f, 0f);
                        
                            play.gameObject.SetActive(false);
                       
                            //wrong.SetActive(true);
                            theAct.losepoint();
                        
                            ceilingstopper.SetActive(false);
                            if(failprompt == false)
                            {
                                 StartCoroutine(stuntfailing());
                            }
                        }
                    }
                }
                
                if (currentpos > generatedistance)
                {
                    
                    if (distance < generatedistance + 1)
                    {
                        if (currentpos >= distance)
                        {
                            thePlayer.moveSpeed = 0f;
                            StopCoroutine("StopWatch");
                       
                        
                                if (thePlayer.moveSpeed == 0)
                            {
                                theparax.moveSpeed = 0;
                                ceilingstopper.SetActive(false);
                                thePlayer.transform.position = new Vector2(distance, -1);
                                msectxt.text = ":" + FAcorrectmsec;
                                sectxt.text = ":0" + correctsec;
                                thePlayer.lost = true;
                                thePlayer.standup = true;
                                play.gameObject.SetActive(false);
                                fastslowtxt = ("fast!");
                                //wrong.SetActive(true);
                                theAct.losepoint();
                                buzzzing();
                                if(failprompt == false)
                                {
                                    StartCoroutine(stuntfailing());
                                }
                           

                            }
                        }
                            

                    }
                    if (distance > generatedistance + 1)
                    {
                        if (currentpos > generatedistance + 1)
                        {
                            msectxt.text = ":" + FAcorrectmsec;
                            sectxt.text = ":0" + correctsec;
                            theparax.moveSpeed = 0;
                            thePlayer.moveSpeed = 0;
                            if (ragdollready)
                            {
                                playerDead = true;
                                wasted();
                                StartCoroutine(playerspawn());
                                thecamera.stuntScene = true;
                                play.gameObject.SetActive(false);
                                ceilingstopper.SetActive(false);
                                fastslowtxt = ("fast!");
                                //wrong.SetActive(true);
                                theAct.losepoint();
                                if(failprompt == false)
                           
                                {
                                    thePlayer.standup = true;
                                    StartCoroutine(stuntfailing());
                                }
                           
                            }
                        }
                    }

                }
                if (pInput < correctanswerRO)
                {
                    if (currentpos > generatedistance)
                    {
                        thePlayer.moveSpeed = 0f;
                        StopCoroutine("StopWatch");
                       
                        
                        if (thePlayer.moveSpeed == 0)
                        {
                            theparax.moveSpeed = 0;
                          thePlayer.transform.position = new Vector2(generatedistance, thePlayer.transform.rotation.y);
                          msectxt.text = ":" + FAcorrectmsec;
                          sectxt.text = ":0" + correctsec;
                           thePlayer.lost = true;
                           thePlayer.standup = true;
                           play.gameObject.SetActive(false);
                           fastslowtxt = ("fast!");
                           //wrong.SetActive(true);
                           theAct.losepoint();
                           buzzzing();
                            if(failprompt == false)
                        {
                            StartCoroutine(stuntfailing());
                        }
                           

                        }

                    }
                     
                    
                }
            }
           

           
           
            

           
            if (sec <= Asec)
            {
               
                sectxt.text = string.Format(":{0:00}",sec);
                if (thePlayer.moveSpeed > 0)
                {
                    msectxt.text = string.Format(":{0:00}", msec);
                }
               
            }
            /*if (sec >= Asec & msec >= Amsec)
            {
                StopCoroutine("StopWatch");
               
            }   */         
            
               
             
            
    
        }
       

    }
    IEnumerator showproblem()
    {
         transition.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        //level2manager.SetActive(false);
        question.SetActive(true);
        generatetimeIN = (Random.Range(2.5f, 2.9f));
        generatedistance = (Random.Range(6, 8));
        generatetime = (float)System.Math.Round(generatetimeIN, 1);
        correctanswer = generatedistance / generatetime;
        thetilegenerator.mapwitdh = (int)generatedistance + 1;
        correctmsec = generatetime % 1;
        FAcorrectmsec = Mathf.Round(correctmsec * 100f) / 1.0f;
        correctanswerRO = (float)System.Math.Round(correctanswer, 2);
        correctsec = generatetime - correctmsec;
        start.gameObject.SetActive(false);
        play.gameObject.SetActive(true);
        //givenspeed.text = generatetime.ToString("F1");
        //givendistance.text = generatedistance.ToString("F1");
        //thecam.actioncam = 1;
        distancemarking.transform.position = new Vector2(generatedistance, -1);
        theboard.transit = 1;
        stuntground.transform.position = new Vector3(generatedistance + 3, -3, 2);
        meter.transform.localScale = new Vector2(generatedistance, 0.05f);
        metertxt.text = generatedistance.ToString() + ("m");
        meterimage.transform.position = new Vector2(generatedistance/2, -1.67f);
        if (tileDrawn)
            {
                thetilegenerator.createQuadtilemap ();
                tileDrawn = false;
            }
            button1.Play(0);
        //banana.transform.position = new Vector2(generatedistance, -1);
        speedquestion.text =    ("The ceiling is about to crumble down and the only safe spot is <b>")+ generatedistance.ToString("F1") + ("</b> meter away from <b>") + PlayerPrefs.GetString("name") + ("</b>. If <b>") + PlayerPrefs.GetString("name") + ("</b> have exactly <b>") + generatetime.ToString("F1") + ("</b> seconds to run into the safe spot, What should be ") + posessiveP + (" velocity so that ") + pronouns + (" will not get hit by the crumbling debris?");
 
         


    }

    public void resetgame()
    {
        button2.Play();
        buzz = true;
        busted = true;  
        ding = true;  
        thePlayer.gameObject.SetActive(true);
        thePlayer.transform.position = PlayerStartPoint;
        thePlayer.moveSpeed = 0;
        retry.gameObject.SetActive(false);
        resetstopwatch();
        simulate = false;
        thePlayer.happy = false;
        thePlayer.lost = false;
        StartCoroutine(resettiles());
        //wrong.SetActive(false);
        ragdollready = true;
        stuntground.transform.position = stuntgroundstartpoint;
        question.SetActive(false);
        //StartCoroutine(showstartbutton());
        theboard.transit = 2; 
        thecamera.camActive = false;
         thecamera.stuntScene = false;
         distancemarking.transform.position = distancemarkstartpoint;
        theAct.losepointReady = false;
         msectxt.text = string.Format(":{0:00}",0);
        sectxt.text = ":0" + 0;
        StartCoroutine(resetstick());
        ceilingstopper.SetActive(true);
        Time.timeScale = 1f;
        stuntfailed.SetActive(false);
        failprompt = false;
        thePlayer.positioning();
        thePlayer.standup = false;
        theStopper.stopperReset();
        meter.transform.localScale = new Vector2(0, 0.05f);
        metertxt.text = ("0m");
        meterimage.transform.position = new Vector2(0, -1.67f);
        transition.SetActive(true);
        StartCoroutine(showproblem());
       
       
        
    }
   
    public void resetstopwatch()
    {
        distanceTimer = 0;
       // timertxt.text = "00:00:00";
    }
    public void FixedtimeStart()
    {
        StartCoroutine("StopWatch");
    }
    IEnumerator StopWatch()
    {
        while (true)
        {
            distanceTimer += Time.deltaTime;
            msec = (int)((distanceTimer - (int)distanceTimer) * 100);
            sec = (int)(distanceTimer % 60);
            min = (int)(distanceTimer / 60 % 60);
            timertxt.text = string.Format("{0:00}", min);
           
            

            yield return null;
        }
    }

    public void Enter()
    {
        
        simulate = true;
        
        //ceilingstopperside.SetActive(false);
        speedInput = float.Parse(time.text);
        theparax.moveSpeed = -0.2f;

        //answers = speedInput;
        pInput = speedInput;
        thePlayer.moveSpeed = speedInput;
        thecamshake.debrisRange = generatedistance;


        FAmsec = generatetime % 1;
        Amsec = Mathf.Round(FAmsec * 100f) / 1.0f;
        Asec = generatetime - FAmsec;
        //theStopper.moveSpeed = speedInput - 0.3f;
        
        
        forwardstunt = true;
        thecamera.camActive = true;
        //thecam.Action();
        //thecam.playercam = true;
       
        
        
    }
    IEnumerator showstartbutton()
    {
        yield return new WaitForSeconds(2);
        start.gameObject.SetActive(true);
    }
    IEnumerator resettiles()
    {
        tileDrawn = true;
        destroytiles = true;
        yield return new WaitForSeconds(0);
        destroytiles = false;
    }
     IEnumerator resetstick()
    {
       
        destroystick = true;
        yield return new WaitForSeconds(1);
        destroystick = false;

    }
    IEnumerator cameraAction()
    {
        yield return new WaitForSeconds(3);
        Enter();
        FixedtimeStart();
    }
    public void Action()
    {
        thedirector.action = true;
        thedirector.playAction();
        StartCoroutine("cameraAction");
        play.gameObject.SetActive(false);
        
    }
    IEnumerator stuntfailing()
    {   
        failprompt = true;
        yield return new WaitForSeconds(2);
        buzzzing();
        stuntfailed.SetActive(true);      
        //correctanswertxt.text = correctanswerRO.ToString();
        stuntsfailtxt.text = PlayerPrefs.GetString("name") + (" is too") + fastslowtxt + (", the correct answer is ")+ correctanswerRO.ToString();
        yield return new WaitForSeconds(2);
        retry.gameObject.SetActive(true); 
            

        
    }
    IEnumerator playerspawn()
    {
        thePlayer.ragdollspawn();
        ragdollready = false;
        yield return new WaitForSeconds(3);
        //thePlayer.gameObject.SetActive(true);
        //thePlayer.transform.position = thestick.transform.position;
        //Destroy(thestick.gameObject);
    }
    void buzzzing()
    {
        if(buzz)
        {
            wrongsfx.Play(0);
            thecamshake.collapse();
            buzz = false;       
        }
        
    }
     void wasted()
    {
        if(busted)
        {
            StartCoroutine(cutvoice());
            busted = false;       
        }
        
    }
    IEnumerator cutvoice()
    {
        yield return new WaitForSeconds(1);
        bustedsfx.Play(0);
    }
    void dingling()
    {
        if(ding)
        {
            thePlayer.happy = true;
            thecamshake.collapse();
            correctsfx.Play();
            StartCoroutine(cutvoice());    
            ding = false;
        }
    }

   
    





}
