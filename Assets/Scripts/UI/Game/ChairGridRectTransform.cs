using UnityEngine;

public class ChairGridRectTransform : MonoBehaviour
{
    public static ChairGridRectTransform Instance { get; private set; }

    public RectTransform[] chairSlots { get; set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        chairSlots = new RectTransform[gameObject.GetComponentsInChildren<RectTransform>().Length];
        for (int i = 0; i < chairSlots.Length; i++)
        {
            chairSlots[i] = new RectTransform();
        }

        chairSlots = gameObject.GetComponentsInChildren<RectTransform>();
    }
}