using UnityEngine;

public class BedGridRectTransform : MonoBehaviour
{
    public static BedGridRectTransform Instance { get; private set; }

    public RectTransform[] bedSlots { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        GameObject[] slots = GameObject.FindGameObjectsWithTag("BedSlot");

        bedSlots = new RectTransform[slots.Length];
        for (int i = 0; i < bedSlots.Length; i++)
        {
            bedSlots[i] = new RectTransform();
        }

        for (int i = 0; i < bedSlots.Length; i++)
        {
            bedSlots[i] = slots[i].GetComponent<RectTransform>();
        }
    }
}