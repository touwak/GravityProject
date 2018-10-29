using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine;

public class GooglePlayLogin : MonoBehaviour {

    void Start() {
        /*PlayGamesPlatform.DebugLogEnabled = true;

        PlayGamesClientConfiguration config =
            new PlayGamesClientConfiguration.Builder().Build();

        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.Activate();

        Social.localUser.Authenticate(success => { });*/

        PlayGamesClientConfiguration Config = new PlayGamesClientConfiguration.Builder().Build();
        PlayGamesPlatform.InitializeInstance(Config);

        Social.localUser.Authenticate((bool Status) => {
            //ConnectionStatus = Status;
        });
    }


}
