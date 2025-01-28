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

    [Header("Grupos")]
    public GameObject numpadCodes;
    public GameObject wordle;
    public GameObject tangram;
    public GameObject periodicTable;
    public GameObject blood;
    public GameObject proximidadSonora;
    public GameObject ondas;
    public GameObject cordonUmbilical;


    [Header("Modificacion de datos")]
    public TextMeshProUGUI numpadCode;
    public Image[] wordleCells;
    public Sprite[] wordleSprites;
    public Image tangramCell;
    public Sprite[] tangramResultSprites;
    public Image[] bloodCells;
    public Sprite[] bloodResultSprites;
    public TextMeshProUGUI periodicNumber;
    public TextMeshProUGUI proximidadSonoraNumber;

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
            proximidadSonoraNumber.text = "X: " + ProximidadSonora.level1Sum.ToString() + "\nY: " + ProximidadSonora.level2Sum.ToString() + "\nZ: " + ProximidadSonora.level3Sum.ToString();
        }

        if (PuzzleManager.instance.GetPuzzle("CordonUmbilicarPuzzle").itHasbeenCompleted)
        {
            //proximidadSonoraNumber.text = "X: " + ProximidadSonora.level1Sum.ToString() + "\nY: " + ProximidadSonora.level2Sum.ToString() + "\nZ: " + ProximidadSonora.level3Sum.ToString();
        }




    }


    public void ShowDrawIcon()
    {

    }


}
