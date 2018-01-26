using UnityEngine;

public class DeviceSelector : MonoBehaviour
{

    public static int DEVICE;
    public GameObject iPad, iPhone;

    void Awake()
    {
        if (SystemInfo.deviceModel.Contains("iPad"))
        {
            //DEVICE = 1;
            //Destroy(iPhone);
        }
        else
        {
            //DEVICE = 0;
            //Destroy(iPad);

            DEVICE = 1;
            Destroy(iPhone);
        }
    }
}
