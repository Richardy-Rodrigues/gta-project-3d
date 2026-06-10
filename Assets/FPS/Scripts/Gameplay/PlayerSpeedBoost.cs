using System.Collections;
using Unity.FPS.Game;
using UnityEngine;

namespace Unity.FPS.Gameplay
{
    // Applies a temporary ground-speed boost to the player.
    // Lives on the player (like Jetpack) so the boost survives the pickup being destroyed.
    [RequireComponent(typeof(PlayerCharacterController))]
    public class PlayerSpeedBoost : MonoBehaviour
    {
        PlayerCharacterController m_Controller;
        float m_BaseSpeed;
        Coroutine m_BoostRoutine;

        void Awake()
        {
            m_Controller = GetComponent<PlayerCharacterController>();
            DebugUtility.HandleErrorIfNullGetComponent<PlayerCharacterController, PlayerSpeedBoost>(
                m_Controller, this, gameObject);

            m_BaseSpeed = m_Controller.MaxSpeedOnGround;
        }

        public void ApplyBoost(float multiplier, float duration)
        {
            // Restart from the base speed so overlapping boosts refresh the timer
            // instead of stacking the multiplier.
            if (m_BoostRoutine != null)
                StopCoroutine(m_BoostRoutine);

            m_BoostRoutine = StartCoroutine(BoostRoutine(multiplier, duration));
        }

        IEnumerator BoostRoutine(float multiplier, float duration)
        {
            m_Controller.MaxSpeedOnGround = m_BaseSpeed * multiplier;
            yield return new WaitForSeconds(duration);
            m_Controller.MaxSpeedOnGround = m_BaseSpeed;
            m_BoostRoutine = null;
        }
    }
}
