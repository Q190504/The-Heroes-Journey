using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheHeroesJourney
{
    public class FakeWall : MonoBehaviour
    {
        private Renderer wallRenderer;

        // Start is called before the first frame update
        void Start()
        {
            wallRenderer = GetComponent<Renderer>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision != null && collision.gameObject.CompareTag("Player"))
            {
                StartCoroutine(FadeWall(1.0f, 0f, 0.5f)); // Thời gian ẩn: 0.5 giây
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision != null && collision.gameObject.CompareTag("Player"))
            {
                StartCoroutine(FadeWall(0f, 1.0f, 0.5f)); // Thời gian hiện: 0.5 giây
            }
        }

        private IEnumerator FadeWall(float startAlpha, float targetAlpha, float duration)
        {
            float currentTime = 0.0f;
            Color startColor = wallRenderer.material.color;

            while (currentTime < duration)
            {
                currentTime += Time.deltaTime;

                // Lerp giữa startAlpha và targetAlpha theo thời gian
                float alpha = Mathf.Lerp(startAlpha, targetAlpha, currentTime / duration);

                // Đặt alpha vào màu của vật thể
                Color newColor = new Color(startColor.r, startColor.g, startColor.b, alpha);
                wallRenderer.material.color = newColor;

                yield return null;
            }

            // Đảm bảo đặt màu cuối cùng
            wallRenderer.material.color = new Color(startColor.r, startColor.g, startColor.b, targetAlpha);
        }
    }
}
