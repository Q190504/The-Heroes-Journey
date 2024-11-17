using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheHeroesJourney
{
    public enum EnemyType
    {
        Creep,
        Boss
    }

    public class EnemyManager : MonoBehaviour
    {
        private static EnemyManager _instance;

        public List<BaseEnemy> _enemyListPrefab;
        private Queue<BaseEnemy> activeCreep;
        private Queue<BaseEnemy> activeBoss;
        public Transform _poolPanel;

        public static EnemyManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType<EnemyManager>();
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
            activeBoss = new Queue<BaseEnemy>();
            activeCreep = new Queue<BaseEnemy>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public BaseEnemy GetAnEnemyPrefab(int index)
        {
            if (index >= 0 && index < _enemyListPrefab.Count) { return _enemyListPrefab[index]; }
            return null;
        }

        //public int SpawnCreepInRandomPos(int num)
        //{
        //    if (num <= 0) return 0;
        //    for (int i = 0; i < num; i++)
        //    {
        //        float xPosition = Random.Range(0f, 2f);
        //        float yPosition = Random.Range(0f, 2f);

        //        xPosition = Mathf.Clamp01(xPosition);
        //        yPosition = Mathf.Clamp01(yPosition);
        //        Vector3 randomPosition = new Vector3(xPosition, (float)(yPosition - 0.3), 0);
        //        randomPosition = Camera.main.ViewportToWorldPoint(randomPosition);

        //        BaseEnemy creep = Instance.Take(0, num);
        //        creep.transform.position = randomPosition;
        //    }
        //    return num;
        //}

        //public int SpawnBoss(int num)
        //{
        //    if (num <= 0) return 0;
        //    for (int i = 0; i < num; i++)
        //    {
        //        float xPosition = 1f;
        //        float yPosition = Random.Range(0f, 2f);

        //        xPosition = Mathf.Clamp01(xPosition);
        //        yPosition = Mathf.Clamp01(yPosition);
        //        Vector3 position = new Vector3(xPosition, (float)(yPosition - 0.3), 0);
        //        position = Camera.main.ViewportToWorldPoint(position);

        //        BaseEnemy boss = Instance.Take(1, num);
        //        boss.transform.position = position;
        //    }
        //    return num;
        //}

        private void PrepareEnemy(EnemyType type, int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                BaseEnemy enemy = Instantiate(_enemyListPrefab[0], _poolPanel);
                enemy.gameObject.SetActive(false);
                if (type == EnemyType.Creep)
                    activeCreep.Enqueue(enemy);
                else if (type == EnemyType.Boss)
                    activeBoss.Enqueue(enemy);
            }
        }
        public BaseEnemy Take(EnemyType type, int num)
        {
            BaseEnemy enemy = null;
            if (type == EnemyType.Creep)
            {
                if (this.activeCreep.Count <= 0)
                    PrepareEnemy(type, num);
                enemy = this.activeCreep.Dequeue();
            }
            else if (type == EnemyType.Boss)
            {
                if (this.activeBoss.Count <= 0)
                    PrepareEnemy(type, num);
                enemy = this.activeBoss.Dequeue();
            }
            enemy.gameObject.SetActive(true);
            return enemy;
        }
        public void Return(BaseEnemy enemy, EnemyType type)
        {
            if (type == EnemyType.Creep)
            {
                this.activeCreep.Enqueue(enemy);
            }
            else if (type == EnemyType.Boss)
            {
                this.activeBoss.Enqueue(enemy);
            }
            enemy.transform.SetParent(this._poolPanel);
            enemy.gameObject.SetActive(false);
        }
    }
}
