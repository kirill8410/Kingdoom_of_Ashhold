using TMPro;
using UnityEngine;

public class TourmeyName : MonoBehaviour
{
    public string Name;
    public TextMeshProUGUI text;

    public void Add(string letter)
    {
        if(Name.Length < 10)
        {
            Name += letter;
            text.text = Name;
        }
    }

    public void Remove()
    {
        if (Name.Length > 0)
        {
            Name = Name.Substring(0, Name.Length - 1);
            text.text = Name;
        }
    }
}
