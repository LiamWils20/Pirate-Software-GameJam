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
            // Finding the wall object, as this script is spawned
            walkThroughWall = GameObject.Find("WalkThroughWall");
            // Finding original Player Object
            player = GameObject.Find("Player");
            
            // Setting wall to be trigger
            walkThroughWall.GetComponent<BoxCollider>().isTrigger = true;
            // Setting player to trigger, so shadow can walk through player
           // player.GetComponent<BoxCollider>().isTrigger = true;
            // Setting gravity to false, so player doesn't phase through the
            // floor, when it's set to a trigger
            player.GetComponent<Rigidbody>().useGravity = false;
            // Setting player script to be false
            player.GetComponent<Player>().enabled = false;

            // Spawning the shadow varient of the player
            spawnedShadowPlayer = Instantiate(shadowPlayerPrefab);
            spawnedShadowPlayer.transform.position = player.transform.position;
            spawnedShadowPlayer.transform.rotation = player.transform.rotation;
        }
    }
    public void ResetPotion()
    {
        usingPotion = false;
        timer = 0;

        walkThroughWall.GetComponent<BoxCollider>().isTrigger = false;
        //player.GetComponent<BoxCollider>().isTrigger = false;
        player.GetComponent<Rigidbody>().useGravity = true;
        player.GetComponent<Player>().enabled = true;

        Destroy(spawnedShadowPlayer);
        

        //gameObject.AddComponent<ByeByePotion>();
        //Destroy(this);
    }

    public void UpdateBool(bool t)
    {
        usingPotion = t;
    }
}
