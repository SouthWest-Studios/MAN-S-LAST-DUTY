using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class NotasMentalesManager : MonoBehaviour
{

    public GameObject notasMentales;
    public GameObject iconoActualizarNotas;

    public TextMeshProUGUI numpadCode;
    public Image[] wordleCells;
    public Sprite[] wordleSprites;
    public Image tangramCell;
    public Sprite[] tangramResultSprites;
    public Image[] bloodCells;
    public Sprite[] bloodResultSprites;
    public TextMeshProUGUI periodicNumber;

    // Start is called before the first frame update
    void Start()
    {
        numpadCode.text = "";
    }

    // Update is called once per frame
    void Update()
    {

        //NO HAY QUE HACER -> SIMOON DICE, GUITAR, PIPES, LUCES

        string numpadActualCode = PuzzleManager.numpadActualCode;

        if (numpadActualCode != "****") {
            numpadCode.text = PuzzleManager.numpadActualCode;
        }

        if (PuzzleManager.instance.GetPuzzle("WordlePuzzle").itHasbeenCompleted)
        {
            for(int i = 0; i<wordleCells.Length; i++)
            {
                wordleCells[i].sprite = wordleSprites[WordleController.correctCombination[i]];
            }
        }

        if (PuzzleManager.instance.GetPuzzle("TangramPuzzle").itHasbeenCompleted)
        {
            tangramCell.sprite = tangramResultSprites[TangramManager.randForm];
        }

        if (PuzzleManager.instance.GetPuzzle("BloodPuzzle").itHasbeenCompleted)
        {
            for (int i = 0; i <  BloodManager.blood.Count; i++)
            {
                bloodCells[i].sprite = bloodResultSprites[BloodManager.blood[i].correctNumber];
            }
        }

        if (PuzzleManager.instance.GetPuzzle("PeriodicTablePuzzle").itHasbeenCompleted)
        {
            periodicNumber.text = PeriodicPuzzle.correctNum.ToString();
        }

        if (PuzzleManager.instance.GetPuzzle("WavePuzzle").itHasbeenCompleted)
        {

        }

        if (PuzzleManager.instance.GetPuzzle("ProximidadSonoraPuzzle").itHasbeenCompleted)
        {

        }
        



    }


    public void ShowDrawIcon()
    {

    }


}
