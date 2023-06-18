using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activator : MonoBehaviour
{
    public void Ativar()
    {
        gameObject.SetActive(true);
    }
    public void Desativar()
    {
        gameObject.SetActive(false);
    }

    public void OffAnimation()
    {
        gameObject.GetComponent<Animator>().enabled = false;
    }
    public void Destroy()
    {
        Destroy(gameObject);
    }
}
