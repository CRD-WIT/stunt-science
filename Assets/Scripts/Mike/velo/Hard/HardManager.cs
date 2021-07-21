using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HardManager : MonoBehaviour
{
    public BossScript boss;
    PlayerV1_1 myPlayer;
    QuestionControllerVThree qc;
    float x, y, bossV;
    // Start is called before the first frame update
    void Start()
    {
        boss = FindObjectOfType<BossScript>();
        myPlayer = FindObjectOfType<PlayerV1_1>();
        qc = FindObjectOfType<QuestionControllerVThree>();
        SetUp();
    }

    // Update is called once per frame
    void Update()
    {
        // boss.VelocityOfTheHead(x, y, -bossV);
    }
    void SetUp()
    {
        x = Random.Range(3f, 10f);
        y = Random.Range(2f, 5f);
        bossV = Random.Range(8f, 10f);
        boss.SetVelocityOfTheHead(x, y, -bossV);
    }
}
