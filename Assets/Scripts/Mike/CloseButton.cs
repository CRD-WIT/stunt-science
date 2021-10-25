using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CloseButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void GotoMainMenu(){
        SceneManager.LoadScene("GameMenu");
    }
    public void OpenCredit()
    {
        SceneManager.LoadScene("Credits");
    }
}
