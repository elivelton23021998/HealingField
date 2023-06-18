using UnityEngine;

public class QueueGridRectTransform : MonoBehaviour
{
    public RectTransform[] queueSlots { get; private set; }
    
    void Start()
    {
        queueSlots = gameObject.GetComponentsInChildren<RectTransform>();
    }
}
