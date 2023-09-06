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


        itemPrfabs.Add("Fuse3F", GameManager.Resource.Load<GameObject>("SpecialItems/Fuse3F"));
        itemPrfabs.Add("Fuse2F_1", GameManager.Resource.Load<GameObject>("SpecialItems/Fuse2F_1"));
        itemPrfabs.Add("Fuse2F_3", GameManager.Resource.Load<GameObject>("SpecialItems/Fuse2F_2"));

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
