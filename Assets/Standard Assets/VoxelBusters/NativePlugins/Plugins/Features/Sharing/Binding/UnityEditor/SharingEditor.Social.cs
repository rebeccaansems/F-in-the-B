using UnityEngine;
using System.Collections;

#if USES_SHARING && UNITY_EDITOR
using VoxelBusters.DebugPRO;

namespace VoxelBusters.NativePlugins
{
	using Internal;

	public partial class SharingEditor : Sharing 
	{
		#region Overriden API's 
		
		public override bool IsFBShareServiceAvailable ()
		{
			Console.LogError(Constants.kDebugTag, Constants.kNotSupportedInEditor);

			return base.IsTwitterShareServiceAvailable();
		}
		
		public override bool IsTwitterShareServiceAvailable ()
		{
			Console.LogError(Constants.kDebugTag, Constants.kNotSupportedInEditor);

			return base.IsTwitterShareServiceAvailable();
		}
		
		#endregion
	}
}
#endif