using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [Header("Set in Inspector")]
    public float rotationsPerSecond = 0.1f;

    [Header("Set Dynamically")]
    public int levelShown = 0;

    //non-public variable: it will not show in inspector
    Material mat;
    
    // Start is called before the first frame update
    void Start() {
        mat = GetComponent<Renderer>().material; ;
    }

    // Update is called once per frame
    void Update() {
        //Read current shield level from Hero Singleton
        int currLevel = Mathf.FloorToInt(Hero.S.shieldLevel);
        
        //If different from LevelShown
        if(levelShown != currLevel) {
            levelShown = currLevel;

            //Adjust texture offset to show shield level
            mat.mainTextureOffset = new Vector2(0.2f * levelShown, 0);
        }


        //rotate shield every frame based off of time
        float rZ = -(rotationsPerSecond * Time.time * 360) % 360f;
        transform.rotation = Quaternion.Euler(0, 0, rZ);
    }
}
