using UnityEngine;

public class PlayerStats : CharacterStats
{
    public float MovementSpeed = 8f;
    public float RotationSpeed = 15f;
    public float GroundedGravity = -0.1f;
    public float OnAirGravity = -9.81f;
    public float OnAirMovementSpeedModifier = 0.5f;
    public float JumpPower = 30f;
    public float DodgeDuration = 1f;
    public AnimationCurve DodgeCurve;

    public float LandingDuration = 1.5f;
    public float LandingDistance = 10f;
}
