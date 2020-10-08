using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy0 : MonoBehaviour
{
    [Header("Set in Inspector: Enemy")]
    public float speed = 10f;
    public float fireRate = 0.3f;
    public float health = 10;
    public int score = 100;
    public float showDamageDuration = 0.1f;     //num of seconds to show damage
    public float powerUpDropChance = 1f;        //Chance to drop a PowerUp

    [Header("Set Dynamically: Enemy")]
    public Color[] originalColors;
    public Material[] materials;                //All materials of this and its children
    public bool showingDamage = false;
    public float damageDoneTime;                //time to stop showing damage
    public bool notifiedOfDestruction = false;  //will be used later

    protected BoundsCheck bndCheck;

    //This is a Property: a method that acts as a field
    public Vector3 pos {
        get { return (this.transform.position); }
        set { this.transform.position = value; }
    }

    void Awake() {
        bndCheck = GetComponent<BoundsCheck>();

        //Get materials & colors for this GO and children
        materials = Utils.GetAllMaterials(gameObject);
        originalColors = new Color[materials.Length];
        for(int i = 0; i < materials.Length; i++) {
            originalColors[i] = materials[i].color;
        }
    }

    // Update is called once per frame
    public virtual void Update() {
        Move();

        if(showingDamage && Time.time > damageDoneTime) {
            UnShowDamage();
        }

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

    void OnCollisionEnter(Collision coll) {
        GameObject otherGO = coll.gameObject;
        switch(otherGO.tag) {
            case "ProjectileHero":
                Projectile p = otherGO.GetComponent<Projectile>();

                //If enemy is off screen, don't damage it.
                if(!bndCheck.isOnScreen) {
                    Destroy(otherGO);
                    break; 
                }
                //Hurt this enemy
                ShowDamage();
                
                //Get dmg amount from Main WEAP_DICT
                health -= Main.GetWeaponDefinition(p.type).damageOnHit;
                if(health <= 0) {
                    // Tell the Main singleton that this ship was destroyed
                    if (!notifiedOfDestruction) {
                        Main.S.ShipDestroyed(this);
                    }
                    notifiedOfDestruction = true;

                    //Destroy this enemy
                    Destroy(this.gameObject);
                }
                Destroy(otherGO);
                break;

            default:
                print("Enemy hit by non-ProjectileHero object: " + otherGO.name);
                break;
        }
    }

    void ShowDamage() {
        //print("grilled cheese");
        foreach(Material m in materials) {
            m.color = Color.white;
        }
        showingDamage = true;
        damageDoneTime = Time.time + showDamageDuration;
    }

     protected void UnShowDamage() {
        //print("regular cheese");
        for(int i = 0; i < materials.Length; i++) {
            materials[i].color = originalColors[i];
        }
        showingDamage = false;
    }
}
