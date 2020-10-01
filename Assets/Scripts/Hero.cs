using System.Collections;
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

    [Header("Set Dynamically")]
    [SerializeField]
    private float shieldLevelScore = 1;

    //variable holds a reference to last triggering GameObject
    private GameObject lastTriggerGo = null;

    void Awake() {
        if(S == null) {
            S = this;
        } else {
            Debug.LogError("Hero.Awake() - Attempted to assign second Hero.S!");
        }
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
            Destroy(go);        //and destroy the enemy
        } else {
            print("Triggered by non-Enemy: " + go.name);
        }
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
}
