using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundSelection : MonoBehaviour
{
    public int device;
    public List<Sprite> BackgroundImage;

    public static bool s_NotFirstOpen;

    private void Start()
    {
        if (device == DeviceSelector.DEVICE)
        {
            DontDestroyOnLoad(transform.gameObject);

            if (FindObjectsOfType(GetType()).Length > 1 && Time.time > 1)
            {
                Destroy(transform.gameObject);
            }

            if (!s_NotFirstOpen)
            {
                ChangeBackground();
                s_NotFirstOpen = true;
            }
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void ChangeBackground()
    {
        int randomNum = Random.Range(0, BackgroundImage.Count - 1);
        this.GetComponentsInChildren<SpriteRenderer>().Select(x => x.sprite = BackgroundImage[randomNum]).ToList();
    }
}
