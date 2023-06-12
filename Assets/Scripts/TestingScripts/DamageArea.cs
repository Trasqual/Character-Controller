using Scripts.HealthSystem;
using UnityEngine;

public class DamageArea : MonoBehaviour
{
    [SerializeField] private float _damageFrequency = 1f;
    private float _damageTimer;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IDamagable damagable))
        {
            _damageTimer = 0f;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out IDamagable damagable))
        {
            _damageTimer += Time.deltaTime;
            if (_damageTimer >= _damageFrequency)
            {
                damagable.TakeDamage(0f);
                _damageTimer = 0f;
            }
        }
    }
}
