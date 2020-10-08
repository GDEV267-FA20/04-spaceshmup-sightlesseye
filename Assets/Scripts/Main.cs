using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; //loading and reloading scenes

public class Main : MonoBehaviour
{
    static public Main S;

    static Dictionary<WeaponType, WeaponDefinition> WEAP_DICT;

    [Header("Set in Inspector")]
    public GameObject[] prefabEnemies;
    public float enemySpawnPerSecond = 0.5f;
    public float enemyDefaultPadding = 1.5f;

    public WeaponDefinition[] weaponDefinitions;

    public GameObject prefabPowerUp;  
    public WeaponType[] powerUpFrequency = new WeaponType[] {       
                                          WeaponType.blaster, WeaponType.blaster,
                                          WeaponType.spread,  WeaponType.shield };

    private BoundsCheck bndCheck;

    public void ShipDestroyed(Enemy0 e)
    {   
        // Potentially generate a PowerUp
        if (Random.value <= e.powerUpDropChance) {   
            // Choose which PowerUp to pick
            // Pick one from the possibilities in powerUpFrequency
            int ndx = Random.Range(0, powerUpFrequency.Length);
            WeaponType puType = powerUpFrequency[ndx];

            // Spawn a PowerUp
            GameObject go = Instantiate(prefabPowerUp) as GameObject;
            PowerUp pu = go.GetComponent<PowerUp>();

            // Set it to the proper WeaponType
            pu.SetType(puType);

            // Set it to the position of the destroyed ship
            pu.transform.position = e.transform.position;
        }
    }

    void Awake() {
        S = this;

        //bndCheck references BoundsCheck component on this GameObject
        bndCheck = GetComponent<BoundsCheck>();

        // Calls SpawnEnemy() method once every <variable> seconds
        Invoke("SpawnEnemy", 1f / enemySpawnPerSecond);

        //Generic Dictionary with WeaponType as key
        WEAP_DICT = new Dictionary<WeaponType, WeaponDefinition>();
        foreach(WeaponDefinition def in weaponDefinitions) {
            WEAP_DICT[def.type] = def;
        }
    }

    public void SpawnEnemy() {
        //pick random prefab to instantiate
        int ndx = Random.Range(0, prefabEnemies.Length);
        GameObject go = Instantiate<GameObject>(prefabEnemies[ndx]);

        //Position enemy above screen at random x position
        float enemyPadding = enemyDefaultPadding;
        if(go.GetComponent<BoundsCheck>() != null) {
            enemyPadding = Mathf.Abs(go.GetComponent<BoundsCheck>().radius);
        }

        //Set initial position for spawned enemy
        Vector3 pos = Vector3.zero;
        float xMin = -bndCheck.camWidth + enemyPadding;
        float xMax = bndCheck.camWidth - enemyPadding;
        pos.x = Random.Range(xMin, xMax);
        pos.y = bndCheck.camHeight + enemyPadding;
        go.transform.position = pos;

        //invoke SpawnEnemy() method again
        Invoke("SpawnEnemy", 1f / enemySpawnPerSecond);
    }

    public void DelayedRestart (float delay) {
        //invoke Restart() in delay seconds
        Invoke("Restart", delay);
    }

    public void Restart() {
        //reload scene to restart game
        SceneManager.LoadScene("Scene 0");
    }

    /// <summary>
    /// Static function that gets a WeaponDefinition from the WEAP_DICT static
    /// protected field of the Main class.
    /// </summary>

    /// <returns>
    /// The WeaponDefinition or, if there is no WeaponDefinition with
    /// the WeaponType passed in, returns a new WeaponDefinition with a
    /// WeaponType of none.
    /// </returns>

    /// <param name="wt"> 
    /// The WeaponType of the desired WeaponDefinition 
    /// </param>

    static public WeaponDefinition GetWeaponDefinition(WeaponType wt) {
        // Check to make sure that the key exists in the Dictionary.
        // Attempting to retrieve a key that didn't exist would throw an error,
        // so the following if statement is important.
        if(WEAP_DICT.ContainsKey(wt)) {
            return WEAP_DICT[wt];
        }

        //This returns a new WeaponDefinition with a type of WeaponType.none,
        //meaning it failed to find the right WeaponDefinition.
        return (new WeaponDefinition());
    }
}
