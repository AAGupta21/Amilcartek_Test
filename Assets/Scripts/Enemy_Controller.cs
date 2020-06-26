using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Controller : MonoBehaviour
{
    public bool IsAlive = true;
    [SerializeField] private GameObject ExplosionEffect = null;

    public void ResetEnemy()
    {
        IsAlive = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            IsAlive = false;

            Instantiate(ExplosionEffect, transform.position, ExplosionEffect.transform.rotation);

            gameObject.SetActive(false);
        }
    }
}
