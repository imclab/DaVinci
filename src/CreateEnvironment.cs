using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using System.Collections.Generic;
public class CreateEnvironment : MonoBehaviour
{
    private string urlPath;
    private List<Information> info;
    public GameObject[] activeObjects;
    

    void Start()
    {



    }
    public IEnumerator intiateRoom(string path)
    {
        Debug.Log("Yielding");
        yield return new WaitForSeconds(1);
        StartCoroutine(DownloadXML(path));
        Debug.Log("Unity: Start");
    }


    public void doLevelSetup(string xmlContent)
    {
        Debug.Log("Doing lvl setup");
        //addCollidersToLevel();
        MakeObjects(XMLParser.Parse(xmlContent));
        GetComponent<SplashScreen>().StartFade();


    }

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
    
    private IEnumerator DownloadXML(string url)
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
            doLevelSetup(loader.text);

        }
    }
    
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
                StartCoroutine(download(inf.TextureForObject(), objects));
                Debug.Log("Adding scripts to object");
                string[] scripts = inf.ScriptsToBeUsed();
                foreach (string s in scripts)
                {
                    objects.AddComponent(s);
                }
                objects.GetComponent<ToolTip>().tooltip = inf.UseTooltip(); ;
                Debug.Log("Rendering texture for object with id: " + i);
                
            }


            Debug.Log("Object:" + i + " is now Ready");
            i++;
        }
        
    }

    

    private void AddCollidersToLevel()
    {
        GameObject[] level = GameObject.FindGameObjectsWithTag("BOX");
        foreach (GameObject g in level)
        {
            g.AddComponent<MeshCollider>();

        }

    }
}


