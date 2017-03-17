using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changeTreeColor : MonoBehaviour {

    public Material treeMaterial;
    private Color color;
	// Use this for initialization
	void Start () {
        color = new Color(0.3f,0.5f,0,0.9f);
	}
	
	// Update is called once per frame
	void Update () {
        color.r += (Mathf.Sin(Time.time) * 0.005f);
        color.g += (Mathf.Sin(Time.time) * 0.005f);
        treeMaterial.SetColor("_FadeOutColorFirst", color);
	}
}
