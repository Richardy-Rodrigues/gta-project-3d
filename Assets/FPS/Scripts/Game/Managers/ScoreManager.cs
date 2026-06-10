using UnityEngine;

namespace Unity.FPS.Game
{
    // Tracks the player's score by listening to enemy kill events.
    // Add this component to a manager GameObject in the gameplay scene.
    public class ScoreManager : MonoBehaviour
    {
        [Tooltip("Points awarded for each enemy killed")]
        public int PointsPerEnemy = 100;

        public int CurrentScore { get; private set; }

        // Kept across scene loads so the WinScene can display the final score.
        public static int LastRunScore;

        void OnEnable()
        {
            EventManager.AddListener<EnemyKillEvent>(OnEnemyKill);
        }

        void OnDisable()
        {
            EventManager.RemoveListener<EnemyKillEvent>(OnEnemyKill);
        }

        void OnEnemyKill(EnemyKillEvent evt)
        {
            CurrentScore += PointsPerEnemy;
            LastRunScore = CurrentScore;
        }
    }
}
