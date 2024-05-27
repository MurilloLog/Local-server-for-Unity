using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update() { }

    /* Main activity */
    public void home2Type1()
    {
        SceneManager.LoadScene("Type1");
    }
    
    public void home2Type2()
    {
        SceneManager.LoadScene("Type2");
    }
    
    public void home2Type3()
    {
        SceneManager.LoadScene("Type3");
    }

    public void GoHome()
    {
        SceneManager.LoadScene("Main");
    }

    /* Type 2 activity */
    public void Host()
    {
        SceneManager.LoadScene("Host");
    }

    public void Guest()
    {
        SceneManager.LoadScene("Guest");
    }

    /* Game */
    public void SetConfig2Game()
    {
        SceneManager.LoadScene("Config");
    }
    
    /* Admin */
    public void Admin()
    {
        SceneManager.LoadScene("Admin");
    }
}
