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
    float i;
    IEnumerator Animation()
    {
        for( i = 0;i < 100; i+=1)
        {
            image.fillAmount = i / 100;
            yield return new WaitForSeconds(time);
        }
        for (i = 0; i < 100; i += 1)
        {
            sprites[0].GetComponent<Image>().color = new Color(1-i/100, 1 - i / 100 , 1 - i / 100);
            sprites[1].GetComponent<Image>().color = new Color(1 - i / 100, 1 - i / 100, 1 - i / 100);
            sprites[2].GetComponent<Image>().color = new Color( 1 - i / 100, 1 - i / 100, 1 - i / 100);
            yield return new WaitForSeconds(0.01f);
        }
        sprites[1].SetActive(false);
        sprites[2].SetActive(false);
        for (i = 0; i < 100; i += 1)
        {
            sprites[0].GetComponent<Image>().color = new Color(0,0,0,1-i/100);
            yield return new WaitForSeconds(0.01f);
        }
    }
}
