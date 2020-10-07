using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [Header("Set in Inspector")]
    //Unusual but handy use of Vector2's. X holds a min value and y holds a max value for Random.Range to be used later.
    public Vector2 rotMinMax = new Vector2(15, 90);
    public Vector2 driftMinMax = new Vector2(.25f, 2);
    public float lifeTime = 6f;                         //seconds the PowerUp exists
    public float fadeTime = 4f;                         //seconds the PowerUp f a d e s

    [Header("Set Dynamically")]
    public WeaponType type;         //type of PowerUp
    public GameObject cube;         //Reference to Cube child
    public TextMesh letter;         //Reference to TextMesh
    public Vector3 rotPerSecond;    //Euler rotation speed
    public float birthTime;

    private Rigidbody rigid;
    private BoundsCheck bndCheck;
    private Renderer cubeRend;

    void Awake() {
        //find Cube reference
        cube = transform.Find("Cube").gameObject;

        //find TextMesh and other components
        letter = GetComponent<TextMesh>();
        rigid = GetComponent<Rigidbody>();
        bndCheck = GetComponent<BoundsCheck>();
        cubeRend = cube.GetComponent<Renderer>();

        //set a random velocity
        Vector3 vel = Random.onUnitSphere;
        //Random.onUnitSphere gives a vector point somewhere on surface of sphere w/ radius 1m, about the origin
        vel.z = 0;          //flatter velocity to XY plane
        vel.Normalize();    //Normalizing the Vector3 makes it length 1m

        vel *= Random.Range(driftMinMax.x, driftMinMax.y);
        rigid.velocity = vel;

        //Set rotation of this GameObject to R:[0,0,0]
        transform.rotation = Quaternion.identity;
        //similar to the Identiy Matrix or other Identity properties: add this to rotation and it stays the same
        //Quaternion.identity is equivalent to no rotation.

        //Set up rotPerSecond for Cube child using rotMinMax x and y
        rotPerSecond = new Vector3(Random.Range(rotMinMax.x, rotMinMax.y),
                                   Random.Range(rotMinMax.x, rotMinMax.y),
                                   Random.Range(rotMinMax.x, rotMinMax.y));
        birthTime = Time.time;
    }

    void Update() {
        cube.transform.rotation = Quaternion.Euler(rotPerSecond * Time.time);

        //Fade out the PowerUp over time
        //Given default values, PowerUp exists for 10 seconds
        //and fades out over 4 seconds.
        float u = (Time.time - (birthTime + lifeTime)) / fadeTime;
        //For lifeTime seconds, u will be less than or equal to 0.
        //Then, it'll transition to 1 over <fadeTime> seconds.

        //If u >= 1, destroy this PowerUp
        if(u >= 1) {
            Destroy(this.gameObject);
            return;
        }

        //Use u to determine the alpha value of the Cube and Letter
        if(u > 0) {
            Color c = cubeRend.material.color;
            c.a = 1f - u;
            cubeRend.material.color = c;

            //fade Letter too, just not as much.
            c = letter.color;
            c.a = 1f - (u * 0.5f);
            letter.color = c;
        }

        if(!bndCheck.isOnScreen) {
            //if the PowerUp has drifted entirely off screen, destroy it
            Destroy(gameObject);
        }
    }

    public void SetType(WeaponType wt) {
        //Grab the WeaponDefinition from Main
        WeaponDefinition def = Main.GetWeaponDefinition(wt);

        //Set the color of the Cube child
        cubeRend.material.color = def.color;

        //letter.color = def.color;     //we could colorize the letter too
        letter.text = def.letter;       //set the ketter that is shown
        type = wt;                      //Actually set the type
    }

    public void AbsorbedBy(GameObject target) {
        //This function is called by the Hero when a PowerUp is collected
        //Could tween into target and shrink in size, but currently destroy this.gameObject
        Destroy(this.gameObject);
    }
}
