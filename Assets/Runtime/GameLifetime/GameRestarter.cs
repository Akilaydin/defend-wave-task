using UnityEngine.SceneManagement;

namespace DefendTheWave.GameLifetime
{
	public class GameRestarter
	{
		public void RestartGame()
		{
			SceneManager.LoadScene(0);
		}
	}
}
