using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class UIManager : MonoBehaviour{
	public Text ScoreText;
    	public InputField username;
    	public string Name;
	public GameObject MainManager;

	public static UIManager Instance;

    	private void Awake(){
		if (Instance != null){
			Destroy(gameObject);
			return;
		}
		Instance = this;
		DontDestroyOnLoad(gameObject);
	}
    	public void StartNew(){
		SceneManager.LoadScene(1);
    	}
    	public void GetName(){
		Name = username.text;
		}
	}

