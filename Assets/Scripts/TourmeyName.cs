using UnityEngine;

public class TourmeyName : MonoBehaviour
{
    int i = 0;
    public string name;
    // Update is called once per frame
    void Update()
    {
        
    }

    public void Add(string letter)
    {
        if(i < 10)
        {
            name += letter;
            i++;
        }
    }
}
