using UnityEngine;
using System.Collections;
using System.Xml;
using System.IO;
using System.Text;
using System.Collections.Generic;

public class XMLParser
{
    private Information info;
    private string XMLcontent;
    private StringBuilder sb = new StringBuilder();
    public XMLParser()
    {
        Debug.Log("Parser was created");
        // FOR READING EXTERNAL XML BEGIN
        // Create a resolver with default credentials.
        
        // END   



    }
    
    
    public List<Information> parse(string fileName)
    {
        StringReader sr = new StringReader(fileName);
        // Skip the BOM, Unity bug
        sr.Read();
        XmlReader reader = XmlReader.Create(sr);

        
        Debug.Log("Parsing file: " + fileName);
        //Do proper format for parsing the Xml as a string
        //TODO Need to fix arraysize
        List<Information> objectInfo = new List<Information>();
        int currentID = 0;
        int nextID = 0;
        string x, y, z = "";
        while (reader.Read())
        {

            switch (reader.Name)
            {   case "object":
                    if(reader.HasAttributes)
                    {
                        reader.MoveToNextAttribute();
                        if (int.Parse(reader.Value) == nextID)
                        {
                            currentID = nextID;
                            objectInfo.Add(new Information());
                            nextID++;
                        }
                    }
                    break;
                case "dimensions":
                    if (reader.HasAttributes)
                    {
                        Debug.Log("Reading dimensions for object with ID: " + currentID);
                        //Move to attribute
                        reader.MoveToNextAttribute();
                        //Get value and trim whitespace
                        x = reader.Value;
                        reader.MoveToNextAttribute();
                        y = reader.Value;
                        reader.MoveToNextAttribute();
                        z = reader.Value;
                        //Set object dimension and convert string to int32
                        objectInfo[currentID].ObjectDimension(int.Parse(x), int.Parse(y), int.Parse(z));
                    }

                    break;

                case "placement":
                    if (reader.HasAttributes)
                    {
                        Debug.Log("Reading placement for object with ID: " + currentID);
                        reader.MoveToNextAttribute();
                        x = reader.Value;
                        reader.MoveToNextAttribute();
                        y = reader.Value;
                        reader.MoveToNextAttribute();
                        z = reader.Value;
                        objectInfo[currentID].ObjectPlacement(int.Parse(x), int.Parse(y), int.Parse(z));
                    }
                    break;
                case "rotation":
                    if (reader.HasAttributes)
                    {
                        Debug.Log("Reading rotation for object with ID: " + currentID);
                        reader.MoveToNextAttribute();
                        x = reader.Value;
                        reader.MoveToNextAttribute();
                        y = reader.Value;
                        reader.MoveToNextAttribute();
                        z = reader.Value;
                        objectInfo[currentID].ObjectRotation(int.Parse(x), int.Parse(y), int.Parse(z));
                    }
                    break;
                case "information":
                    if (reader.HasAttributes)
                    {   
                      
                        reader.MoveToNextAttribute();
                        objectInfo[currentID].setType(reader.Value);
                        
                        if(objectInfo[currentID].getType() == "painting")
                        {
                            Debug.Log("Reading information (Scripts/Path) for object with ID: " + currentID);
                            reader.MoveToNextAttribute();
                            objectInfo[currentID].AddScripts(reader.Value.Split(':'));
                            reader.MoveToNextAttribute();
                            objectInfo[currentID].setTexturePath(reader.Value);
                        }
                       
                    }
                    break;
                case "tooltip":
                    if (reader.HasAttributes)
                    {
                        Debug.Log("Reading tooltip for object with ID: " + currentID);
                        reader.MoveToNextAttribute();
                        objectInfo[currentID].SetToolTip(reader.Value);
                    }
                    break;
        }
        }
        reader.Close();
        return objectInfo;
    }
}










