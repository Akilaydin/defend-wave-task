using TMPro;

using UnityEngine;

namespace DefendTheWave.Player.Health
{
	public class PlayerHealthView : MonoBehaviour
	{
		[field: SerializeField] public TMP_Text HealthText { get; private set; }
	}
}
