using System.Collections;
using System.Collections.Generic;
using TheHeroesJourney;
using UnityEngine;

public class Jump : MonoBehaviour
{
    [SerializeField] private InputController input = null;
    [SerializeField, Range(0f, 30f)] private float jumpHeight = 20f;
    [SerializeField, Range(0f, 5)] private int maxAirJumps;
    [SerializeField, Range(0f, 5f)] private float downwardMovementMultiplier = 3f;
    [SerializeField, Range(0f, 5f)] private float upwardMovementMultiplier = 1.7f;
    [SerializeField, Range(0f, 0.3f)] private float coyoteTime;
    [SerializeField, Range(0f, 0.3f)] private float jumpBufferTime = 0.2f;

    [Header("Camera")]
    [SerializeField] private GameObject _cameraFollowGO;
    private CameraFollowObject _cameraFollowObject;

    private float _fallSpeedYDampingChangeThreshold;


    private Rigidbody2D body;
    private CollisionDataRetriever ground;
    private Vector3 velocity;

    private int jumpPhase;
    private float defaultGravityScale;
    private float coyoteCounter;
    private float jumpBufferCounter;

    public bool desiredJump;
    private bool onGround;
    private bool isJumping;

    public Player player;

    private bool isJumpButtonHeldDown;

    void Awake()
    {
        player = GetComponent<Player>();
        body = GetComponent<Rigidbody2D>();
        ground = GetComponent<CollisionDataRetriever>();
        _cameraFollowObject = _cameraFollowGO.GetComponent<CameraFollowObject>();

        _fallSpeedYDampingChangeThreshold = CameraManager.Instance._fallSpeedYDampingChangeThreshold;

        defaultGravityScale = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (PauseManager.isPause || (IsTigerActive() && player.isDashing) || player.isDogHealing)
            return;

        ////BAN PHIM
        //desiredJump |= input.RetrieveJumpInput();

        //if player is falling past a certain speed threshold
        if (body.velocity.y < _fallSpeedYDampingChangeThreshold && !CameraManager.Instance.IsLerpingYDamping && !CameraManager.Instance.LerpedFromPlayerFalling)
            CameraManager.Instance.LerpYDamping(true);

        //if player is stading still or moving up
        if (body.velocity.y >= 0f && !CameraManager.Instance.IsLerpingYDamping && CameraManager.Instance.LerpedFromPlayerFalling)
        {
            //reset so it can be called again
            CameraManager.Instance.LerpedFromPlayerFalling = false;

            CameraManager.Instance.LerpYDamping(false);
        }

    }

    private void FixedUpdate()
    {
        onGround = ground.OnGround;
        velocity = body.velocity;

        player.GetComponentInChildren<Animator>().SetFloat("yVelocity", velocity.y);

        if (onGround && body.velocity.y == 0)
        {
            if (isJumping)
                AudioManager.Instance.Play("PlayerLand");

            player.GetComponentInChildren<Animator>().SetBool("isJumping", false);
            jumpPhase = 0;
            coyoteCounter = coyoteTime;
            isJumping = false;
        }
        else
        {
            coyoteCounter -= Time.deltaTime;
        }

        if (desiredJump)
        {
            desiredJump = false;
            jumpBufferCounter = jumpBufferTime;
        }
        else if (!desiredJump && jumpBufferCounter > 0)
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        if (jumpBufferCounter > 0)
        {
            JumpAction();
        }

        ////BAN PHIM
        //if (input.RetrieveJumpHoldInput() && body.velocity.y > 0)
        //{
        //    body.gravityScale = upwardMovementMultiplier;
        //}
        //else if ((!input.RetrieveJumpHoldInput() || body.velocity.y < 0) && !player.isDashing)
        //{
        //    body.gravityScale = downwardMovementMultiplier;
        //}
        //else if (body.velocity.y == 0)
        //{
        //    body.gravityScale = defaultGravityScale;
        //}

        //BUTTON
        if (isJumpButtonHeldDown && body.velocity.y > 0)
        {
            body.gravityScale = upwardMovementMultiplier;
        }
        else if ((!isJumpButtonHeldDown || body.velocity.y < 0) && !player.isDashing)
        {
            body.gravityScale = downwardMovementMultiplier;
        }
        else if (body.velocity.y == 0)
        {
            body.gravityScale = defaultGravityScale;
        }

        body.velocity = velocity;
    }

    private void JumpAction()
    {
        int jumpTime = player._currentCharacterIndex == 2 ? 1 : 0;

        if (coyoteCounter > 0f || (jumpPhase < jumpTime && isJumping))
        {
            if (isJumping)
            {
                if (TutorialManager.Instance.GetCurrentPopUpIndex() == 10)
                    TutorialManager.Instance.penguinSkillPressed = true;
                jumpPhase++;
            }
            AudioManager.Instance.Play("PlayerJump");

            player.GetComponentInChildren<Animator>().SetBool("isJumping", true);
            jumpBufferCounter = 0;
            coyoteCounter = 0;
            float jumpSpeed = Mathf.Sqrt(-2f * Physics2D.gravity.y * jumpHeight);
            isJumping = true;

            if (velocity.y > 0f)
            {
                jumpSpeed = Mathf.Max(0f, jumpSpeed - velocity.y);
            }
            velocity.y += jumpSpeed;
        }
    }

    public void SetDefaultValueJumpButtons()
    {
        desiredJump = false;
        isJumpButtonHeldDown = false;
    }

    public void JumpButtonDown()
    {
        desiredJump = true;
        isJumpButtonHeldDown = true;
    }

    public void JumpButtonUp()
    {
        desiredJump = false;
        isJumpButtonHeldDown = false;
    }

    private bool IsTigerActive()
    {
        return player._currentCharacterIndex.Equals(1);
    }
}
