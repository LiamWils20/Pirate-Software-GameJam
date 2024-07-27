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
        if (numberBlocksSelected == 2)
        {
            // Setting the objects to their new locations
            selectedBlocks[0].transform.position = objectPos[1];
            selectedBlocks[1].transform.position = objectPos[0];

            ResetPotion();
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
        gameObject.AddComponent<TheOlSwitchERoPotion>();
        Destroy(this);
    }

}
