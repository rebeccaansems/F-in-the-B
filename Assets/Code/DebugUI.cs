using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugUI : MonoBehaviour
{
    public WinUI WinUi;

    public void SkipLevel()
    {
        WinUi.MakeWinVisible();
    }
}
