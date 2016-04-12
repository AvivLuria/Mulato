using UnityEngine;
using System.Collections;

public class particalesystem : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
	    GetComponent<Renderer>().sortingLayerName = "partical";
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
