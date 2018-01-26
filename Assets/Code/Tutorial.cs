using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    private static Tutorial _instance;

    public static Tutorial Instance { get { return _instance; } }

    public Image[] TutRings;

    public int CurrentTut = -1, Device;

    private void Awake()
    {
        if (Device == DeviceSelector.DEVICE)
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this;
            }
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void Start()
    {
        if (PlayerStats.s_TutorialOn != 1)
        {
            Destroy(this.gameObject);
        }

        StartCoroutine(ShowTutorial(0));
    }

    public IEnumerator ShowTutorial(int tutNum)
    {
        if (CurrentTut + 1 == tutNum)
        {
            if (tutNum > 0)
            {
                TutRings[tutNum - 1].enabled = false;
            }
            else if (tutNum == 0)
            {
                yield return new WaitForSeconds(0.7f);
            }

            if (tutNum < TutRings.Length)
            {
                StartCoroutine(FadeIn(TutRings[tutNum]));
            }
            CurrentTut = tutNum;
        }

        if (CurrentTut == TutRings.Length)
        {
            PlayerStats.Instance.TutorialFinished();
        }
    }

    IEnumerator FadeIn(Image circle)
    {
        circle.enabled = true;
        while (circle.color.a != 1)
        {
            circle.color = new Color(circle.color.r, circle.color.g, circle.color.b, circle.color.a + 0.07f);
            yield return new WaitForEndOfFrame();
        }
    }
}
