using UnityEngine;

public class DeviceSelector : MonoBehaviour
{
    public static int DEVICE;
    public GameObject TabletBackground, TabletGame, PhoneBackground, PhoneGame;

    void Awake()
    {
        if (Camera.main.aspect > 0.6f)
        {
            DEVICE = 1;
            if (PhoneBackground != null)
            {
                Destroy(PhoneBackground);
                Destroy(PhoneGame);
            }
            TabletBackground.GetComponent<BackgroundSelection>().DontDestroy();
        }
        else
        {
            DEVICE = 0;
            if (TabletBackground != null)
            {
                Destroy(TabletBackground);
                Destroy(TabletGame);
            }
            PhoneBackground.GetComponent<BackgroundSelection>().DontDestroy();
        }
    }
}
