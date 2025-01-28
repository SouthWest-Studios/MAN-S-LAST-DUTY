using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NavigationManager : MonoBehaviour
{
    public ProximidadSonora proximidadSonora;
    public TangramManager tangramManager;
    public correctWaveScript correctWaveScript;

    public List<TextMeshPro> ListproximidadSonora;
    public List<TextMeshPro> ListtangramManager;
    public List<TextMeshPro> ListcorrectWaveScript;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void CheckWin()
    {
        int checkWin = 0;
        foreach (TextMeshPro textObject in ListproximidadSonora)
        {
            if (textObject.gameObject.activeSelf) // Verifica si este objeto está activo
            {
                if (textObject.text == proximidadSonora.coordinatesText.text)
                {
                    checkWin++;
                }
            }
        }

        foreach (TextMeshPro textObject in ListtangramManager)
        {
            if (textObject.gameObject.activeSelf) // Verifica si este objeto está activo
            {
                if (textObject.text == tangramManager.formName)
                {
                    checkWin++;
                }
            }
        }
    }

    
}
