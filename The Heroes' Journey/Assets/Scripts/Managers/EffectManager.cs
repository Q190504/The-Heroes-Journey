using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheHeroesJourney
{
    public enum EffectType
    {
        SaveGameEffect,
        HealEffect,
        BidingEffect,
        SwordSlashEffect
    }

    public class EffectManager : MonoBehaviour
    {
        private static EffectManager _instance;
        public List<GameObject> _effectListPrefab;
        public Transform _poolPanel;

        private GameObject healEffect;
        private GameObject saveGameEffect;
        private GameObject bidingEffect;
        private GameObject swordSlashEffect;

        public static EffectManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType<EffectManager>();
                return _instance;
            }
        }

        private void Awake()
        {
            if (_instance == null)
                _instance = this;
            else
                Destroy(this.gameObject);

        }

        // Start is called before the first frame update
        void Start()
        {
            PrepareEffect();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public GameObject GetAEffectPrefab(int index)
        {
            if (index >= 0 && index < _effectListPrefab.Count) { return _effectListPrefab[index]; }
            return null;
        }

        private void PrepareEffect()
        {
            saveGameEffect = Instantiate(_effectListPrefab[0], _poolPanel);
            saveGameEffect.SetActive(false);

            healEffect = Instantiate(_effectListPrefab[1], _poolPanel);
            healEffect.SetActive(false);

            bidingEffect = Instantiate(_effectListPrefab[2], _poolPanel);
            bidingEffect.SetActive(false);            
            
            swordSlashEffect = Instantiate(_effectListPrefab[3], _poolPanel);
            swordSlashEffect.SetActive(false);
        }
        public GameObject Take(EffectType type)
        {
            GameObject effect = null;
            if (type == EffectType.SaveGameEffect)
                effect = saveGameEffect;
            else if (type == EffectType.HealEffect)
                effect = healEffect;
            else if(type == EffectType.BidingEffect)
                effect = bidingEffect;            
            else if(type == EffectType.SwordSlashEffect)
                effect = swordSlashEffect;

            effect.SetActive(true);
            return effect;
        }
        public void Return(GameObject effect)
        {
            effect.transform.SetParent(this._poolPanel);
            effect.SetActive(false);
        }
    }
}
