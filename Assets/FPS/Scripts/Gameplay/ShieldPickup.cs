using UnityEngine;

namespace Unity.FPS.Gameplay
{
    public class ShieldPickup : Pickup
    {
        [Header("Parameters")]
        [Tooltip("How long the invincibility shield lasts, in seconds")]
        public float Duration = 5f;

        protected override void OnPicked(PlayerCharacterController player)
        {
            var shield = player.GetComponent<PlayerShield>();
            if (!shield)
                shield = player.gameObject.AddComponent<PlayerShield>();

            shield.ActivateShield(Duration);
            PlayPickupFeedback();
            Destroy(gameObject);
        }
    }
}
