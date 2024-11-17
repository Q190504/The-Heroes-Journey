using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheHeroesJourney
{
    public enum BulletType
    {
        NormBullet,
        Trophy,
        WindBullet,
        ThunderBirdBullet
    }
    public class BulletManager : MonoBehaviour
    {
        private static BulletManager _instance;
        public List<BaseBullet> _bulletListPrefab;
        private Queue<BaseBullet> activeNormBullet;
        private Queue<BaseBullet> activeTrophy;
        private Queue<BaseBullet> activeWindBullet;
        private Queue<BaseBullet> activeThunderBirdBullet;
        public Transform _poolPanel;
        public int bulletPrepare;

        public static BulletManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType<BulletManager>();
                return _instance;
            }
        }

        private void Awake()
        {
            if (_instance == null)
                _instance = this;
            else
                Destroy(this.gameObject);

            bulletPrepare = 20;
        }

        // Start is called before the first frame update
        void Start()
        {
            activeNormBullet = new Queue<BaseBullet>();
            activeTrophy = new Queue<BaseBullet>();
            activeWindBullet = new Queue<BaseBullet>();
            activeThunderBirdBullet = new Queue<BaseBullet>();
            PrepareBullet(BulletType.NormBullet);
            PrepareBullet(BulletType.Trophy);
            PrepareBullet(BulletType.WindBullet);
            PrepareBullet(BulletType.ThunderBirdBullet);
        }

        // Update is called once per frame
        void Update()
        {

        }

        public BaseBullet GetABulletPrefab(int index)
        {
            if (index >= 0 && index < _bulletListPrefab.Count) { return _bulletListPrefab[index]; }
            return null;
        }

        private void PrepareBullet(BulletType bulletType)
        {
            for (int i = 0; i < bulletPrepare; i++)
            {
                if (bulletType == BulletType.NormBullet)
                {
                    BaseBullet bullet = Instantiate(_bulletListPrefab[0], _poolPanel);
                    bullet.gameObject.SetActive(false);
                    activeNormBullet.Enqueue(bullet);
                }
                else if (bulletType == BulletType.Trophy)
                {
                    BaseBullet bullet = Instantiate(_bulletListPrefab[1], _poolPanel);
                    bullet.gameObject.SetActive(false);
                    activeTrophy.Enqueue(bullet);
                }
                else if (bulletType == BulletType.WindBullet)
                {
                    BaseBullet bullet = Instantiate(_bulletListPrefab[2], _poolPanel);
                    bullet.gameObject.SetActive(false);
                    activeWindBullet.Enqueue(bullet);
                }
                else if (bulletType == BulletType.ThunderBirdBullet)
                {
                    BaseBullet bullet = Instantiate(_bulletListPrefab[3], _poolPanel);
                    bullet.gameObject.SetActive(false);
                    activeThunderBirdBullet.Enqueue(bullet);
                }
            }
        }
        public BaseBullet Take(BulletType type)
        {
            BaseBullet b = null;
            if (type == BulletType.NormBullet)
            {
                if (this.activeNormBullet.Count <= 0)
                    PrepareBullet(type);
                b = this.activeNormBullet.Dequeue();
            }
            else if (type == BulletType.Trophy)
            {
                if (this.activeTrophy.Count <= 0)
                    PrepareBullet(type);
                b = this.activeTrophy.Dequeue();
            }
            else if (type == BulletType.WindBullet)
            {
                if (this.activeWindBullet.Count <= 0)
                    PrepareBullet(type);
                b = this.activeWindBullet.Dequeue();
            }
            else if (type == BulletType.ThunderBirdBullet)
            {
                if (this.activeThunderBirdBullet.Count <= 0)
                    PrepareBullet(type);
                b = this.activeThunderBirdBullet.Dequeue();
            }
            b.gameObject.SetActive(true);
            return b;
        }
        public void Return(BaseBullet b, BulletType type)
        {
            if (type == BulletType.NormBullet)
            {
                this.activeNormBullet.Enqueue(b);
            }
            else if (type == BulletType.Trophy)
            {
                this.activeTrophy.Enqueue(b);
            }
            else if (type == BulletType.WindBullet)
            {
                this.activeWindBullet.Enqueue(b);
            }
            else if (type == BulletType.ThunderBirdBullet)
            {
                this.activeThunderBirdBullet.Enqueue(b);
            }
            b.transform.SetParent(this._poolPanel);
            b.gameObject.SetActive(false);
        }
    }
}

