using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HasteMoviment : MonoBehaviour
{
    [SerializeField] private GameObject act,haste;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Desce()
    {
        act.GetComponent<PreparationUI>().OpenPreparationWindow();
    }
    public void Sobe()
    {
        haste.GetComponent<Animator>().Play("ExitHaste");
    }
    public void Fecha()
    {
        act.GetComponent<PreparationUI>().ClosePreparationWindow();
    }
}
