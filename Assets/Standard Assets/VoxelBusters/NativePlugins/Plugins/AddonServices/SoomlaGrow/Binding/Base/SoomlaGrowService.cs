using UnityEngine;
using System.Collections;

#if USES_SOOMLA_GROW
using VoxelBusters.DebugPRO;

namespace VoxelBusters.NativePlugins
{
	using Internal;

	public partial class SoomlaGrowService : MonoBehaviour 
	{
		#region Constants

		private		const	string	kReferrerName	= "voxelbusters";

		#endregion

		#region Unity Methods

		private void Awake ()
		{
			SoomlaGrowServiceSettings	_settings	= NPSettings.AddonServicesSettings.SoomlaGrowService;

			// Initialise component
			Initialise(_settings.GameKey, _settings.EnvironmentKey, kReferrerName);
		}

		private void OnEnable ()
		{
#if USES_BILLING
			// Register for billing events
			Billing.DidFinishProductPurchaseEvent		+= OnDidFinishProductPurchase;
			Billing.DidFinishRestoringPurchasesEvent	+= OnDidFinishRestoringPurchases;
#endif
		}

		private void OnDisable ()
		{
#if USES_BILLING
			// Unregister from billing events
			Billing.DidFinishProductPurchaseEvent		-= OnDidFinishProductPurchase;
			Billing.DidFinishRestoringPurchasesEvent	-= OnDidFinishRestoringPurchases;
#endif
		}

		#endregion

		#region Methods

		protected virtual void Initialise (string _gameKey, string _environmentKey, string _referrerName)
		{
			Console.Log(Constants.kDebugTag, "[SoomlaGrow] Initialising SDK.");
		}

		#endregion

		#region Billing Methods

		internal void ReportOnBillingSupported (bool _isSupported)
		{
			if (_isSupported)
				ReportOnBillingSupported();
			else
				ReportOnBillingNotSupported();
		}

		protected virtual void ReportOnBillingSupported ()
		{
			Console.Log(Constants.kDebugTag, "[SoomlaGrow] Reporting: OnBillingSupported.");
		}

		protected virtual void ReportOnBillingNotSupported ()
		{
			Console.Log(Constants.kDebugTag, "[SoomlaGrow] Reporting: OnBillingNotSupported.");
		}
		
		internal virtual void ReportOnBillingPurchaseStarted (string _productID)
		{
			Console.Log(Constants.kDebugTag, "[SoomlaGrow] Reporting: OnBillingPurchaseStarted.");
		}

		internal virtual void ReportOnBillingPurchaseFinished (string _productID, long _priceInMicros, string _currencyCode)
		{			
			Console.Log(Constants.kDebugTag, "[SoomlaGrow] Reporting: OnBillingPurchaseFinished.");
		}

		internal virtual void ReportOnBillingPurchaseCancelled (string _productID)
		{
			Console.Log(Constants.kDebugTag, "[SoomlaGrow] Reporting: OnBillingPurchaseCancelled.");
		} 

		internal virtual void ReportOnBillingPurchaseFailed (string _productID)
		{		
			Console.Log(Constants.kDebugTag, "[SoomlaGrow] Reporting: OnBillingPurchaseFailed.");
		}

		internal virtual void ReportOnBillingPurchasesRestoreStarted ()
		{
			Console.Log(Constants.kDebugTag, "[SoomlaGrow] Reporting: OnBillingRestoreStarted.");
		}

		internal virtual void ReportOnBillingPurchasesRestoreFinished (bool _success)
		{			
			Console.Log(Constants.kDebugTag, "[SoomlaGrow] Reporting: OnBillingRestoreFinished.");
		}

		internal virtual void ReportOnBillingPurchaseVerificationFailed ()
		{			
			Console.Log(Constants.kDebugTag, "[SoomlaGrow] Reporting: OnBillingVerificationFailed.");
		}
	
		#endregion

		#region Social Feature Methods

		internal virtual void ReportOnSocialLoginStarted (eSocialProvider _provider)
		{		
			Console.Log(Constants.kDebugTag, "[SoomlaGrow] Reporting: OnSocialLoginStarted.");
		}

		internal virtual void ReportOnSocialLoginFinished (eSocialProvider _provider, string _userID)
		{		
			Console.Log(Constants.kDebugTag, "[SoomlaGrow] Reporting: OnSocialLoginFinished.");
		}

		internal virtual void ReportOnSocialLoginCancelled (eSocialProvider _provider)
		{
			Console.Log(Constants.kDebugTag, "[SoomlaGrow] Reporting: OnSocialLoginCancelled.");
		}

