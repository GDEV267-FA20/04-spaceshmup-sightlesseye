﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is an enum of the various possible weapon types.
/// It also includes a "shield" type to allow a shield power-up.
/// Items marked [NI] below are Not Implemented in the IGDPD book.
/// </summary>


public enum WeaponType
{
    none,       // The default / no weapon
    blaster,    // A simple blaster
    spread,     // Two shots simultaneously
    phaser,     // [NI] Shots that move in waves
    missile,    // [NI] Homing missiles
    laser,      // [NI] Damage over time
    shield      // Raise shieldLevel
}

/// <summary>
/// The WeaponDefinition class allows you to set the properties
/// of a specific weapon in the Inspector. The Main class has
/// an array of WeaponDefinitions that makes this possible.
/// </summary>

[System.Serializable]

public class WeaponDefinition {
    public WeaponType type = WeaponType.none;
    public string letter;               //Letter to show on powerup
    public Color color = Color.white;   //Color of Collar and powerup
    public GameObject projectilePrefab; //Prefab for Projectiles
    public Color projectileColor = Color.white;
    public float damageOnHit = 0;       //Damage caused per hit
    public float continuousDamage = 0;  //damage per second of laser
    public float delayBetweenShots = 0;
    public float velocity = 20;         //Speed of projectiles

}

public class Weapon : MonoBehaviour
{
    static public Transform PROJECTILE_ANCHOR;

    [Header("Set Dynamically")]
    [SerializeField]
    private WeaponType typeValue = WeaponType.none;
    public WeaponDefinition def;
    public GameObject collar;
    public float lastShotTime; //time when last shot was fired

    private Renderer collarRend;

    // Start is called before the first frame update
    void Start() {
        collar = transform.Find("Collar").gameObject;
        collarRend = collar.GetComponent<Renderer>();

        //call SetType() for the default typeValue of WeaponType.none
        SetType(typeValue);

        //Dynamically create an anchor for all Projectiles
        if(PROJECTILE_ANCHOR == null) {
            GameObject go = new GameObject("ProjectileAnchor");
            PROJECTILE_ANCHOR = go.transform;
        }
        //Find fireDelegate of root GameObject
        GameObject rootGO = transform.root.gameObject;
        if(rootGO.GetComponent<Hero>() != null) {
            rootGO.GetComponent<Hero>().fireDelegate += Fire;
        }
    }

    public WeaponType type {
        get { return (typeValue); }
        set { SetType (value); }
    }

    public void SetType(WeaponType wt) {
        typeValue = wt;
        if(type == WeaponType.none) {
            this.gameObject.SetActive(false);
            return;
        } else {
            this.gameObject.SetActive(true);
        }
        def = Main.GetWeaponDefinition(typeValue);
        collarRend.material.color = def.color;
        lastShotTime = 0;           //Can fire immediately after typeValue is set
    }

    public void Fire() {
        //if this.gameObject is inactive, return
        if (!gameObject.activeInHierarchy) return;

        //if it hasn't been enough time between shots, return
        if(Time.time - lastShotTime < def.delayBetweenShots) {
            return;
        }

        Projectile p;
        Vector3 vel = Vector3.up * def.velocity;
        if(transform.up.y < 0) {
            vel.y = -vel.y;
        }
        switch (type) {
            case WeaponType.blaster:
                p = MakeProjectile();
                p.rigid.velocity = vel;
                break;

            case WeaponType.spread:
                p = MakeProjectile();       //middle projectile
                p.rigid.velocity = vel;
                p = MakeProjectile();       //right projectile
                p.transform.rotation = Quaternion.AngleAxis(10, Vector3.back);
                p.rigid.velocity = p.transform.rotation * vel;
                p = MakeProjectile();       //left projectile
                p.transform.rotation = Quaternion.AngleAxis(-10, Vector3.back);
                p.rigid.velocity = p.transform.rotation * vel;
                break;
        }
    }

    public Projectile MakeProjectile() {
        GameObject go = Instantiate<GameObject>(def.projectilePrefab);
        if (transform.parent.gameObject.tag == "Hero") {
            go.tag = "ProjectileHero";
            go.layer = LayerMask.NameToLayer("ProjectileHero");
        } else {
            go.tag = "ProjetileEnemy";
            go.layer = LayerMask.NameToLayer("ProjectileEnemy");
        }
        go.transform.position = collar.transform.position;
        go.transform.SetParent(PROJECTILE_ANCHOR, true);
        Projectile p = go.GetComponent<Projectile>();
        p.type = type;
        lastShotTime = Time.time;
        return (p);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
