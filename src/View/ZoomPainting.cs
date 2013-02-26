using UnityEngine;
using System.Collections;

public class ZoomPainting : MonoBehaviour {

    private bool isClicked;
    private string toolTip;
    private Texture tex;
    private Texture2D resizeTex;
    private float x = 0f;
    private float y = 0f;
    private float doToWidth = Screen.width - (Screen.width / 2);
    private float doToHeight = Screen.height - (Screen.height / 3);
    private float dampingFactor = 2f;
    
    void Start()
    {   
        isClicked = false;
        toolTip = GetComponent<HoverToolTipScript>().tooltip;
       

    }
    void OnMouseDown()
    {
        isClicked = true;
    }

    void OnGUI()
    {
        if (isClicked)
        {
            tex = renderer.material.mainTexture;
            x = Mathf.Lerp(x, doToHeight, Time.deltaTime);
            y = Mathf.Lerp(y, doToWidth, Time.deltaTime);
            GUILayout.Window(2, new Rect((Screen.width - doToWidth)/2, (Screen.height- doToHeight)/2, doToHeight, x-1), DoZoomPainting, "Press ESC to exit");
            
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isClicked = false;
            x = 0f;
            y = 0f;
        }

    }
    
    void DoZoomPainting(int windowID)
    {
        
            GUILayout.Box(new GUIContent(tex), GUILayout.MaxWidth(doToWidth), GUILayout.MaxHeight(x-1));
            GUILayout.Label(new GUIContent(toolTip));
            GUILayout.Label(new GUIContent(""));
        
    }
}