using UnityEngine;
using UnityEngine.UI;

namespace DefendTheWave.GUI
{
	public class VictoryPopupView : BasePopupView
	{
		[field: SerializeField] public Button ContinueButton { get; private set; }
	}
}
