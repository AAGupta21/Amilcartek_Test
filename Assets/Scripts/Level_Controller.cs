using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_Controller : MonoBehaviour
{
    [SerializeField] private GameObject[] Levels = null;
    
    public void DeactivateAllLevels()
    {
        for(int i = 0; i< Levels.Length; i++)
        {
            if(Levels[i].activeInHierarchy)
            {
                Levels[i].SetActive(false);
            }
        }
    }

    public void DeactivateCurrLevel(int level_num)
    {
        Levels[level_num].SetActive(false);
    }

    public void LoadLevel(int level_num)
    {
        Levels[level_num].SetActive(true);
        Levels[level_num].GetComponent<Enemy_Manager>().ResetPos();
    }

    public bool CheckForWin(int level_num)
    {
        return !Levels[level_num].GetComponent<Enemy_Manager>().CheckEnemyAlive();
    }

    public void ResetCurrLevel(int level_num)
    {
        Levels[level_num].GetComponent<Enemy_Manager>().ResetPos();
    }

    public Vector3 ReturnPlayerPos(int level_num)
    {
        return Levels[level_num].GetComponent<Enemy_Manager>().PlayerIniPos;
    }
}