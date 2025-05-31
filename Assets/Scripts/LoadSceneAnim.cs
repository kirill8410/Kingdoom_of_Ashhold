using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LoadSceneAnim : MonoBehaviour
{
    [SerializeField] float time;
    [SerializeField]GameObject[] sprites;
    [SerializeField] Image image;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Switch();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Switch()
    {
        StartCoroutine(Animation());
    }
    int i;
    IEnumerator Animation()
    {
        for( i = 0;i < 100; i+=3)
        {
            yield return new WaitForSeconds(time);
            image.fillAmount = i/100;
        }
        for (i = 0;i < 100; i++)
        {

        }
    }
}
