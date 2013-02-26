using UnityEngine;
using System.Collections;

public class SplashScreen : MonoBehaviour
{

    public int guiDepth = 0;
    public Texture2D splashLogo; // the logo to splash;
    public float fadeSpeed = 0.3f;
    public float waitTime = 5f; // seconds to wait before fading out
    public bool waitForInput = false; // if true, this acts as a "press any key to continue"
    private float timeFadingInFinished = 0.0f;
    private float alpha = 1f;
    private enum FadeStatus
    {
        Paused,
        FadeIn,
        FadeWaiting,
        FadeOut
    }
    private FadeStatus status = FadeStatus.Paused;
  

    void Start()
    {
        StartCoroutine(GetComponent<CreateEnvironment>().intiateRoom("localhost:8080/beta.xml"));

    }

    public void StartSplash()
    {
        status = FadeStatus.FadeIn;
    }

    void Update()
    {
        switch (status)
        {
            case FadeStatus.FadeIn:
                alpha += fadeSpeed * Time.deltaTime;
                break;
            case FadeStatus.FadeWaiting:
                if ((!waitForInput && Time.time >= timeFadingInFinished + waitTime) || (waitForInput && Input.anyKey))
                {
                    alpha = 1.0f;
                    status = FadeStatus.FadeOut;
                }
                break;
            case FadeStatus.FadeOut:
                alpha += -fadeSpeed * Time.deltaTime;
                break;
            case FadeStatus.Paused:
                break;
        }
    }

    void OnGUI()
    {
        GUI.depth = guiDepth;
      
        
            GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b,Mathf.Clamp01(alpha));
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), splashLogo);
        
        }
    

    public void StartFade()
    {
        status = FadeStatus.FadeWaiting;
    }


    
}