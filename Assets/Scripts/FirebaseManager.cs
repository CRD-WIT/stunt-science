using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Firestore;
using Firebase.Extensions;
using System;


public class FirebaseManager : MonoBehaviour
{
    FirebaseApp app;
    string keyCode = "";

    void Start()
    {

    }

    public void SetKeyCode(string value)
    {
        keyCode = value;
    }

    public async void CheckIfKeyCodeValid(Action callback)
    {
        bool isValid;

    }

#nullable enable
    public void GameLogMutation(int? levelValue = 1, int? stageValue = 1, string? difficultyValue = "Easy", string actionValue = "Next", float? value = 0)
    {
        Debug.Log(SystemInfo.deviceType);
        // Check if user is using desktop or not
        if (SystemInfo.deviceType == DeviceType.Desktop)
        {
            // Create and hold a reference to your FirebaseApp,
            // where app is a Firebase.FirebaseApp property of your application class.
            app = Firebase.FirebaseApp.DefaultInstance;

            string gender = PlayerPrefs.GetString("Gender");
            string id_code = PlayerPrefs.GetString("IDCode");
            FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
            DocumentReference docRef;
            docRef = db.Collection("game_logs").Document();
            Dictionary<string, object> log = new Dictionary<string, object>
                {
                {"device_id", SystemInfo.deviceUniqueIdentifier},
                {"level" , levelValue},
                {"difficulty" , difficultyValue},
                {"action" , actionValue},
                {"id_code" , id_code },
                {"time_stamp" , Timestamp.GetCurrentTimestamp()},
                {"stage" , stageValue},
                {"gender" , gender.Length > 1 ? gender : "Male"},
                {"value" , value}
                };
            docRef.SetAsync(log).ContinueWithOnMainThread(task =>
            {
                Debug.Log("Added data to the log.");
            });
        }
        else
        {
            Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
                {
                    var dependencyStatus = task.Result;
                    if (dependencyStatus == Firebase.DependencyStatus.Available)
                    {
                        app = Firebase.FirebaseApp.DefaultInstance;

                        string gender = PlayerPrefs.GetString("Gender");
                        string id_code = PlayerPrefs.GetString("IDCode");
                        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
                        DocumentReference docRef;
                        docRef = db.Collection("game_logs").Document();
                        Dictionary<string, object> log = new Dictionary<string, object>
                {
                {"device_id", SystemInfo.deviceUniqueIdentifier},
                {"level" , levelValue},
                {"difficulty" , difficultyValue},
                {"action" , actionValue},
                {"id_code" , id_code },
                {"time_stamp" , Timestamp.GetCurrentTimestamp()},
                {"stage" , stageValue},
                {"gender" , gender.Length > 1 ? gender : "Male"},
                {"value" , value}
                };
                        docRef.SetAsync(log).ContinueWithOnMainThread(task =>
                                    {
                                        Debug.Log("Added data to the log.");
                                    });
                    }
                    else
                    {
                        UnityEngine.Debug.LogError(System.String.Format(
                        "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                    }
                });
        }
    }
}