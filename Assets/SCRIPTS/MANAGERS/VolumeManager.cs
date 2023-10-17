using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class VolumeManager : MonoBehaviour
{
    [SerializeField] private TMP_Text volumeText;
    [SerializeField] private Slider slider;

    [SerializeField] private string volumeParameter;
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private float multiplier = 30.0f;

    private void Awake()
    {
        slider.onValueChanged.AddListener(HandleSliderValueChanged);
    }
    
    private void OnEnable()
    {
        SetVolume();
    }

    private void OnDisable()
    {
        SetVolume();
    }
    
    void Start()
    {
        slider.value = PlayerPrefs.GetFloat(volumeParameter, slider.value);
    }
    
    private void HandleSliderValueChanged(float value)
    {
        mixer.SetFloat(volumeParameter, Mathf.Log10((value)) * multiplier);
        volumeText.text = slider.value.ToString("f1");
    }

    private void SetVolume()
    {
        PlayerPrefs.SetFloat(volumeParameter, slider.value);
        slider.value = PlayerPrefs.GetFloat(volumeParameter);
    }
}
