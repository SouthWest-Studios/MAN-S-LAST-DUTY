using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SetTextByLanguage : MonoBehaviour
{
    public string spanishText;
    public string englishText;
    public string catalanText;

    private TextMeshProUGUI m_tmp;


    // Start is called before the first frame update
    void Start()
    {
        m_tmp = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        int idiomaIndex = PuzzleManager.instance.idiomaIndex;
        if(idiomaIndex == 0)
        {
            m_tmp.text = spanishText;
        }else if(idiomaIndex == 1)
        {
            m_tmp.text = englishText;
        }
        else if (idiomaIndex == 2)
        {
            m_tmp.text = catalanText;
        }
    }
}
