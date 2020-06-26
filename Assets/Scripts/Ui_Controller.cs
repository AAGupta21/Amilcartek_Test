using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ui_Controller : MonoBehaviour
{
    [SerializeField] private GameObject Main_Menu = null;
    [SerializeField] private GameObject Pause_Menu = null;
    [SerializeField] private GameObject Settings_Menu = null;
    [SerializeField] private GameObject Play_Menu = null;
    [SerializeField] private GameObject Victory_page = null;
    [SerializeField] private GameObject Defeat_page = null;

    [SerializeField] private Text lives_cnt = null;
    [SerializeField] private Text MusicInfo = null;

    public void Ini_MainMenu()
    {
        DeactivateAllUI();
        Main_Menu.SetActive(true);
    }

    public void Ini_PauseMenu()
    {
        DeactivateAllUI();
        Pause_Menu.SetActive(true);
    }

    public void Ini_SettingsMenu()
    {
        DeactivateAllUI();
        Settings_Menu.SetActive(true);
    }

    public void Ini_PlayMenu()
    {
        DeactivateAllUI();
        Play_Menu.SetActive(true);
    }

    public void Ini_Victory()
    {
        DeactivateAllUI();
        Victory_page.SetActive(true);
    }

    public void Ini_Defeat()
    {
        DeactivateAllUI();
        Defeat_page.SetActive(true);
    }

    public void ShowLives(int num)
    {
        lives_cnt.text = "LIVES : " + num.ToString();
    }

    public void SwitchMusicInfo(bool MusicIsOn)
    {
        if(MusicIsOn)
        {
            MusicInfo.text = "MUSIC : ON";
        }
        else
        {
            MusicInfo.text = "MUSIC : OFF";
        }
    }

    private void DeactivateAllUI()
    {
        if(Main_Menu.activeInHierarchy)
        {
            Main_Menu.SetActive(false);
        }

        if (Pause_Menu.activeInHierarchy)
        {
            Pause_Menu.SetActive(false);
        }

        if (Settings_Menu.activeInHierarchy)
        {
            Settings_Menu.SetActive(false);
        }

        if (Play_Menu.activeInHierarchy)
        {
            Play_Menu.SetActive(false);
        }

        if(Victory_page.activeInHierarchy)
        {
            Victory_page.SetActive(false);
        }

        if(Defeat_page.activeInHierarchy)
        {
            Defeat_page.SetActive(false);
        }
    }
    
}
