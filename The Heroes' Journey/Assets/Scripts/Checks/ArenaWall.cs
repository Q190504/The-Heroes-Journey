using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheHeroesJourney
{
    public class ArenaWall : MonoBehaviour
    {
        public string wallSide;
        public JumpKing jumpKing;

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision != null && collision.CompareTag("Player") && jumpKing != null)
            {
                if (wallSide == "Left")
                    jumpKing.canJumpLeft = false;
                else if (wallSide == "Right")
                    jumpKing.canJumpRight = false;
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision != null && collision.CompareTag("Player") && jumpKing != null)
            {
                if (wallSide == "Left")
                    jumpKing.canJumpLeft = true;
                else if (wallSide == "Right")
                    jumpKing.canJumpRight = true;
            }
        }
    }
}
