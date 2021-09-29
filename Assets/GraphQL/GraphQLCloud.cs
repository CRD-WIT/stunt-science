using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleGraphQL;
using System;

public class GraphQLCloud : MonoBehaviour
{
    public async void GameLogMutation(int? levelValue = 1, int? stageValue = 1, string? difficultyValue = "Easy", string actionValue = "Next", float? value = 0)
    {
        DateTime? time_stamp = DateTime.Now;

        string gender = PlayerPrefs.GetString("Gender");

        var client = new GraphQLClient("https://stunt-science-cloud.herokuapp.com/graphql");

        var request = new Request
        {
            Query = @"
                    mutation AddNewGameLog(
                    $level: Int
                    $difficulty: ENUM_GAMELOG_DIFFICULTY
                    $action: ENUM_GAMELOG_ACTION
                    $device: String
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
                            TimeStamp: $time_stamp
                            Stage: $stage
                            Gender: $gender
                            Value: $value
                        }
                        }
                    ) {
                        gameLog {
                        DeviceID
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
                device = SystemInfo.deviceUniqueIdentifier,
                time_stamp = time_stamp,
                stage = stageValue,
                gender = gender.Length > 1 ? gender : "Male",
                value = value
            }
        };
        var responseType = new
        { };
        var response = await client.Send(() => responseType, request);

        Debug.Log(response.Data.ToString());
    }
}
