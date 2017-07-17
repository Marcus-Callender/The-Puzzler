using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class ReturnToMainMenu : MonoBehaviour
{
    void Start()
    {
        UnityEngine.UI.Button btn = gameObject.GetComponent<UnityEngine.UI.Button>();
        btn.onClick.AddListener(Exit);
    }

    void Exit()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(0);
    }
}
