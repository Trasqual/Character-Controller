using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class GravityHandler : MonoBehaviour
{
    [SerializeField] private Vector3 _groundedGravity = new Vector3(0f, -0.1f, 0f);
    [SerializeField] private Vector3 _notGroundedGravity = new Vector3(0f, -10f, 0f);

    public bool IsGrounded { get; private set; }

    public Vector3 Gravity
    {
        get
        {
            return IsGrounded ? _groundedGravity : _notGroundedGravity;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Ground ground))
        {
            IsGrounded = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Ground ground))
        {
            IsGrounded = false;
        }
    }
}
