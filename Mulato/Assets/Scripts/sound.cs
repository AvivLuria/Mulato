using UnityEngine;
using System.Collections;

public class sound : MonoBehaviour
{

    public AudioSource audio;

    void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    public void onClick()
    {

        if (audio.mute)
        {
            audio.mute = false;
        }
        else
        {
            audio.mute = true;
        }
    }
}
