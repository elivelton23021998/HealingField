using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiGameScene : MonoBehaviour
{
    public static UiGameScene instance;

    private SimpleSlideShow slideShow;
    [SerializeField] private List<GameObject> sliderElements;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        slideShow = GetComponent<SimpleSlideShow>();
        slideShow.Initialize(Screen.width, Screen.height, sliderElements);
    }

    public void EndDay()
    {
        EndDayUi.instance.EndDay();
    }
    
    public void BtnToBeds()
    {
        slideShow.Next();
    }

    public void BtnToChairs()
    {
        slideShow.Previous();
    }

}
