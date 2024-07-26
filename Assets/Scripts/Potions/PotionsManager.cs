using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionsManager : MonoBehaviour
{
    public static PotionsManager instance {get; private set;}


    #region Potion Variables
    [SerializeField] bool hasPotion; // Used to check if the player has a potion.
    [SerializeField] int potionId; // Used to determin the potion which the player is to learn
    #endregion

    [SerializeField] GameObject[] potionBtns;
    [SerializeField] GameObject[] organisedPotionBtn;

    [SerializeField] GameObject player;
    
    [SerializeField] PotionTeacher potionTeacher;

    [SerializeField] int timer; // Used to ensure interact only happens once each time the key is pressed
    
    [SerializeField] float distance;

    private void Awake()
    {
        // Prevents multiple instances of this one script. 
        if(instance == null)
        {
            instance = this;
            potionBtns = GameObject.FindGameObjectsWithTag("PotionsBtn");
            foreach(GameObject p in potionBtns)
            {
                p.SetActive(false);
            }
            organisedPotionBtn[0] = potionBtns[2];
            organisedPotionBtn[1] = potionBtns[1];
            organisedPotionBtn[2] = potionBtns[0];
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        potionTeacher = GameObject.Find("PotionTeacher").GetComponent<PotionTeacher>();
    }

    private void Update()
    {
        // Grabs potion bools stored on the 'PotionTeacher' aka the book of spells
        potionId = potionTeacher.potionId;

        // Used to determin distance from book, so if player is close enough,
        // they can interact with book
        distance = Vector3.Distance(player.transform.position, potionTeacher.gameObject.transform.position);
        
        if(distance <= 3)
        {
            LearnPotion();
        }
    }

    void LearnPotion()
    {
        if (timer == 0 && InputHandler.instance.GetInteract() && !hasPotion)
        {
            if (potionId == 0)
            {
                gameObject.GetComponent<ByeByePotion>().enabled = true;
                organisedPotionBtn[0].SetActive(true);
                potionTeacher.potionId++;
                
            }
            else if (potionId == 1)
            {
                gameObject.GetComponent<IncognitoPotion>().enabled = true;
                organisedPotionBtn[1].SetActive(true);
                potionTeacher.potionId++;
                
            }
            else if (potionId == 2)
            {
                gameObject.GetComponent<TheOlSwitchERoPotion>().enabled = true;
                organisedPotionBtn[2].SetActive(true);
                potionTeacher.potionId = 0;
                
            }
            timer = 1;
            //hasPotion = true;
            Invoke(nameof(ResetTimer), 0.5f);
        }
    }

    public void UpdateHasPotion(bool newBool)
    {
        hasPotion = newBool;
    }

    void ResetTimer()
    {
        timer = 0;
    }

}
