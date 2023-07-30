using UnityEngine;
using UnityEngine.UI;

namespace DefendTheWave.GUI.Views
{
	public class LostPopupView : BasePopupView
	{
		[field: SerializeField] public Button RestartButton { get; private set; }
	}
}
