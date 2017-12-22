﻿using UnityEngine;
using System.Collections;

#if UNITY_ANDROID
using VoxelBusters.DebugPRO;

namespace VoxelBusters.NativePlugins
{
	using Internal;

	public partial class SharingAndroid : Sharing
	{
		private const string	kClosed	= "closed";
		private const string	kFailed	= "failed";

		#region Parse Methods
		
		protected override void ParseSharingFinishedData (string _resultString, out eShareResult _shareResult)
		{
			if (_resultString.Equals(kClosed) || _resultString.Equals(kFailed))
			{
				_shareResult = eShareResult.CLOSED;
			}
			else
			{
				//Return always closed if not implemented.
				Console.LogWarning(Constants.kDebugTag, "This status not implemented. sending closed event. [Fix this] " + _resultString);
				_shareResult = eShareResult.CLOSED;
			}
		}
		
		#endregion

		#region Response Methods
		
		protected override string SharingFailedResponse ()
		{
			return kFailed;
		}
		
		#endregion
	}
}
#endif