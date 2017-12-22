using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using VoxelBusters.Utility;

namespace VoxelBusters.NativePlugins
{
	using Internal;

	/// <summary>
	/// For getting user reviews, this class provides unique way to prompt user based on configured settings.
	/// </summary>
	public class RateMyApp 
	{
		#region Fields
	
		private 	RateMyAppSettings		m_rateMyAppSettings;
		private		IRateMyAppController	m_controller;

		#endregion

		#region Properties

		public IRateMyAppDelegate Delegate
		{
			private get;
			set;
		}

		#endregion

		#region Constructors

		public RateMyApp (IRateMyAppController _controller)
			: this (_settings: NPSettings.Utility.RateMyApp, _controller: _controller)
		{}

		public RateMyApp (RateMyAppSettings _settings, IRateMyAppController _controller)
		{
			// Set properties
			m_rateMyAppSettings	= _settings;
			m_controller		= _controller;

			MarkIfLaunchIsFirstTime();
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Checks if review prompt needs to be shown as per the settings done. This needs to be constantly called to check if conditions are met for showing a review prompt.
		/// </summary>
		public void AskForReview ()
		{
			if (!CanAskForReview())
				return;

			m_controller.ExecuteRoutine(ShowDialogRoutine());
		}

		/// <summary>
		/// Show review prompt now irrespective of settings.
		/// </summary>
		public void AskForReviewNow ()
		{
			ShowDialog();
		}

		public void RecordAppLaunch ()
		{
			int _appUsageCount	= PlayerPrefs.GetInt(m_controller.GetKeyNameAppUsageCount(), 0) + 1;

			// Write to disk 
			PlayerPrefs.SetInt(m_controller.GetKeyNameAppUsageCount(), _appUsageCount);
			PlayerPrefs.Save();
		}

		#endregion

		#region Private Methods

		private void MarkIfLaunchIsFirstTime ()
		{
			bool	_keyFound			= (PlayerPrefs.GetInt(m_controller.GetKeyNameShowPromptAfter(), -1) != -1);
			bool	_isFirstTimeLaunch	= (!_keyFound)
				? true
				: IsFirstTimeLaunch();

			PlayerPrefs.SetInt(
				key: m_controller.GetKeyNameIsFirstTimeLaunch(), 
				value: _isFirstTimeLaunch 
				? 1
				: 0
			);	
		}

		private int GetAppUsageCount ()
		{
			return PlayerPrefs.GetInt(m_controller.GetKeyNameAppUsageCount(), 0);
		}

		private bool IsFirstTimeLaunch ()
		{
			return PlayerPrefs.GetInt(m_controller.GetKeyNameIsFirstTimeLaunch(), 0) == 1;
		}

		private bool CanAskForReview ()
		{
			try
			{
				// Check if user has asked not to show rate
				if (PlayerPrefs.GetInt(m_controller.GetKeyNameDontShow(), 0) == 1)
					return false;
				
				// Check if this version or previous version was already rated
				string		_lastVersionReviewed	= PlayerPrefs.GetString(m_controller.GetKeyNameVersionLastRated());
				if (!string.IsNullOrEmpty(_lastVersionReviewed))
				{
					string	_currentVersion			= VoxelBusters.Utility.PlayerSettings.GetBundleVersion();
					if (_currentVersion.CompareTo(_lastVersionReviewed) <= 0)
						return false;
				}

				// Find out whether app was just installed and is used for first time
				// If so, set hours after which rate me is prompted for first time
				DateTime 	_utcNow					= DateTime.UtcNow;
				int 		_showPromptAfterHours	= PlayerPrefs.GetInt(m_controller.GetKeyNameShowPromptAfter(), -1);

				if (_showPromptAfterHours == -1)
				{
					_showPromptAfterHours	= m_rateMyAppSettings.ShowFirstPromptAfterHours;
						
					PlayerPrefs.SetInt(m_controller.GetKeyNameShowPromptAfter(), m_rateMyAppSettings.ShowFirstPromptAfterHours);
					PlayerPrefs.SetString(m_controller.GetKeyNamePromptLastShown(), _utcNow.ToString());
				} 

				// Check for rest of trigger conditions
				string 		_promptLastShownOnString	= PlayerPrefs.GetString(m_controller.GetKeyNamePromptLastShown());
				DateTime 	_promptLastShown			= DateTime.Parse(_promptLastShownOnString);
				int 		_hoursSincePromptLastShown  = (int)(_utcNow - _promptLastShown).TotalHours;
				int			_appUsageCount				= GetAppUsageCount();

				// Need to wait until time exceeds
				if (_showPromptAfterHours > _hoursSincePromptLastShown)
					return false;
				
				// Make sure usage count exceeds min count before showing prompt
				if (!IsFirstTimeLaunch())
				{
					if (_appUsageCount <= m_rateMyAppSettings.SuccessivePromptAfterLaunches)
						return false;
				}
				
				// Store information in the preference file
				PlayerPrefs.SetInt(m_controller.GetKeyNameIsFirstTimeLaunch(), 0);
				PlayerPrefs.SetInt(m_controller.GetKeyNameAppUsageCount(), 0);
				PlayerPrefs.SetString(m_controller.GetKeyNamePromptLastShown(), _utcNow.ToString());

				return true;
			}
			finally
			{
				PlayerPrefs.Save(); 
			}
		}

		private IEnumerator ShowDialogRoutine ()
		{
			if (Delegate != null)
			{
				while (!Delegate.CanShowRateMyAppDialog())
					yield return new WaitForSeconds(seconds: 1f);
			}

			ShowDialog();
		}

		private void ShowDialog ()
		{
			if (Delegate != null)
				Delegate.OnBeforeShowingRateMyAppDialog();

			List<string> _buttonList	= new List<string>();
			_buttonList.Add(m_rateMyAppSettings.RateItButtonText);
			_buttonList.Add(m_rateMyAppSettings.RemindMeLaterButtonText);

			if (!string.IsNullOrEmpty(m_rateMyAppSettings.DontAskButtonText))
				_buttonList.Add(m_rateMyAppSettings.DontAskButtonText);

			m_controller.ShowDialog(
				m_rateMyAppSettings.Title, 		
				m_rateMyAppSettings.Message, 
				_buttonList.ToArray(), 	
				(_buttonName) =>
				{
					if (_buttonName.Equals(m_rateMyAppSettings.RemindMeLaterButtonText))
					{
						OnPressingRemindMeLaterButton();
					}
					else if (_buttonName.Equals(m_rateMyAppSettings.RateItButtonText))
					{
						OnPressingRateItButton();
					}
					else
					{
						OnPressingDontShowButton();
					}

					PlayerPrefs.Save();
				}
			);
		}
		
		private void OnPressingRemindMeLaterButton ()
		{
			PlayerPrefs.SetInt(m_controller.GetKeyNameShowPromptAfter(), m_rateMyAppSettings.SuccessivePromptAfterHours);

			m_controller.OnPressingRemindMeLaterButton();
		}

		private void OnPressingRateItButton ()
		{
			// Save current version to main bundle
			string _currentVersion	= VoxelBusters.Utility.PlayerSettings.GetBundleVersion();
			PlayerPrefs.SetString(m_controller.GetKeyNameVersionLastRated(), _currentVersion);

			m_controller.OnPressingRateItButton();
		}

		private void OnPressingDontShowButton ()
		{
			PlayerPrefs.SetInt(m_controller.GetKeyNameDontShow(), 1);

			m_controller.OnPressingDontShowButton();
		}

		#endregion
	}
}