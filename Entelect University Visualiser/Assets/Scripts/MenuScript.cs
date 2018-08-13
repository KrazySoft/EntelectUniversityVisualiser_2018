using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour {

    public Dropdown mapSelector;

	public void startVisualiser()
    {
        int map = mapSelector.value;
        PlayerPrefs.SetInt("map_num", map);
        SceneManager.LoadScene(1);
    }

    public void quitApplication()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
