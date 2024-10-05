using UnityEngine;

public class SystemManagerYG : MonoBehaviour
{
    private void OnApplicationFocus(bool focus)
    {
        if (!focus)
        {
            Time.timeScale = 0.0f;
            AudioListener.pause = true;
        }
        else
        {
            Time.timeScale = 1.0f;
            AudioListener.pause = false;
        }
    }
}
