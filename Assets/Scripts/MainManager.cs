using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using System;
using System.Text.RegularExpressions;

public class MainManager : MonoBehaviour{
	public static MainManager Instance;
	
    	public Brick BrickPrefab;
    	public int LineCount = 6;
    	public Rigidbody Ball;

    	public Text ScoreText;
    	public GameObject GameOverText;
    	public GameObject UIManager;
    	public Text HighScoreText;
    	public string Name;
	public string HSName;
    	public int NewHS;
    
    	private bool m_Started = false;
    	private int m_Points;
    
    	private bool m_GameOver = false;

    	// Start is called before the first frame update
	private void Awake(){
		if (Instance != null){
			Destroy(gameObject);
			return;
			}
		Instance = this;
		DontDestroyOnLoad(gameObject);
		Load();
		}
    	void Start(){
		Ball = GameObject.FindGameObjectWithTag("Boll").GetComponent<Rigidbody>();
		UIManager = GameObject.FindGameObjectWithTag("UIManager");
		Name = UIManager.GetComponent<UIManager>().Name;
	        const float step = 0.6f;
        	int perLine = Mathf.FloorToInt(4.0f / step);
        
	        int[] pointCountArray = new [] {1,1,2,2,5,5};
        	for (int i = 0; i < LineCount; ++i){
            		for (int x = 0; x < perLine; ++x){
                		Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                		var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                		brick.PointValue = pointCountArray[i];
                		brick.onDestroyed.AddListener(AddPoint);
            			}
        		}
    		}

    	private void Update(){
        	if (!m_Started){
            		if (Input.GetKeyDown(KeyCode.Space)){
                		m_Started = true;
                		float randomDirection = UnityEngine.Random.Range(-1.0f, 1.0f);
                		Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                		forceDir.Normalize();
                		Ball.transform.SetParent(null);
                		Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
				}

			//Regex.Replace(Variable,@"\D","") obtains the numbers from a string
			//Regex.Replace(Variable,@"\d","") obtains the letters from a string
			string HST = Regex.Replace(HighScoreText.text,@"\D","");
			string CST = Regex.Replace(ScoreText.text,@"\D","");
			NewHS = Math.Max(Int32.Parse(CST),Int32.Parse(HST));
			string HSName = Name;
			HighScoreText.text = $"High Score: {HSName}: {NewHS}";
            		
        		}
        	else if (m_GameOver){
			Save();
            		if (Input.GetKeyDown(KeyCode.Space)){
                		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
				Load();
            			}
       			}
    		}

    	void AddPoint(int point){
        	m_Points += point;
        	ScoreText.text = $"Score : {m_Points}";
    	}

    	public void GameOver(){
		m_GameOver = true;
        	GameOverText.SetActive(true);
		Save();
		}
	[System.Serializable]
    	public class SaveData{
		public int HighScore;
		public string HSName;
	}
    	public void Save(){
		SaveData data = new SaveData();
		data.HSName = HSName;
		data.HighScore = NewHS;
		string json = JsonUtility.ToJson(data);
		File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    	}
    	public void Load(){
		string path = Application.persistentDataPath + "/savefile.json";
		if (File.Exists(path)){
			string json = File.ReadAllText(path);
			SaveData data = JsonUtility.FromJson<SaveData>(json);
			NewHS = data.HighScore;
			HSName = data.HSName;
			}
		}

}
