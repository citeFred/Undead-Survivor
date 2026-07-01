using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    [Header("# Game Objects")]
    public Player player;
    public PoolManager pool;
    public LevelUp uiLevelUp; // 레벨업 선택 UI

    [Header("# Game Controls")]
    public bool isLive; // 일시정지용
    public float gameTime;
    public float maxGameTime;

    [Header("# Player Data")]
    public int level;
    public int kill;
    public int exp;
    public List<int> nextExp = new List<int> { 3, 5, 10, 20, 150, 210, 280, 360, 450, 600 }; // 동적 배열을 위해 일반 배열 대신 리스트 자료구조 변경
    public int health;
    public int maxHealth = 100;
    
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        isLive = true;
        uiLevelUp.Select(0); // 기본 무기 (0:삽) 제공
        health = maxHealth; // 게임 시작시 체력을 최대체력으로 초기화
    }

    void Update()
    {
        if (!isLive) // 일시 정지 상태에서는 시간 누적 중단
            return;
        
        // 매 프레임마다 실제 흐른 시간을 누적
        gameTime += Time.deltaTime;

        // 최대 시간을 넘지 않도록 고정 (게임 종료 등 처리에 활용)
        if (gameTime > maxGameTime)
        {
            gameTime = maxGameTime;
        }
    }
    
    // 경험치 획득 및 레벨업 로직
    public void GetExp()
    {
        exp++;

        if (exp == nextExp[level])
        {
            level++;
            exp = 0;
            uiLevelUp.Show(); // 레벨업 -> 아이템 선택 UI 표시
            
            // 초기 레벨 이상 초과시, 경험치 테이블을 추가하면서 최대 경험치를 복사
            if (level >= nextExp.Count)
            {
                nextExp.Add(nextExp[nextExp.Count - 1]);
            }
        }
    }

    public void Stop()
    {
        isLive = false;
        Time.timeScale = 0; // 시간 흐름 속도 0 -> 게임 전체 정지
    }

    public void Resume()
    {
        isLive = true;
        Time.timeScale = 1; // 시간 흐름 속도 1 -> 정상 속도 복원
    }
}