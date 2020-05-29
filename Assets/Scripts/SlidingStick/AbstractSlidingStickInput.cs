using UnityEngine;

public abstract class AbstractSlidingStickInput : MonoBehaviour
{
    public Vector2 Direction { get; protected set; }
    public Vector2 StartPosition { get; protected set; }
    public Vector2 CurrentPosition { get; protected set; }
    public bool HasInput { get; protected set; }

    public abstract void InitInput(float centerRadius, float smoothTime);
    public abstract void UpdateInput();
}
