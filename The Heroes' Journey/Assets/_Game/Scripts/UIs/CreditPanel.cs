using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheHeroesJourney
{
    public class CreditPanel : MonoBehaviour
    {
        public void EndCredit()
        {
            AudioManager.Instance.Stop("CreditTheme");
            FirebaseManager.LogEvent("Event_EndCredit");
            LevelManager.Instance.ReturnToMainMenu();
        }
    }
}

