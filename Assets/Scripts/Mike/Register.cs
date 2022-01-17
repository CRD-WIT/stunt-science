using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
// using Firebase;
// using Firebase.Firestore;
// using Firebase.Extensions;
using System.Collections.Generic;

public class Register : MonoBehaviour
{
    public GameObject pName;
    public InputField pCode;
    public Text popUp;
    public WarningErrorUI warningErrorUI;
    public FirebaseManager firebaseManager;
    // FirebaseApp app;

    void test()
    {
        Debug.Log("Called test");
    }

    void ToggleError(string errorMessages)
    {
        warningErrorUI.message = errorMessages;
        warningErrorUI.togglePanel();
    }


    public void RegisterPlayer()
    {
        // app = Firebase.FirebaseApp.DefaultInstance;
        // FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        // DocumentReference docRef;
        // docRef = db.Collection("game_keys").Document(pCode.text);
        // Dictionary<string, object> log = new Dictionary<string, object>{
        //                     {"binded_device", SystemInfo.deviceUniqueIdentifier},
        //                     {"activated", pCode.text=="05ada8"?false:true}
        //                 };

        // Debug.Log("Registering player");
        PlayerPrefs.SetInt("Life", 3);
        RegistrationManager.playerName = pName.GetComponent<Text>().text;
        RegistrationManager.playerCode = pCode.GetComponent<InputField>().text;
        PlayerPrefs.SetString("Name", RegistrationManager.playerName);
        PlayerPrefs.SetString("IDCode", RegistrationManager.playerCode);
        PlayerPrefs.SetString("Gender", RegistrationManager.playerGender);
        SceneManager.LoadScene("Onboarding");

        // await docRef.SetAsync(log, SetOptions.MergeAll).ContinueWithOnMainThread(task =>
        //                             {
        //                                 Debug.Log("Data updated to the log.");
        //                                 SceneManager.LoadScene("Onboarding");
        //                             });

    }

    // async void RegisterPlayer()
    // {
    //     app = Firebase.FirebaseApp.DefaultInstance;
    //     FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
    //     DocumentReference docRef;
    //     docRef = db.Collection("game_keys").Document(pCode.text);
    //     Dictionary<string, object> log = new Dictionary<string, object>{
    //                         {"binded_device", SystemInfo.deviceUniqueIdentifier},
    //                         {"activated", pCode.text=="05ada8"?false:true}
    //                     };

    //     Debug.Log("Registering player");
    //     PlayerPrefs.SetInt("Life", 3);
    //     RegistrationManager.playerName = pName.GetComponent<Text>().text;
    //     RegistrationManager.playerCode = pCode.GetComponent<InputField>().text;
    //     PlayerPrefs.SetString("Name", RegistrationManager.playerName);
    //     PlayerPrefs.SetString("IDCode", RegistrationManager.playerCode);
    //     PlayerPrefs.SetString("Gender", RegistrationManager.playerGender);

    //     await docRef.SetAsync(log, SetOptions.MergeAll).ContinueWithOnMainThread(task =>
    //                                 {
    //                                     Debug.Log("Data updated to the log.");
    //                                     SceneManager.LoadScene("Onboarding");
    //                                 });

    // }

    public async void StoreProfile()
    {
        // Error checkup
        bool error = false;
        string errorMessages = "";


        if (pName.GetComponent<Text>().text.Length < 2)
        {
            error = true;
            errorMessages += "Invalid name. Please review your name.\n";
        }

        if (RegistrationManager.playerGender == null)
        {
            error = true;
            errorMessages += "Missing gender selection.\n";
        }

        // if (pCode.text.Length < 3)
        // {
        //     error = true;
        //     errorMessages += "Invalid code. Please check or ask your teacher.\n";
        // }

        if (error)
        {
            Debug.Log(errorMessages);
            ToggleError(errorMessages);
        }
        else
        {
            RegisterPlayer();
        }
    }


    // if (SystemInfo.deviceType == DeviceType.Desktop)
    // {
    //     // app = Firebase.FirebaseApp.DefaultInstance;
    //     // FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
    //     // DocumentReference docRef;

    //     // if (pCode.text.Length > 0)
    //     // {
    //     //     docRef = db.Collection("game_keys").Document(pCode.text);

    //     //     await docRef.GetSnapshotAsync().ContinueWith((task) =>
    //     //     {
    //     //         var snapshot = task.Result;
    //     //         if (snapshot.Exists)
    //     //         {
    //     //             Debug.Log($"Key exists");

    //     //             Dictionary<string, object> gkey = snapshot.ToDictionary();

    //     //             Debug.Log($"Activated Status: {gkey["activated"]}");

    //     //             if (bool.Parse(gkey["activated"].ToString()) == true)
    //     //             {
    //     //                 error = true;
    //     //                 errorMessages += "Your keycode already taken.\n";
    //     //             }
    //     //         }
    //     //         else
    //     //         {
    //     //             error = true;
    //     //             errorMessages += "Your keycode is invalid. Please review your entry.\n";

    //     //         }
    //     //     });
    // }
    // else
    // {
    //     await Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(async (task) =>
    //         {
    //             var dependencyStatus = task.Result;
    //             if (dependencyStatus == Firebase.DependencyStatus.Available)
    //             {
    //                 app = Firebase.FirebaseApp.DefaultInstance;
    //                 FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
    //                 DocumentReference docRef;

    //                 if (pCode.text.Length > 0)
    //                 {
    //                     docRef = db.Collection("game_keys").Document(pCode.text);

    //                     await docRef.GetSnapshotAsync().ContinueWith((task) =>
    //                     {
    //                         var snapshot = task.Result;
    //                         if (snapshot.Exists)
    //                         {
    //                             Debug.Log($"Key exists");

    //                             Dictionary<string, object> gkey = snapshot.ToDictionary();

    //                             Debug.Log($"Activated Status: {gkey["activated"]}");

    //                             if (bool.Parse(gkey["activated"].ToString()) == true)
    //                             {
    //                                 error = true;
    //                                 errorMessages += "Your keycode already taken.\n";
    //                             }
    //                         }
    //                         else
    //                         {
    //                             error = true;
    //                             errorMessages += "Your keycode is invalid. Please review your entry.\n";

    //                         }
    //                     });

    //                     if (pName.GetComponent<Text>().text.Length < 2)
    //                     {
    //                         error = true;
    //                         errorMessages += "Invalid name. Please review your name.\n";
    //                     }

    //                     if (RegistrationManager.playerGender == null)
    //                     {
    //                         error = true;
    //                         errorMessages += "Missing gender selection.\n";
    //                     }

    //                     if (pCode.text.Length < 3)
    //                     {
    //                         error = true;
    //                         errorMessages += "Invalid code. Please check or ask your teacher.\n";
    //                     }

    //                     if (error)
    //                     {
    //                         Debug.Log(errorMessages);
    //                         ToggleError(errorMessages);
    //                     }
    //                     else
    //                     {
    //                         RegisterPlayer();
    //                     }
    //                 }
    //                 else
    //                 {
    //                     error = true;
    //                     errorMessages += "Empty code. Please check or ask your teacher.\n";
    //                     ToggleError(errorMessages);
    //                 }
    //             }
    //             else
    //             {
    //                 error = true;
    //                 errorMessages += "Unable to detect Google play dependencies.\n";
    //                 ToggleError(errorMessages);
    //             }
    //         });
    // }
    // }
    IEnumerator LateCall()
    {
        popUp.gameObject.SetActive(true);
        yield return new WaitForSeconds(3f);
        popUp.gameObject.SetActive(false);
    }
}