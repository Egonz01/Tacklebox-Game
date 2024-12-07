using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Mathematics;

public class ScreenSettings : MonoBehaviour
{
    [SerializeField] TMP_Dropdown resolutionDropdown;

    Resolution[] resolutions;

    void Start()
    {

        resolutions = Screen.resolutions;
        Resolution currRes = Screen.currentResolution;
        int currResIndex = PlayerPrefs.GetInt("ResIndex", resolutions.Length - 1);
        for (int i = 0; i < resolutions.Length; i++) {
            string resolutionString = resolutions[i].width.ToString() + "x" + resolutions[i].height.ToString();
            resolutionDropdown.options.Add(new TMP_Dropdown.OptionData(resolutionString));
        }
        currResIndex = math.min(currResIndex, resolutions.Length - 1);
        resolutionDropdown.value = currResIndex;
        SetResolution();
    }

    public void SetResolution() {
        int resIndex = resolutionDropdown.value;
        Screen.SetResolution(resolutions[resIndex].width, resolutions[resIndex].height, true);
        PlayerPrefs.SetInt("ResIndex", resolutionDropdown.value);
    }
}
