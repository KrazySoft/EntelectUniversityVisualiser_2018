using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepoScript : MonoBehaviour {

    public TextMesh elementText;
    public int index;
    public string element;

    // Use this for initialization
    void Start () {
        elementText.text = element;

    }

    // Update is called once per frame
    void Update()
    {
        elementText.text = element;
    }
}
