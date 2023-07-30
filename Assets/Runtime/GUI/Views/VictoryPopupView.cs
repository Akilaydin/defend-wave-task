using UnityEngine;
using UnityEngine.UI;

namespace DefendTheWave.GUI.Views
{
	public class VictoryPopupView : BasePopupView
	{
		[field: SerializeField] public Button ContinueButton { get; private set; }
	}
}
