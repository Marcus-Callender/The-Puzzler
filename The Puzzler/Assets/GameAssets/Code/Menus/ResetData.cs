using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.SceneManagement;

using System;
using System.IO;

public class ResetData : MonoBehaviour
{
    void Start()
    {
        UnityEngine.UI.Button btn = gameObject.GetComponent<UnityEngine.UI.Button>();
        btn.onClick.AddListener(DeleteFiles);
    }
    
    void DeleteFiles()
    {

        string directory = Directory.GetCurrentDirectory();
        
        string saveDir = directory + "\\save.sav";
        string posDir = directory + "\\posSave.sav";

        if (File.Exists(saveDir))
        {
            File.Delete(saveDir);
        }

        if (File.Exists(posDir))
        {
            File.Delete(posDir);
        }
    }
}
