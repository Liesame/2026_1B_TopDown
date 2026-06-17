using UnityEngine;
using System.IO;

public class GameDataManager : MonoBehaviour
{

    public static GameDataManager Instance;
    public GameSettingData gameSettingData;
    public SaveData saveData;
    public int isTutorialFinished;

    private string savePath;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            savePath = Application.persistentDataPath + "/saveData.json";

            LoadJsonData();
            LoadPlayerPrefs();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public int GetPlayerHp()
    {
        int baseHp = gameSettingData.startHp;
        int bonusHp = saveData.deathCount * gameSettingData.hpBonusPerDeath;

        return baseHp + bonusHp * saveData.deathCount;
    }

    public int GetPlayerAttack()
    {
        int baseAttack = gameSettingData.startAttack;
        int bonusAttack = saveData.deathCount * gameSettingData.atkBonusPerDeath;

        return baseAttack + bonusAttack * saveData.deathCount;
    }

    public float GetPlayerMoveSpeed()
    {
        return gameSettingData.playerMoveSpeed;
    }

    public void SaveGameResult()
    {
        saveData.deathCount++;

        SaveJsonData();
    }

    public void SaveJsonData()
    {
        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(savePath, json);

        Debug.Log("JSON 저장 완료: " + savePath);
    }

    public void LoadJsonData()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            saveData = JsonUtility.FromJson<SaveData>(json);
        }
        else
        {
            saveData = new SaveData();
            SaveJsonData();
        }
    }

    public void DeleteJsonData()
    {
        if (File.Exists(savePath))
        {
            File.Delete(savePath);
        }

        saveData = new SaveData();
        SaveJsonData();

        Debug.Log("JSON 저장 데이터 삭제");
    }

    public void LoadPlayerPrefs()
    {
        isTutorialFinished = PlayerPrefs.GetInt("TUTORIAL", 0);
    }

    public void SavePlayerPrefs()
    {
        PlayerPrefs.SetInt("TUTORIAL", isTutorialFinished);
        PlayerPrefs.Save();

        Debug.Log("PlayerPrefs 저장 완료");
    }

    public void DeletePlayerPrefs()
    {
        PlayerPrefs.DeleteKey("TUTORIAL");
        LoadPlayerPrefs();

        Debug.Log("PlayerPrefs 삭제 완료");
    }

    public void CheckAchievement(int achievementIndex)
    {
        if (achievementIndex >= 0 && achievementIndex < saveData.achievementCompleted.Length)
        {
            if (!saveData.achievementCompleted[achievementIndex])
            {
                saveData.achievementCompleted[achievementIndex] = true;
                Debug.Log($"도전과제 {achievementIndex} 달성!");
                SaveJsonData(); // 변경 즉시 저장
            }
        }
    }

    // 조건에 따른 자동 체크 예시
    public void UpdateProgress(string action)
    {
        switch (action)
        {
            case "KnifeGet":
                saveData.knifeGet = true;
                CheckAchievement(0); // 0번 과제: 칼 획득
                break;
            case "KillEnemy":
                saveData.killEnemy = true;
                CheckAchievement(2); // 1번 과제: 적 처치
                break;
            case "StartPlayer":
                saveData.startPlayer = true;
                CheckAchievement(1); // 1번 과제: 적 처치
                break;
            case "NextStage":
                saveData.nextStage = true;
                CheckAchievement(3); // 1번 과제: 적 처치
                break;
        }
    }

    public void OnEnemyDeath()
    {
        // 1. 데이터 업데이트
        GameDataManager.Instance.UpdateProgress("KillEnemy");

    }

    public void OnKnifeGet()
    {
        // 1. 데이터 업데이트
        GameDataManager.Instance.UpdateProgress("KnifeGet");
    }

    public void OnStartPlayer()
    {
        // 1. 데이터 업데이트
        GameDataManager.Instance.UpdateProgress("StartPlayer");
    }

    public void OnNextStage()
    {
        // 1. 데이터 업데이트
        GameDataManager.Instance.UpdateProgress("NextStage");
    }

    public void AddScore(int amount)
    {
        saveData.totalScore += amount;
        Debug.Log($"스코어 증가! 현재 스코어: {saveData.totalScore}");
        SaveJsonData(); // 변경 시마다 JSON에 자동 저장
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
