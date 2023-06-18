using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UiSwipeEffect
{
    
    public static void SwipeElements(GameObject[] objects)
    {
        for (int i = 0; i < objects.Length; i++)
        {
            if (i != objects.Length - 1)
            {
                float difference = objects[i + 1].transform.position.x - objects[i].transform.position.x;
                objects[i].transform.position -= new Vector3(difference, 0, 0);
            }  
            else
            {
                float difference =  objects[i].transform.position.x + objects[i - 1].transform.position.x;
                objects[i].transform.position -= new Vector3(difference, 0, 0);
            }
        }
    }

    public static void SwipeElements(Vector2 centralPosition, GameObject[] objects)
    {
        for (int i = 0; i < objects.Length; i++)
        {
            float diffence;
            if (i != objects.Length - 1)
            {
                diffence = objects[i].transform.position.x - objects[i + 1].transform.position.x;
            }
            else
            {
                diffence = objects[i].transform.position.x - objects[i - 1].transform.position.x;
            }
        }
    }

}
