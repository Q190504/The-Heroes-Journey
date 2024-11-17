using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheHeroesJourney
{
    public class FallingRock : BaseSkill
    {
        private Rigidbody2D rb;
        private Animator anim;

        private float fallingSpeed;
        private bool isBreaking = false;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            anim = GetComponent<Animator>();
        }

        // Start is called before the first frame update
        void Start()
        {
            SetGravity(0);
        }

        // Update is called once per frame
        void Update()
        {
            if (InGameManager.Instance.isGameFinished)
                DestroySkill();

            anim.SetFloat("yVelocity", rb.velocity.y);

            if (isBreaking)
                rb.velocity = Vector3.zero;
        }

        public void SetGravity(float gravity)
        {
            rb.gravityScale = gravity;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision != null)
            {
                if (collision.CompareTag("Player"))
                {
                    DealDamage();
                    DestroySkill();        
                }
                else if (collision.CompareTag("Terrain"))
                    DestroySkill();
            }
        }

        public override void DestroySkill()
        {
            StartCoroutine(BreakAnimation());
        }

        IEnumerator BreakAnimation()
        {
            SetGravity(0);

            isBreaking = true;
            AudioManager.Instance.Play("RockBreak");
            anim.SetTrigger("break");

            yield return new WaitForSeconds(0.5f);
            isBreaking = false;
            SkillManager.Instance.Return(this, SkillType.FallingRock);
        }
    }
}

