using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class UpdateAudioSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer Mixer;

    // Start is called before the first frame update
    private void Start()
    {
        if (!PlayerPrefs.HasKey(Mixer.name))
        {
            PlayerPrefs.SetFloat(Mixer.name, .5f);
        }

        var volume = PlayerPrefs.GetFloat(Mixer.name);

        SetVolumeNormalized(volume);
    }

    private void SetVolumeNormalized(float volume)
    {
        float logVolume = Mathf.Log10(volume) * 20;
        logVolume = Mathf.Clamp(logVolume, -80, 0);

        Mixer.SetFloat("Volume", logVolume);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
