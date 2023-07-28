using TMPro;

using UnityEngine;

namespace Runtime.GUI
{
	public class HealthView : MonoBehaviour
	{
		[field: SerializeField] public TMP_Text HealthText { get; private set; }
	}
}
