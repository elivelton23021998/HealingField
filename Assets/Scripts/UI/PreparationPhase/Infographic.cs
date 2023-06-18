using System.Collections.Generic;
using UnityEngine;

public class Infographic : MonoBehaviour
{
    private SimpleSlideShow slideShow;
    [SerializeField] private List<GameObject> sliderElements;

    private void Start()
    {
        slideShow = GetComponent<SimpleSlideShow>();
        slideShow.Initialize(Screen.width, Screen.height, sliderElements);
    }

    public void BtnNext()
    {
        slideShow.Next();
    }

    public void BtnPrevious()
    {
        slideShow.Previous();
    }

    public void BtnBack()
    {
        slideShow.Initialize(Screen.width, Screen.height, sliderElements);
    }

}
