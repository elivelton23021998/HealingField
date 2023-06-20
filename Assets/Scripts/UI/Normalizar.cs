using UnityEngine;
using UnityEngine.UI;

public class Normalizar : MonoBehaviour
{
    public GameObject cena;
    
    void Update()
    {
        if (cena.activeSelf)
        {
            GetComponent<Button>().interactable = false;
            GetComponent<Animator>().Play("Normalizar");
        }
        else
        {
            GetComponent<Button>().interactable = true;
            GetComponent<Animator>().Play("Clarear");
        }
    }
}
