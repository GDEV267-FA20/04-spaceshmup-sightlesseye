using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // <== remember to use this anytime you use Scenes

public class PlayButton : MonoBehaviour {
    
    public void PlayGame() {
        //SceneManager deals with the scenes 
        //GetActiveScene gets current scene
        //buildIndex is the index of where the scene is in Unity's scene queue
        //SO
        //This code loads the next scene.
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
