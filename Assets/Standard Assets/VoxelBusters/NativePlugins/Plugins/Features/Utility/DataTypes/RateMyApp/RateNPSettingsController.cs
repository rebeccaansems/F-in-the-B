#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using VoxelBusters.Utility;
using UnityEditor;

namespace VoxelBusters.NativePlugins.Internal
{
	public class RateNPSettingsController : IRateMyAppController 
	{
		#region Constants

		private const string kIsFirstTimeLaunch		= "npsettings-is-first-time-launch";
		private const string kVersionLastRated   	= "npsettings-version-last-rated";
		private const string kShowPromptAfter		= "npsettings-show-prompt-after";
		private const string kPromptLastShown		= "npsettings-prompt-last-shown";
		private const string kDontShow	           	= "npsettings-dont-show";
		private const string kAppUsageCount			= "npsettings-app-usage-count";

		#endregion

		#region Public Methods

		public string GetKeyNameIsFirstTimeLaunch ()
		{
			return kIsFirstTimeLaunch;
		}

		public string GetKeyNameVersionLastRated ()
		{
			return kVersionLastRated;
		}

		public string GetKeyNameShowPromptAfter ()
		{
			return kShowPromptAfter;
		}

		public string GetKeyNamePromptLastShown ()
		{
			return kPromptLastShown;
		}

		public string GetKeyNameDontShow ()
		{
			return kDontShow;
		}

		public string GetKeyNameAppUsageCount ()
		{
			return kAppUsageCount;
		}

		public void ExecuteRoutine (IEnumerator _routine)
		{
			EditorUtils.StartCoroutine(_routine);
		}

		public void ShowDialog (string _title, string _message, string[] _buttons, UI.AlertDialogCompletion _onCompletion)
		{
			bool _okClicked	= EditorUtility.DisplayDialog(
				_title,
				_message,
				_buttons[0],
				_buttons[1]
			);

			if (_onCompletion != null)
			{
				_onCompletion(_okClicked
					? _buttons[0]
					: _buttons[1]
				);
			}
		}

		public void OnPressingRemindMeLaterButton ()
		{}
			
		public void OnPressingRateItButton ()
		{
			Application.OpenURL(Constants.kProductURL);
		}

		public void OnPressingDontShowButton ()
		{}

		#endregion
	}
}
#endif