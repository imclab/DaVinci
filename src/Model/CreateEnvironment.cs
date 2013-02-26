using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using System.Collections.Generic;
public class CreateEnvironment : MonoBehaviour
{
    private string urlPath;
    private XMLParser xml;
    private List<Information> info;
    public GameObject[] activeObjects;
    private string XMLcontent;

    /**
     *Model class for instansiate a room based on the information in provided XML file.
     * 
     * 
     */

    void Start()
    {



    }
    public IEnumerator intiateRoom(string path)
    {
        Debug.Log("Yielding");
        yield return new WaitForSeconds(1);
        StartCoroutine(downloadXML(path));
        Debug.Log("Unity: Start");
    }

    /**
     * Texturedownload coroutine, downloads texture and sets the texture to the corresponding gameobject 
     * @param string url
     * @param int index
     * @param GameObject toBeRendered
     * @return IEnumerator
     */
    private IEnumerator download(string url, GameObject toBeRendered)
    {
        WWW loader = new WWW(url);
        Debug.Log("Downloading");
        yield return loader;
        if (loader.error != null)
        {
            Debug.Log("Error when downloading texture from url: " + url + "With error: " + loader.error);
        }
        else
        {
            Debug.Log("Download successful from url: " + url);
            toBeRendered.renderer.material.mainTexture = loader.texture;
        }
    }
    /**
   * XML download coroutine, downloads 
   * @param string url
   * @return IEnumerator
   */
    private IEnumerator downloadXML(string url)
    {
        WWW loader = new WWW(url);
        Debug.Log("Downloading XML");
        yield return loader;
        if (loader.error != null)
        {
            Debug.Log("Error when downloading XML from url: " + url + "With error: " + loader.error);
        }
        else
        {
            Debug.Log("Download of XML successful from url: " + url);
            XMLcontent = loader.text;
            Debug.Log("Doing setup");
            doLevelSetup();

        }
    }
    /*
   * Reads information for all objects to be instantiated in the scene. Placement, rotation, scale, scripts associated with a certain object are set here.,
   * @param List<Information> info
   * @return GameObject[]
   */
    public void MakeObjects(List<Information> info)
    {
        GameObject objects;
        int i = 0;
        foreach (Information inf in info)
        {
            objects = GameObject.CreatePrimitive(PrimitiveType.Cube);
            // TODO objects[i].active = false;
            objects.transform.localScale = inf.UseDimensions();
            objects.transform.position = inf.UsePlacement();
            objects.transform.Rotate(inf.UseRotation());


            Debug.Log(i);
            Debug.Log(info.Count);
            if (inf.getType().Equals("painting"))
            {
                Debug.Log("Object with id: " + i + "is a painting");
                StartCoroutine(download(inf.TextureForObject(), objects));
                Debug.Log("Adding scripts to object");
                string[] scripts = inf.ScriptsToBeUsed();
                foreach (string s in scripts)
                {
                    objects.AddComponent(s);
                }
                objects.GetComponent<HoverToolTipScript>().tooltip = inf.UseTooltip(); ;
                Debug.Log("Rendering texture for object with id: " + i);
                
            }


            Debug.Log("Object:" + i + " is now Ready");
            i++;
        }
        
    }

    private void readInformation()
    {
        info = xml.parse(XMLcontent);
    }

    private void addCollidersToLevel()
    {
        GameObject[] level = GameObject.FindGameObjectsWithTag("BOX");
        for (int j = 0; j < level.Length; j++)
        {
            level[j].AddComponent<BoxCollider>();

        }

    }
    public void doLevelSetup()
    {
        Debug.Log("Doing lvl setup");
        //addCollidersToLevel();
        xml = new XMLParser();
        readInformation();
        MakeObjects(info);
        GetComponent<SplashScreen>().StartFade();


    }
}


