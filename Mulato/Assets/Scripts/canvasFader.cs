using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class canvasFader : MonoBehaviour {

    private float alpha = 0.8f;
    private float timeToConvert = 3;

	// Use this for initialization
	void Start () {
        this.GetComponent<Image>().CrossFadeAlpha(0, 0, false);
    }

    public void OnFade()
    {
        this.GetComponent<Image>().CrossFadeAlpha(alpha, timeToConvert, false);
    }

    public void OffFade()
    {
        this.GetComponent<Image>().CrossFadeAlpha(0, 0, false);
    }
}
