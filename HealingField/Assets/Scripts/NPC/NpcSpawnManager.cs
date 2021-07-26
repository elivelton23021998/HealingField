using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcSpawnManager : MonoBehaviour
{
    #region Attributes

    private SortitionTools sortitionTools;  //Script que contém algumas funções para facilitar o Ramdom entre intervalos de valores

    private Canvas canvas;
    private RectTransform rectTransform;
    
    #region NPC attributes
    [Header("NPC prefab")]
    [SerializeField] private GameObject npcPrefab;

    //Atributos necessários para setar os valores referentes aos NPCs
    [Header("NPC sprite groups")]
    [SerializeField] private NpcArtGroup[] artGroups;

    [Header("Symptoms attributes")]
    [SerializeField] private SymptomMasterData symptomMasterData;
    [SerializeField] private float[] plasmodiumSpawnRate;
    [SerializeField] private int numberSymptomsMin;
    [SerializeField] private int numberSymptomsMax;
    private float[] plasmodiumSpawnMinValueInterval;
    private float[] plasmodiumSpawnMaxValueInterval;
    private float[] tempValueForSymptomSortitonRangeMin;
    private float[] tempValueForSymptomSortitonRangeMax;
    private float[,] symptomSortitionMinValueInterval;
    private float[,] symptomSortitionMaxValueInterval;

    [Header("Complaints attributes")]
    [SerializeField] private string[] complaints;
    [SerializeField] private float[] complaintIndex;
    private float[] complaintIndexMinRange;
    private float[] complaintIndexMaxRange;
    #endregion

    #endregion

    void Start()
    {
        sortitionTools = GetComponent<SortitionTools>();

        canvas = FindObjectOfType<Canvas>();
        rectTransform = canvas.GetComponent<RectTransform>();
        
        #region setup npc attributes
        //definindo o intervalo do sorteio dos plasmodiums
        sortitionTools.SortitionRange(ref plasmodiumSpawnMinValueInterval, ref plasmodiumSpawnMaxValueInterval, plasmodiumSpawnRate);

        //definindo o intervalo das queixas
        sortitionTools.SortitionRange(ref complaintIndexMinRange, ref complaintIndexMaxRange, complaintIndex);

        //tamanho dos arrays. Aqui estou usando o segundo índice sendo o valor específico de sintomas da lista 1 (tentar alterar depois)
        symptomSortitionMinValueInterval = new float[symptomMasterData.PlasmodiumName.Length, symptomMasterData.Plasmodium1_SymptomChance.Length];
        symptomSortitionMaxValueInterval = new float[symptomMasterData.PlasmodiumName.Length, symptomMasterData.Plasmodium1_SymptomChance.Length];

        //não consegui fazer uma função para facilitar esse processo
        SymptomSortitionValueIntervalRangeMaker(symptomMasterData.Plasmodium1_SymptomChance, 0);
        SymptomSortitionValueIntervalRangeMaker(symptomMasterData.Plasmodium2_SymptomChance, 1);
        SymptomSortitionValueIntervalRangeMaker(symptomMasterData.Plasmodium3_SymptomChance, 2);
        #endregion
        
    }
    
    private void Update()
    {
        ////    <<< ----------  DEBUG  ---------- >>>
        if (Input.GetKeyDown(KeyCode.Space))
            NpcSpawn();
    }

    public void NpcSpawn()
    {
        int nameSorted = Random.Range(0, symptomMasterData.CharacterNames.names.Length);
        string name = symptomMasterData.CharacterNames.names[nameSorted];

        //Sorteia o plasmodium e retorna seu index
        int plasmodiumType = sortitionTools.Sortition(0, 100, plasmodiumSpawnMinValueInterval, plasmodiumSpawnMaxValueInterval);

        //imagem do npc
        int spriteImageIndex = Random.Range(0, 10);
        Sprite spriteNpc = artGroups[plasmodiumType].NpcImage[spriteImageIndex];

        //Sorteia a lista de sintomas e retorna seu index
        int symptomListNumber = sortitionTools.Sortition(0, 100, symptomSortitionMinValueInterval, symptomSortitionMaxValueInterval, plasmodiumType);

        //Ramdom quantidade de sintomas
        int symptomsQuantity = Random.Range(numberSymptomsMin, numberSymptomsMax + 1);

        //Ramdom dos sintomas
        string[] symptomsFromSortition = new string[symptomsQuantity];

        for (int i = 0; i < symptomsFromSortition.Length; i++)
        {
            int newIndex = Random.Range(0, symptomsQuantity);

            for (int a = 0; a < symptomsFromSortition.Length; a++)
            {
                if (symptomMasterData.SymptomsList[symptomListNumber].Symptoms[newIndex] == symptomsFromSortition[a])
                {
                    newIndex = Random.Range(0, symptomsQuantity);
                    a = -1;
                }
            }
            symptomsFromSortition[i] = symptomMasterData.SymptomsList[symptomListNumber].Symptoms[newIndex];
        }

        //Chance de estar com malária
        float malariaIndexTotal = symptomsQuantity * symptomMasterData.MalariaIndex[symptomListNumber];

        //o índice pode ficar acima de 100%, isso verifica se ultrapassou e o limita a 100%
        if (malariaIndexTotal > 100)
            malariaIndexTotal = 100;

        int complaintIndex = sortitionTools.SortedValueVerification((int)malariaIndexTotal, complaintIndexMinRange, complaintIndexMaxRange);
        string complaintText = complaints[complaintIndex];

        //INSTANCIA DO NOVO NPC
        GameObject newPacient = Instantiate(npcPrefab);

        SettingNewNpc(newPacient, name, spriteNpc, plasmodiumType, symptomsFromSortition, malariaIndexTotal, complaintText);

        RoundManager.Instance.QueueAdd(newPacient);  //adiciona o npc gerado na fila caso haja vaga
    }

    private void SettingNewNpc(GameObject newNpc, string npcName, Sprite npcSprite, int newPlasmodiumType, string[] newSymptoms, float newMalariaIndex, string newComplaintText)
    {
        newNpc.transform.SetParent(canvas.transform);  //transforma o GameObject em filho do Canvas
        newNpc.transform.localPosition = new Vector2(rectTransform.rect.xMin, rectTransform.rect.yMax + 200); //posiciona o novo npc fora da tela

        Pacient pacient = newNpc.GetComponent<Pacient>();

        pacient.characterName = npcName;
        pacient.npcImage = npcSprite;
        pacient.plasmodiumName = symptomMasterData.PlasmodiumName[newPlasmodiumType];

        pacient.symptoms = new string[newSymptoms.Length];
        for (int i = 0; i < newSymptoms.Length; i++)
        {
            pacient.symptoms[i] = newSymptoms[i];
        }

        pacient.malariaIndex = newMalariaIndex;

        if (symptomMasterData.PlasmodiumName[newPlasmodiumType] != "Sem")
            pacient.isContaminated = "Sim";
        else
            pacient.isContaminated = "Não";

        pacient.complaint = newComplaintText;
        pacient.sector = RoundManager.Instance.queue;
    }
    
    private void SymptomSortitionValueIntervalRangeMaker(float[] plasmodiumSymptomChance, int plasmodiumIndex)
    {
        //salva os valores min e max do range de sorteio da chance de selecionar a lista de sintomas
        sortitionTools.SortitionRange(ref tempValueForSymptomSortitonRangeMin, ref tempValueForSymptomSortitonRangeMax, plasmodiumSymptomChance);
        
        //popula o array que salva as chances de vir cada sintoma
        for (int i = 0; i < tempValueForSymptomSortitonRangeMin.Length; i++)
        {
            symptomSortitionMinValueInterval[plasmodiumIndex, i] = tempValueForSymptomSortitonRangeMin[i];
            symptomSortitionMaxValueInterval[plasmodiumIndex, i] = tempValueForSymptomSortitonRangeMax[i];
        }
    }
    
}
