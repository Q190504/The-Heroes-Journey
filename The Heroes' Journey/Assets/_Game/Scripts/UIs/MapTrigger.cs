using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheHeroesJourney
{
    public class MapTrigger : MonoBehaviour
    {
        public GameObject map;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision != null && collision.CompareTag("Player"))
                UIManager.Instance.SetCurrentMap(map.name);
        }
    }

}
