using DG.Tweening;

using UnityEngine;

namespace FlappyCat.Popups
{
	[CreateAssetMenu(fileName = nameof(BounceAppearanceSettings), menuName = "FlappyCat/" + nameof(BounceAppearanceSettings),
		order = 0)]
	public class BounceAppearanceSettings : ScriptableObject
	{
		[field: SerializeField]
		public VectorTimePair[] AppearanceVectorTimePairs  { get; private set; }
		
		[field: SerializeField]
		public VectorTimePair[] HidingVectorTimePairs  { get; private set; }
		
		[field: SerializeField]
		public Ease AppearanceEaseType { get; private set; }
		
		[field: SerializeField]
		public Ease HidingEaseType { get; private set; }
	}
}
