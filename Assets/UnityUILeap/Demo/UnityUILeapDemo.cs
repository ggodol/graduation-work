using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class UnityUILeapDemo : MonoBehaviour {
    public Text ValueText;
    public void ChangeText(float value) {
        ValueText.text = value.ToString();

    }

}
