using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class startScene : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void loadOpenScene()
    {
        SceneManager.LoadScene("StartScene", LoadSceneMode.Single);
    }
    public void StartGame()
    {
        SceneManager.LoadScene("scene", LoadSceneMode.Single);
    }
    public void ShowCredits()
    {
        SceneManager.LoadScene("Credits", LoadSceneMode.Single);
    }
}
