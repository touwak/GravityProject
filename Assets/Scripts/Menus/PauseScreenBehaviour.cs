﻿using UnityEngine;
using UnityEngine.SceneManagement; // SceneManager
public class PauseScreenBehaviour : MainMenuBehaviour {

    public static bool paused;

    [Tooltip("Reference to the pause menu object to turn on/off")]  
    public GameObject pauseMenu;

    // Reference to the player for the score
    public PlayerBehaviour player;

    /// <summary>
    /// Reloads our current level, effectively "restarting" the
    /// game
    /// </summary>
    public void Restart() {

        //TODO increment the achievements
        if (player) {
            GooglePlayGame.CheckAdchievements((int)player.Score);
        }

        if (UnityAdController.restartWithoutAds >= UnityAdController.restartAdsThreshold &&
            UnityAdController.showAds) {

            UnityAdController.restartWithoutAds = 0;
            UnityAdController.ShowAd();

            if (pauseMenu.activeInHierarchy) {
                pauseMenu.SetActive(false);
            }
        }
        else {
            UnityAdController.restartWithoutAds++;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            UnityAdController.nextRewardAvalible = true;
        }        
    }
    
    /// <summary>
    /// Will turn our pause menu on or off
    /// </summary>
    /// <param name="isPaused"></param>
    public void SetPauseMenu(bool isPaused) {
        paused = isPaused;
        // If the game is paused, timeScale is 0, otherwise 1
        Time.timeScale = (paused) ? 0 : 1;
        pauseMenu.SetActive(paused);
    }

    public override void LoadLevel(string levelName) {
        //TODO increment the achievements
        if (player) {
            GooglePlayGame.CheckAdchievements((int)player.Score);
        }

        base.LoadLevel(levelName);
    }

    protected override void Start() {
        // Initalize Ads if needed
        base.Start();
        paused = false;
        
        // If no ads at all, just unpause
        #if !UNITY_ADS
        SetPauseMenu(false);
        #else
        
        // If we support ads but they're removed, unpause as well
        if (!UnityAdController.showAds) {
            SetPauseMenu(false);
        }
        #endif
    }

    #region Share Score via Twitter
    
    // Web address in order to create a tweet
    private const string tweetTextAddress =
        "http://twitter.com/intent/tweet?text=";
    
    // Where we want players to visit
    private string appStoreLink = "https://play.google.com/store/apps/details?id=com.touwapp.gravitycube";
    
    /// <summary>
    /// Will open Twitter with a prebuilt tweet. When called on iOS or
    /// Android will open up Twitter app if installed
    /// </summary>
    public void TweetScore() {
        // Get contents of the tweet (in URL friendly format)
        string tweet = "I got " + string.Format("{0:0}", player.Score)
        + " points in #GravityCube! Can you do better?";
        
        // Open the URL to create the tweet
        Application.OpenURL(tweetTextAddress + WWW.EscapeURL(tweet +
        "\n" + appStoreLink));
    }
    #endregion
}