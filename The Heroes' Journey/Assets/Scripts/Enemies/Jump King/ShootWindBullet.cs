using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheHeroesJourney
{
    public class ShootWindBullet : MonoBehaviour
    {
        private JumpKing jumpKing;
        public DelayAfterFinishTimeConfig delayAfterFinishTimeConfig;

        // Start is called before the first frame update
        void Start()
        {
            jumpKing = GetComponent<JumpKing>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void WindBullet()
        {
            StartCoroutine(Shoot());
        }

        IEnumerator Shoot()
        {
            AudioManager.Instance.Play("WindBulletFly");

            BaseBullet bullet = BulletManager.Instance.Take(BulletType.WindBullet);
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
            jumpKing.isAttacking = false;
        }
    }
}
