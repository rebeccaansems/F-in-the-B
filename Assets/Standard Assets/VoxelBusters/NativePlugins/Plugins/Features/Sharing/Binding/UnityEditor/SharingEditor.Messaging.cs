using UnityEngine;
using System.Collections;

#if USES_SHARING && UNITY_EDITOR
using VoxelBusters.DebugPRO;

namespace VoxelBusters.NativePlugins
{
	using Internal;

	public partial class SharingEditor : Sharing 
	{
		#region Methods

		public override bool IsMessagingServiceAvailable ()
		{
			Console.LogError(Constants.kDebugTag, Constants.kNotSupportedInEditor);

			return base.IsMessagingServiceAvailable();
		}

		#endregion
	}
}
#endif