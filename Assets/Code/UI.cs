using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    public void OpenPopup(CanvasGroup go)
    {
        if (go.gameObject.GetComponent<AnimationController>() != null && go.gameObject.GetComponent<AnimationController>().AnimateOnVisible)
        {
            go.gameObject.GetComponent<Animator>().SetBool("AnimateIn", true);
        }

        go.alpha = 1;
        go.interactable = true;
        go.blocksRaycasts = true;
    }

    public void ClosePopup(CanvasGroup go)
    {
        if (go.gameObject.GetComponent<AnimationController>() != null && go.gameObject.GetComponent<AnimationController>().AnimateOnVisible)
        {
            go.gameObject.GetComponent<Animator>().SetBool("AnimateIn", false);
            StartCoroutine(ClosePopupWaitForAnimation(go.gameObject.GetComponent<Animator>(), new CanvasGroup[] { go }));
        }
        else
        {
            go.interactable = false;
            go.blocksRaycasts = false;
            go.alpha = 0;
        }
    }

    public void ClosePopup(CanvasGroup[] go)
    {
        if (go[0].gameObject.GetComponent<AnimationController>() != null && go[0].gameObject.GetComponent<AnimationController>().AnimateOnVisible)
        {
            go[0].gameObject.GetComponent<Animator>().SetBool("AnimateIn", false);
            StartCoroutine(ClosePopupWaitForAnimation(go[0].gameObject.GetComponent<Animator>(), go));
        }
        else
        {
            foreach (CanvasGroup cg in go)
            {
                cg.interactable = false;
                cg.blocksRaycasts = false;
                cg.alpha = 0;
            }
        }
    }

    private void FinishClosingPopup(CanvasGroup[] go)
    {
        foreach (CanvasGroup cg in go)
        {
            cg.interactable = false;
            cg.blocksRaycasts = false;
            cg.alpha = 0;
        }
    }

    private IEnumerator ClosePopupWaitForAnimation(Animator anim, CanvasGroup[] go)
    {
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        FinishClosingPopup(go);
    }
}
