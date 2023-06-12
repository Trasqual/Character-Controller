using UnityEngine;

namespace Scripts.MovementSystem
{
    public abstract class MovementBase : MonoBehaviour
    {
        protected bool _canMove;

        public void EnableMovement()
        {
            _canMove = true;
        }

        public void DisableMovement()
        {
            _canMove = false;
        }

        public abstract void Move();
    }
}
