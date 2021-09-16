using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleGraphQL;
using System;

public class GraphQLCloud : MonoBehaviour
{
    public GraphQLConfig Config;
    public async void GameLogMutation()
    {
        var client = new GraphQLClient("https://stunt-science-cloud.herokuapp.com/graphql");

        var request = new Request
        {
            Query = @"
             mutation AddNewGameLog(
                $level: Int, 
                $difficulty: ENUM_GAMELOG_DIFFICULTY, 
                $action: ENUM_GAMELOG_ACTION, 
                $device: String,  
                $end: DateTime,
                $start: DateTime,
                $stage: Int,
                $gender: ENUM_GAMELOG_GENDER,
                $mode: ENUM_GAMELOG_MODE
                ){
                createGameLog(
                    input: {
                    data: {
                        Level: $level
                        Difficulty: $difficulty
                        Action: $action
                        DeviceID: $device
                        End: $end
                        Start: $start
                        Stage: $stage
                        Gender: $gender  
                        Mode: $mode
                    }
                    }
                ) {
                    gameLog {
                    DeviceID
                    Start
                    Level
                    Stage
                    Difficulty
                    End
                    Action
                    Gender
                    Mode
                    }
                 }
                }
            ",
            Variables = new
            {
                level = 1,
                difficulty = "Easy",
                action = "Completed",
                device = "123456",
                end = DateTime.Now,
                start = DateTime.Now,
                stage = 1,
                gender = "Male",
                mode="Problem"
            }
        };
        var responseType = new
        {};
        var response = await client.Send(() => responseType, request);

        Debug.Log(response.ToString());
    }
    // Start is called before the first frame update
    void Start()
    {
        GameLogMutation();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
