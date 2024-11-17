using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


#if FIREBASE
using Firebase;
using Firebase.Extensions;
using Firebase.Analytics;
#endif

public class FirebaseManager : MonoBehaviour
{
    private static bool _isInitFirebase;
    public static bool IsInitFirebase => _isInitFirebase;

#if FIREBASE
    private static DependencyStatus _dependencyStatus;
    private static FirebaseApp _app;
#endif

    private static FirebaseManager _instance;

    public static FirebaseManager Instance
    {
        get
        {
            if (_instance == null)
            {
                var gObject = GameObject.Find("FirebaseManager");
                if (gObject == null) gObject = new GameObject("FirebaseManager");

                _instance = gObject.GetComponent<FirebaseManager>() == null
                    ? gObject.AddComponent<FirebaseManager>()
                    : gObject.GetComponent<FirebaseManager>();

                _instance.enabled = true;
                if (Application.isPlaying) DontDestroyOnLoad(_instance.gameObject);
            }

            return _instance;
        }
    }
    public static event System.Action<bool> onFirebaseInitCallback;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Init()
    {
        Instance.InitFirebase();
    }
    public void InitFirebase()
    {
        StartCoroutine(OnInitFirebase());
    }
    private static IEnumerator OnInitFirebase()
    {
        yield return new WaitForEndOfFrame();
        Debug.Log("Init firebase");
#if FIREBASE
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            _dependencyStatus = task.Result;

            if (_dependencyStatus == DependencyStatus.Available)
            {
                _isInitFirebase = true;
                _app = FirebaseApp.DefaultInstance;
                FirebaseInitSuccess();
                Debug.Log("Init firebase success");
            }
            else
            {
                _isInitFirebase = false;
                Debug.LogError("@LOG Could not resolve all Firebase dependencies: " + _dependencyStatus);
            }
            onFirebaseInitCallback?.Invoke(_isInitFirebase);
        });
#endif
    }
    public static void FirebaseInitSuccess()
    {
        #if FIREBASE
        FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
        Debug.Log("Set user properties.");
            // Set the user's sign up method.
        FirebaseAnalytics.SetUserProperty(
            FirebaseAnalytics.UserPropertySignUpMethod,
            "Google");
            // Set the user ID.
        FirebaseAnalytics.SetUserId(SystemInfo.deviceUniqueIdentifier);
        FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventLogin);
        #endif
    }
    public static void SetUserProperty(string propertyName, string property)
    {
        if(!IsInitFirebase)
        {
            return;
        }
        #if FIREBASE
        FirebaseAnalytics.SetUserProperty(propertyName, property);
        #endif
    }
    public static void LogEvent(string eventName)
    {
        if(!IsInitFirebase)
        {
            return;
        }
        #if FIREBASE
        FirebaseAnalytics.LogEvent(eventName);
        #endif
    }
    public static void LogEvent(string eventName, Dictionary<string, string> values)
    {
        if(!IsInitFirebase)
        {
            return;
        }
        #if FIREBASE
        List<Parameter> parameters = new List<Parameter>();
        foreach (var pair in values)
        {
            parameters.Add(new Parameter(pair.Key, pair.Value.ToString()));
        }
        
        FirebaseAnalytics.LogEvent(eventName, parameters.ToArray());
        #endif
    }
}
