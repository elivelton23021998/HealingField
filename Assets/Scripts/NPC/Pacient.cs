using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pacient : MonoBehaviour
{
    [SerializeField] private GameObject barsGroup;
    [SerializeField] private float barOffset;

    [HideInInspector] public Image image;

    //Variáveis que receberão os dados do NpcSpawnManager
    [HideInInspector] public Sprite npcImage;
    [HideInInspector] public string characterName;
    [HideInInspector] public string plasmodiumName;
    [HideInInspector] public string[] symptoms;
    [HideInInspector] public float malariaIndex;
    [HideInInspector] public string isContaminated;
    [HideInInspector] public string complaint;

    [HideInInspector] public bool usingKit;

    private string _sector;
    public int currentSectorPosition { get; set; }

    public string sector
    {
        get { return _sector; }
        set
        {
            if (value == "queue" || value == "chair" || value == "bed")
                _sector = value;
            else
                _sector = "queue";
        }
    }
    
    void Start()
    {
        image = GetComponent<Image>();

        image.sprite = npcImage;
        image.SetNativeSize();
        image.rectTransform.localScale = new Vector3(.6f, .6f, 1f);

        barsGroup.transform.localPosition = new Vector2(image.sprite.rect.x / 2, (image.sprite.rect.yMax / 2) + barOffset);
    }
    
}
