using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncognitoPotion : MonoBehaviour
{
    [SerializeField] List<Light> levelLightSources = new List<Light>();
    [SerializeField] int timer;
    [SerializeField] bool canFindLight;

    // Update is called once per frame
    void Update()
    {
        if(levelLightSources != null)
        {
            canFindLight = true;
        }
        if (canFindLight)
        {
            for (int i = 0; i <= levelLightSources.Count; i++)
            {
                GameObject c = GameObject.FindGameObjectWithTag("LightSource");
                levelLightSources.Add(c.GetComponent<Light>());
            }
        }
        

        if (timer == 0 && InputHandler.instance.GetDrink())
        {
            timer = 1;
            Invoke(nameof(ResetTimer), 0.5f);
            
        }
    }
    void ResetTimer()
    {
        timer = 0;
    }
}
