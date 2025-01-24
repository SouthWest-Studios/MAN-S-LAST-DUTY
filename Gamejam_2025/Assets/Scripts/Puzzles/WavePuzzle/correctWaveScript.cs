using UnityEngine;

public class correctWaveScript : MonoBehaviour
{
    void Start()
    {
        int childCount = transform.childCount;

        int randomIndex = Random.Range(0, childCount);

        for (int i = 0; i < childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(i == randomIndex);
        }
        gameObject.SetActive(false);
    }

    public void ShowCorrectWave()
    {
        gameObject.SetActive(true);
        StartCoroutine(DeactivateAfterDelay(0.2f));
    }

    private System.Collections.IEnumerator DeactivateAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false);
    }
}
