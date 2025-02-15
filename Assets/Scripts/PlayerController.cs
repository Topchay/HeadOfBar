using UnityEngine;
using System.Collections;
using System;
using static UnityEngine.GraphicsBuffer;
using Unity.VisualScripting;

public class PlayerController : MonoBehaviour
{
    Inventory Inventory;
    PlayerManager PlayerManager;

    private Rigidbody rb;
    private Animator playerAnim;

    private float moveSpeed;

    Vector3 mv;

    void Awake()
    {
        PlayerManager = GetComponent<PlayerManager>();
        Inventory = FindObjectOfType<Inventory>();
        rb = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();
    }

    private void Start()
    {
        moveSpeed = PlayerManager.Speed;
    }

    void FixedUpdate()
    {
        CharacterRotation();
    }

    void Update()
    {
        UiController();
        ControlMovement();
        StartAnim();
    }

    void ControlMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        mv = new Vector3(horizontalInput, 0, verticalInput).normalized;

        rb.velocity = mv * moveSpeed;
    }

    void CharacterRotation()
    {
        Plane playerPlane = new Plane(Vector3.up, transform.position);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float hitdist = 0.0f;

        if (playerPlane.Raycast(ray, out hitdist))
        {
            Vector3 targetPoint = ray.GetPoint(hitdist);
            Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10.0f);
        }
    }

    void StartAnim()
    {
        if (mv != Vector3.zero)
        {
            float angle = Vector3.SignedAngle(mv, transform.forward, Vector3.up);

            if (angle >= -45 && angle <= 45)
            {
                SetAnim("run");
            }
            else if (angle > 45 && angle <= 135)
            {
                SetAnim("runLeft");
            }
            else if ((angle > 135 && angle <= 180) || (angle >= -180 && angle < -135))
            {
                SetAnim("runBack");
            }
            else if (angle < -45 && angle >= -135)
            {
                SetAnim("runRight");
            }
        }
        else
        {
            SetAnim();
        }
    }

    void SetAnim(string anim = null)
    {
        playerAnim.SetBool("runBack", false);
        playerAnim.SetBool("run", false);
        playerAnim.SetBool("runLeft", false);
        playerAnim.SetBool("runRight", false);

        if (anim != null)
        {
            playerAnim.SetBool(anim, true);
        }
    }

    public void UiController()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            Inventory.OpenClose();
        }
    }
}