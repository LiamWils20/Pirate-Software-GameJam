using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeButton : MonoBehaviour
{
    TheOlSwitchERoPotion script;
    [SerializeField] int num;
    [SerializeField] bool hasDrankPotion;

    void Start()
    {
        script = PotionsManager.instance.GetComponent<TheOlSwitchERoPotion>();   
    }

    void Update()
    {
        num = script.numberBlocksSelected;
        hasDrankPotion = script.usingPotion;
    }

    private void OnMouseUpAsButton()
    {
        if (hasDrankPotion)
        {
            if (num == 0)
            {
                script.PotionFunction(gameObject);
                script.objectPos.Add(gameObject.transform.position);
            }
            else if (num == 1)
            {
                script.PotionFunction(gameObject);
                script.objectPos.Add(gameObject.transform.position);
            }
        }
    }
}
