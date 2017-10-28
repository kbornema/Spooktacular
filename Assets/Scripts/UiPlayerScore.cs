using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiPlayerScore : MonoBehaviour 
{
    [SerializeField]
    private Image _image;
    [SerializeField]
    private Text _text;


    public void SetColor(Color c)
    {
        _image.color = c;
        _text.color = c;
    }

    public void SetScore(int score)
    {
        _text.text = score.ToString();
    }
}
