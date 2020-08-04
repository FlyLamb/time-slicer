﻿using UnityEngine;

public class Player : MonoBehaviour
{
    public Transform cam;

    public float speed = 5f;
    public float jumpForce = 0.1f;
    public float gravityMultiplier = 0.1f;

    public float maxHP;

    public float HP;
    private float xRot = 0;
    private float yRot = 0;
    private float dxRot = 0;
    private float dyRot = 0;

    private Vector3 gravity;
    private CharacterController controller;

    
    [ColorUsage(true, true)]
    public Color fullColor;

    [ColorUsage(true, true)]
    public Color emptyColor;

    [ColorUsage(true, true)]
    public Color stopGlowColor;

    public Material EnergyVisualiser;
    public Material HealthVisualiser;

    public float playerEnergy = 5;
    //[System.NonSerialized]
    public float energy = 5;
    private Vector3 movement;

    private GameObject cmhelper;

    private Vector2 camMotion;
    private Vector2 camLast;



    #region Singleton
    public static Player instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance);
        }

        instance = this;
    }
    #endregion

    private void Start()
    {
        playerEnergy = LevelInfo.instance.playerEnergy;
        energy = LevelInfo.instance.playerEnergy;
        HP = LevelInfo.instance.playerHealth;
        maxHP = LevelInfo.instance.playerHealth;


        Cursor.lockState = CursorLockMode.Locked;
        controller = GetComponent<CharacterController>();
        cmhelper = new GameObject("Camera Rotation Helper");
        HP = maxHP;
    }

    private void Update()
    {
        camMotion = new Vector2(xRot, yRot) - camLast;
        camLast = new Vector2(xRot, yRot);

        HealthVisualiser.SetFloat("Saturation", HP / maxHP);
        EnergyVisualiser.SetColor("_EmissionColor", Color.Lerp(emptyColor, fullColor, energy / playerEnergy));

        if (TimeController.instance.playbackMode)
        {
            return;
        }

        gravity = Vector3.zero;


        dxRot -= Input.GetAxis("Mouse Y");
        dyRot += Input.GetAxis("Mouse X");

        //limit rotation
        if (dxRot > 90)
        {
            dxRot = 90;
        }

        if (dxRot < -90)
        {
            dxRot = -90;
        }

        xRot = Mathf.Lerp(xRot, dxRot, Time.deltaTime * 50);
        yRot = Mathf.Lerp(yRot, dyRot, Time.deltaTime * 50);

        transform.rotation = Quaternion.Euler(0, yRot, 0);
        cam.localRotation = Quaternion.Euler(xRot, 0, 0);








        float my = movement.y;
        movement = (transform.forward * Input.GetAxis("Vertical") + transform.right * Input.GetAxis("Horizontal")) * speed;
        movement.y = my;


        if (controller.isGrounded)
        {
            movement.y = 0;
            if (Input.GetButton("Jump"))
            {
                movement.y = jumpForce;
            }
        }



        movement.y -= gravityMultiplier * Time.deltaTime;
        controller.Move(Time.deltaTime * movement);
    }

    public void CameraLook(Vector3 position)
    {
        cmhelper.transform.LookAt(position);
        dyRot -= camMotion.y * 70;
        dxRot -= camMotion.x * 70;

    }

}
