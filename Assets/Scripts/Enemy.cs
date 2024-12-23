using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    public int health;

    internal void TakeDamage(int damageToInflict)
    {
        health -= damageToInflict;
    }
}
