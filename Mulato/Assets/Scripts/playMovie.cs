using UnityEngine;
using System.Collections;

public class playMovie : MonoBehaviour {

    public MovieTexture movTexture;
    void Start()
    {
        GetComponent<Renderer>().material.mainTexture = movTexture;
        movTexture.Play();
    }
}
