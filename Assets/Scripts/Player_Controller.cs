using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour
{
    [SerializeField] private GameObject Ghost_Ball_Prefab = null;
    [SerializeField] private int Num_Of_Ghost_Ball = 5;
    [SerializeField] private float Time_Diff = 0.05f;

    [SerializeField] private GameObject Projectile_Ball_Prefab = null;
    [SerializeField] private float Max_Projectile_Ball_Force = 10f;
    [SerializeField] private float Min_Projectile_Ball_Force = 2f;
    [SerializeField] private float Max_Distance_From_Projectile = 0.5f;
    [SerializeField] private float Min_Distance_From_Projectile = 0f;

    [HideInInspector] public Vector3 Initial_Projectile_Position = Vector3.zero;

    private bool IsPlayersMove = false;
    private bool IsPlayersMoveBegunMouse = false;
    private bool IsPlayersMoveBegunTouch = false;
    private List<GameObject> GhostBall_List = null;
    private GameObject ProjectileBall = null;
    private Vector3 direc = Vector3.zero;
    private float Projectile_Force_val = 0f;
    private bool IsOnPause = false;

    public delegate void TurnExpendedByPlayerEventHandler();
    public event TurnExpendedByPlayerEventHandler TurnExpended;

    public void InitiatePlayer()
    {
        GhostBall_List = new List<GameObject>();
        for(int i = 0; i < Num_Of_Ghost_Ball; i++)
        {
            GameObject g = Instantiate(Ghost_Ball_Prefab, Vector3.zero, Quaternion.identity, transform);
            g.SetActive(false);
            GhostBall_List.Add(g);
        }
        
        ProjectileBall = Instantiate(Projectile_Ball_Prefab, Vector3.zero, Quaternion.identity, transform);
        ProjectileBall.SetActive(false);
    }

    public void Start_Play()
    {
        ProjectileBall.GetComponent<Rigidbody>().useGravity = false;
        ProjectileBall.GetComponent<Rigidbody>().velocity = Vector3.zero;
        ProjectileBall.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        ProjectileBall.transform.position = Initial_Projectile_Position;
        ProjectileBall.SetActive(true);
        IsOnPause = false;
        IsPlayersMove = true;
    }

    public void Stop_Play()
    {
        Time.timeScale = 1f;
        StopAllCoroutines();
        IsPlayersMove = IsPlayersMoveBegunMouse = IsPlayersMoveBegunTouch = false;
        Deactivate_Ghost_Balls();
        ProjectileBall.SetActive(false);
    }

    public void Pause_Play()
    {
        Time.timeScale = 0f;

        IsOnPause = true;

        if(IsPlayersMoveBegunMouse || IsPlayersMoveBegunTouch)
        {
            Deactivate_Ghost_Balls();
            IsPlayersMoveBegunMouse = IsPlayersMoveBegunTouch = false;
        }
    }

    public void Resume_Play()
    {
        IsOnPause = false;

        Time.timeScale = 1f;
    }

    private void Update()
    {
        if(!IsOnPause)
        {
            if (IsPlayersMove && (Input.GetMouseButton(0) || Input.touchCount > 0))
            {
                Vector3 input_Pos = Vector3.zero;

                if (Input.touchCount > 0)
                {
                    IsPlayersMoveBegunTouch = true;
                    input_Pos = Camera.main.ScreenToWorldPoint(Input.touches[0].position);
                }
                else
                {
                    IsPlayersMoveBegunMouse = true;
                    input_Pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                }

                input_Pos.z = 0f;

                if (!GhostBall_List[0].activeInHierarchy)
                {
                    Activate_Ghost_Balls();
                }

                direc = (Initial_Projectile_Position - input_Pos).normalized;

                float angle = Vector3.SignedAngle(Vector3.right, direc, Vector3.forward);

                Projectile_Force_val = Mathf.Lerp(Min_Projectile_Ball_Force, Max_Projectile_Ball_Force,
                    Vector3.Distance(Initial_Projectile_Position, input_Pos) / (Max_Distance_From_Projectile - Min_Distance_From_Projectile));

                DrawGhostBalls(Projectile_Force_val, angle);
            }

            if ((IsPlayersMoveBegunMouse && Input.GetMouseButtonUp(0)) || (IsPlayersMoveBegunTouch && Input.touchCount == 0))
            {
                IsPlayersMove = IsPlayersMoveBegunMouse = IsPlayersMoveBegunTouch = false;
                Deactivate_Ghost_Balls();
                LaunchBall(direc, Projectile_Force_val);
            }
        }
    }

    private void DrawGhostBalls(float force_val, float angle)
    {
        for(int i = 1; i <= Num_Of_Ghost_Ball; i++)
        {
            GhostBall_List[i - 1].transform.position = Initial_Projectile_Position +  new Vector3
                (i * Time_Diff * force_val * Mathf.Cos(angle * Mathf.Deg2Rad), 
                (i * Time_Diff * force_val * Mathf.Sin(angle * Mathf.Deg2Rad)) - (i * Time_Diff * i * Time_Diff * 0.5f * Physics.gravity.magnitude), 
                0f);
        }
    }

    private void LaunchBall(Vector3 direc, float force)
    {
        ProjectileBall.GetComponent<Rigidbody>().useGravity = true;
        ProjectileBall.GetComponent<Rigidbody>().AddForce(direc * force, ForceMode.Impulse);
        StartCoroutine(BallLaunch());
    }

    private IEnumerator BallLaunch()
    {
        yield return new WaitForSeconds(3f);
        TurnExpended?.Invoke();
    }

    private void Deactivate_Ghost_Balls()
    {
        foreach(GameObject g in GhostBall_List)
        {
            g.SetActive(false);
        }
    }

    private void Activate_Ghost_Balls()
    {
        foreach(GameObject g in GhostBall_List)
        {
            g.SetActive(true);
        }
    }
}