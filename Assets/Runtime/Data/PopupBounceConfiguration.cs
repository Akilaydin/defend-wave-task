using DefendTheWave.Common;

using UnityEngine;

namespace DefendTheWave.Data
{
	[CreateAssetMenu(fileName = nameof(PopupBounceConfiguration), menuName = GameConstants.ScriptableObjectsRoot + nameof(PopupBounceConfiguration), order = 0)]
	public class PopupBounceConfiguration : ScriptableObject
	{
		[field: SerializeField] public VectorTimeMap[] VectorTimeMaps { get; private set; }
	}
}
