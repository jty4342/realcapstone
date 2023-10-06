using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;



public class Timer : MonoBehaviour
{
    
    public TextMeshProUGUI timerText; // Timer UI 텍스트
    public TextMeshProUGUI stage1TimeText;
    public TextMeshProUGUI stage2TimeText;
    public TextMeshProUGUI stage3TimeText;
    public TextMeshProUGUI stage4TimeText;
    public TextMeshProUGUI totalTimeText; // 총 시간 UI 텍스트
    public TextMeshProUGUI decisionTimeText; // 결정 시간 UI 텍스트
    public Button startButton; // Start 버튼
    public CanvasGroup startButtonCanvasGroup; // Start 버튼의 캔버스 그룹
    
    private bool isTimerActive=false; // 타이머 작동 여부
    private float timer; // 타이머 값
    
    private SavePoint currentSavePoint;
    
    public bool isStage1Completed { get; private set; }
    public bool isStage2Completed { get; private set; }
    public bool isStage3Completed { get; private set; }
    public bool isStage4Completed { get; private set; }

    public SavePoint savePoint;
    public SavePoint savePoint1; // Reference to SavePoint 1
    public SavePoint savePoint2; // Reference to SavePoint 2
    public SavePoint savePoint3; // Reference to SavePoint 3

    public float stage1Time = 0f; // Stage 1 시간 
    public float stage2Time = 0f; // Stage 2 시간 
    public float stage3Time = 0f; // Stage 3 시간 
    public float stage4Time = 0f; // Stage 4 시간 

    public float stage1Duration; // Stage 1 시간 기록
    public float stage2Duration; // Stage 2 시간 기록
    public float stage3Duration; // Stage 3 시간 기록
    public float stage4Duration; // Stage 4 시간 기록
    public float totalDuration; // Total duration
    public float decisionDuration; // Decision duration
    public Score scoreInstance;
    private bool stage4DurationUpdated = false;
    public void restart () {Start();}
        
    
     // 스코어 점수
    
    private void OnDestroy()
    {
        SavePoint[] savePoints = FindObjectsOfType<SavePoint>();
        if (savePoints != null)
        {
            foreach (SavePoint sp in savePoints)
            {
                sp.TouchSavePointEvent -= OnTouchSavePoint;
            }
        }
    }
    public void OnTouchSavePoint(int stageIndex)
    {
       switch(stageIndex)
        {
            case 0:
                if (!isStage1Completed)
                {
                    isStage1Completed = true;
                    stage1Duration = timer;
                    Debug.Log("Stage 1 completed!");
                    UpdateStageTimeUI(stage1TimeText, stage1Duration);
                    StartStage2Timer();
                }
                break;
            case 1:
                if (!isStage2Completed)
                {
                    isStage2Completed = true;
                    stage2Duration = timer - stage1Duration;
                    Debug.Log("Stage 2 completed!");
                    UpdateStageTimeUI(stage2TimeText, stage2Duration);
                    StartStage3Timer();
                }
                break;
            case 2:
                if (!isStage3Completed)
                {
                    isStage3Completed = true;
                    stage3Duration = timer - stage1Duration - stage2Duration;
                    Debug.Log("Stage 3 completed!");
                    UpdateStageTimeUI(stage3TimeText, stage3Duration);
                    StartStage4Timer();
                }
                break;
            case 3:
                if (!isStage4Completed)
                {
                    isStage4Completed = true;
                    stage4Duration = timer - stage1Duration - stage2Duration - stage3Duration;
                    Debug.Log("Stage 4 completed!");
                    UpdateStageTimeUI(stage4TimeText, stage4Duration);
                    EndGame();
                }
                break;
            default:
                Debug.LogWarning("Invalid stageIndex value: " + stageIndex);
                break;
        }
        UpdateTotalDurationAndDecisionDuration(); // 여기에 추가.
    }

