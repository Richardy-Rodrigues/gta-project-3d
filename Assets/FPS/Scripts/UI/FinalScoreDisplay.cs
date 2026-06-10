using TMPro;
using Unity.FPS.Game;
using UnityEngine;

namespace Unity.FPS.UI
{
    // Displays the score from the last run. Use this on the WinScene.
    public class FinalScoreDisplay : MonoBehaviour
    {
        [Header("Score")] [Tooltip("Text component for displaying the final score")]
        public TextMeshProUGUI ScoreText;

        void Start()
        {
            ScoreText.text = "Pontuação: " + ScoreManager.LastRunScore;
        }
    }
}
