using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine;

public class GooglePlayGame : MonoBehaviour {

	void Start () {
        PlayGamesClientConfiguration config = 
            new PlayGamesClientConfiguration.Builder().Build();

        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.Activate();

        SignIn();
	}

    void SignIn() {
        Social.localUser.Authenticate(success => { });
    }

    #region ACHIEVEMENTS
    public static void UnlockAchievement(string id, double progess) {
        Social.ReportProgress(id, progess, success => { });
    }

    public static void IncrementAchievement(string id, int stepsToIncrement) {
        PlayGamesPlatform.Instance.IncrementAchievement(id, stepsToIncrement, succes => { });
    }

    public static void ShowAchievementSUI() {
        Social.ShowAchievementsUI();
    }

    public static void CheckAdchievements(int score) {      
       IncrementAchievement(GPGSIds.achievement_100_points, score);
       IncrementAchievement(GPGSIds.achievement_200_points, score); 
       IncrementAchievement(GPGSIds.achievement_300_points, score);  
       IncrementAchievement(GPGSIds.achievement_400_points, score);    
       IncrementAchievement(GPGSIds.achievement_500_points, score);    
       IncrementAchievement(GPGSIds.achievement_750_points, score);   
       IncrementAchievement(GPGSIds.achievement_1000_points, score); 
    }
    #endregion

    #region LEADERBOARD
    public static void AddScoreToLeaderboard(string leaderboardId, long score) {
        Social.ReportScore(score, leaderboardId, succes => { });
    }

    public static void ShowLeaderboardsUI() {
        Social.ShowLeaderboardUI();
    }
    #endregion

}
