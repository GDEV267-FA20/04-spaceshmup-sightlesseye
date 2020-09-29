﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Set in Inspector: Enemy")]
    public float speed = 10f;
    public float fireRate = 0.3f;
    public float health = 10;
    public int score = 100;

    private BoundsCheck bndCheck;

    //This is a Property: a method that acts as a field
    public Vector3 pos {
        get {
            return (this.transform.position);
        }
        set {
            this.transform.position = value;
        }
    }

    void Awake() {
        bndCheck = GetComponent<BoundsCheck>();
    }

    // Update is called once per frame
    void Update() {
        Move();
        if(bndCheck != null && bndCheck.offDown) {
            // object must be off bottom of screen
            if(pos.y < bndCheck.camHeight - bndCheck.radius) {
                Destroy(gameObject);
            }
        }
    }

    public virtual void Move() {
        Vector3 tempPos = pos;
        tempPos.y -= speed * Time.deltaTime;
        pos = tempPos;
    }
}