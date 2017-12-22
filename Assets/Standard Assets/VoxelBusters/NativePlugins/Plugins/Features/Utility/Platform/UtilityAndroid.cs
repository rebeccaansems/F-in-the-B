﻿#if UNITY_ANDROID
using UnityEngine;
using System.Collections;

namespace VoxelBusters.NativePlugins.Internal
{
	public sealed partial class UtilityAndroid : IUtilityPlatform 
	{
		#region Constructors
		
		public UtilityAndroid ()
		{
			Plugin = AndroidPluginUtility.GetSingletonInstance(Native.Class.NAME);
		}
		
		#endregion
		
		#region Public Methods

		public void OpenStoreLink (string _applicationID)
		{
			Application.OpenURL("http://play.google.com/store/apps/details?id=" + _applicationID);
		}

		public void SetApplicationIconBadgeNumber (int _badgeNumber)
		{
			Plugin.Call(Native.Methods.SET_APPLICATION_ICON_BADGE_NUMBER, _badgeNumber);
		}

		#endregion
	}
}
#endif