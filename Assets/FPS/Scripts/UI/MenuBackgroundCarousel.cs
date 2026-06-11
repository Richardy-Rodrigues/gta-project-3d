using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Unity.FPS.UI
{
    // Cycles through a list of background textures on a UI RawImage, with an optional fade.
    // Put this on the menu's BackgroundImage (which uses a RawImage component).
    [RequireComponent(typeof(RawImage))]
    public class MenuBackgroundCarousel : MonoBehaviour
    {
        [Tooltip("Textures to alternate as the background, in order")]
        public Texture[] Images;

        [Tooltip("How long each image stays on screen, in seconds")]
        public float DisplayTime = 4f;

        [Tooltip("Duration of the fade between images, in seconds (0 = instant swap)")]
        public float FadeDuration = 0.5f;

        RawImage m_Image;
        int m_Index;

        void Start()
        {
            m_Image = GetComponent<RawImage>();

            if (Images == null || Images.Length == 0)
                return;

            // Start showing the first image
            m_Image.texture = Images[0];

            // Only cycle if there is more than one image
            if (Images.Length > 1)
                StartCoroutine(CarouselRoutine());
        }

        IEnumerator CarouselRoutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(DisplayTime);

                m_Index = (m_Index + 1) % Images.Length;

                if (FadeDuration > 0f)
                {
                    yield return Fade(1f, 0f);   // fade out current image
                    m_Image.texture = Images[m_Index];
                    yield return Fade(0f, 1f);   // fade in next image
                }
                else
                {
                    m_Image.texture = Images[m_Index];
                }
            }
        }

        IEnumerator Fade(float from, float to)
        {
            float elapsed = 0f;
            Color color = m_Image.color;

            while (elapsed < FadeDuration)
            {
                elapsed += Time.deltaTime;
                color.a = Mathf.Lerp(from, to, elapsed / FadeDuration);
                m_Image.color = color;
                yield return null;
            }

            color.a = to;
            m_Image.color = color;
        }
    }
}
