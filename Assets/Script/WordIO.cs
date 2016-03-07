using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System;

//영어단어 자료 클래스
public class Word
{
    public int id; //영어단어의 구분자
    public string value; //영어 단어
}

public class WordIO
{
    //Xml파일을 읽어서 Word클래스의 List로 반환하는 메소드
    public static List<Word> Read(string filePath)
    {
        XmlDocument Document = new XmlDocument();
        Document.Load(filePath);
        XmlElement WordListElement = Document["MyTest"];

        List<Word> WordList = new List<Word>();

        foreach (XmlElement WordElement in WordListElement.ChildNodes)
        {
            Word word = new Word();
            word.id = Convert.ToInt32(WordElement.GetAttribute("id"));
            word.value = WordElement.GetAttribute("value");
            WordList.Add(word);
        }
        return WordList;
    }
}