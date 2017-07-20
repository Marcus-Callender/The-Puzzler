using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.IO;

public class SaveData : MonoBehaviour
{
    public string m_data = "";
    public string m_directory = "";

    void Start()
    {
        m_directory = Directory.GetCurrentDirectory();
        //m_directory += "\\Saves";
        m_directory += "\\save.sav";

        Debug.Log("File is : " + m_directory);

        Load();
    }
    
    public void Save()
    {
        File.WriteAllText(m_directory, m_data);
        Debug.Log("Saved: " + m_directory + " : " + m_data);
    }

    public void Load()
    {
        if (File.Exists(m_directory))
        {
            m_data = File.ReadAllText(m_directory);
            Debug.Log("Loaded: " + m_directory + " : " + m_data);
        }
        else
        {
            File.WriteAllText(m_directory, "Text");
            Debug.Log("Created new file.");
        }
    }
}
