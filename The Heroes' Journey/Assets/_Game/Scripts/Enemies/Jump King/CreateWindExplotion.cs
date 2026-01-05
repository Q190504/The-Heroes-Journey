using System.Collections;
using System.Collections.Generic;
using TheHeroesJourney;
using UnityEngine;
using Random = System.Random;

namespace TheHeroesJourney
{
    public class CreateWindExplotion : MonoBehaviour
    {
        private JumpKing jumpKing;
        public DelayAfterFinishTimeConfig delayAfterFinishTimeConfig;
        public int explotionAmount;
        public float explotionInterval;

        public int minXRange;
        public int maxXRange;
        public int minYRange;
        public int maxYRange;

        private Random random = new Random();

        // Start is called before the first frame update
        void Start()
        {
            jumpKing = GetComponent<JumpKing>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void WindExplotion()
        {
            StartCoroutine(Shoot());
        }

        IEnumerator Shoot()
        {
            for (int i = 0; i < explotionAmount; i++)
            {
                yield return new WaitForSeconds(explotionInterval);
                BaseSkill windExplotion = SkillManager.Instance.Take(SkillType.WindExplotion);
                float randomX = random.Next(minXRange, maxXRange);
                float randomY = random.Next(minYRange, maxYRange);

                windExplotion.transform.localPosition = transform.position + new Vector3(randomX, randomY, 0);
            }

            yield return new WaitForSeconds(delayAfterFinishTimeConfig.delayAfterFinishTime);
            jumpKing.isAttacking = false;
        }
    }
}

