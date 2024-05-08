using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDebug : MonoBehaviour
{
    private bool isPaused = false;

    public void ChangeTimeScale(float value)
    {
        Time.timeScale = value;
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        Time.timeScale = isPaused ? 0 : 1;
    }

    public void SkipFwd()
    {
        Time.timeScale = 1;

        StartCoroutine(FramePause());
    }

    private IEnumerator FramePause()
    {
        yield return new WaitForSeconds(Time.deltaTime);
        Time.timeScale = 0;
    }
}
