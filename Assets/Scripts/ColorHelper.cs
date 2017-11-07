using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorHelper : MonoBehaviour {

	public Material blackMaterial;
	public static Material black;

	public Material greenMaterial;
	public static Material green;

	// Use this for initialization
	void Start () {
		ColorHelper.black = blackMaterial;
		ColorHelper.green = greenMaterial;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
