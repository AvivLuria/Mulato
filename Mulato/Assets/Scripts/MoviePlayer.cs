using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(RawImage))]
public class MoviePlayer : MonoBehaviour
{
/*#if UNITY_STANDALONE_WIN
    public MovieTexture m_TheMovie;

    private AudioSource m_AudioSource;
    private RawImage m_VideoSource;   
    private bool loadLevelRequested = false;

    // Use this for initialization
    void Start()
    {
        loadLevelRequested = false;
        m_AudioSource = this.GetComponent<AudioSource>();
        m_VideoSource = this.GetComponent<RawImage>();


        m_AudioSource.Play();
        if (Application.platform == RuntimePlatform.Android)
        {
            Handheld.PlayFullScreenMovie("trailERMOV.mov");
        } else
        {
            m_TheMovie.Play();
            m_VideoSource.texture = m_TheMovie;      
        }
       
       
    }

    IEnumerator LoadLevel()
    {
        AsyncOperation async = SceneManager.LoadSceneAsync("StartScene", LoadSceneMode.Additive);
        yield return async;
        Debug.Log("Loading complete");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            loadLevelRequested = true;

            Debug.Log("Sent Request");
            //StartCoroutine(LoadLevel());
            //Application.LoadLevel("StartScene");
            SceneManager.LoadScene("StartScene");

        }
    }
    */
    void Start()
    {
        AudioSource m_AudioSource = this.GetComponent<AudioSource>();
        m_AudioSource.Play();
        Handheld.PlayFullScreenMovie("trailerMP4.mp4", Color.black, FullScreenMovieControlMode.Hidden);
        SceneManager.LoadScene("StartScene");
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SceneManager.LoadScene("StartScene");
        }
    }
}
