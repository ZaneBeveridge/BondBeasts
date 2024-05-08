using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeManager : MonoBehaviour
{
    public GameObject explodeParticle;
    public Transform spawnLocation;

    public void Explode()
    {
        GameObject obj = Instantiate(explodeParticle, spawnLocation);
    }
}
