using UnityEngine;

public class TourmeyName : MonoBehaviour
{
    public string name;

    public void Add(string letter)
    {
        if(name.Length < 10)
        {
            name += letter;
        }
    }

    public void Remove()
    {
        if (name.Length > 0)
        {
            name = name.Substring(0, name.Length - 1);
        }
    }
}
