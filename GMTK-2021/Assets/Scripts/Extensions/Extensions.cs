using UnityEngine;

namespace Extensions
{
    public static class Extensions
    {
        public static void AddPosition(this Rigidbody2D rigidbody, Vector2 addedPosition)
        {
            rigidbody.MovePosition(
                new Vector2(
                rigidbody.transform.position.x + addedPosition.x,
                rigidbody.transform.position.y + addedPosition.y));
        }
    }
}