using UnityEngine;

public class Normalizar : MonoBehaviour
{
    public GameObject cena;
    
    void Update()
    {
        if (cena.activeSelf)
        {
            GetComponent<Animator>().Play("Normalizar");
        }
        else
        {
            GetComponent<Animator>().Play("Clarear");
        }
    }
}
