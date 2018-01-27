using UnityEngine;

public class DeviceSelector : MonoBehaviour
{
    public static int DEVICE;
    public GameObject TabletBackground, TabletGame, PhoneBackground, PhoneGame;

    void Awake()    
    {
        Debug.Log(Camera.main.aspect);
        if (Camera.main.aspect > 0.6f)
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
    }
}
