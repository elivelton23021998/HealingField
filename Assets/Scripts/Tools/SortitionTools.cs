using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SortitionTools : MonoBehaviour
{
    //Função que cria e salva o primeiro e segundo valor do range de sorteio (intervalo da porcentagem) 
    //ex.: elemento 1: 10% de 1 até 10; elemento 2: 10% de 11 até 20
    public void SortitionRange(ref float[] saveFirstValue, ref float[] saveSecondValue, float[] sortitionValues)
    {
        saveFirstValue = new float[sortitionValues.Length];
        saveSecondValue = new float[sortitionValues.Length];

        for (int i = 0; i < sortitionValues.Length; i++)
        {
            if (i == 0)
            {
                if (sortitionValues[i] != 0)
                {
                    saveFirstValue[i] = 1f;
                    saveSecondValue[i] = sortitionValues[i];
                }
                else
                    saveFirstValue[i] = saveSecondValue[i] = 0f;
            }
            else
            {
                if (sortitionValues[i] != 0)
                {
                    saveFirstValue[i] = saveSecondValue[i - 1] + 1f;
                    saveSecondValue[i] = saveSecondValue[i - 1] + sortitionValues[i];
                }
                else
                    saveFirstValue[i] = saveSecondValue[i] = 0f;
            }
        }
    }

    //Realiza o sorteio, verifica em qual intervalo está e retorna o index do intervalo.
    public int Sortition(int sortitionRangeValueA, int sortitionRangeValueB, float[] valueToSortitionMin, float[] valueToSortitionMax)
    {
        int sortition = Random.Range(sortitionRangeValueA + 1, sortitionRangeValueB + 1);

        int rangeIndex = 0;

        for (int i = 0; i < valueToSortitionMin.Length; i++)
        {
            if (sortition >= valueToSortitionMin[i] && sortition <= valueToSortitionMax[i])
            {
                rangeIndex = i;
            }
        }
        return rangeIndex;
    }

    public int Sortition(int sortitionRangeValueA, int sortitionRangeValueB, float[,] valueToSortitionMin, float[,] valueToSortitionMax, 
                            int firstValueToSortitionIndex)
    {
        int sortition = Random.Range(sortitionRangeValueA + 1, sortitionRangeValueB + 1);

        int rangeIndex = 0;

        for (int i = 0; i < valueToSortitionMin.GetLength(1); i++)
        {
            if (sortition >= valueToSortitionMin[firstValueToSortitionIndex, i] && 
                sortition <= valueToSortitionMax[firstValueToSortitionIndex, i])
            {
                rangeIndex = i;
            }
        }
        return rangeIndex;
    }

    //Verifica o range em que o valor previamente sorteado está e retorna seu index
    public int SortedValueVerification(int valueSorted, float[] valueToSortitionMin, float[] valueToSortitionMax)
    {
        int rangeIndex = 0;

        for (int i = 0; i < valueToSortitionMin.Length; i++)
        {
            if (valueSorted >= valueToSortitionMin[i] && valueSorted <= valueToSortitionMax[i])
            {
                rangeIndex = i;
            }
        }
        return rangeIndex;
    }

}