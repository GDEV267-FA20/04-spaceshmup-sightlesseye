using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : Enemy0
{
    [Header("Set in Inspector: Enemy1")]
    //num of seconds for a full sine wave
    public float waveFrequency = 2;
    //sine wave width in meters
    public float waveWidth = 4;
    public float waveRotY = 45;

    private float x0; //Initial x value of pos
    private float birthTime;

    void Start() {
        //set x0 to initial x pos of Enemy1
        x0 = pos.x;
        birthTime = Time.time;
    }

    //Override Move Function on Enemy
    public override void Move() {
        //pos is proerty, so cannot directly set pos.x
        //get pos as editable Vector3
        Vector3 tempPos = pos;

        //theta adjusts based on time
        float age = Time.time - birthTime;
        float theta = Mathf.PI * 2 * age / waveFrequency;
        float sin = Mathf.Sin(theta);
        tempPos.x = x0 + waveWidth * sin;
        pos = tempPos;

        //rotate a tad around y
        Vector3 rot = new Vector3(0, sin * waveRotY, 0);
        this.transform.rotation = Quaternion.Euler(rot);

        //base.Move() still handles movement in y axis
        base.Move();

        //print(bndCheck.isOnScreen);
    }
}
