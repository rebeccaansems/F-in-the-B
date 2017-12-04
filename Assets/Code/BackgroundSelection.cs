using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundSelection : MonoBehaviour
{
    public List<Sprite> BackgroundImage;

    void Start()
    {
        int randomNum = Random.Range(0, BackgroundImage.Count - 1);
        this.GetComponentsInChildren<SpriteRenderer>().Select(x => x.sprite = BackgroundImage[randomNum]).ToList();
    }
}
