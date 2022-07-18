using System.Collections;
using System.Collections.Generic;
using E7.Introloop;
using UnityEngine;

public class PlayIntroLoop : MonoBehaviour
{
    [SerializeField] IntroloopAudio Track;
    // Start is called before the first frame update
    void Start()
    {
        IntroloopPlayer.Instance.Play(Track);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
