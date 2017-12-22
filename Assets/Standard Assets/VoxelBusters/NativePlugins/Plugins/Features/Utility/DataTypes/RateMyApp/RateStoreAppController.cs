using UnityEngine;
using System.Collections;

namespace VoxelBusters.NativePlugins.Internal
{
	public class RateStoreAppController : IRateMyAppController 
	{
		#region Constants

		private const string kIsFirstTimeLaunch		= "np-is-first-time-launch";
		private const string kVersionLastRated   	= "np-version-last-rated";
		private const string kShowPromptAfter		= "np-show-prompt-after";
		private const string kPromptLastShown		= "np-prompt-last-shown";
		private const string kDontShow	           	= "np-dont-show";
		private const string kAppUsageCount			= "np-app-usage-count";

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
			NPBinding.Utility.StartCoroutine(_routine);
		}

		public void ShowDialog (string _title, string _message, string[] _buttons, UI.AlertDialogCompletion _onCompletion)
		{
			NPBinding.UI.ShowAlertDialogWithMultipleButtons(
				_title, 		
				_message, 
				_buttons, 	
				_onCompletion
			);
		}

		public void OnPressingRemindMeLaterButton ()
		{}

		public void OnPressingRateItButton ()
		{
			NPBinding.Utility.OpenStoreLink(NPSettings.Application.StoreIdentifier);
			
#if USES_SOOMLA_GROW
			NPBinding.SoomlaGrowService.ReportOnUserRating();
#endif
		}

		public void OnPressingDontShowButton ()
		{
		}

		#endregion
	}
}