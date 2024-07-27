using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionsManager : MonoBehaviour
{
    public static PotionsManager instance {get; private set;}


    #region Potion Variables
    [SerializeField] int potionId; // Used to determin the potion which the player is to learn
    #endregion

    [SerializeField] GameObject potionBtnsParent;
    [SerializeField] Transform[] potionBtns;

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
            potionBtnsParent = GameObject.FindGameObjectWithTag("PotionsBtn");
            potionBtns[0] = potionBtnsParent.transform.GetChild(0);
            potionBtns[1] = potionBtnsParent.transform.GetChild(1);
            potionBtns[2] = potionBtnsParent.transform.GetChild(2);
            foreach(Transform p in potionBtns)
            {
                p.gameObject.SetActive(false);
            }
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
        if (timer == 0 && InputHandler.instance.GetInteract())
        {
            if (potionId == 0)
            {
                gameObject.GetComponent<ByeByePotion>().enabled = true;
                potionBtns[0].gameObject.SetActive(true);
                potionTeacher.potionId++;
            }
            else if (potionId == 1)
            {
                gameObject.GetComponent<IncognitoPotion>().enabled = true;
                potionBtns[1].gameObject.SetActive(true);
                potionTeacher.potionId++;
            }
            else if (potionId == 2)
            {
                gameObject.GetComponent<TheOlSwitchERoPotion>().enabled = true;
                potionBtns[2].gameObject.SetActive(true);
                potionTeacher.potionId = 0;
                
            }
            timer = 1;
            Invoke(nameof(ResetTimer), 0.5f);
        }
    }

    void ResetTimer()
    {
        timer = 0;
    }

}
