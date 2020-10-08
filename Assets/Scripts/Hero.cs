﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    static public Hero S; //Singleton

    [Header("Set in Inspector")]

    //Fields control the moevement of the ship
    public float speed = 30;
    public float rollMult = -30;
    public float pitchMult = 20;
    public float gameStartDelay = 2f;
    public GameObject projectilePrefab;
    public float projectileSpeed = 40;
    public Weapon[] weapons;

    [Header("Set Dynamically")]
    [SerializeField]
    private float shieldLevelScore = 1;

    //variable holds a reference to last triggering GameObject
    private GameObject lastTriggerGo = null;

    //Declares a new delegate type WeaponFireDelegate
    public delegate void WeaponFireDelegate();

    //Create a WeaponFireDelegate field named fireDelegate
    public WeaponFireDelegate fireDelegate;


    void Start() {
        if(S == null) {
            S = this;
        } else {
            Debug.LogError("Hero.Awake() - Attempted to assign second Hero.S!");
        }
        //fireDelegate += TempFire;

        //reset the Weapons to start Hero with 1 blaster
        ClearWeapons();
        weapons[0].SetType(WeaponType.blaster);
    }

    // Update is called once per frame
    void Update() {
        //Pull in info from Input class
        float xAxis = Input.GetAxis("Horizontal");
        float yAxis = Input.GetAxis("Vertical");

        //Change transform.poition based on axes
        Vector3 pos = transform.position;
        pos.x += xAxis * speed * Time.deltaTime;
        pos.y += yAxis * speed * Time.deltaTime;
        transform.position = pos;

        //Rotate ship to make it feel dynamic
        transform.rotation = Quaternion.Euler(yAxis * pitchMult, xAxis * rollMult, 0);

        //Now, we use the fireDelegate to fire Weapons
        //First, ensure button is pressed: Axis("Jump")
        //Then ensure that FireDelegate isn't null to avoid errors.
        if(Input.GetAxis("Jump") == 1 && fireDelegate != null) {
            fireDelegate();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Transform rootT = other.gameObject.transform.root;
        GameObject go = rootT.gameObject;
        //print("Triggered: " + go.name);

        //Make sure this triggering object isnt same as previous triggering GO
        if (go == lastTriggerGo)
        {
            return;
        }
        lastTriggerGo = go;
        if (go.tag == "Enemy")
        {                       //If shield was triggered by enemy, then...
            shieldLevel--;      //Decrease shield level, and...
            Destroy(go);        //destroy the enemy
        }
        else if (go.tag == "PowerUp") {
            //if shield was triggered by a PowerUp
            AbsorbPowerUp(go);
        } else {
            print("Triggered by non-Enemy: " + go.name);
        }
    }

    public void AbsorbPowerUp(GameObject go) {
        PowerUp pu = go.GetComponent<PowerUp>();
        switch (pu.type) {
            case WeaponType.shield:
                shieldLevel++;
                break;

            default:
                if(pu.type == weapons[0].type) {        //If it is the same type
                    Weapon w = GetEmptyWeaponSlot();
                    if(w != null) {
                        //Set it to pu.type
                        w.SetType(pu.type);
                    } 
                } else {                                //If it is the same type
                    ClearWeapons();
                    weapons[0].SetType(pu.type);
                }
                break;
        }
        pu.AbsorbedBy(this.gameObject);
    }

    public float shieldLevel {
        get {
            return shieldLevelScore;
        }
        set {
            shieldLevelScore = Mathf.Min(value, 4);
            //if shield is to be set to less than 0
            if(value < 0) {
                Destroy(this.gameObject);

                //tell Main.S to restart game after delay
                Main.S.DelayedRestart(gameStartDelay);
            }
        }
    }

    Weapon GetEmptyWeaponSlot() {
        for(int i = 0; i < weapons.Length; i++) {
            if(weapons[i].type == WeaponType.none) {
                return (weapons[i]);
            }
        }
        return null;
    }

    void ClearWeapons() {
        foreach (Weapon w in weapons) {
            w.SetType(WeaponType.none);
        }
    }
}
