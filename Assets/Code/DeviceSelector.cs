using UnityEngine;

public class DeviceSelector : MonoBehaviour
{
    public static int DEVICE;
    public GameObject iPad, iPhone;

    void Awake()
    {
#if UNITY_IOS && !UNITY_EDITOR
        if (UnityEngine.iOS.Device.generation.ToString().Contains("iPad"))
        {
            DEVICE = 1;
        }
        else
        {   
            DEVICE = 0;
        }
#else
        DEVICE = 0;
#endif
    }
}
