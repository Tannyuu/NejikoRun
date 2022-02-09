using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;//UIはText型を認識するために必要

public class GameController : MonoBehaviour
{
    public NejikoController nejiko;
    public Text scoreText;
    public LifePanel lifePanel;

    // Update is called once per frame
    void Update()
    {
        int score = CalcScore();
        scoreText.text = $"Score:{score}m";//scoreText.fontSize = 30;
        lifePanel.UpdateLife(nejiko.Life());//Component型だったら　lifePanel.UpdateLife(nejiko.gameObject.GetComponent<NejikoController>().Life())
        //GameOblect型だったら lifePanel.UpdateLife(nejiko.GetComponent<NejikoController>().Life())
        //transformは特殊 .transformができる
    }

    int CalcScore()
    {
        return (int)nejiko.transform.position.z;//.gameObject.GetComponent～～  ～～はanimatorやcharacterなど transformは特殊だから.transformでできる
    }
}
