using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropBox : MonoBehaviour
{
    FullScreenMode screenMode;
    public Dropdown resolutionDropdown;
    [SerializeField] Toggle fullscreenbtn;
    [SerializeField] List<Resolution> resolutions = new List<Resolution>();
    int resolutionNum = 0;
    void Start()
    {
        fullscreenbtn = GameObject.Find("Ui").transform.GetChild(2).GetChild(2).GetChild(1).GetComponent<Toggle>();
        resolutionDropdown = GameObject.Find("Ui").transform.GetChild(2).GetChild(2).GetChild(0).GetComponent<Dropdown>();
        Init();

    }
    void Init()
    {
        for(int i = 0; i<Screen.resolutions.Length; i++)
        {
            //if (Screen.resolutions[i].refreshRate==60)
                resolutions.Add(Screen.resolutions[i]);
        }
        resolutionDropdown.options.Clear();
        int optionNum = 0;
        resolutions.AddRange(Screen.resolutions);
        resolutionDropdown.options.Clear();
        foreach(Resolution item in resolutions)
        {
            Dropdown.OptionData option = new Dropdown.OptionData();
            option.text = item.width + "X" + item.height;
            resolutionDropdown.options.Add(option);
           if(item.width == Screen.width && item.height == Screen.height)
            {
                resolutionDropdown.value = optionNum;
            }
            optionNum++;
        }
        resolutionDropdown.RefreshShownValue();

        fullscreenbtn.isOn = Screen.fullScreenMode.Equals(FullScreenMode.FullScreenWindow) ? true : false;
    }

    public void DropOptionChange(int x)
    {
        resolutionNum = x;
    }
    public void FullScreenBtn(bool isFull)
    {
        screenMode = isFull ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
    }
    public void OkBtnClick()
    {
        Screen.SetResolution(resolutions[resolutionNum].width, resolutions[resolutionNum].height,screenMode);
    }
}