    public float GetTimer()
    {
        return timer;
    }
    private void Start()
    {
        // 게임 시작 시 초기화
        ResetGame();
        StartTimer();
        isStage1Completed = false;
        isStage2Completed = false;
        isStage3Completed = false;
        isStage4Completed = false;
        SavePoint[] savePoints = FindObjectsOfType<SavePoint>();
        foreach (SavePoint sp in savePoints)
        {
            sp.TouchSavePointEvent += (int stageIndex) => OnTouchSavePoint(stageIndex);
        }
        /*if (startButton != null)
        {
            startButton.onClick.AddListener(OnStartButtonClick);
        }*/
        GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag("free_male_1");
        foreach (GameObject obj in taggedObjects)
        {
            // 게임 오브젝트 내에서 Score 스크립트를 찾습니다.
            Score foundScore = obj.GetComponentInChildren<Score>();

            if (foundScore != null)
            {
                scoreInstance = foundScore;
                break;
            }
        }


        if(scoreInstance == null)
        {
            Debug.LogWarning("Score instance is not assigned in the inspector!");
        }
    }
    public void StartTimer()
    {
        // Start the timer
        isTimerActive = true;
    }
    public void StopTimer()
    {
        // Stop the timer
        isTimerActive = false;
    }

    public void StartStage1Timer()
    {
        isStage1Completed = false;
        stage1Time = Time.time;
    }

    public void StartStage2Timer()
    {
        isStage2Completed = false;
        stage2Time = Time.time;
        
    }

    public void StartStage3Timer()
    {
        isStage3Completed = false;
        stage3Time = Time.time;
        
    }

    public void StartStage4Timer()
    {
        isStage4Completed = false;
        stage4Time = Time.time;
        
    }

    public float GetStage1Time()
    {
        return stage1Time;
    }

    public float GetStage2Time()
    {
        return stage2Time;
    }

    public float GetStage3Time()
    {
        return stage3Time;
    }

    public float GetStage4Time()
    {
        return stage4Time;
    }
    public void SetStage1Completed()
    {
        isStage1Completed = true;
    }

    public void SetStage2Completed()
    {
        isStage2Completed = true;
    }

    public void SetStage3Completed()
    {
        isStage3Completed = true;
    }

    public void SetStage4Completed()
    {
        isStage4Completed = true;
    }

    
    private void Update()
    {
        // 타이머가 작동 중이면 갱신
        
        
        

        if (isTimerActive)
        {   
           
            timer += Time.deltaTime;
            //totalDuration = timer;
            UpdateTimerUI();
        }
        if (isStage4Completed && !stage4DurationUpdated)
        {
            stage4DurationUpdated = true;
            UpdateTotalDurationAndDecisionDuration();
            StopTimer();
            
        }

        
    }
    private void UpdateTotalDurationAndDecisionDuration()
    {
        int currentScore = scoreInstance.CurrentScore; 
        float totalDuration = timer;//stage1Duration + stage2Duration + stage3Duration + stage4Duration;
        UpdateTotalTimeUI(totalDuration);
        Debug.Log("Total Duration: " + totalDuration);
        
        Debug.Log("Current Score: " + currentScore);

        float decisionDuration = totalDuration - (currentScore * 2);
        UpdateDecisionTimeUI(decisionDuration);
        Debug.Log("Decision Duration: " + decisionDuration);
          
        
    }
    
    private void UpdateTimerUI()
    {
        // 타이머 UI 업데이트
        
        float hours = Mathf.FloorToInt(timer / 3600);
        float minutes = Mathf.FloorToInt((timer % 3600) / 60);
        float seconds = Mathf.FloorToInt(timer % 60);

        if (timerText != null)
        {
            string timerString = string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
            timerText.text = "Time: " + timerString;
        }
        
    }
    
