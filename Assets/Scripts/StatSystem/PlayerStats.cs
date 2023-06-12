using UnityEngine;

namespace Scripts.StatSystem
{
    public class PlayerStats : CharacterStats
    {
        [Header("Movement")]
        public float MovementSpeed = 8f;
        public float RotationSpeed = 15f;

        [Header("Gravity")]
        public float GroundedGravity = -0.1f;
        public float OnAirGravity = -9.81f;

        [Header("Jump")]
        public float OnAirMovementSpeedModifier = 0.5f;
        public float JumpPower = 30f;

        [Header("Dodge")]
        public float DodgeDuration = 1f;
        public AnimationCurve DodgeCurve;

        [Header("Landing")]
        public float LandingDuration = 1.5f;
        public float LandingDistance = 10f;

        [Header("Damage Taken")]
        public float DamageTakenDuration = 0.5f;
    }
}
