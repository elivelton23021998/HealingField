using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoundManager : MonoBehaviour
{
    public static RoundManager Instance { get; private set; }
    private NpcSpawnManager spawnManager;
    private ChairsAndBedsControlSlots slotsControl;

    #region VARIÁVEIS: GRIDS E COISAS DOS SETORES
    [HideInInspector] public string queue = "queue", chair = "chair", bed = "bed";

    [SerializeField] public QueueGridRectTransform queueGridRect;
    public bool[] queueOccupation;
    private bool queueFull = false;

    public ChairGridRectTransform instaceChair;
    public bool[] chairsOccupation;
    private bool chairsFull = false;

    private List<GameObject> inactiveNpcs = new List<GameObject>();

    [SerializeField] private float bedOffsetX;
    [SerializeField] private float bedOffsetY;
    private BedGridRectTransform bedInstance;
    private bool[] bedOccupation;
    private bool bedFull = false;
    #endregion
    
    private int[] npcsPerRound;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        slotsControl = GetComponent<ChairsAndBedsControlSlots>();

        AlocateNpcsPerRound(CampaignManager.instance.npcPerRound);

        spawnManager = GetComponent<NpcSpawnManager>();

        instaceChair = ChairGridRectTransform.Instance;
        bedInstance = BedGridRectTransform.Instance;

        queueOccupation = new bool[queueGridRect.queueSlots.Length];
        chairsOccupation = new bool[slotsControl.currentChairsEnable];
        bedOccupation = new bool[slotsControl.currentBedsEnable];
        
        //SPAWN DE NPCS NA RODADA
        SpawnSystem();
    }
    
    //atualiza a lista de npcs por round
    public void AlocateNpcsPerRound(int[] npcsPerRound)
    {
        this.npcsPerRound = npcsPerRound;
    }

    private void SpawnSystem()
    {
        //for (int i = 0; i < 20; i++)
        for (int i = 0; i < npcsPerRound[CampaignManager.instance.currentDay - 1]; i++)
        {
            spawnManager.NpcSpawn();
        }
    }

    public void QueueAdd(GameObject npc)
    {
        if (queueFull)
        {
            inactiveNpcs.Add(npc);
            npc.SetActive(false);
        }
        else
        {
            for (int i = 1; i < queueOccupation.Length; i++)
            {
                if (!queueOccupation[i])
                {
                    npc.transform.SetParent(queueGridRect.queueSlots[i].transform);
                    npc.transform.localPosition = queueGridRect.queueSlots[i].rect.center;
                    queueOccupation[i] = true;
                    npc.GetComponent<Pacient>().currentSectorPosition = i;

                    if (i == (queueOccupation.Length - 1))
                        queueFull = true;
                    return;
                }
            }
        }
    }
    
    public void UpdateQueueStatus()
    {
        for (int i = 0; i < queueOccupation.Length; i++)
        {
            if (queueOccupation[i] == false)
                queueFull = false;
        }

        if (inactiveNpcs.Count != 0)
        {
            GameObject nextNpcQueue = inactiveNpcs[0];
            nextNpcQueue.SetActive(true);
            QueueAdd(nextNpcQueue);
            inactiveNpcs.RemoveAt(0);
        }
    }

    public void ChairAdd(GameObject npc)
    {
        if (!chairsFull)
        {
            if (chairsOccupation != null)
                Debug.Log(chairsOccupation.Length.ToString());
            for (int i = 1; i < chairsOccupation.Length; i++)
            {
                if (!chairsOccupation[i])
                {
                    npc.transform.SetParent(instaceChair.chairSlots[i].transform);
                    npc.transform.localPosition = instaceChair.chairSlots[i].rect.center;
                    chairsOccupation[i] = true;

                    Pacient pacient = npc.GetComponent<Pacient>();
                    queueOccupation[pacient.currentSectorPosition] = false;
                    pacient.currentSectorPosition = i;
                    pacient.sector = chair;
                    
                    UpdateQueueStatus();

                    if (i == (chairsOccupation.Length - 1))
                        chairsFull = true;

                    InformationUI.Instance.CloseInfoWindow();

                    npc.GetComponent<PacientHealthSystem>().IncreseLifetime(); //aumenta o tempo do paciente nas cadeiras
                    return;
                }
            }
        }
        //      <<<<<IMPORTANTE>>>>>>>>  
        //Depois fazer um retorno aqui para que caso estejam cheias as cadeiras,  
        //o jogador receba alguma informação sobre isso
    }
    
    public void UpdateChairStatus()
    {
        for (int i = 0; i < chairsOccupation.Length; i++)
        {
            if (chairsOccupation[i] == false)
                chairsFull = false;
        }
    }

    public void BedAdd(GameObject npc)
    {
        if (!bedFull)
        {
            for (int i = 1; i < bedOccupation.Length; i++)
            {
                if (!bedOccupation[i])
                {
                    UnityEngine.Events.UnityAction eventAction = () => { npc.GetComponent<PacientUiUpdater>().UpdateUI(); };
                    bedInstance.bedSlots[i].GetComponentInChildren<Button>().onClick.RemoveAllListeners();
                    bedInstance.bedSlots[i].GetComponentInChildren<Button>().onClick.AddListener(eventAction);

                    npc.transform.SetParent(bedInstance.bedSlots[i].transform);
                    npc.transform.localPosition = new Vector2((bedInstance.bedSlots[i].rect.x / 2) + bedOffsetX, (bedInstance.bedSlots[i].rect.y / 2) + bedOffsetY);

                    RectTransform[] mask = bedInstance.bedSlots[i].GetComponentsInChildren<RectTransform>();

                    if (mask[1].name == "Mask")
                        npc.transform.SetParent(mask[1].transform);

                    bedOccupation[i] = true;

                    Pacient pacient = npc.GetComponent<Pacient>();
                    chairsOccupation[pacient.currentSectorPosition] = false;
                    pacient.currentSectorPosition = i;
                    pacient.sector = bed;

                    UpdateChairStatus();

                    if (i == (bedOccupation.Length - 1))
                        bedFull = true;

                    InformationUI.Instance.CloseInfoWindow();
                    
                    return;
                }
            }
        }
        //      <<<<<IMPORTANTE>>>>>>>>  
        //Depois fazer um retorno aqui para que caso estejam cheias as cadeiras,  
        //o jogador receba alguma informação sobre isso
    }
    
    public void UpdateBedStatus()
    {
        for (int i = 0; i < bedOccupation.Length; i++)
        {
            if (bedOccupation[i] == false)
                bedFull = false;
        }
    }

    public void RemoveFromSectorPosition(string sector, int npcPosition)
    {
        switch (sector)
        {
            case "queue":
                queueOccupation[npcPosition] = false;
                break;
            case "chair":
                chairsOccupation[npcPosition] = false;
                break;
            case "bed":
                bedOccupation[npcPosition] = false;
                break;
        }
    }
    
}
