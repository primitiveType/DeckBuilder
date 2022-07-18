using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace App
{
    public class AudioSlider : MonoBehaviour
    {
        [SerializeField] public AudioMixer Mixer;

        private Slider slider;

        private void Awake()
        {
            slider = GetComponent<Slider>();
            slider.onValueChanged.AddListener(OnSliderChanged);
            if (!PlayerPrefs.HasKey(Mixer.name))
            {
                PlayerPrefs.SetFloat(Mixer.name, .5f);
            }

            var volume = PlayerPrefs.GetFloat(Mixer.name);
            slider.value = volume;
            SetVolumeNormalized(volume);
        }

        private void OnSliderChanged(float volume)
        {
            SetVolumeNormalized(volume);
        }

        private void SetVolumeNormalized(float volume)
        {
            PlayerPrefs.SetFloat(Mixer.name, volume);
            PlayerPrefs.Save();

            float logVolume = Mathf.Log10(volume) * 20;
            logVolume = Mathf.Clamp(logVolume, -80, 0);

            Mixer.SetFloat("Volume", logVolume);
        }
    }
}
