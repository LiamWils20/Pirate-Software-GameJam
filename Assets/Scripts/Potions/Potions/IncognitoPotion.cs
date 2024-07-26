using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncognitoPotion : MonoBehaviour
{
    [SerializeField] GameObject[] levelLightSources;
    [SerializeField] int timer;
    [SerializeField] bool usingPotion;

    // Update is called once per frame
    void Update()
    {
        if(levelLightSources.Length == 0)
        {
            levelLightSources = GameObject.FindGameObjectsWithTag("LightSource");
        }
        

        if (timer == 0 && usingPotion)
        {
            timer = 1;
            Invoke(nameof(ResetTimer), 0.5f);
            
            foreach(GameObject l in levelLightSources)
            {
                l.GetComponent<Light>().enabled = false;
            }
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
