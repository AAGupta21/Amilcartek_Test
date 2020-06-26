using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Manager : MonoBehaviour
{
    [SerializeField] private GameObject[] enemy_array = null;
    [SerializeField] private Vector3[] enemy_pos = null;

    public Vector3 PlayerIniPos = Vector3.zero;

    public void ResetPos()
    {
        for (int i = 0; i < enemy_array.Length; i++)
        {
            enemy_array[i].GetComponent<Rigidbody>().velocity = Vector3.zero;
            enemy_array[i].GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

            enemy_array[i].transform.position = enemy_pos[i];
            enemy_array[i].transform.rotation = Quaternion.identity;

            enemy_array[i].GetComponent<Enemy_Controller>().ResetEnemy();
            enemy_array[i].SetActive(true);
        }
    }

    public bool CheckEnemyAlive()
    {
        for(int i = 0; i< enemy_array.Length; i++)
        {
            if (enemy_array[i].GetComponent<Enemy_Controller>().IsAlive)
            {
                return true;
            }
        }

        return false;
    }
}