using UnityEngine;

namespace Unity.FPS.Gameplay
{
    public class SpeedBoostPickup : Pickup
    {
        [Header("Parameters")]
        [Tooltip("How much the player's ground speed is multiplied during the boost")]
        public float SpeedMultiplier = 1.5f;

        [Tooltip("How long the speed boost lasts, in seconds")]
        public float Duration = 5f;

        protected override void OnPicked(PlayerCharacterController player)
        {
            var speedBoost = player.GetComponent<PlayerSpeedBoost>();
            if (!speedBoost)
                speedBoost = player.gameObject.AddComponent<PlayerSpeedBoost>();

            speedBoost.ApplyBoost(SpeedMultiplier, Duration);
            PlayPickupFeedback();
            Destroy(gameObject);
        }
    }
}
