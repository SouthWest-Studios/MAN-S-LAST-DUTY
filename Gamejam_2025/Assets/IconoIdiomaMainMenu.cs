using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconoIdiomaMainMenu : MonoBehaviour
{

    public Sprite[] idiomasIconos;
    public Image iconoIdioma;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        int idiomaIndex = PuzzleManager.instance.idiomaIndex;
        if(idiomaIndex >= 0 && idiomaIndex <= 2)
        {
            iconoIdioma.sprite = idiomasIconos[idiomaIndex];
        }
        
       
    }
}
