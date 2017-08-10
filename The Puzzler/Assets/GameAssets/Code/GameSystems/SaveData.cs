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
    public string m_positionDirectory = "";
    public bool[] m_upgradeArray = new bool[(int)E_UPGRADES.SIZE];

    //public Vector3 m_savedPos;
    //public Quaternion m_savedRot;
    //
    //public bool m_savedLeft_right;
    //public bool m_savedUse3D;

    public CharicterPosition m_savedPosData;

    private SoundSystem m_soundSystem;

    private PlayerData m_playerData;

    // this is insted of a start function to decide when this function is called
    public void Initialize()
    {
        m_directory = Directory.GetCurrentDirectory();
        m_positionDirectory = m_directory;
        m_directory += "\\save.sav";
        m_positionDirectory += "\\posSave.sav";

        Debug.Log("File is : " + m_directory);

        m_playerData = gameObject.GetComponent<PlayerData>();

        m_savedPosData = new CharicterPosition();

        Load();
        LoadPosition();

        m_upgrades = (char)int.Parse(m_data);

        GetSavedUpgrades();

        m_soundSystem = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<SoundSystem>();

        if (m_upgrades > (char)0)
        {
            m_soundSystem.NextTrack();
        }

        if (m_upgrades > (char)1)
        {
            m_soundSystem.NextTrack();
        }

        if (m_upgrades > (char)3)
        {
            m_soundSystem.NextTrack();
        }
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

    public void LoadPosition()
    {
        if (File.Exists(m_positionDirectory))
        {
            string[] lines = File.ReadAllLines(m_positionDirectory);

            // removes the brackets() at the start and end of the string
            lines[0] = lines[0].Substring(1, lines[0].Length - 2);

            // creates a new string where ever there is a ','
            string[] pos = lines[0].Split(',');

            m_savedPosData.pos.x = float.Parse(pos[0]);
            m_savedPosData.pos.y = float.Parse(pos[1]);
            m_savedPosData.pos.z = float.Parse(pos[2]);

            // removes the brackets() at the start and end of the string
            lines[1] = lines[1].Substring(1, lines[1].Length - 2);

            // creates a new string where ever there is a ','
            string[] rot = lines[1].Split(',');

            m_savedPosData.rot.x = float.Parse(rot[0]);
            m_savedPosData.rot.y = float.Parse(rot[1]);
            m_savedPosData.rot.z = float.Parse(rot[2]);
            m_savedPosData.rot.w = float.Parse(rot[3]);

            m_savedPosData.left_right = (lines[2] == "True");
            m_savedPosData.use3D = (lines[3] == "True");

            transform.position = m_savedPosData.pos;
            transform.rotation = m_savedPosData.rot;

            m_playerData.m_left_right = m_savedPosData.left_right;
            m_playerData.m_use3D = m_savedPosData.use3D;
        }
        else
        {
            m_savedPosData.pos = transform.position;
            m_savedPosData.rot = transform.rotation;

            m_savedPosData.left_right = m_playerData.m_left_right;
            m_savedPosData.use3D = m_playerData.m_use3D;

            File.WriteAllText(m_positionDirectory, m_savedPosData.pos + "\n" + m_savedPosData.rot + "\n" + 
                m_playerData.m_left_right + "\n" + m_playerData.m_use3D);

            Debug.Log("Created new pos file.");
        }

        //m_upgrades = (char)int.Parse(m_data);
    }

    public void AddUpgrade(E_UPGRADES type)
    {
        m_upgrades |= (char)IntToBit((int)type);
        m_data = ((int)m_upgrades).ToString();

        Save();

        if (m_soundSystem)
        {
            m_soundSystem.NextTrack();
        }
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

    public void SavePosition(Vector3 pos, Quaternion rot)
    {
        m_savedPosData.pos = pos;
        m_savedPosData.rot = rot;

        m_savedPosData.left_right = m_playerData.m_left_right;
        m_savedPosData.use3D = m_playerData.m_use3D;
        
        File.WriteAllText(m_positionDirectory, m_savedPosData.pos + "\n" + m_savedPosData.rot + "\n" + 
            m_playerData.m_left_right + "\n" + m_playerData.m_use3D);
    }
}
