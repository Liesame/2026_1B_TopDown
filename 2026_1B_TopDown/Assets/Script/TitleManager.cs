using UnityEngine;

public class TitleManager : MonoBehaviour
{
   public void GameStartButton()
    {
        GameManager.Instance.StartGame();
    }

    public void ExitGame()
    {
        Debug.Log("게임 종료 버튼 클릭됨!");

        Application.Quit();
    }
}
