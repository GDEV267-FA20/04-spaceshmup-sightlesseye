using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private BoundsCheck bndCheck;
    private Renderer rend;

    [Header("Set Dynamically")]
    public Rigidbody rigid;

    [SerializeField]
    private WeaponType typeValue;

    //public property masks field typeValue and takes action when set
    public WeaponType type {
        get {
            return typeValue;
        }
        set {
            SetType(value);
        }
    }

    void Awake() {
        bndCheck = GetComponent<BoundsCheck>();
        rend = GetComponent<Renderer>();
        rigid = GetComponent<Rigidbody>();
    }

    void Update() {
        if(bndCheck.offUp) {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Sets the _type private field and colors this projectile to match the
    /// WeaponDefinition.
    /// </summary>

    /// <param name="eType">
    /// The WeaponType to use.
    /// </param>

    public void SetType(WeaponType eType) {
        //set typeValue
        typeValue = eType;
        WeaponDefinition def = Main.GetWeaponDefinition(typeValue);
        rend.material.color = def.projectileColor;
    }
}
