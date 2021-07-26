using UnityEngine;

public class PacientUiUpdater : MonoBehaviour
{
    private Pacient pacient;
    
    private void Start()
    {
        pacient = GetComponent<Pacient>();
    }

    //método chamado ao clicar no paciente
    public void UpdateUI()
    {
        //Abre a interface da Ficha de Informações
        InformationUI.Instance.OpenInformationUI(gameObject, pacient.sector, pacient.characterName, pacient.plasmodiumName, pacient.isContaminated, pacient.malariaIndex.ToString(), pacient.complaint, pacient.symptoms);
    }
    
}
