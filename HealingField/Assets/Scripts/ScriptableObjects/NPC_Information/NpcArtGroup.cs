using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = ("NPC Image Group"))]
public class NpcArtGroup : ScriptableObject
{

    [SerializeField] private Sprite[] npcImage;

    public Sprite[] NpcImage { get { return npcImage; } }

}
