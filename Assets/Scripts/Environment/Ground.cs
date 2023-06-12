using UnityEngine;

namespace Scripts.Environment
{
    public class Ground : MonoBehaviour
    {
        [field: SerializeField] public GroundType GroundType { get; private set; }
    }
}
