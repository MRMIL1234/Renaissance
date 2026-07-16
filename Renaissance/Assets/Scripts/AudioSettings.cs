using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    public AudioMixer mixer;
    public Slider slider;

       public void SetVolume()
{
    Debug.Log("WORKS");

    float value = slider.value;
    mixer.SetFloat("MasterVolume", Mathf.Log10(value) * 20);
}

}