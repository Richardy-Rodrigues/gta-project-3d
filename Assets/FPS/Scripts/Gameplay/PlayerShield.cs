using System.Collections;
using Unity.FPS.Game;
using UnityEngine;

namespace Unity.FPS.Gameplay
{
    // Grants the player temporary invincibility (a shield).
    // Lives on the player so the effect survives the pickup being destroyed.
    // Added automatically to the player by ShieldPickup when first collected.
    public class PlayerShield : MonoBehaviour
    {
        Health m_Health;
        Coroutine m_ShieldRoutine;

        void Awake()
        {
            m_Health = GetComponent<Health>();
            DebugUtility.HandleErrorIfNullGetComponent<Health, PlayerShield>(m_Health, this, gameObject);
        }

        public void ActivateShield(float duration)
        {
            // Restart the timer so overlapping pickups refresh the duration
            // instead of ending the shield early.
            if (m_ShieldRoutine != null)
                StopCoroutine(m_ShieldRoutine);

            m_ShieldRoutine = StartCoroutine(ShieldRoutine(duration));
        }

        IEnumerator ShieldRoutine(float duration)
        {
            m_Health.Invincible = true;
            yield return new WaitForSeconds(duration);
            m_Health.Invincible = false;
            m_ShieldRoutine = null;
        }
    }
}
