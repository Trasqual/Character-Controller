using UnityEngine;

public class PlayerStats : CharacterStats
{
    public float MovementSpeed = 8f;
    public float RotationSpeed = 15f;
    public float DodgeCooldown = 2f;
    public float DodgeSpeed = 15f;
    public float DodgeDuration = 1f;
    public AnimationCurve DodgeCurve;
}
