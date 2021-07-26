using UnityEngine;

public class ChairsAndBedsControlSlots : MonoBehaviour
{
    public static ChairsAndBedsControlSlots instance;
    
    private int totalChairs;
    private int totalBeds;

    private int totalChairsGameObject;
    private int totalBedsGameObject;

    public int currentChairsEnable { get; private set; }
    public int currentBedsEnable { get; private set; }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        
        DataManager instaceDataManager = DataManager.instance;
        UpdateCurrentSlots(instaceDataManager.enableChairs, instaceDataManager.enableBeds, CampaignManager.instance.totalChairs, CampaignManager.instance.totalBeds);
    }

    private void Start()
    {
        totalChairsGameObject = ChairGridRectTransform.Instance.chairSlots.Length;
        totalBedsGameObject = BedGridRectTransform.Instance.bedSlots.Length;
        
        for (int i = currentChairsEnable; i < totalChairs; i++)
        {
            ChairGridRectTransform.Instance.chairSlots[i].gameObject.SetActive(false);
        }

        for (int i = currentBedsEnable; i < totalBeds; i++)
        {
            BedGridRectTransform.Instance.bedSlots[i].gameObject.SetActive(false);
        }
    }

    public void UpdateCurrentSlots(int chairs, int beds, int totalChair, int totalBeds)
    {
        currentChairsEnable = chairs + 1;
        currentBedsEnable = beds + 1;

        totalChairs = totalChair + 1;
        this.totalBeds = totalBeds + 1;
    }
}
