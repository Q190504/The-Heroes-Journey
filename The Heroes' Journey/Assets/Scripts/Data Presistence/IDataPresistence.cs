using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheHeroesJourney
{
    public interface IDataPresistence 
    {
        void LoadData(GameData data);

        void SaveData(GameData data);
    }
}
