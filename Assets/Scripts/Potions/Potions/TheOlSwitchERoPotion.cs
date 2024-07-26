using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TheOlSwitchERoPotion : MonoBehaviour
{
    public int numberBlocksSelected;
    [SerializeField] List<GameObject> selectedBlocks = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void IncreaseNumberBlocksSelected()
    {
        numberBlocksSelected++;
    }

    public void UpdateSelectedBlocks(GameObject newObj)
    {
        selectedBlocks.Add(newObj);
    }
}
