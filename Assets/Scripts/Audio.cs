using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio : MonoBehaviour
{
    public AudioSource aSource;
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
    // Start is called before the first frame update
    public void Play()
    {
        aSource.Play();
    }
}