    public void RecordStageTime(float stageTime)
    {
        StartStage1Timer();
        if (currentSavePoint != null)
        {
            currentSavePoint.stageTime = stageTime;
            if (!isStage1Completed && currentSavePoint == savePoint)
            {
                isStage1Completed = true;
                stage1Time = stageTime + stage1Duration;

                Debug.Log("Stage 1 Time: " + FormatTime(stage1Time));
                UpdateStageTimeUI(stage1TimeText, stage1Time);
                UpdateTotalDurationAndDecisionDuration();
                StartStage2Timer();
            }
            else if (!isStage2Completed && currentSavePoint == savePoint1)
            {
                isStage2Completed = true;
                stage2Time = stageTime + stage2Duration;

                Debug.Log("Stage 2 Time: " + FormatTime(stage2Time));
                UpdateStageTimeUI(stage2TimeText, stage2Time);
                UpdateTotalDurationAndDecisionDuration();
                StartStage3Timer();
            }
            else if (!isStage3Completed && currentSavePoint == savePoint2)
            {
                isStage3Completed = true;
                stage3Time = stageTime + stage3Duration;

                Debug.Log("Stage 3 Time: " + FormatTime(stage3Time));
                UpdateStageTimeUI(stage3TimeText, stage3Time);
                UpdateTotalDurationAndDecisionDuration();
                StartStage4Timer();
            }
            else if (!isStage4Completed && currentSavePoint == savePoint3)
            {
                isStage4Completed = true;
                stage4Time = stageTime + stage4Duration;

                Debug.Log("Stage 4 Time: " + FormatTime(stage4Time));
                UpdateStageTimeUI(stage4TimeText, stage4Time);
                UpdateTotalDurationAndDecisionDuration();
                
            }
        }
    }
    public string FormatTime(float time)
    {
        int hours = Mathf.FloorToInt(time / 3600);
        int minutes = Mathf.FloorToInt((time % 3600) / 60);
        int seconds = Mathf.FloorToInt(time % 60);

        return string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
    }
    private void UpdateTotalTimeUI(float totalDuration)
    {
        // 총 시간 UI 업데이트
        if (totalTimeText != null)
        {
            totalTimeText.text = "Total Time: " + FormatTime(totalDuration);
        }
    }
    private void UpdateDecisionTimeUI(float decisionDuration)
    {
        // 결정 시간 UI 업데이트
        if (decisionTimeText != null)
        {
            decisionTimeText.text = "Decision Time: " + FormatTime(decisionDuration);
        }
    }
    public void UpdateStageTimeUI(TextMeshProUGUI stageTimeText, float stageDuration)
    {
        // 스테이지 시간 UI 업데이트
        if (stageTimeText != null)
        {
            stageTimeText.text = "Stage " + stageTimeText.name.Substring(5) + ": " + FormatTime(stageDuration);
            
        }
    }
    public void StartGame()
    {
        ResetTimer();
        isTimerActive = true;
        startButtonCanvasGroup.interactable = false;
        startButtonCanvasGroup.blocksRaycasts = false;
        
    }

    public void EndGame()
    {
        StopTimer();
        isTimerActive = false;
        startButtonCanvasGroup.interactable = true;
        startButtonCanvasGroup.blocksRaycasts = true;
    }
    public void ResetGame()
    {
        // 게임 초기화
        isTimerActive = false;
        timer = 0f;

        isStage1Completed = false;
        isStage2Completed = false;
        isStage3Completed = false;
        isStage4Completed = false;

        stage1Time = 0f; // Stage 1 시간 설정 
        stage2Time = 0f; // Stage 2 시간 설정
        stage3Time = 0f; // Stage 3 시간 설정 
        stage4Time = 0f; // Stage 4 시간 설정 

        ResetTimer();
        UpdateTimerUI();

        // Start 버튼 활성화
        startButtonCanvasGroup.alpha = 1;
        startButtonCanvasGroup.interactable = true;
        startButtonCanvasGroup.blocksRaycasts = true;
    }
    private void ResetTimer()
    {
        stage1Duration = 0f;
        stage2Duration = 0f;
        stage3Duration = 0f;
        stage4Duration = 0f;
    }
    
    private void GameOver()
    {

        

        // 게임 종료 처리
        // 여기에 원하는 게임 종료 로직을 작성하세요.
    }
}
