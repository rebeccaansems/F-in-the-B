using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundSelection : MonoBehaviour
{
    public List<Sprite> BackgroundImage;

    public static bool s_NotFirstOpen;

    private int count = 0;

    private void Start()
    {
        if (GameObject.FindGameObjectsWithTag("Background Tablet").Length > 1
            || GameObject.FindGameObjectsWithTag("Background Phone").Length > 1)
        {
            Destroy(transform.gameObject);
        }

        if (!s_NotFirstOpen)
        {
            ChangeBackground();
            s_NotFirstOpen = true;
        }
        count++;
    }

    public void DontDestroy()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    public void ChangeBackground()
    {
        int randomNum = Random.Range(0, BackgroundImage.Count - 1);
        this.GetComponentsInChildren<SpriteRenderer>().Select(x => x.sprite = BackgroundImage[randomNum]).ToList();
    }
}
