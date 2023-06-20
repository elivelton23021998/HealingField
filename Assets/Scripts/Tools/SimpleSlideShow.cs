using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleSlideShow : MonoBehaviour
{
    private float screenWidth;
    private float screenHeight;
    private List<GameObject> elements = new List<GameObject>();

    [HideInInspector] public bool smoothMoveState = false;

    public void Initialize(float screenWidth, float screenHeight, List<GameObject> elements)
    {
        this.screenWidth = screenWidth;
        this.screenHeight = screenHeight;
        this.elements = elements;

        for (int i = 0; i < elements.Count; i++)
        {
            if (i == 0)
                elements[i].transform.position = new Vector3(screenWidth / 2, screenHeight / 2, 0f);
            else
                elements[i].transform.position = elements[i - 1].transform.position + new Vector3(screenWidth, 0f, 0f);
        }
    }

    public void Next()
    {
        SlideMovement(Vector3Int.right);
    }

    public void Previous()
    {
        SlideMovement(Vector3Int.left);
    }
    
    private void SlideMovement(Vector3Int xDirection)
    {
        if (!smoothMoveState)
        {
            for (int i = 0; i < elements.Count; i++)
            {
                Vector3 newPosition = Vector3.zero;
                if (xDirection.x > 0 && elements[elements.Count - 1].GetComponent<RectTransform>().anchoredPosition.x != 0)
                {
                    newPosition = elements[i].transform.position - (new Vector3(screenWidth, 0f, 0f) * xDirection.x);

                    StartCoroutine(SmoothMove(i, elements[i].transform.position, newPosition, .5f,true));
                }
                else if (xDirection.x < 0 && elements[0].GetComponent<RectTransform>().anchoredPosition.x != 0)
                {
                    newPosition = elements[(elements.Count - 1) - i].transform.position - (new Vector3(screenWidth, 0f, 0f) * xDirection.x);

                    StartCoroutine(SmoothMove((elements.Count - 1) - i, elements[(elements.Count - 1) - i].transform.position, newPosition, .5f,false));
                }
            }
        }
    }
    
    private IEnumerator SmoothMove(int elementIndex, Vector3 startPos, Vector3 endPos, float seconds, bool up)
    {
        smoothMoveState = true;
        float t = 0f;
        while (t <= 1)
        {
            t += Time.deltaTime / seconds;
            elements[elementIndex].transform.position = Vector3.Lerp(startPos, endPos, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }
        smoothMoveState = false;
    }

}
