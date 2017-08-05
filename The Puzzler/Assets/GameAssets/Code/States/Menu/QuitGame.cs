using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class QuitGame : MonoBehaviour
{
    void Start()
    {
        UnityEngine.UI.Button btn = gameObject.GetComponent<UnityEngine.UI.Button>();
        btn.onClick.AddListener(Quit);
    }

    void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit ();
#endif
    }
}
