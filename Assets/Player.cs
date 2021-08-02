﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Player : MonoBehaviour
{
    Animator animator;
    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        bulletLight = GetComponentInChildren<Light>(true).gameObject;
    }
    void Update()
    {
        if (Time.deltaTime == 0)
            return;
        LookAtMouse();
        Move();
        Fire();
    }
    Plane plane = new Plane(new Vector3(0, 1, 0), 0);

    private void LookAtMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(plane.Raycast(ray,out float enter))
        {
            Vector3 hitPoint = ray.GetPoint(enter);
            Vector3 dir = hitPoint - transform.position;
            dir.y = transform.position.y;
            dir.Normalize();
            transform.forward = dir;
        }
    }

    private void Move()
    {
        Vector3 move = Vector3.zero;
        if (Input.GetKey(KeyCode.W)) move.z = 1;
        if (Input.GetKey(KeyCode.S)) move.z = -1;
        if (Input.GetKey(KeyCode.A)) move.x = -1;
        if (Input.GetKey(KeyCode.D)) move.x = 1;
        if (move != Vector3.zero)
        {
            Vector3 relateMove;
            relateMove = Camera.main.transform.forward * move.z;
            relateMove += Camera.main.transform.right * move.x;
            relateMove.y = 0;
            move = relateMove;

            move.Normalize();
            transform.Translate(move * speed * Time.deltaTime, Space.World);
        }
            animator.SetFloat("DirX", move.x);
            animator.SetFloat("DirY", move.y);
            animator.SetFloat("Speed", move.sqrMagnitude);
    }
    public float speed = 5;
}
    