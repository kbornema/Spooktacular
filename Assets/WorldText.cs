using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldText : MonoBehaviour 
{
    public AnimationCurve fadeCurve;

    public float speed = 1.0f;
    public float time = 1.0f;
    public Text text;

    private float curTime;

    private Vector3 moveAxis = new Vector3(0, 1, 0);
    public void SetMoveAxis(Vector3 axis)
    {
        moveAxis = axis;
    }


    void Start()
    {
        curTime = 0.0f;
    }

    public void Set(string text, Color color)
    {
        this.text.text = text;
        this.text.color = color;
    }


    void Update()
    {
        curTime += Time.deltaTime;

        float t = curTime / time;

        Color a = text.color;
        a.a = fadeCurve.Evaluate(t);
        text.color = a;

        gameObject.transform.position += moveAxis * speed * Time.deltaTime;

        if (t >= 1.0f)
            Destroy(gameObject);
    }
}
