using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour
{
    [SerializeField] private int Line_Accuracy = 5;
    [SerializeField] private float Time_Diff = 0.05f;
    [SerializeField] private LineRenderer Pathway_object = null;

    [SerializeField] private GameObject Projectile_Ball_Prefab = null;
    [SerializeField] private float Max_Projectile_Z_Angle = 75f;
    [SerializeField] private float Max_Input_Angle = 0.5f;
    [SerializeField] private float Initial_Projectile_Speed = 20f;

    [HideInInspector] public Vector3 Initial_Projectile_Position = Vector3.zero;

    private bool IsPlayersMove = false;
    private bool IsPlayersMoveBegunMouse = false;
    private bool IsPlayersMoveBegunTouch = false;
    private bool IsAPathDrawn = false;
    private Vector3[] PathwayArray = null;
    private GameObject ProjectileBall = null;
    private Vector3 direc = Vector3.zero;
    private Vector3 Ini_Pos = Vector3.zero;
    private Vector3 Final_Pos = Vector3.zero;

    private bool IsOnPause = false;
    
    public delegate void TurnExpendedByPlayerEventHandler();
    public event TurnExpendedByPlayerEventHandler TurnExpended;

    public void InitiatePlayer()
    {
        Ui_Controller.ButtonHasBeenPressedEventHandler += BallLaunchButtonPressRequest;
        PathwayArray = new Vector3[Line_Accuracy];
        ProjectileBall = Instantiate(Projectile_Ball_Prefab, Vector3.zero, Quaternion.identity, transform);
        ProjectileBall.SetActive(false);
        Pathway_object.positionCount = Line_Accuracy;
    }

    public void Start_Play()
    {
        ProjectileBall.GetComponent<Rigidbody>().useGravity = false;
        ProjectileBall.GetComponent<Rigidbody>().velocity = Vector3.zero;
        ProjectileBall.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        ProjectileBall.transform.position = Initial_Projectile_Position;
        ProjectileBall.SetActive(true);
        IsOnPause = IsAPathDrawn = false;
        IsPlayersMove = true;
    }

    public void Stop_Play()
    {
        Time.timeScale = 1f;
        StopAllCoroutines();
        IsPlayersMove = IsPlayersMoveBegunMouse = IsPlayersMoveBegunTouch = false;
        Deactivate_Line();
        ProjectileBall.SetActive(false);
    }

    public void Pause_Play()
    {
        Time.timeScale = 0f;

        IsOnPause = true;

        if(IsPlayersMoveBegunMouse || IsPlayersMoveBegunTouch)
        {
            Deactivate_Line();
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
        if (!IsOnPause)
        {
            if (IsPlayersMove && ((Input.GetMouseButtonDown(0) || Input.touchCount > 0 )))
            {
                if (Input.touchCount > 0)
                {
                    Vector3 p = Input.touches[0].position;
                    p.z = 10f;
                    Ini_Pos = Camera.main.ScreenToWorldPoint(p);
                    IsPlayersMoveBegunTouch = true;
                }
                else
                {
                    Vector3 p = Input.mousePosition;
                    p.z = 10f;
                    Ini_Pos = Camera.main.ScreenToWorldPoint(p);
                    IsPlayersMoveBegunMouse = true;
                }
            }

            if ( (IsPlayersMoveBegunMouse && Input.GetMouseButtonUp(0)) || ( IsPlayersMoveBegunTouch && Input.touchCount == 0))
            {
                if(IsPlayersMoveBegunMouse)
                {
                    Vector3 p = Input.mousePosition;
                    p.z = 10f;
                    Final_Pos = Camera.main.ScreenToWorldPoint(p);
                }
                else
                {
                    Vector3 p = Input.touches[Input.touches.Length - 1].position;
                    p.z = 10f;
                    Final_Pos = Camera.main.ScreenToWorldPoint(p);
                }

                float Z_angle = Mathf.Lerp(0f, Max_Projectile_Z_Angle, Vector3.Distance(Ini_Pos, Final_Pos) / Max_Input_Angle);

                direc = (Final_Pos - Ini_Pos).normalized;

                float AngleOfXY = Vector3.SignedAngle(Vector3.right, direc, Vector3.right);
                
                direc = Quaternion.Euler(0f, 90f - Z_angle, 0f) * direc;
                
                DrawGhostPath(AngleOfXY, 90f - Z_angle);

                IsPlayersMoveBegunMouse = IsPlayersMoveBegunTouch = false;
            }
        }
    }

    private void DrawGhostPath(float AngleWithXY, float AngleWithXZ)
    {
        for(int i = 0; i < Line_Accuracy; i++)
        {
            PathwayArray[i] = Initial_Projectile_Position +
                new Vector3(i * Time_Diff * Initial_Projectile_Speed * Mathf.Cos(AngleWithXY * Mathf.Deg2Rad) * Mathf.Cos(AngleWithXZ * Mathf.Deg2Rad),
                 i * Time_Diff * Initial_Projectile_Speed * Mathf.Sin(AngleWithXY * Mathf.Deg2Rad) - 0.5f * Physics.gravity.magnitude * i * Time_Diff * i * Time_Diff,
                 i * Time_Diff * Initial_Projectile_Speed * Mathf.Cos(AngleWithXY * Mathf.Deg2Rad) * Mathf.Sin(AngleWithXZ * Mathf.Deg2Rad));

            if(PathwayArray[i].z < 0f)
            {
                PathwayArray[i] = new Vector3(PathwayArray[i].x, PathwayArray[i].y, -(PathwayArray[i].z));
            }
        }
        Pathway_object.SetPositions(PathwayArray);
        if(!IsAPathDrawn)
        {
            IsAPathDrawn = true;
            Activate_Line();
        }
    }

    private void LaunchBall(Vector3 direc)
    {
        if(direc.z < 0f)
        {
            direc = new Vector3(direc.x, direc.y, -direc.z);
        }

        ProjectileBall.GetComponent<Rigidbody>().useGravity = true;
        ProjectileBall.GetComponent<Rigidbody>().velocity = direc * Initial_Projectile_Speed;
        StartCoroutine(BallLaunch());
    }

    private IEnumerator BallLaunch()
    {
        yield return new WaitForSeconds(5f);
        TurnExpended?.Invoke();
    }

    private void BallLaunchButtonPressRequest()
    {
        if(IsAPathDrawn)
        {
            IsPlayersMoveBegunMouse = IsPlayersMoveBegunTouch = IsPlayersMove = false;
            Deactivate_Line();
            LaunchBall(direc);
        }
    }

    private void Deactivate_Line()
    {
        Pathway_object.gameObject.SetActive(false);
    }

    private void Activate_Line()
    {
        Pathway_object.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        Ui_Controller.ButtonHasBeenPressedEventHandler -= BallLaunchButtonPressRequest;
    }
}