using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheHeroesJourney
{
    public class SaveGameButton : MonoBehaviour
    {
        public void SaveGame()
        {
            DataPresistenceManager.Instance.SaveGame();
        }
    }
}
