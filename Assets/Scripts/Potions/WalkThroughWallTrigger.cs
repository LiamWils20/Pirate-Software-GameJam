using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkThroughWallTrigger : MonoBehaviour
{
    [SerializeField] int numberOfTimesEntered;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if(numberOfTimesEntered < 2)
            {
                numberOfTimesEntered++;
            }
            else if (numberOfTimesEntered == 2)
            {
                PotionsManager.instance.GetComponent<ByeByePotion>().ResetPotion();
            }
        }
    }
}
