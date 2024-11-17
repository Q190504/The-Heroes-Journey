using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheHeroesJourney
{
    public class CreateThunderstrike : MonoBehaviour
    {
        private CultisPriest cultisPriest;
        [SerializeField] private int extraCreateAmount;
        [SerializeField] private float minXCreateRange;
        [SerializeField] private float maxXCreateRange;
        [SerializeField] private float intervalThunderstrike;
        public DelayAfterFinishTimeConfig delayAfterFinishTimeConfig;

        private Transform target;


        // Start is called before the first frame update
        void Start()
        {
            cultisPriest = GetComponent<CultisPriest>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void ThunderstrikeAbility()
        {
            StartCoroutine(Cast());
        }

        IEnumerator Cast()
        {
            target = cultisPriest.player.transform;

            AudioManager.Instance.Play("ThunderstrikePrepare");
            Thunderstrike thunderstrike = (Thunderstrike)SkillManager.Instance.Take(SkillType.ThunderStrike);
            thunderstrike.transform.position = new Vector3(target.position.x, cultisPriest.gameObject.transform.position.y + 2, cultisPriest.gameObject.transform.position.y);

            if (cultisPriest.IsBelowHalfHealth())
                StartCoroutine(CreateMultiple());

            yield return new WaitForSeconds(delayAfterFinishTimeConfig.delayAfterFinishTime);
            cultisPriest.isAttacking = false;
        }

        IEnumerator CreateMultiple()
        {
            for (int i = 0; i < extraCreateAmount; i++)
            {
                yield return new WaitForSeconds(intervalThunderstrike);
                AudioManager.Instance.Play("ThunderstrikePrepare");
                float randomX = Random.Range(minXCreateRange, maxXCreateRange);

                Thunderstrike thunderstrike = (Thunderstrike)SkillManager.Instance.Take(SkillType.ThunderStrike);
                thunderstrike.transform.position = new Vector3(target.position.x + randomX, cultisPriest.gameObject.transform.position.y + 2, cultisPriest.gameObject.transform.position.y);
            }
        }
    }
}

