using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.IO;

public class SaveData : MonoBehaviour
{
    public string m_data = "";
    public char m_upgrades;
    public string m_directory = "";
    public bool[] m_upgradeArray = new bool[(int)E_UPGRADES.SIZE];

    // this is insted of a start function to decide when this function is called
    public void Initialize()
    {
        m_directory = Directory.GetCurrentDirectory();
        //m_directory += "\\Saves";
        m_directory += "\\save.sav";

        Debug.Log("File is : " + m_directory);

        Load();

        m_upgrades = (char)int.Parse(m_data);

        GetSavedUpgrades();
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
            File.WriteAllText(m_directory, "0");
            Debug.Log("Created new file.");
            m_data = "0";
        }

        m_upgrades = (char)int.Parse(m_data);
    }

    public void AddUpgrade(E_UPGRADES type)
    {
        m_upgrades |= (char)IntToBit((int)type);
        m_data = ((int)m_upgrades).ToString();

        Save();
    }

    private int IntToBit(int input)
    {
        // function return examples
        // 0 => 1
        // 3 => 8
        // 7 => 128

        int bit = 1;

        for (int z = 0; z < input; z++)
        {
            bit *= 2;
        }

        return bit;
    }

    public void GetSavedUpgrades()
    {
        m_upgradeArray[(int)E_UPGRADES.MOVE_CRATE] = (m_upgrades & IntToBit((int)E_UPGRADES.MOVE_CRATE)) > 0;
        m_upgradeArray[(int)E_UPGRADES.GHOST_1] = (m_upgrades & IntToBit((int)E_UPGRADES.GHOST_1)) > 0;
        m_upgradeArray[(int)E_UPGRADES.GHOST_2] = (m_upgrades & IntToBit((int)E_UPGRADES.GHOST_2)) > 0;
    }
}
