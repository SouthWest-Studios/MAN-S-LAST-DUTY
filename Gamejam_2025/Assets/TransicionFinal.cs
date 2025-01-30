using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransicionFinal : MonoBehaviour
{

    public float timerToChangeScene;
    public float timerToFadeBlackScene;
    private float contador = 0;

    public Animator fadeTObLACK;

    public bool empiezaTransicionFinal = false;


    public static TransicionFinal instance;

    private void Awake()
    {
        if (instance == null) { instance = this; }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (empiezaTransicionFinal)
        {
            contador += Time.deltaTime;



            if (contador >= timerToFadeBlackScene)
            {
                fadeTObLACK.Play("fadetoblackfinal");

                    timerToFadeBlackScene = 100000000;
            }

            if (contador >= timerToChangeScene)
            {
                SceneManager.LoadScene(2);
                timerToChangeScene = 1000000000;
            }

        }
    }



    public void PlayTransicionFinal()
    {
        GetComponent<Rewind>().enabled = false;
        empiezaTransicionFinal = true;
    }


}