		internal virtual void ReportOnSocialLoginFailed (eSocialProvider _provider)
		{			
			Console.Log(Constants.kDebugTag, "[SoomlaGrow] Reporting: OnSocialLoginFailed.");
		}

		internal virtual void ReportOnSocialLogoutStarted (eSocialProvider _provider)
		{		
			Console.Log(Constants.kDebugTag, "[SoomlaGrow] Reporting: OnSocialLogoutStarted.");
		}

		internal virtual void ReportOnSocialLogoutFinished (eSocialProvider _provider)
		{			
			Console.Log(Constants.kDebugTag, "[SoomlaGrow] Reporting: OnSocialLogoutFinished.");
		}

		internal virtual void ReportOnSocialLogoutFailed (eSocialProvider _provider)
		{	
			Console.Log(Constants.kDebugTag, "[SoomlaGrow] Reporting: OnSocialLogoutFailed.");
		}

		internal virtual void ReportOnGetContactsStartedForProvider (eSocialProvider _provider)
		{	
			Console.Log(Constants.kDebugTag, "[SoomlaGrow] Reporting: OnGetContactsStartedForProvider.");
		}
		
		internal virtual void ReportOnGetContactsFinishedForProvider (eSocialProvider _provider)
		{
			Console.Log(Constants.kDebugTag, "[SoomlaGrow] Reporting: OnGetContactsFinishedForProvider.");
		}
		
		internal virtual void ReportOnGetContactsFailedForProvider (eSocialProvider _provider)
		{
			Console.Log(Constants.kDebugTag, "[SoomlaGrow] Reporting: OnGetContactsFailedForProvider.");
		}
		
		internal virtual void ReportOnSocialActionStarted (eSocialActionType _actionType, eSocialProvider _provider)
		{
			Console.Log(Constants.kDebugTag, "[SoomlaGrow] Reporting: OnSocialActionStarted.");
		}
		
		internal virtual void ReportOnSocialActionFinished (eSocialActionType _actionType, eSocialProvider _provider)
		{
			Console.Log(Constants.kDebugTag, "[SoomlaGrow] Reporting: OnSocialActionFinished.");
		}
		
		internal virtual void ReportOnSocialActionCancelled (eSocialActionType _actionType, eSocialProvider _provider)
		{
			Console.Log(Constants.kDebugTag, "[SoomlaGrow] Reporting: OnSocialActionCancelled.");
		}
		
		internal virtual void ReportOnSocialActionFailed (eSocialActionType _actionType, eSocialProvider _provider)
		{			
			Console.Log(Constants.kDebugTag, "[SoomlaGrow] Reporting: OnSocialActionFailed.");
		}

		#endregion

		#region Game Services Methods

		internal virtual void ReportOnLatestScore (string _scoreID, double _latestScore)
		{
			Console.Log(Constants.kDebugTag, "[SoomlaGrow] Reporting: OnLatestScore.");
		}

		#endregion

		#region Misc. Methods
	
		internal virtual void ReportOnUserRating ()
		{
			Console.Log(Constants.kDebugTag, "[SoomlaGrow] Reporting: OnUserRating.");
		}

		#endregion

		#region Callback Methods

#if USES_BILLING
		private void OnDidFinishProductPurchase (BillingTransaction _transaction)
		{
			string	_productID	= _transaction.ProductIdentifier;

			// Based on receipt verification, report event
			if (_transaction.VerificationState == eBillingTransactionVerificationState.SUCCESS)
			{
				if (_transaction.TransactionState == eBillingTransactionState.PURCHASED)
				{
					BillingProduct 	_productInfo	= NPBinding.Billing.GetStoreProduct(_productID);
					
					if (_productInfo == null)
					{
						Console.Log(Constants.kDebugTag, "[SoomlaGrow] The operation could not be completed because product information is not available.");
					}
					else
					{
						ReportOnBillingPurchaseFinished(_productID, (long)(_productInfo.Price * 1000000), _productInfo.CurrencyCode);
					}
				}
				else if (_transaction.TransactionState == eBillingTransactionState.FAILED)
				{
					if (_productID == null)
					{
						Console.Log(Constants.kDebugTag, "[SoomlaGrow] The operation could not be completed because product identifier information is not available.");
					}
					else
					{
						ReportOnBillingPurchaseFailed(_productID);
					}
				}
			}
			else if (_transaction.VerificationState == eBillingTransactionVerificationState.FAILED)
			{
				ReportOnBillingPurchaseVerificationFailed();

				return;
			}
		}

		private void OnDidFinishRestoringPurchases (BillingTransaction[] _transactions, string _error)
		{
			ReportOnBillingPurchasesRestoreFinished(_error == null);
		}
#endif

		#endregion
	}
}
#endif