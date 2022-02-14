using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;//UIはText型を認識するために必要
using UnityEngine.SceneManagement;

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

        if (nejiko.Life() <= 0)
        {
            enabled = false;//GameControllerのコンポーネントをオフにできる プロパティ

            if (PlayerPrefs.GetInt("HightScore") < score)
            {
                PlayerPrefs.SetInt("HighScore", score);
            }

            Invoke("ReturnToTitle", 2.0f);//指定時間後にReturnToTitleを呼ぶ　引数が必要なものは呼べない
        }
    }

    int CalcScore()
    {
        return (int)nejiko.transform.position.z;//.gameObject.GetComponent～～  ～～はanimatorやcharacterなど transformは特殊だから.transformでできる
    }

    void ReturnToTitle()
    {
        SceneManager.LoadScene("Title");
    }
}
