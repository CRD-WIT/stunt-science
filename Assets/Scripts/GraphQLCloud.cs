using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleGraphQL;
using System;

public class GraphQLCloud : MonoBehaviour
{
    public GraphQLConfig Config;

    public async void GameLogMutation(string? modeValue = "Problem", string? genderValue="Male", int? stageValue=1, int? levelValue=1, string? difficultyValue="Easy", string actionValue="Next", string deviceID="", DateTime? startTime=null, DateTime? endTime=null)
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
                level = levelValue,
                difficulty = difficultyValue,
                action = actionValue,
                device = deviceID,
                end = startTime,
                start = endTime,
                stage = stageValue,
                gender = genderValue,
                mode=modeValue
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

    }

    // Update is called once per frame
    void Update()
    {

    }
}
