using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FirebaseSample : MonoBehaviour
{
    public Text txtMessage;
    public int level = 0;
    private void OnEnable()
    {
        FirebaseManager.onFirebaseInitCallback -= this.FirebaseInit;
        FirebaseManager.onFirebaseInitCallback += this.FirebaseInit;
        
    }
    private void OnDisable()
    {
        FirebaseManager.onFirebaseInitCallback -= this.FirebaseInit;
    }
    public void FirebaseInit(bool isInit)
    {
        this.txtMessage.text = $"Firebase Init: {isInit}";
    }

    public void LevelUp(int level)
    {
        FirebaseManager.LogEvent($"Level_Up_{level}");
        Dictionary<string, string> dic = new Dictionary<string, string>();
        dic.Add("Level", level.ToString());
        FirebaseManager.LogEvent("Level_Up", dic);
    }

    public void ClickLevelUp()
    {
        this.level ++;
        this.LevelUp(this.level);
    }
}
