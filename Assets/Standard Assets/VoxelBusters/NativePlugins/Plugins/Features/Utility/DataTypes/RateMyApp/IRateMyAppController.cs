using UnityEngine;
using System.Collections;

namespace VoxelBusters.NativePlugins.Internal
{
	public interface IRateMyAppController 
	{
		#region Methods

		string GetKeyNameIsFirstTimeLaunch ();
		string GetKeyNameVersionLastRated ();  
		string GetKeyNameShowPromptAfter ();  
		string GetKeyNamePromptLastShown ();  	
		string GetKeyNameDontShow ();  	        
		string GetKeyNameAppUsageCount ();  

		void ExecuteRoutine (IEnumerator _routine);		

		void ShowDialog (string _title, string _message, string[] _buttons, UI.AlertDialogCompletion _onCompletion);
		void OnPressingRemindMeLaterButton ();
		void OnPressingRateItButton ();
		void OnPressingDontShowButton ();

		#endregion
	}
}