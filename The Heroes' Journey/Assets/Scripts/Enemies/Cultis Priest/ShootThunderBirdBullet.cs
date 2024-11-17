using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheHeroesJourney
{
    public class ShootThunderBirdBullet : MonoBehaviour
    {
        private CultisPriest cultisPriest;
        public DelayAfterFinishTimeConfig delayAfterFinishTimeConfig;

        // Start is called before the first frame update
        void Start()
        {
            cultisPriest = GetComponent<CultisPriest>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void ThunderBirdBullet()
        {
            StartCoroutine(Shoot());
        }

        IEnumerator Shoot()
        {
            BaseBullet bullet = BulletManager.Instance.Take(BulletType.ThunderBirdBullet);
            if (transform.localScale.x > 0)
            {
                bullet.bulletMoveDirection = new Vector3(1, 0, 0);
                bullet.transform.SetLocalPositionAndRotation(transform.position - new Vector3(0, 0.35f, 0), Quaternion.Euler(0, 0, bullet.config.angle));
            }
            else if (transform.localScale.x < 0)
            {
                bullet.bulletMoveDirection = new Vector3(-1, 0, 0);
                bullet.transform.SetLocalPositionAndRotation(transform.position - new Vector3(0, 0.35f, 0), Quaternion.Euler(0, 0, bullet.config.angle - 180));
            }

            yield return new WaitForSeconds(delayAfterFinishTimeConfig.delayAfterFinishTime);
            cultisPriest.isAttacking = false;
        }
    }
}
