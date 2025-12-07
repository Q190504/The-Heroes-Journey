using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheHeroesJourney
{
    public class Doonggeuni : NPC
    {
        private void Awake()
        {
            anim = GetComponent<Animator>();
            rb = GetComponent<Rigidbody2D>();
        }

        // Start is called before the first frame update
        private void Start()
        {
            initScale = transform.localScale;
            currentPoint = rightPoint.transform;
            anim.SetBool("isMoving", true);
        }

        // Update is called once per frame
        private void Update()
        {
            if (!isTalking)
            {
                FaceTarget(currentPoint);
                Patrol();
            }
            else
            {
                anim.SetBool("isMoving", false);
                rb.linearVelocity = new Vector2(0, 0);
                FaceTarget(InGameManager.Instance.player.transform);
            }
        }
    }
}
