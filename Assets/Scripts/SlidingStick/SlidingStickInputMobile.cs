using UnityEngine;
using UnityEngine.EventSystems;

public class SlidingStickInputMobile : AbstractSlidingStickInput
{
    private float m_CenterRadius;
    private float m_SmoothTime;
    private Vector2 m_Axis;
    private bool m_Initialized;

    public override void InitInput(float centerRadius, float smoothTime)
    {
        m_CenterRadius = centerRadius;
        m_SmoothTime = smoothTime;
        m_Initialized = true;
    }

    public override void UpdateInput()
    {
        if (!m_Initialized)
        {
#if UNITY_EDITOR
            Debug.LogError($"[{typeof(SlidingStickInputMobile).Name}] is not Initialized. InitInput() must be called before calling UpdateInput()");
#endif
            return;
        }

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            bool isPointerOverGameObject = false;
            if (EventSystem.current)
            {
                isPointerOverGameObject = EventSystem.current.IsPointerOverGameObject(touch.fingerId);
            }

            if (touch.phase == TouchPhase.Began && !isPointerOverGameObject)
            {
                HasInput = true;
                StartPosition = touch.position;
                CurrentPosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {
                if (HasInput)
                {
                    CurrentPosition = touch.position;

                    if (Mathf.Abs(Vector2.Distance(StartPosition, CurrentPosition)) > m_CenterRadius)
                    {
                        Vector3 curVel = Vector3.zero;
                        StartPosition = Vector3.SmoothDamp(StartPosition, CurrentPosition + (StartPosition - CurrentPosition).normalized * m_CenterRadius, ref curVel, m_SmoothTime);
                    }

                    m_Axis.x = (CurrentPosition - StartPosition).x;
                    m_Axis.y = (CurrentPosition - StartPosition).y;
                }
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                HasInput = false;
                m_Axis = Vector3.zero;
            }

            Direction = m_Axis.normalized;
        }
    }
}
