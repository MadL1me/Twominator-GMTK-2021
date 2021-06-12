using UnityEngine;
using UnityEngine.UIElements;

namespace LogicalElements
{
    public class PositionStateElement : MonoBehaviour
    {
        public void SetState(PositionState state)
        {
            transform.localPosition = state.StartPosition;
            if (TryGetComponent<Rigidbody2D>(out var rigidbody))
                rigidbody.velocity = Vector2.zero;
        }

        public PositionState GetState()
        {
            return new PositionState() { StartPosition = transform.localPosition };
        }
    }

    public struct PositionState
    {
        public Vector2 StartPosition;
    }
}