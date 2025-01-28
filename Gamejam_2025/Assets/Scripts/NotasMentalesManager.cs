using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NotasMentalesManager : MonoBehaviour
{

    public GameObject notasMentales;
    public GameObject iconoActualizarNotas;

    public TextMeshProUGUI numpadCode;
    public SpriteRenderer[] wordleCells;
    public Sprite[] wordleSprites;

    // Start is called before the first frame update
    void Start()
    {
        numpadCode.text = "";
    }

    // Update is called once per frame
    void Update()
    {
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

        
    }


    public void ShowDrawIcon()
    {

    }


}
