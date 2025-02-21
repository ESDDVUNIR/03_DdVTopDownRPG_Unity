using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float explosionDelay = 5f;
    public float explosionRadius = 2f;
    public int explosionDamage = 50; 
    public LayerMask damageableLayers; 
    public GameObject explosionEffect;
    private bool hasExploded = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
