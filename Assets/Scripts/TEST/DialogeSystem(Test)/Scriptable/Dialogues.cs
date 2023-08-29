using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialogues", menuName = "Data/Dialogues")]
public class Dialogues : ScriptableObject
{
    [SerializeField] public List<DialogDatas> dialogue;

    [SerializeField] public List<DialogDatas> Dialog { get { return dialogue; } }

}

