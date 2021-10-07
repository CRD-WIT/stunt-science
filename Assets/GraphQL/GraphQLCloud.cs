using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleGraphQL;
using System;

public class GraphQLCloud : MonoBehaviour
{
    public WarningErrorUI warningErrorUI;    
    public async void GameLogMutation(int? levelValue = 1, int? stageValue = 1, string? difficultyValue = "Easy", string actionValue = "Next", float? value = 0)
    {
        var client = new GraphQLClient("https://stunt-science-cloud.herokuapp.com/graphql");

        DateTime? time_stamp = DateTime.Now;

        string gender = PlayerPrefs.GetString("Gender");        

        var request = new Request
        {
            Query = @"
                    mutation AddNewGameLog(
                    $level: Int
                    $difficulty: ENUM_GAMELOG_DIFFICULTY
                    $action: ENUM_GAMELOG_ACTION
                    $device: String
                    $id_code: String
                    $time_stamp: DateTime
                    $stage: Int
                    $gender: ENUM_GAMELOG_GENDER
                    $value: Float
                    ) {
                    createGameLog(
                        input: {
                        data: {
                            Level: $level
                            Difficulty: $difficulty
                            Action: $action
                            DeviceID: $device
                            IDCode: $id_code
                            TimeStamp: $time_stamp
                            Stage: $stage
                            Gender: $gender
                            Value: $value
                        }
                        }
                    ) {
                        gameLog {
                        DeviceID
                        IDCode
                        TimeStamp
                        Level
                        Stage
                        Difficulty
                        Action
                        Gender
                        Value
                        }
                    }
                    }
            ",
            Variables = new
            {
                level = levelValue,
                difficulty = difficultyValue,
                action = actionValue,
                id_code = PlayerPrefs.GetString("IDCode"),
                device = SystemInfo.deviceUniqueIdentifier,
                time_stamp = time_stamp,
                stage = stageValue,
                gender = gender.Length > 1 ? gender : "Male",
                value = value
            }
        };
        var responseType = new
        { };

        if (warningErrorUI)
        {
            if (warningErrorUI.promptInternetConnection)
            {
                StartCoroutine(warningErrorUI.checkInternetConnection(async (isConnected) =>
                {
                    if (isConnected)
                    {

                        var response = await client.Send(() => responseType, request);

                        if (response.Data.ToString().Length > 0)
                        {
                            Debug.Log("GraphQL Success");
                            Debug.Log($"Transaction: lv-{levelValue} s-{stageValue} d-{difficultyValue} a-{actionValue} v-{value}");
                        }
                        else
                        {
                            Debug.Log("GraphQL Failed");
                        }
                    }
                    else
                    {
                        warningErrorUI.message = "Could not complete action. Please check your internet.";
                        warningErrorUI.toggleInternetConnectionError();
                    }

                }));
            }
        }
    }

    public async void GameKeyQuery(String IDCode)
    {       
        var client = new GraphQLClient("https://stunt-science-cloud.herokuapp.com/graphql"); 

        var request = new Request
        {
            Query = @"
                query($id_code:String){
                    gameKeys(where:{IDCode:$id_code}) {
                        IDCode
                    }
                }
            ",
            Variables = new
            {
                id_code = IDCode,
            }
        };
        var responseType = new
        { };

        if (warningErrorUI.promptInternetConnection)
        {
            StartCoroutine(warningErrorUI.checkInternetConnection(async (isConnected) =>
            {
                if (isConnected)
                {

                    var response = await client.Send(() => responseType, request);

                    if (response.Data.ToString().Length > 0)
                    {
                        Debug.Log("GraphQL Success");
                        Debug.Log($"{response.ToString()}");
                    }
                    else
                    {
                        Debug.Log("GraphQL Failed");
                    }
                }
                else
                {
                    warningErrorUI.message = "Could not complete action. Please check your internet.";
                    warningErrorUI.toggleInternetConnectionError();
                }

            }));
        }

    }
}
