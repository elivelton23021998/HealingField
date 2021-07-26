using UnityEngine;

public class AnimeSwitch : MonoBehaviour
{
    [HideInInspector] public int x;

    void Start()
    {
        x = Random.Range(1, 17);
        GetComponent<Animator>().Play(x.ToString());
    }
} 
