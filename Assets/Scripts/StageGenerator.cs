using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageGenerator : MonoBehaviour
{
    const int StageChipSize = 30;

    int currentChipIndex;//生成済みの最大インデックス

    public Transform character;
    public GameObject[] stageChips;
    public int startChipIndex;//1
    public int preInstantiate;//前方にいくつ作るか 5
    public List<GameObject> generatedStageList = new List<GameObject>();//Listをnewしている

    // Start is called before the first frame update
    void Start()
    {
        currentChipIndex = startChipIndex - 1;//1-1=0 生成済みは0
        UpdateStage(preInstantiate);
    }

    // Update is called once per frame
    void Update()
    {
        int charaPositionIndex = (int)(character.position.z / StageChipSize);

        if(charaPositionIndex + preInstantiate > currentChipIndex)
        {
            UpdateStage(charaPositionIndex + preInstantiate);
        }
    }

    void UpdateStage(int toChipIndex)
    {
        if(toChipIndex <= currentChipIndex)//あってもなくても変わらない
        {
            return;
        }
        for(int i=currentChipIndex + 1; i <= toChipIndex; i++)
        {
            GameObject stageObject = GenerateStage(i);

            generatedStageList.Add(stageObject);
        }

        while(generatedStageList.Count > preInstantiate + 2)
        {
            DestroyOldestStage();
        }
        currentChipIndex = toChipIndex;
    }

    GameObject GenerateStage(int chipIndex)
    {
        int nextStageChip = Random.Range(0, stageChips.Length);

        GameObject stageObject = (GameObject)Instantiate(
            stageChips[nextStageChip],
            new Vector3(0,0,chipIndex * StageChipSize),//(0,0,30)
            Quaternion.identity
            );

        return stageObject;
    }

    void DestroyOldestStage()
    {
        GameObject oldStage = generatedStageList[0];//住所取得
        generatedStageList.RemoveAt(0);//インデックスを指定して消すのはRemoveAt
        Destroy(oldStage);//ゲーム画面からも削除
    }
}
