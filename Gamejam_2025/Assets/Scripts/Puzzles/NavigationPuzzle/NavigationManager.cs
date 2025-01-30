using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NavigationManager : MonoBehaviour
{
    public ProximidadSonora proximidadSonora;
    public TangramManager tangramManager;
    public correctWaveScript CorrectWaveScript;

    public List<TextMeshPro> ListproximidadSonoraX;
    public List<TextMeshPro> ListproximidadSonoraY;
    public List<TextMeshPro> ListproximidadSonoraZ;
    public List<TextMeshPro> ListtangramManager;
    public List<TextMeshPro> ListcorrectWaveScript;

    private PuzzleManager puzzleManager;

    public GameObject objectToShow;
    // Start is called before the first frame update
   

    public void InitializeNavigationSonor()
    {
        FillRandomNumbers(ListproximidadSonoraX, 30, "X = ", (int)proximidadSonora.GetLevel1Sum(), false);
        Debug.Log("getterX =" + proximidadSonora.GetLevel1Sum());
        FillRandomNumbers(ListproximidadSonoraY, 30, "Y = ", (int)proximidadSonora.GetLevel2Sum(), false);
        FillRandomNumbers(ListproximidadSonoraZ, 30, "Z = ", (int)proximidadSonora.GetLevel3Sum(), false);
        
        
    }

    public void InitializeNavigationWaves()
    {
        
        if (CorrectWaveScript.finalText.text != null)
        {
            FillRandomNumbers(ListcorrectWaveScript, 10, "", int.Parse(CorrectWaveScript.finalText.text), true);
        }

    }

    void FillRandomNumbers(List<TextMeshPro> textList, int random, string initialText, int numberToInsert, bool isArray)
    {
        if (textList == null || textList.Count < 7)
        {
            Debug.LogError("La lista no tiene suficientes elementos.");
            return;
        }

        HashSet<int> uniqueNumbers = new HashSet<int>(); // Para evitar duplicados

        // Escoger una posici�n aleatoria para insertar `numberToInsert`
        int insertIndex = Random.Range(0, 7);
        uniqueNumbers.Add(numberToInsert); // Asegurar que el n�mero a insertar es �nico

        for (int i = 0; i < 8; i++)
        {
            if (isArray)
            {
                // Si es un array, generamos 4 n�meros aleatorios �nicos
                List<int> arrayNumbers = new List<int>();

                if (i == insertIndex)
                {
                    arrayNumbers.Add(numberToInsert);
                }
                else
                {
                    for (int j = 0; j < 4; j++)
                    {
                        int randomNumber;
                        do
                        {
                            randomNumber = Random.Range(0, random);
                        } while (arrayNumbers.Contains(randomNumber) || uniqueNumbers.Contains(randomNumber)); // Evitar duplicados en el array interno

                        arrayNumbers.Add(randomNumber);
                    }
                }
                

                // Si este �ndice es el seleccionado, reemplazar un n�mero aleatorio con `numberToInsert`
                

                // Convertir los n�meros en un string separado por espacios
                textList[i].text = initialText + string.Join("", arrayNumbers);
            }
            else
            {
                // Si no es un array, seguir la l�gica original
                if (i == insertIndex)
                {
                    textList[i].text = initialText + numberToInsert.ToString();
                }
                else
                {
                    int randomNumber;
                    do
                    {
                        randomNumber = Random.Range(0, random);
                    } while (uniqueNumbers.Contains(randomNumber)); // Asegurar que sea �nico

                    uniqueNumbers.Add(randomNumber);
                    textList[i].text = initialText + randomNumber.ToString();
                }
            }
        }
    }





    public void CheckWin()
    {
        int checkWin = 0;
        foreach (TextMeshPro textObject in ListproximidadSonoraX)
        {
            if (textObject.gameObject.activeSelf)
            {
                if (textObject.text == "X = " + (int)proximidadSonora.GetLevel1Sum())
                {
                    checkWin++;
                }
            }
        }

        foreach (TextMeshPro textObject in ListproximidadSonoraY)
        {
            if (textObject.gameObject.activeSelf)
            {
                if (textObject.text == "Y = " + (int)proximidadSonora.GetLevel2Sum())
                {
                    checkWin++;
                }
            }
        }

        foreach (TextMeshPro textObject in ListproximidadSonoraZ)
        {
            if (textObject.gameObject.activeSelf)
            {
                if (textObject.text == "Z = " + (int)proximidadSonora.GetLevel3Sum())
                {
                    checkWin++;
                }
            }
        }

        foreach (TextMeshPro textObject in ListtangramManager)
        {
            Debug.Log("FormName = " + tangramManager.formName);
            if (textObject.gameObject.activeSelf)
            {
                Debug.Log("TextObject = " + textObject.text);
                if (textObject.text == tangramManager.formName)
                {
                    checkWin++;
                }
            }
        }

        foreach (TextMeshPro textObject in ListcorrectWaveScript)
        {
            if (textObject.gameObject.activeSelf)
            {
                if (textObject.text == CorrectWaveScript.finalText.text)
                {
                    checkWin++;
                }
            }
        }
        if(checkWin >= 5)
        {
            puzzleManager = FindAnyObjectByType<PuzzleManager>();
            puzzleManager.CompletePuzzle("NavigationPuzzle");
        }
        Debug.Log("CheckWin = " + checkWin);
        objectToShow.SetActive(true);
    }

    
}
