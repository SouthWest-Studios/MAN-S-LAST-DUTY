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

    public static NotasMentalesManager instance;

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
    public Image cordonUmbilicalCell;
    public Sprite[] cordonUmbilicalSprites;
    public TextMeshProUGUI ondaNumero;


    private void Awake()
    {
        if(instance == null) { instance = this; }
    }

    // Start is called before the first frame update
    void Start()
    {
        numpadCode.text = "";
        numpadCodes.SetActive(false);
        wordle.SetActive(false);
        tangram.SetActive(false);
        periodicTable.SetActive(false);
        blood.SetActive(false);
        proximidadSonora.SetActive(false);
        ondas.SetActive(false);
        cordonUmbilical.SetActive(false);

    }


    // Update is called once per frame
    void Update()
    {
        GestionMente();


    }


    private void GestionMente()
    {
        //NO HAY QUE HACER -> SIMON DICE, GUITAR, PIPES, LUCES -> te dan el codigo estos

        string numpadActualCode = PuzzleManager.numpadActualCode;

        if (numpadActualCode != "****")
        {
            numpadCodes.SetActive(true);
            numpadCode.text = PuzzleManager.numpadActualCode;
        }

        if (PuzzleManager.instance.GetPuzzle("WordlePuzzle").itHasbeenCompleted)
        {
            wordle.SetActive(true);
            for (int i = 0; i < wordleCells.Length; i++)
            {
                wordleCells[i].sprite = wordleSprites[WordleController.correctCombination[i]];
            }
        }

        if (PuzzleManager.instance.GetPuzzle("TangramPuzzle").itHasbeenCompleted)
        {
            tangram.SetActive(true);
            tangramCell.sprite = tangramResultSprites[TangramManager.randForm];
        }

        if (PuzzleManager.instance.GetPuzzle("BloodPuzzle").itHasbeenCompleted)
        {
            blood.SetActive(true);
            for (int i = 0; i < BloodManager.blood.Count; i++)
            {
                bloodCells[i].sprite = bloodResultSprites[BloodManager.blood[i].correctNumber];
            }
        }

        if (PuzzleManager.instance.GetPuzzle("PeriodicTablePuzzle").itHasbeenCompleted)
        {
            periodicTable.SetActive(true);
            periodicNumber.text = PeriodicPuzzle.correctNum.ToString();
        }

        if (PuzzleManager.instance.GetPuzzle("WavePuzzle").itHasbeenCompleted)
        {
            ondas.SetActive(true);
            ondaNumero.text = "";
            for(int i = 0; i< correctWaveScript.finalRandNum.Length; i++)
            {
                ondaNumero.text += correctWaveScript.finalRandNum[i].ToString();
            }
            

        }

        if (PuzzleManager.instance.GetPuzzle("ProximidadSonoraPuzzle").itHasbeenCompleted)
        {
            proximidadSonora.SetActive(true);
            proximidadSonoraNumber.text = "X: " + ProximidadSonora.level1Sum.ToString() + "\nY: " + ProximidadSonora.level2Sum.ToString() + "\nZ: " + ProximidadSonora.level3Sum.ToString();
        }

        if (PuzzleManager.instance.GetPuzzle("CordonUmbilicalPuzzle").itHasbeenCompleted)
        {
            cordonUmbilical.SetActive(true);
            if (CordonUmbilical.correctGroupIndex == 0)
            {
                if (CordonUmbilical.correctObjectInGroupIndex == 0) { cordonUmbilicalCell.sprite = cordonUmbilicalSprites[0]; }
                if (CordonUmbilical.correctObjectInGroupIndex == 1) { cordonUmbilicalCell.sprite = cordonUmbilicalSprites[1]; }
                if (CordonUmbilical.correctObjectInGroupIndex == 2) { cordonUmbilicalCell.sprite = cordonUmbilicalSprites[2]; }

            }
            else if (CordonUmbilical.correctGroupIndex == 1)
            {
                if (CordonUmbilical.correctObjectInGroupIndex == 0) { cordonUmbilicalCell.sprite = cordonUmbilicalSprites[3]; }
                if (CordonUmbilical.correctObjectInGroupIndex == 1) { cordonUmbilicalCell.sprite = cordonUmbilicalSprites[4]; }
                if (CordonUmbilical.correctObjectInGroupIndex == 2) { cordonUmbilicalCell.sprite = cordonUmbilicalSprites[5]; }
            }
            else if (CordonUmbilical.correctGroupIndex == 2)
            {
                if (CordonUmbilical.correctObjectInGroupIndex == 0) { cordonUmbilicalCell.sprite = cordonUmbilicalSprites[6]; }
                if (CordonUmbilical.correctObjectInGroupIndex == 1) { cordonUmbilicalCell.sprite = cordonUmbilicalSprites[7]; }
                if (CordonUmbilical.correctObjectInGroupIndex == 2) { cordonUmbilicalCell.sprite = cordonUmbilicalSprites[8]; }
            }
            else if (CordonUmbilical.correctGroupIndex == 3)
            {
                if (CordonUmbilical.correctObjectInGroupIndex == 0) { cordonUmbilicalCell.sprite = cordonUmbilicalSprites[9]; }
                if (CordonUmbilical.correctObjectInGroupIndex == 1) { cordonUmbilicalCell.sprite = cordonUmbilicalSprites[10]; }
                if (CordonUmbilical.correctObjectInGroupIndex == 2) { cordonUmbilicalCell.sprite = cordonUmbilicalSprites[11]; }
            }
        }
    }


    public void ShowDrawIcon()
    {

    }


}
