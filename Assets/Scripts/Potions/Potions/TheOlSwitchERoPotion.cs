using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TheOlSwitchERoPotion : MonoBehaviour
{
    public int numberBlocksSelected;
    [SerializeField] List<GameObject> selectedBlocks = new List<GameObject>();
    public bool usingPotion;

    public List<Vector3> objectPos = new List<Vector3>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (numberBlocksSelected >= 2)
        {
            // Collecting the object positions.
            //objectOnePos = selectedBlocks[0].transform.position;
            //objectTwoPos = selectedBlocks[1].transform.position;

            // Setting the objects to their new locations
            //selectedBlocks[0].transform.position = objectPos[0];
            //selectedBlocks[1].transform.position = objectPos[1];

            for(int i = 0; i < selectedBlocks.Count; i++)
            {
                selectedBlocks[i].transform.position = objectPos[i];
                Debug.Log(objectPos[i]);
            }


            // Calling function which resets the potion
            //ResetPotion();
        }
    }

    public void PotionFunction(GameObject newObj)
    {
        numberBlocksSelected++;
        selectedBlocks.Add(newObj);
    }

    public void UpdateBool(bool t)
    {
        usingPotion = t;
    }

    public void ResetPotion()
    {
        UpdateBool(false);
        numberBlocksSelected = 0;
    }

}
