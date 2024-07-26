using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ByeByePotion : MonoBehaviour
{
    [SerializeField] GameObject walkThroughWall;
    [SerializeField] GameObject player;
    [SerializeField] GameObject shadowPlayerPrefab;
    [SerializeField] GameObject spawnedShadowPlayer;
    [SerializeField] bool usingPotion;
    [SerializeField] int timer;

    // Update is called once per frame
    void Update()
    {
        if (timer == 0 && usingPotion)
        {
            timer = 1;
            Invoke(nameof(ResetTimer), 0.5f);
            // Setting "HasPotion" bool to false, as potion is drank
            PotionsManager.instance.UpdateHasPotion(false);
            // Finding the wall object, as this script is spawned
            walkThroughWall = GameObject.Find("WalkThroughWall");
            // Finding original Player Object
            player = GameObject.Find("Player");
            
            // Setting wall to be trigger
            walkThroughWall.GetComponent<BoxCollider>().isTrigger = true;
            // Setting player to trigger, so shadow can walk through player
            player.GetComponent<BoxCollider>().isTrigger = true;
            // Setting gravity to false, so player doesn't phase through the
            // floor, when it's set to a trigger
            player.GetComponent<Rigidbody>().useGravity = false;
            // Setting player script to be false
            player.GetComponent<Player>().enabled = false;

            // Spawning the shadow varient of the player
            spawnedShadowPlayer = Instantiate(shadowPlayerPrefab);
            spawnedShadowPlayer.transform.position = new Vector3 (player.transform.position.x + 2, player.transform.position.y, player.transform.position.z);
            spawnedShadowPlayer.transform.rotation = player.transform.rotation;
        }
    }
    void ResetTimer()
    {
        timer = 0;
    }

    public void UpdateBool(bool t)
    {
        usingPotion = t;
    }
}
