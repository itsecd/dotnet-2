using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    bool isFullScreen = false;

    public AudioMixer Am;

    Resolution[] rsl;
    List<string> resolutions;
    public Dropdown Dropdown;
    public void FullSceenToggle()
    {
        isFullScreen = !isFullScreen;
        Screen.fullScreen = isFullScreen;
    }

    public void AudioVolume(float sliderValue)
    {
        Am.SetFloat("MasterVolume", sliderValue);
    }

    public void Quality(int q)
    {
        QualitySettings.SetQualityLevel(q);
    }

    public void Awake()
    {
        resolutions = new List<string>();
        rsl = Screen.resolutions;

        foreach (var i in rsl)
            resolutions.Add(i.width + "x" + i.height);

        Dropdown.ClearOptions();
        Dropdown.AddOptions(resolutions);
    }

    public void Resolution(int r)
    {
        Screen.SetResolution(rsl[r].width, rsl[r].height, isFullScreen);
    }
}
