using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class actionCount : MonoBehaviour
{
    public GameObject cut1;
    public GameObject cut2;
    public GameObject cut3;

    public GameObject question1;
    public GameObject question2;
    public GameObject question3;
    public GameObject wrong;
    public GameObject retry1;
    public GameObject retry2;
    public GameObject retry3; 
    public GameObject showproblem;
    public GameObject gameoverbg;
     public GameObject startbg;
    
    public int actionAttempt;

    public Player theplayer;
    public GameManager themanager;
    public level2Manager thelevel2;
    public level3Manager thelevel3;

    private Vector2 theplayerstartpoint;
    public  bool destroytiles;
    public bool losepointReady;
    public GameObject monster;
    Vector2 monsterstartpoint;
    public GameObject stuntground;
    Vector2 stuntgroundstartpoint;
    public GameObject distancemarking;
    Vector2 distancemarkstartpoint;
    public camerascript thecamera;
    public GameObject cuttxt;
    public AudioSource Gameoversfx;
    public AudioSource bgm;


    

    // Start is called before the first frame update
    void Start()
    {
        theplayer = FindObjectOfType<Player>();
        themanager = FindObjectOfType<GameManager>();
        thelevel2 = FindObjectOfType<level2Manager>();
        thelevel3 = FindObjectOfType<level3Manager>();
        thecamera = FindObjectOfType<camerascript>();
        theplayerstartpoint = theplayer.transform.position;
        monsterstartpoint = monster.transform.position;
        stuntgroundstartpoint = stuntground.transform.position;
        distancemarkstartpoint = distancemarking.transform.position;
        actionAttempt = 3;
        
         StartCoroutine(startBGgone());
        
    }

    // Update is called once per frame
    void Update()
    {
        
       
        if (actionAttempt == 2)
        {
            cut3.SetActive(false);   
        }
         if (actionAttempt == 1)
        {
            cut2.SetActive(false);   
        }
        
        if (actionAttempt == 0)
        {
            cut1.SetActive(false);
            Time.timeScale = 0.4f;
            actionAttempt = 3;
            
            StartCoroutine(actionreset());  
            StartCoroutine(gameover());
        }  
    }
    public void restart()
    {
        /*theplayer.gameObject.SetActive(true);
        theplayer.transform.position = theplayerstartpoint;
        question2.SetActive(false);
        question3.SetActive(false);
        question1.SetActive(false);
        retry1.SetActive(false);
        retry2.SetActive(false);
        retry3.SetActive(false);
        wrong.SetActive(false);
        
        thelevel2.levelstart = false;
        thelevel2.backwardstunt = false;
        thelevel2.monsterready = true;
        thelevel3.levelstart = false;
        showproblem.SetActive(true);
        monster.transform.position = monsterstartpoint;
        stuntground.transform.position = stuntgroundstartpoint;
        distancemarking.transform.position = distancemarkstartpoint;
        thecamera.camActive = false;
        thecamera.stuntScene = false;
        themanager.simulate = false;
        
        theplayer.moveSpeed = 0;
        
    
         StartCoroutine(resettiles());*/
          
       

    }
    IEnumerator actionreset()
    {
        yield return new WaitForSeconds(3);
        
        cut1.SetActive(true);
        cut2.SetActive(true);
        cut3.SetActive(true); 
        
    }
    IEnumerator gameover()
    {
        bgm.Stop();
        Gameoversfx.Play();
        StartCoroutine(endBGgone());
       yield return new WaitForSeconds(1);
       
        cuttxt.SetActive(true);
        yield return new WaitForSeconds(1);
       
        SceneManager.LoadScene("LevelOne");
        Time.timeScale = 1f;
    }
    IEnumerator endBGgone()
    {
        gameoverbg.SetActive(true);
        yield return new WaitForSeconds(3);
        gameoverbg.SetActive(false);
    }
     IEnumerator endBGnextlevel()
    {
        gameoverbg.SetActive(true);
        yield return new WaitForSeconds(1);
        gameoverbg.SetActive(false);
    }
    
    public void endbgexit()
    {
        StartCoroutine(endBGnextlevel());
    }
    public IEnumerator startBGgone()
    {
      startbg.SetActive(true);
      yield return new WaitForSeconds(1);  
      startbg.SetActive(false);

    }
    public void startbgentrance()
    {
        StartCoroutine(startBGgone());
    }
    IEnumerator resettiles()
    {
       themanager.tileDrawn = true;
        destroytiles = true;
        yield return new WaitForSeconds(0);
        destroytiles = false;
    }
    public void losepoint()
    {
        if (losepointReady == false)
        {
            actionAttempt -= 1;
            losepointReady = true;
        }
    }
}
