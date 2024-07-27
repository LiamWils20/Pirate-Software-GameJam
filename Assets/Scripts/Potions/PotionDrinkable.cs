using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PotionDrinkable : MonoBehaviour
{
    [SerializeField] Button byeByePotionBtn;
    [SerializeField] Button incognitoPotionBtn;
    [SerializeField] Button theOlSwitchERooPotionBtn;

    private void Awake()
    {
        byeByePotionBtn.onClick.AddListener(() =>
        {
            PotionsManager.instance.GetComponent<ByeByePotion>().UpdateBool(true);
        });

        incognitoPotionBtn.onClick.AddListener(() =>
        {
            PotionsManager.instance.GetComponent<IncognitoPotion>().UpdateBool(true);
        });

        theOlSwitchERooPotionBtn.onClick.AddListener(() =>
        {
            PotionsManager.instance.GetComponent<TheOlSwitchERoPotion>().UpdateBool(true);
        });
    }
}
