using UnityEngine;
using System.Collections;

public class ToolTip : MonoBehaviour
{

    public string tooltip = "";
    private string currentToolTip;
    private bool startAnimation = false;
    private float width = 0f;
    private float height = 0f;
    private float doToSizeWidth = 100f;
    private float doToSizeHeight = 50f;
    private float dampingFactor = 2f;
    private float MAX_SIZE = 512;
    
    void Start()
    {

        currentToolTip = "";
        collider.isTrigger = true;

    }
    void OnMouseOver()
    {

        currentToolTip = tooltip;
        startAnimation = true;

    }
    void OnMouseExit()
    {
        currentToolTip = "";
        startAnimation = false;
        width = 0f;
        height = 0f;
    }


    void OnGUI()
    {
        if (startAnimation)
        {
            // Interpolation between x --> doToSize over deltatime
            width = Mathf.Lerp(width, doToSizeWidth, Time.deltaTime);
            height = Mathf.Lerp(height, doToSizeHeight, Time.deltaTime);
            GUILayout.Window(1, new Rect(0, 0, width, height), DoToolTip, "Description");


        }
    }
    
    void DoToolTip(int windowID)
    {   
        GUILayout.Box(new GUIContent(tooltip), GUILayout.MaxWidth(width+MAX_SIZE), GUILayout.MaxHeight(height));

    }
}
