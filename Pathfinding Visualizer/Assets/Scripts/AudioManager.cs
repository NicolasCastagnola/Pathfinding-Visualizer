using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource beep;
    public static AudioManager Instance { get { return _instance; } }
    private static AudioManager _instance;
    private void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(gameObject);

        else
            _instance = this;
    }

    private void OnDestroy()
    {
        _instance = null;
    }

    public void PlayAudio(float modifier)
    {
        beep.pitch = Mathf.Lerp(.5f, Mathf.Clamp(modifier/1.5f, 1f, 5f), Time.time);
        beep.Play();
    }
}
