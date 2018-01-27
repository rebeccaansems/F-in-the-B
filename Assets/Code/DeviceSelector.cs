using UnityEngine;

public class DeviceSelector : MonoBehaviour
{
    public static int DEVICE;
    public GameObject TabletBackground, TabletGame, PhoneBackground, PhoneGame;

    void Awake()
    {
#if UNITY_IOS && !UNITY_EDITOR
        if (UnityEngine.iOS.Device.generation.ToString().Contains("iPad"))
        {
            DEVICE = 1;
            Destroy(PhoneBackground);
            Destroy(PhoneGame);
            TabletBackground.GetComponent<BackgroundSelection>().DontDestroy();
        }
        else
        {
            DEVICE = 0;
            Destroy(TabletBackground);
            Destroy(TabletGame);
            PhoneBackground.GetComponent<BackgroundSelection>().DontDestroy();
        }
#else
        DEVICE = 1;
        Destroy(PhoneBackground);
        Destroy(PhoneGame);
        TabletBackground.GetComponent<BackgroundSelection>().DontDestroy();
#endif
    }
}
