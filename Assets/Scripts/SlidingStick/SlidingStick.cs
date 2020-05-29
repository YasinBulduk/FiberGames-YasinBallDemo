using UnityEngine;
using UnityEditor;

public enum StickInput { MOBILE, MOUSE }

[RequireComponent(typeof(SlidingStickInputMobile), typeof(SlidingStickInputMouse))]
public class SlidingStick : MonoBehaviour
{
    public bool HasInput => input.HasInput;
    public Vector2 Direction => input.Direction;
    public Vector2 HandleCurrentPosition => input.CurrentPosition;
    public Vector2 CenterPosition => input.StartPosition;

    public bool Enabled { get; set; }

    [Tooltip("Pixels in screen space. Referance screen resolution : 1080x1920")]
    [SerializeField] private float centerRadius = 200f;
    [Tooltip("Pixels in screen space. Referance screen resolution : 1080x1920")]
    [SerializeField] private float handleRadius = 100f;
    [SerializeField] private float smoothTime = 0.05F;
    [SerializeField] private StickInput inputType = StickInput.MOBILE;

    private Resolution referanceResolution = new Resolution() { width = 1080, height = 1920 };
    private Texture centerTexture;
    private Texture handleTexture;
    private AbstractSlidingStickInput input;

    void Awake()
    {
        Resolution res;
#if UNITY_EDITOR
        Vector2 gameViewResolution = GetMainGameViewSize();
        res = new Resolution() { width = (int)gameViewResolution.x, height = (int) gameViewResolution.y };
#endif
#if UNITY_ANDROID || UNITY_IOS
        res = Screen.currentResolution;
#endif
        centerRadius = SetValueToCurrentScreenResolution(referanceResolution, res, centerRadius);
        handleRadius = SetValueToCurrentScreenResolution(referanceResolution, res, handleRadius);

        handleTexture = Resources.Load<Texture>("StickHandle");
        centerTexture = Resources.Load<Texture>("StickCenter");

#if UNITY_EDITOR
        if(!handleTexture || !centerTexture)
        {
            Debug.LogError($"[{gameObject.name}] SlidingStick cannot load textures from Resources folder. Requested texture names must be \"StickHandle\" & \"StickCenter\"");
        }
#endif

        //Init Input
        switch(inputType)
        {
            case StickInput.MOBILE:
                input = GetComponent<SlidingStickInputMobile>();
                GetComponent<SlidingStickInputMouse>().enabled = false;
                break;
            case StickInput.MOUSE:
                input = GetComponent<SlidingStickInputMouse>();
                GetComponent<SlidingStickInputMobile>().enabled = false;
                break;
        }
        input.enabled = true;

        input.InitInput(centerRadius, smoothTime);

        Enabled = true;
    }

    void Update()
    {
        if(Enabled)
        {
            input.UpdateInput();
        }
    }

    void OnGUI()
    {
        if (HasInput && Enabled)
        {
            Vector3 centerCirclePosition = new Vector3(CenterPosition.x, Screen.height - CenterPosition.y, 0f);
            float centerDiameter = centerRadius * 2;
            GUI.DrawTexture(new Rect(centerCirclePosition.x - centerDiameter / 2, centerCirclePosition.y - centerDiameter / 2, centerDiameter, centerDiameter),
                centerTexture);

            Vector3 handleCirclePosition = new Vector3(HandleCurrentPosition.x, Screen.height - HandleCurrentPosition.y, 0f);
            float handleDiameter = handleRadius * 2;
            GUI.DrawTexture(new Rect(handleCirclePosition.x - handleDiameter / 2, handleCirclePosition.y - handleDiameter / 2, handleDiameter, handleDiameter),
                handleTexture);
            
#if UNITY_EDITOR
            Vector3 forward = Camera.main.transform.InverseTransformDirection(Camera.main.transform.forward);
            Handles.color = Color.blue;
            Handles.DrawWireDisc(centerCirclePosition, forward, centerRadius);
            Handles.color = Color.green;
            Handles.DrawWireDisc(handleCirclePosition, forward, handleRadius);
#endif
        }
    }

    private float SetValueToCurrentScreenResolution(Resolution referanceResolution, Resolution targetResolution, float value)
    {
        if (targetResolution.width < targetResolution.height) //Portrait
        {
            return (value / referanceResolution.width) * targetResolution.width;
        }
        else //Landscape
        {
            return (value / referanceResolution.height) * targetResolution.width;
        }
    }

    public static Vector2 GetMainGameViewSize()
    {
        System.Type T = System.Type.GetType("UnityEditor.GameView,UnityEditor");
        System.Reflection.MethodInfo GetSizeOfMainGameView = T.GetMethod("GetSizeOfMainGameView", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
        System.Object Res = GetSizeOfMainGameView.Invoke(null, null);
        return (Vector2) Res;
    }
}