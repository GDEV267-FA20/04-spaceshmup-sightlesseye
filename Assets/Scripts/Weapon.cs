using System.Collections;
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
    //Supposedly, this'll be filled in later.
    //¯\_(ツ)_/¯

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
