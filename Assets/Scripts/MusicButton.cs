using UnityEngine;
using UnityEngine.UI;

public class MusicButton : ClickSound {

    public string buttonName;
    private Text buttonText;

    void Start() {
        base.Start();

        if (button) {
            buttonText = button.GetComponentInChildren<Text>();
            SetMusicTextButton(buttonName);
        }
    }

    private void Update() {
        SetMusicTextButton(buttonName);
    }

    public void SetMusicTextButton(string name) {
        if (buttonText) {
            if (PlayerPrefs.GetInt(name) == 1) {
                buttonText.text = name + ": ON";
            }
            else {
                buttonText.text = name + ": OFF";             
            }
        }
    }

    
}
