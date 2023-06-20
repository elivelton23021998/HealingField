using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Infographic : MonoBehaviour
{
    private SimpleSlideShow slideShow;
    [SerializeField] List<GameObject> sliderElements;
    public int actualValue;
    [SerializeField] GameObject btn_Next,btn_Prev;

    private void Start()
    {
        btn_Prev.SetActive(false);
        btn_Next.SetActive(true);
        actualValue = 0;
        slideShow = GetComponent<SimpleSlideShow>();
        slideShow.Initialize(Screen.width, Screen.height, sliderElements);
        
    }

    public void BtnNext()
    {
        if (actualValue < sliderElements.Capacity-1 && !slideShow.smoothMoveState)
        {
            slideShow.Next();
            actualValue++;
            if (actualValue == sliderElements.Capacity - 1)
            {
                if (btn_Next) btn_Next.SetActive(false);
            }
            else if (btn_Prev)
            {
                if (!btn_Prev.activeSelf) btn_Prev.SetActive(true);
            }
        }       
    }

    public void BtnPrevious()
    {
        if (actualValue > 0 && !slideShow.smoothMoveState)
        {
            slideShow.Previous();
            actualValue--;
            if (actualValue == 0)
            {
                if (btn_Prev) btn_Prev.SetActive(false);
            }
            else if (btn_Next)
            {
                if (!btn_Next.activeSelf) btn_Next.SetActive(true);
            }
        }
    }

    public void BtnBack()
    {
        actualValue = 0;
        btn_Prev.SetActive(false);
        btn_Next.SetActive(true);
        slideShow.Initialize(Screen.width, Screen.height, sliderElements);
    }

}
