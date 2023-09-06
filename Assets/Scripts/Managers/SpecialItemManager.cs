using System.Collections.Generic;
using UnityEngine;

public class SpecialItemManager : MonoBehaviour
{

    public int maxSlots = 4;
    public string[] itemList; // Inventory ¿Í °øÀ¯

    //public List<GameObject> itemPrefabs = new List<GameObject>();


    public Dictionary<string,GameObject> itemPrfabs = new Dictionary<string,GameObject>();

    private void Awake()
    {
        itemList = new string[maxSlots];


        itemPrfabs.Add("Fuse1", GameManager.Resource.Load<GameObject>("SpecialItems/Fuse1"));
        itemPrfabs.Add("Fuse2", GameManager.Resource.Load<GameObject>("SpecialItems/Fuse2"));
        itemPrfabs.Add("Fuse3", GameManager.Resource.Load<GameObject>("SpecialItems/Fuse3"));

        itemPrfabs.Add("KeyCard1", GameManager.Resource.Load<GameObject>("SpecialItems/KeyCard1"));
    }


    public void Add(int index,string name) 
    {
        itemList[index] = name;
    }

    public void remove(int index)
    {
        itemList[index] = null;
    }

}
