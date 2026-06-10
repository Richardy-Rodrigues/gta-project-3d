using TMPro;
using Unity.FPS.Game;
using UnityEngine;

namespace Unity.FPS.UI
{
    public class ScoreCounter : MonoBehaviour
    {
        [Header("Score")] [Tooltip("Text component for displaying the current score")]
        public TextMeshProUGUI ScoreText;

        ScoreManager m_ScoreManager;

        void Awake()
        {
            m_ScoreManager = FindAnyObjectByType<ScoreManager>();
            DebugUtility.HandleErrorIfNullFindObject<ScoreManager, ScoreCounter>(m_ScoreManager, this);
        }

        void Update()
        {
            ScoreText.text = "Score: " + m_ScoreManager.CurrentScore;
        }
    }
}
