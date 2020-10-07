using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : Enemy0
{
    [Header("Set in Inspector: Enemy_2")]
    //Determines how much Sine wave will effect movement: the easing
    public float sinEccentricity = 0.6f;
    public float lifeTime = 10;

    [Header("Set Dynamically: Enemy_2")]
    //Uses Sine wave to modify 2-point linear interpolation
    public Vector3 p0;
    public Vector3 p1;
    public float birthTime;

    void Start() {
        //Pick any point on left side of screen
        p0 = Vector3.zero;
        p0.x = -bndCheck.camWidth - bndCheck.radius;
        p0.y = Random.Range(-bndCheck.camHeight, bndCheck.camHeight);

        //Pick any point on right side of screen
        p1 = Vector3.zero;
        p1.x = bndCheck.camWidth + bndCheck.radius;
        p1.y = Random.Range(-bndCheck.camHeight, bndCheck.camHeight);

        //possibly swap sides?
        if(Random.value > 0.5f) {
            //Setting the .x of each point to its' negative will move it to other side of screen
            p0.x *= -1;
            p1.x *= -1;
        }

        //set birthTime to current time;
        birthTime = Time.time;
    }

    // Update is called once per frame
    public override void Update() {
        if (showingDamage && Time.time > damageDoneTime) {
            UnShowDamage();
        }

        //Bezier curves work based on u value between 0 and 1
        float u = (Time.time - birthTime) / lifeTime;

        //if u > 1, then it's been longer than lifeTime since birthTime
        if(u > 1) {
            //Enemy2 has finished its life. 
            //Gamers, can we get an F in chat?
            //print("F");
            Destroy(this.gameObject);
            return;
        }
        //adjust u by adding curve based on Sine wave
        u = u + sinEccentricity * (Mathf.Sin(u * Mathf.PI * 2));

        //Interpolate the two points
        pos = (1 - u) * p0 + u * p1;
    }
}
