using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MineScript : MonoBehaviour {

    public TextMesh elementText;
    public int index;
    public string element;
    public int resourceCount;

	// Use this for initialization
	void Start () {
        elementText.text = element + " (" + resourceCount + ")";
	}
	
	// Update is called once per frame
	void Update () {
        elementText.text = element + " (" + resourceCount + ")";
    }

    public void mineResource()
    {
        resourceCount--;
    }
}
