using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

namespace TheHeroesJourney
{
    public class JumpKingJumpSkill : MonoBehaviour
    {
        public JumpKingJumpSkillConfig jumpKingJumpSkillConfig;
        public DelayAfterFinishTimeConfig delayAfterFinishTimeConfig;
        public float delayJumpTime = 1f;

        private Rigidbody2D rb;
        private Animator anim;
        private JumpKing jumpKing;

        private Random random = new Random();

        private bool _earthBump = false;
        private bool _fallingRock = false;

        public Collider2D spawnRockAreaCollider;
        public float spawnRockIntervalTime;
        public float rockFallingSpeed;

        public int minRockCreate;
        public int maxRockCreate;

        void Start()
        {
            jumpKing = GetComponent<JumpKing>();
            rb = GetComponent<Rigidbody2D>();
            anim = GetComponent<Animator>();
        }

        void Update()
        {

        }

        public void JumpSkill(bool earthBump, bool fallingRock)
        {
            if (!jumpKing.isJumping)
            {
                _earthBump = earthBump;
                _fallingRock = fallingRock;
                jumpKing.isJumping = true;
                rb.velocity = new Vector2(0, 0);
                anim.SetBool("isJumping", true);
                Invoke(nameof(Jump), delayJumpTime);
            }
        }

        void Jump()
        {
            rb.velocity = new Vector2(0, Mathf.Sqrt(jumpKingJumpSkillConfig.jumpForce * 10 * Mathf.Abs(Physics2D.gravity.y)));

            StartCoroutine(Move());
        }

        IEnumerator Move()
        {
            yield return new WaitForSeconds(1);
            rb.velocity = new Vector2(0, 0);

            int direction = 1;
            if (!jumpKing.canJumpLeft)
                direction = 1;
            else if (!jumpKing.canJumpRight)
                direction = -1;
            else if(jumpKing.canJumpLeft && jumpKing.canJumpRight)
                direction = random.Next(0, 2) == 0 ? 1 : -1;
            float currentY = transform.position.y;

            if (_earthBump)
            {
                transform.position = new Vector3(jumpKing.player.transform.position.x + direction * jumpKingJumpSkillConfig.horizontalDistanceEarthBump, currentY, 0);
                AudioManager.Instance.Play("EarthBumpCreate");
            }
            else
                transform.position = new Vector3(jumpKing.player.transform.position.x + direction * jumpKingJumpSkillConfig.horizontalDistanceFallingRock, currentY, 0);


            rb.velocity = new Vector2(0, -Mathf.Sqrt(jumpKingJumpSkillConfig.jumpForce * 20 * Mathf.Abs(Physics2D.gravity.y)));
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision != null && collision.gameObject.CompareTag("Terrain") && jumpKing.isJumping)
            {
                jumpKing.isJumping = false;
                jumpKing.onGround = true;
                anim.SetBool("isJumping", false);

                if (_earthBump)
                {
                    CreateEarthBumps();
                }
                if (_fallingRock)
                {
                    AudioManager.Instance.Play("JumpKingLand");
                    StartCoroutine(CreateFallingRock());
                }

                StartCoroutine(Delay());
            }
        }

        void CreateEarthBumps()
        {
            BaseSkill earthBump1 = SkillManager.Instance.Take(SkillType.EarthBump);
            earthBump1.transform.SetLocalPositionAndRotation(transform.position + new Vector3(2.5f, 0, 0), Quaternion.Euler(0, 0, 0));
            BaseSkill earthBump2 = SkillManager.Instance.Take(SkillType.EarthBump);
            earthBump2.transform.SetLocalPositionAndRotation(transform.position + new Vector3(-2.5f, 0, 0), Quaternion.Euler(0, 180, 0));
        }

        IEnumerator CreateFallingRock()
        {
            int randomAmount = random.Next(minRockCreate, maxRockCreate);
            float minX = spawnRockAreaCollider.bounds.min.x;
            float maxX = spawnRockAreaCollider.bounds.max.x;
            for (int i = 0; i < randomAmount; i++)
            {
                yield return new WaitForSeconds(spawnRockIntervalTime);
                AudioManager.Instance.Play("RockFalling");

                float randomX = minX + (float)(random.NextDouble() * (maxX - minX));
                FallingRock rock = (FallingRock) SkillManager.Instance.Take(SkillType.FallingRock);
                rock.transform.localPosition = new Vector3(randomX, spawnRockAreaCollider.bounds.min.y, 0);

                yield return new WaitForSeconds(0.5f);

                rock.SetGravity(rockFallingSpeed);
            }
        }

        IEnumerator Delay()
        {
            _earthBump = _fallingRock = false;

            yield return new WaitForSeconds(delayAfterFinishTimeConfig.delayAfterFinishTime);
            jumpKing.isAttacking = false;
        }
    }
}

