using UnityEngine;

public class AnimeProfile : MonoBehaviour
{
    public GameObject npc;
   
    void Update()
    {
        //faz com que a msm animacao que foi feita na na fila seja apresentada no quadro
        GetComponent<Animator>().Play(npc.GetComponent<InformationUI>().animeRef.ToString());
    }
}
