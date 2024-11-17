using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheHeroesJourney
{
    public enum SkillType
    {
        WindExplotion,
        EarthBump,
        FallingRock,
        ThunderStrike
    }

    public class SkillManager : MonoBehaviour
    {
        private static SkillManager _instance;
        public List<BaseSkill> _skillListPrefab;
        private Queue<BaseSkill> activeWindExplotion;
        private Queue<BaseSkill> activeEarthBump;
        private Queue<BaseSkill> activeFallingRock;
        private Queue<BaseSkill> activeThunderStrike;
        public Transform _poolPanel;
        public int skillAmountPrepare;

        public static SkillManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType<SkillManager>();
                return _instance;
            }
        }

        private void Awake()
        {
            if (_instance == null)
                _instance = this;
            else
                Destroy(this.gameObject);

            skillAmountPrepare = 20;
        }

        // Start is called before the first frame update
        void Start()
        {
            activeWindExplotion = new Queue<BaseSkill>();
            activeEarthBump = new Queue<BaseSkill>();
            activeFallingRock = new Queue<BaseSkill>();
            activeThunderStrike = new Queue<BaseSkill>();
            PrepareSkill(SkillType.WindExplotion);
            PrepareSkill(SkillType.EarthBump);
            PrepareSkill(SkillType.FallingRock);
            PrepareSkill(SkillType.ThunderStrike);
        }

        // Update is called once per frame
        void Update()
        {

        }

        public BaseSkill GetASkillPrefab(int index)
        {
            if (index >= 0 && index < _skillListPrefab.Count) { return _skillListPrefab[index]; }
            return null;
        }

        private void PrepareSkill(SkillType skillType)
        {
            for (int i = 0; i < skillAmountPrepare; i++)
            {
                if (skillType == SkillType.WindExplotion)
                {
                    BaseSkill skill = Instantiate(_skillListPrefab[0], _poolPanel);
                    skill.gameObject.SetActive(false);
                    activeWindExplotion.Enqueue(skill);
                }
                else if (skillType == SkillType.EarthBump)
                {
                    BaseSkill skill = Instantiate(_skillListPrefab[1], _poolPanel);
                    skill.gameObject.SetActive(false);
                    activeEarthBump.Enqueue(skill);
                }
                else if (skillType == SkillType.FallingRock)
                {
                    BaseSkill skill = Instantiate(_skillListPrefab[2], _poolPanel);
                    skill.gameObject.SetActive(false);
                    activeFallingRock.Enqueue(skill);
                }
                else if (skillType == SkillType.ThunderStrike)
                {
                    BaseSkill skill = Instantiate(_skillListPrefab[3], _poolPanel);
                    skill.gameObject.SetActive(false);
                    activeThunderStrike.Enqueue(skill);
                }
            }
        }
        public BaseSkill Take(SkillType type)
        {
            BaseSkill s = null;
            if (type == SkillType.WindExplotion)
            {
                if (this.activeWindExplotion.Count <= 0)
                    PrepareSkill(type);
                s = this.activeWindExplotion.Dequeue();
            }
            else if (type == SkillType.EarthBump)
            {
                if (this.activeEarthBump.Count <= 0)
                    PrepareSkill(type);
                s = this.activeEarthBump.Dequeue();
            }
            else if (type == SkillType.FallingRock)
            {
                if (this.activeFallingRock.Count <= 0)
                    PrepareSkill(type);
                s = this.activeFallingRock.Dequeue();
            }
            else if (type == SkillType.ThunderStrike)
            {
                if (this.activeThunderStrike.Count <= 0)
                    PrepareSkill(type);
                s = this.activeThunderStrike.Dequeue();
            }
            s.gameObject.SetActive(true);
            return s;
        }
        public void Return(BaseSkill s, SkillType type)
        {
            if (type == SkillType.WindExplotion)
            {
                this.activeWindExplotion.Enqueue(s);
            }
            else if (type == SkillType.EarthBump)
            {
                this.activeEarthBump.Enqueue(s);
            }
            else if (type == SkillType.FallingRock)
            {
                this.activeFallingRock.Enqueue(s);
            }
            else if (type == SkillType.ThunderStrike)
            {
                this.activeThunderStrike.Enqueue(s);
            }

            s.transform.SetParent(this._poolPanel);
            s.gameObject.SetActive(false);
        }
    }
}

