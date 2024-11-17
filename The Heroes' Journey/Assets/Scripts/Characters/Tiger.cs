using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheHeroesJourney
{
    public class Tiger : BaseMainCharacter
    {
        public TigerDashConfig tigerDashConfig;

        [SerializeField] private TrailRenderer trailRenderer;

        private Animator anim;

        private void Awake()
        {
            player = GetComponentInParent<Player>();
            anim = GetComponent<Animator>();
            skillCooldownConfig.cooldownTimer = 0;
        }

        public override void ActiveSkill()
        {
            if (skillCooldownConfig.cooldownTimer <= 0)
            {
                if (TutorialManager.Instance.GetCurrentPopUpIndex() == 12)
                    TutorialManager.Instance.tigerSkillPressed = true;

                UIManager.Instance.ToggleSwitchCharaterButtons(false);
                AudioManager.Instance.Play("TigerDash");

                float dashDirection = _host.move.direction.x;
                StartCoroutine(Dash(dashDirection));
                base.ActiveSkill();
            }
        }

        private IEnumerator Dash(float direction)
        {
            skillCooldownConfig.cooldownTimer = skillCooldownConfig.skillCooldown;

            _host.isDashing = true;
            float defaultGravityScale = _host.body.gravityScale;
            _host.body.gravityScale = 0;
            anim.SetBool("isDashing", true);
            trailRenderer.emitting = true;

            for (float dashTimer = tigerDashConfig.dashingTime; dashTimer > 0; dashTimer -= Time.deltaTime)
            {
                _host.body.velocity = new Vector3(direction * tigerDashConfig.dashingPower, 0, 0);
                yield return null;
            }

            _host.body.velocity = Vector3.zero;
            trailRenderer.emitting = false;
            _host.body.gravityScale = defaultGravityScale;
            _host.isDashing = false;
            anim.SetBool("isDashing", false);
            UIManager.Instance.ToggleSwitchCharaterButtons(true);
        }
    }
}
