using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Screensaver : MonoBehaviour
{
    [SerializeField] GameObject sword;
    [SerializeField] GameObject text;
    private void Start()
    {
        sword.transform.localPosition = new Vector3(0f, 350f, 0f);
        text.transform.localPosition = new Vector3(0f, 300f, 0f);
        StartCoroutine(Animation());
    }

    IEnumerator Animation()
    {
        yield return new WaitForSeconds(2f);
        while (sword.transform.localPosition.y != 0f)
        {
            sword.transform.localPosition = new Vector3(0f, sword.transform.localPosition.y - 1f, 0f);
            yield return new WaitForSeconds(0.002f);
        }
        text.transform.localPosition = new Vector3(0f, 0f, 0f);
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene(1);
    }
}
