using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ReadTest : MonoBehaviour {

    void Start()
    {
        // 저장되어있는 XML 파일을 읽어온다.
        List<Word> WordList = WordIO.Read(Application.dataPath + "/Resource/XML/Items.xml");

        // 결과물 출력.
        for (int i = 0; i < WordList.Count; ++i)
        {
            Word word = WordList[i];
            Debug.Log(string.Format("word[{0}] : ({1}, {2})",
                i, word.id, word.value));
        }
    }
}