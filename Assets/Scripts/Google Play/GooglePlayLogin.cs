using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine;

public class GooglePlayLogin : MonoBehaviour {

    void Start() {
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.Activate();

        Social.localUser.Authenticate((bool success) => {
            if (success) {
                Debug.Log("login success");
            }
            else {
                Debug.Log("login failed");
            }
        });
    }


}
