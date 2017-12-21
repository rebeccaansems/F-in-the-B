using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundSelection : MonoBehaviour
{
    public List<Sprite> BackgroundImage;
    private static bool s_FirstOpen;

    void Start()
    {
        Object.DontDestroyOnLoad(this);
        s_FirstOpen = false;
        int randomNum = Random.Range(0, BackgroundImage.Count - 1);
        this.GetComponentsInChildren<SpriteRenderer>().Select(x => x.sprite = BackgroundImage[randomNum]).ToList();
    }
}
