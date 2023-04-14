using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveSpeedBoost : MonoBehaviour
{

    void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
        Abilities playerAbilities = other.GetComponent<Abilities>();
        if(!playerAbilities.BoostOnCD)
        {
            playerAbilities.StartSpeedBoost();
        }
        
    }
}
