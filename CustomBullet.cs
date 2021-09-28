using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomBullet : MonoBehaviour
{
    //Assignables
    public Rigidbody rb;   
    public GameObject explosion;
    public LayerMask WhatIsEnemies;

    //stats
    [Range(0f, 1f)]
    public float bounciness;
    public bool useGravity;

    //Damage
    public int explosionDamage;
    public float explosionRange;
    public float explosionForce;
    //lifetime
    public int maxCollision;
    public float maxLifeTime;
    public bool explodeOnTouch = true;

    int collisions;
    PhysicMaterial physics_mat;


    private void Start()
    {
        Setup();
    }
    private void Update()
    {

        // when to explode
        if (collisions > maxCollision) Explode();

        //count down lifetime
        maxLifeTime -= Time.deltaTime;
        if (maxLifeTime <= 0) Explode();
    }
    private void Explode()
    {
        //tehd‰‰n r‰j‰dys
        if (explosion != null) Instantiate(explosion, transform.position, Quaternion.identity);

        //cheak for …nemies
        Collider[] enemies = Physics.OverlapSphere(transform.position, explosionRange, WhatIsEnemies);
        for (int i = 0; i < enemies.Length; i++)
        {
            // hae enemy komponetti ja ja kutsu TakeDamege();

            //esimerkki
            //  enemies[i] = GetComponent<ShootingAi>().TakeDamage(exlosionDamage);

            //Add explosin force (if enemy has rigidbody)
            if (enemies[i].GetComponent<Rigidbody>())           
                enemies[i].GetComponent<Rigidbody>().AddExplosionForce(explosionForce, transform.position, explosionRange);
            
        
        }
        //Lis‰‰ viivett‰. Tuhoa luoti pienell‰ viiveell‰ jotta ei tule bugeja
        Invoke("Delay", 0.05f);
    }
    private void Delay()
    {
        Destroy(gameObject);
    }
    private void OnCollisionEnter(Collision collision)
    {
        //count up collision
        collisions++;
        //r‰j‰hd‰ jos osuu viholliseen suoraan sek‰ r‰j‰hd‰ kosketuksesta on p‰‰ll‰
        if (collision.collider.CompareTag("Enemy") && explodeOnTouch) Explode();
    }
    private void Setup()
    {
        //create new physic material
        physics_mat = new PhysicMaterial();
        physics_mat.bounciness = bounciness;
        physics_mat.frictionCombine = PhysicMaterialCombine.Minimum;
        physics_mat.bounceCombine = PhysicMaterialCombine.Maximum;
        //Assign material to collider
        GetComponent<SphereCollider>().material = physics_mat;

        //set gravity
        rb.useGravity = useGravity;

    }

    //piirt‰‰ r‰j‰dys alueen
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRange);
    }

}
