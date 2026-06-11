using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Unity.FPS.UI
{
    // Wire StartGame() to the Start button's OnClick.
    // On press: stops the menu music, plays a confirm sound, waits a delay, then loads the game scene.
    public class StartGameButton : MonoBehaviour
    {
        [Tooltip("Name of the scene to load (must be in Build Settings)")]
        public string SceneName = "";

        [Tooltip("Seconds to wait after pressing before the game starts")]
        public float DelayBeforeStart = 1f;

        [Tooltip("The menu music AudioSource to stop on press (optional)")]
        public AudioSource MenuMusic;

        [Tooltip("Sound played when the button is pressed (optional)")]
        public AudioClip ClickSound;

        bool m_Started;

        public void StartGame()
        {
            // Prevent double clicks during the delay
            if (m_Started)
                return;
            m_Started = true;

            if (MenuMusic)
            {
                MenuMusic.Stop();

                // PlayOneShot plays the confirm sound once, independent of the music clip/loop
                if (ClickSound)
                    MenuMusic.PlayOneShot(ClickSound);
            }

            StartCoroutine(LoadAfterDelay());
        }

        IEnumerator LoadAfterDelay()
        {
            yield return new WaitForSeconds(DelayBeforeStart);
            SceneManager.LoadScene(SceneName);
        }
    }
}
