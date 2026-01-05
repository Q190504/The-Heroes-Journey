using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace TheHeroesJourney
{
    public class Player : BaseCharacter, IDataPresistence
    {
        [Header("Character Settings")]
        //Nhân vật index đang được bật
        public int _currentCharacterIndex;
        private BaseMainCharacter currentCharacter => myCharacters[_currentCharacterIndex];
        //Tổng các nhân vật nó có
        public List<BaseMainCharacter> myCharacters;
        public UnityAction<int> _onChangeCharacter;

        public BaseCharacterConfig playerConfig;

        [Header("Control Settings")]
        public bool inputByKeyboard = true;

        public float respawnTime = 1f;

        [HideInInspector]
        public bool desiredAttack = false;
        public MeleeAttackConfig basicAttackConfig;
        protected float attackCooldownTimer;
        private bool isAttackDuringCooldown = false;
        public Transform attackPos;
        public LayerMask enemyLayer;

        public UnityAction<float> _onCooldownSkill;
        private bool isCooldown = false;

        public UnityAction<float, float> _onChangeHealth;
        public UnityAction _playerDie;

        [HideInInspector]
        public bool isDashing;
        public UnityAction<bool> _onDogHealing;
        public float dogImmobilizationTime;
        public bool isDogHealing;
        private GameObject healEffect;

        public Rigidbody2D body;
        public Move move;

        private bool isRespawning = false;

        private void Awake()
        {
            body = GetComponent<Rigidbody2D>();
            move = GetComponent<Move>();
        }

        private void OnEnable()
        {
            InGameManager.Instance._onSaveGame += PlusHealth;
        }

        private void OnDisable()
        {
            InGameManager.Instance._onSaveGame -= PlusHealth;

        }

        // Start is called before the first frame update
        void Start()
        {
            //Em có thể nhận vào list prefab skill và instatiate, add vào mySkills cho tính năng user có thể chọn các tướng mang vào game
            //Chuẩn bị gắn host
            for (int i = 0; i < myCharacters.Count; i++)
            {
                myCharacters[i].SetPlayerHost(this);
            }

            for (int i = 0; i < myCharacters.Count; i++)
            {
                myCharacters[i].gameObject.SetActive(i == _currentCharacterIndex);
                myCharacters[i].skillCooldownConfig.cooldownTimer = 0;
            }
            _onChangeCharacter?.Invoke(_currentCharacterIndex);
            attackCooldownTimer = 0;


            currentHealth = playerConfig.maxHealth;
            _onChangeHealth?.Invoke(currentHealth, playerConfig.maxHealth);
        }

        // Update is called once per frame
        void Update()
        {
            if(inputByKeyboard)
            {
                //BAN PHIM
                if (Input.GetKeyDown(KeyCode.X))
                    ClickActiveSkill();
                if (Input.GetKeyDown(KeyCode.C))
                    ClickBasicAttack();
            }

            StartCoroutine(Attack());
            StartBasicAttackCooldown();

            StartSkillCooldown();

            if (healEffect != null)
                healEffect.transform.position = transform.position;
        }

        public void ClickBasicAttack()
        {
            desiredAttack = true;
        }

        IEnumerator Attack()
        {
            if (desiredAttack)
            {
                if (attackCooldownTimer <= 0)
                {
                    currentCharacter.GetComponent<Animator>().SetTrigger("meleeAttack");
                    AudioManager.Instance.Play("PlayerBasicAttack");

                    GameObject swordSlashEffect = EffectManager.Instance.Take(EffectType.SwordSlashEffect);
                    if (move.isFacingRight)
                    {
                        swordSlashEffect.transform.position = attackPos.position;
                        swordSlashEffect.transform.rotation = Quaternion.Euler(0, 0, 0);
                    }
                    else
                    {
                        swordSlashEffect.transform.position = attackPos.position;
                        swordSlashEffect.transform.rotation = Quaternion.Euler(0, 180, 0);
                    }

                    desiredAttack = false;
                    Collider2D[] enemiesToHit = Physics2D.OverlapCircleAll(attackPos.position, basicAttackConfig.attackRadius, enemyLayer);

                    for (int i = 0; i < enemiesToHit.Length; i++)
                        if (enemiesToHit[i].GetComponent<BaseEnemy>() != null)
                            enemiesToHit[i].GetComponent<BaseEnemy>().TakeDamage(basicAttackConfig.damage);

                    attackCooldownTimer = basicAttackConfig.timeBtwAttack;
                    StartBasicAttackCooldown();

                    yield return new WaitForSeconds(0.01f);
                    swordSlashEffect.transform.position = attackPos.position;
                    swordSlashEffect.transform.rotation = Quaternion.Euler(0, 0, 0);
                    EffectManager.Instance.Return(swordSlashEffect);
                }
            }
        }

        void StartBasicAttackCooldown()
        {
            if (!isAttackDuringCooldown && attackCooldownTimer > 0)
            {
                isAttackDuringCooldown = true;
                UIManager.Instance.basicAttackCooldownFillImage.gameObject.SetActive(true);
                StartCoroutine(BasicAttackCooldownCoroutine());
            }
        }

        IEnumerator BasicAttackCooldownCoroutine()
        {
            while (attackCooldownTimer > 0)
            {
                attackCooldownTimer -= Time.deltaTime;
                float fillAmount = attackCooldownTimer / basicAttackConfig.timeBtwAttack;
                UIManager.Instance.UpdateCooldownBasicAttackButton(fillAmount);

                yield return null;
            }
            isAttackDuringCooldown = false;
            UIManager.Instance.UpdateCooldownBasicAttackButton(0);
            UIManager.Instance.basicAttackCooldownFillImage.gameObject.SetActive(false);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(attackPos.position, basicAttackConfig.attackRadius);
        }

        public void ClickActiveSkill()
        {
            this.currentCharacter.ActiveSkill();

            //Khong phai Squirrel && penguin
            if (this._currentCharacterIndex != 0 && this._currentCharacterIndex != 2)
                StartSkillCooldown();
        }

        void StartSkillCooldown()
        {
            if (!isCooldown && currentCharacter.skillCooldownConfig.cooldownTimer > 0)
            {
                isCooldown = true;
                UIManager.Instance.skillCooldownFillImage.gameObject.SetActive(true);
                StartCoroutine(CooldownCoroutine());
            }
        }

        IEnumerator CooldownCoroutine()
        {
            while (currentCharacter.skillCooldownConfig.cooldownTimer > 0)
            {
                currentCharacter.skillCooldownConfig.cooldownTimer -= Time.deltaTime;
                float fillAmount = currentCharacter.skillCooldownConfig.cooldownTimer / currentCharacter.skillCooldownConfig.skillCooldown;
                _onCooldownSkill?.Invoke(fillAmount);

                yield return null;
            }

            isCooldown = false;
            _onCooldownSkill?.Invoke(0);
            UIManager.Instance.skillCooldownFillImage.gameObject.SetActive(false);
        }

        /// <summary>
        /// Click Button swap skill và UI
        /// </summary>
        public void ClickSwapToSquirrel()
        {
            this._currentCharacterIndex = 0;
            for (int i = 0; i < myCharacters.Count; i++)
            {
                //bật tắt skill và UI phùy hợp với index đang chọn
                myCharacters[i].gameObject.SetActive(i == _currentCharacterIndex);
            }
            UIManager.Instance.ShowSkillButton();
            UIManager.Instance.TurnOffSkillButton();
            _onChangeCharacter?.Invoke(_currentCharacterIndex);
        }
        public void ClickSwapToTiger()
        {
            this._currentCharacterIndex = 1;
            for (int i = 0; i < myCharacters.Count; i++)
            {
                //bật tắt skill và UI phùy hợp với index đang chọn
                myCharacters[i].gameObject.SetActive(i == _currentCharacterIndex);
            }
            UIManager.Instance.ShowSkillButton();
            UIManager.Instance.TurnOnSkillButton();
            _onChangeCharacter?.Invoke(_currentCharacterIndex);
        }
        public void ClickSwapToPenguin()
        {
            this._currentCharacterIndex = 2;
            for (int i = 0; i < myCharacters.Count; i++)
            {
                //bật tắt skill và UI phùy hợp với index đang chọn
                myCharacters[i].gameObject.SetActive(i == _currentCharacterIndex);
            }
            UIManager.Instance.HideSkillButton();
            UIManager.Instance.TurnOnSkillButton();
            _onChangeCharacter?.Invoke(_currentCharacterIndex);
        }
        public void ClickSwapToCat()
        {
            this._currentCharacterIndex = 3;
            for (int i = 0; i < myCharacters.Count; i++)
            {
                //bật tắt skill và UI phùy hợp với index đang chọn
                myCharacters[i].gameObject.SetActive(i == _currentCharacterIndex);
            }
            UIManager.Instance.ShowSkillButton();
            UIManager.Instance.TurnOnSkillButton();
            _onChangeCharacter?.Invoke(_currentCharacterIndex);

        }
        public void ClickSwapToDog()
        {
            this._currentCharacterIndex = 4;
            for (int i = 0; i < myCharacters.Count; i++)
            {
                //bật tắt skill và UI phù hợp với index đang chọn
                myCharacters[i].gameObject.SetActive(i == _currentCharacterIndex);
            }
            UIManager.Instance.ShowSkillButton();
            UIManager.Instance.TurnOnSkillButton();
            _onChangeCharacter?.Invoke(_currentCharacterIndex);
        }

        public override void TakeDamage(float damageAmount)
        {
            if (InGameManager.Instance.isGameFinished || damageAmount <= 0 || currentHealth <= 0)
                return;

            AudioManager.Instance.Play("PlayerHurt");

            base.TakeDamage(damageAmount);
            gameObject.GetComponentInChildren<Animator>().SetTrigger("hurt");
            _onChangeHealth?.Invoke(currentHealth, playerConfig.maxHealth);
        }

        public void PlusHealth(float plusHealth)
        {
            currentHealth += plusHealth;
            if (currentHealth >= playerConfig.maxHealth)
                currentHealth = playerConfig.maxHealth;
            _onChangeHealth?.Invoke(currentHealth, playerConfig.maxHealth);
        }

        public void HealByDog(float plusHealth)
        {
            _onDogHealing?.Invoke(false);
            StartCoroutine(Heal(plusHealth));
        }
        public void HealByPolarBear(float plusHealth)
        {
            StartCoroutine(Heal(plusHealth));
        }

        IEnumerator Heal(float plusHealth)
        {
            healEffect = EffectManager.Instance.Take(EffectType.HealEffect);
            healEffect.transform.position = transform.position;
            PlusHealth(plusHealth);
            yield return new WaitForSeconds(dogImmobilizationTime);
            EffectManager.Instance.Return(healEffect);
            InGameManager.Instance.player.isDogHealing = false;
            _onDogHealing?.Invoke(true);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Trap") && !isRespawning)
            {
                FirebaseManager.LogEvent("Player_DieTrap");

                Die();
            }
        }

        public override void Die()
        {
            FirebaseManager.LogEvent("Player_Die");

            isRespawning = true;
            UIManager.Instance.SetActiveCultistPriestlEntranceWall(false);
            AudioManager.Instance.Play("PlayerDie");


            gameObject.GetComponentInChildren<Animator>().SetTrigger("die");
            TransitionManager.Instance.StartCrossfadeTrasition();
            CameraManager.Instance.SetCamera();
            StartCoroutine(Respawn(respawnTime));
        }

        IEnumerator Respawn(float duration)
        {
            body.linearVelocity = Vector3.zero;
            yield return new WaitForSeconds(duration);
            transform.position = InGameManager.Instance.respawnPos;
            PlusHealth(playerConfig.maxHealth);
            isRespawning = false;

            if (InGameManager.Instance.isBattleCultisPriest)
            {
                LightManager.Instance.TurnOnPlayerLight();
                InGameManager.Instance.isBattleCultisPriest = false;
                AudioManager.Instance.Stop("BattleCultisPriestTheme");
                AudioManager.Instance.Play("MainGameTheme");
            }


            if (InGameManager.Instance.isBattleJumpKing)
                StartCoroutine(StartCutscene());
        }

        IEnumerator StartCutscene()
        {
            yield return new WaitForSeconds(0.5f);
            UIManager.Instance.jumpKing.SetDefaultStats();
            InGameManager.Instance.enterjumpKingArena.Play();
        }

        public void LoadData(GameData data)
        {
            _currentCharacterIndex = data.currentCharacterIndex;
        }

        public void SaveData(GameData data)
        {
            data.currentCharacterIndex = _currentCharacterIndex;
        }
    }
}

