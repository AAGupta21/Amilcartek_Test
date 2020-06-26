using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Manager : MonoBehaviour
{
    [SerializeField] private Player_Controller pc = null;
    [SerializeField] private Level_Controller lc = null;
    [SerializeField] private Ui_Controller uc = null;
    [SerializeField] private Sound_Manager sm = null;

    [SerializeField] private int Max_Lives = 3;

    private int curr_level = 0;
    private int Lives = 3;

    private void Start()
    {
        uc.Ini_MainMenu();
        pc.InitiatePlayer();
        pc.TurnExpended += CheckWin;
    }

    public void PlayButtonPressed()
    {
        Lives = Max_Lives;

        uc.Ini_PlayMenu();
        uc.ShowLives(Lives);
        lc.LoadLevel(curr_level);
        pc.Initial_Projectile_Position = lc.ReturnPlayerPos(curr_level);

        pc.Start_Play();
    }

    public void PauseButtonPressed()
    {
        pc.Pause_Play();
        uc.Ini_PauseMenu();
    }

    public void ResumeButtonPressed()
    {
        uc.Ini_PlayMenu();
        pc.Resume_Play();
    }

    public void LoadNextLevel()
    {
        pc.Stop_Play();
        lc.DeactivateCurrLevel(curr_level);

        if (curr_level == 2)
        {
            curr_level = 0;
            uc.Ini_MainMenu();
        }
        else
        {
            curr_level++;
            PlayButtonPressed();
        }
    }

    public void RestartLevel()
    {
        pc.Stop_Play();
        lc.DeactivateCurrLevel(curr_level);
        PlayButtonPressed();
    }

    public void MainMenuButtonPressedFromSettings()
    {
        uc.Ini_MainMenu();
    }

    public void MainMenuButtonPressedDuringPlay()
    {
        pc.Stop_Play();
        lc.DeactivateCurrLevel(curr_level);
        uc.Ini_MainMenu();
    }
    
    public void SettingsButtonPressed()
    {
        uc.Ini_SettingsMenu();
    }

    public void ResetLevelButtonPressed()
    {
        curr_level = 0;
    }

    public void MusicButtonSwitch()
    {
        uc.SwitchMusicInfo(sm.SwitchMusic());
    }

    public void QuitButtonPressed()
    {
        Application.Quit();                     //May add quit confirmation later.
    }

    private void CheckWin()
    {
        if(lc.CheckForWin(curr_level))
        {
            StartCoroutine(DelayBeforeWin());
        }
        else
        {
            if(Lives == 1)
            {
                uc.Ini_Defeat();
            }
            else
            {
                uc.ShowLives(--Lives);
                pc.Start_Play();
            }
        }
    }

    private IEnumerator DelayBeforeWin()
    {
        yield return new WaitForSeconds(1.5f);
        uc.Ini_Victory();
    }

    private void OnDisable()
    {
        pc.TurnExpended -= CheckWin;
    }
}