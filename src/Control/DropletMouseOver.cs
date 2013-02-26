using UnityEngine;
using System.Collections;

public class DropletMouseOver : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
    void OnMouseOver()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
       // animation.Play("droplet");
        //Display droplet animation
    }
    void OnMouseExit()
    {

        //Do not display droplet animation
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
