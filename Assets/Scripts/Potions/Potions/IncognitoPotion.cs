using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncognitoPotion : MonoBehaviour
{
    [SerializeField] float countDown;

    [SerializeField] GameObject[] levelLightSources;
    [SerializeField] int timer;
    [SerializeField] bool usingPotion;

    private void Start()
    {
        countDown = 15;
        levelLightSources = GameObject.FindGameObjectsWithTag("LightSource");
    }

    // Update is called once per frame
    void Update()
    {
        if(levelLightSources.Length <= 0)
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

            if(countDown > 0)
            {
                countDown -= 1f;
            }
            else
            {
                ResetPotion();
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


    void ResetPotion()
    {
        foreach (GameObject l in levelLightSources)
        {
            l.GetComponent<Light>().enabled = true;
        }

        gameObject.AddComponent<IncognitoPotion>();
        Destroy(this);
    }

}
