using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheHeroesJourney
{
    public class WallInteractor : MonoBehaviour
    {
        public bool isWallJumping { get; private set; }

        [Header("Wall Slide")]
        [SerializeField][Range(0.1f, 5f)] private float wallSlideMaxSpeed = 2f;
        [Header("Wall Jump")]
        [SerializeField] private Vector2 wallJumpClimb = new Vector2(4f, 12f);
        [SerializeField] private Vector2 wallJumpBounce = new Vector2(10.7f, 10f);
        [SerializeField] private Vector2 wallJumpLeap = new Vector2(14f, 12f);

        private CollisionDataRetriever collisionDataRetriever;
        private Rigidbody2D body;
        private Controller controller;

        private Vector2 velocity;
        private bool onWall;
        private bool onGround;
        private bool desiredJump;

        private float wallDirectionX;

        private Move move;
        private Jump jump;

        // Start is called before the first frame update
        void Start()
        {
            collisionDataRetriever = GetComponent<CollisionDataRetriever>();
            body = GetComponent<Rigidbody2D>();
            controller = GetComponent<Controller>();
            move = GetComponent<Move>();
            jump = GetComponent<Jump>();
        }

        // Update is called once per frame
        void Update()
        {
            ////BAN PHIM
            //if (onWall && !onGround)
            //{
            //    desiredJump |= controller.input.RetrieveJumpInput();
            //}

            //BUTTON
            if (onWall && !onGround)
            {
                desiredJump = jump.desiredJump;
            }
        }

        private void FixedUpdate()
        {
            velocity = body.velocity;
            onWall = collisionDataRetriever.OnWall;
            onGround = collisionDataRetriever.OnGround;
            wallDirectionX = collisionDataRetriever.ContactNormal.x;

            #region Wall Slide
            if (onWall)
            {
                InGameManager.Instance.player.GetComponentInChildren<Animator>().SetTrigger("onWall");
                if (velocity.y < -wallSlideMaxSpeed)
                {
                    velocity.y = -wallSlideMaxSpeed;
                }
            }
            #endregion

            if (onWall && velocity.x == 0)
            {
                isWallJumping = false;
            }
            else if (onGround)
            {
                isWallJumping = false;
                InGameManager.Instance.player.GetComponentInChildren<Animator>().SetBool("onWall", false);
            }

            ////BAN PHIM
            //#region Wall Jump
            //if (desiredJump)
            //{
            //    AudioManager.Instance.Play("PlayerJump");

            //    if (Mathf.Abs(-wallDirectionX - controller.input.RetrieveMoveInput()) < 0.00000000001)
            //    {
            //        InGameManager.Instance.player.GetComponentInChildren<Animator>().SetBool("onWall", false);
            //        move.Turn();
            //        velocity = new Vector2(wallJumpClimb.x * wallDirectionX, wallJumpClimb.y);
            //        isWallJumping = true;
            //        desiredJump = false;
            //    }
            //    else if (controller.input.RetrieveMoveInput() == 0)
            //    {

            //        InGameManager.Instance.player.GetComponentInChildren<Animator>().SetBool("onWall", false);
            //        move.Turn();
            //        velocity = new Vector2(wallJumpBounce.x * wallDirectionX, wallJumpBounce.y);
            //        isWallJumping = true;
            //        desiredJump = false;
            //    }
            //}
            //#endregion


            //BUTTON
            #region Wall Jump
            if (desiredJump)
            {
                AudioManager.Instance.Play("PlayerJump");

                if (Mathf.Abs(-wallDirectionX - move.direction.x) < 0.00000000001)
                {
                    InGameManager.Instance.player.GetComponentInChildren<Animator>().SetBool("onWall", false);
                    move.Turn();
                    velocity = new Vector2(wallJumpClimb.x * wallDirectionX, wallJumpClimb.y);
                    isWallJumping = true;
                    desiredJump = false;
                }
                else if (move.direction.x == 0)
                {
                    InGameManager.Instance.player.GetComponentInChildren<Animator>().SetBool("onWall", false);
                    move.Turn();
                    velocity = new Vector2(wallJumpBounce.x * wallDirectionX, wallJumpBounce.y);
                    isWallJumping = true;
                    desiredJump = false;
                }
            }
            #endregion


            body.velocity = velocity;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            collisionDataRetriever.EvaluateCollision(collision);

            if (collisionDataRetriever.OnWall && !collisionDataRetriever.OnGround && isWallJumping)
            {
                body.velocity = Vector2.zero;
            }
        }
    }
}

