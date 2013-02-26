using UnityEngine;
using System.Collections;

public class Information
{

    private Vector3 dimension;
    private Vector3 placement;
    private Vector3 rotation;
    private string[] scripts;
    private string path;
    private string type;
    private string tooltip;
    public Information()
    {
        scripts = new string[10];
        path = "";
        type = "";
    }

    public void ObjectDimension(int x, int y, int z)
    {

        dimension = new Vector3(x, y, z);

    }
    public void SetToolTip(string tooltip)
    {
        this.tooltip = tooltip;
    }
    public void ObjectPlacement(int x, int y, int z)
    {
        placement = new Vector3(x, y, z);
    }
    public void ObjectRotation(int x, int y, int z)
    {
        rotation = new Vector3(x, y, z);
    }

    public void AddScripts(string[] script)
    {
        scripts = script;
    }

    public void setTexturePath(string path)
    {
        this.path = path;
    }

    public string[] ScriptsToBeUsed()
    {
        return scripts;
    }
    public string TextureForObject()
    {
        return path;
    }
    public Vector3 UsePlacement()
    {
        return placement;
    }
    public Vector3 UseDimensions()
    {
        return dimension;
    }
    public Vector3 UseRotation()
    {
        return rotation;
    }
    public void setType(string type)
    {
        this.type = type;
    }
    public string UseTooltip()
    {
        return tooltip;
    }
    public string getType()
    {
        return type;
    }
}

