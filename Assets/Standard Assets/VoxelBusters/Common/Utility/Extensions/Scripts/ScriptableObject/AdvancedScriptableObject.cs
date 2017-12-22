using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using System.IO;
using System.Text;

namespace VoxelBusters.Utility
{
	public abstract class AdvancedScriptableObject <T> : ScriptableObject where T : ScriptableObject
	{
		#region Static Fields

		private		static		T		instance	= null;

		protected	static		string	assetContainerFolderName	= "VoxelBusters";

		#endregion

		#region Properties

		public static T Instance
		{
			get 
			{ 
				if (instance == null)
					instance	= GetAsset(typeof(T).Name);		

				return instance;
			}
		}

		#endregion

		#region Unity Methods

		protected virtual void Reset ()
		{}

		protected virtual void OnEnable ()
		{
			if (instance == null)
			{
				instance	= this as T;
			}
		}

		protected virtual void OnDisable ()
		{}

		protected virtual void OnDestroy ()
		{}

		#endregion

		#region Public Methods

		public virtual void Save ()
		{
			// Save operation is allowed only on Unity Editor
			// and that too while player is in edit mode
#if UNITY_EDITOR
			if (EditorApplication.isPlaying || EditorApplication.isPaused)
			{
				return;
			}

			// Save the changes
			EditorUtility.SetDirty(this);
			AssetDatabase.SaveAssets();

			// Ask Unity to import changed files
			AssetDatabase.Refresh();
#endif
		}

		#endregion

		#region Static Methods

		private static string GetPathRelativeToResources (string _assetName)
		{
			string	_assetPath	= (assetContainerFolderName == null) 
				? _assetName 
				: string.Format("{0}/{1}", assetContainerFolderName, _assetName);

			return _assetPath;
		}

		private static string GetAssetPath (string _assetName)
		{
			return "Assets/Resources/" + GetPathRelativeToResources(_assetName) + ".asset";
		}

		public static T GetAsset (string _assetName)
		{
#if UNITY_EDITOR
			string	_assetPath	= GetAssetPath(_assetName);
			T		_asset		= (T)AssetDatabase.LoadAssetAtPath(_assetPath, typeof(T));

			if (_asset == null)
			{
				_asset			= CreateAsset(_assetPath);
			}

			return _asset;
#else
			string _assetPath	= GetPathRelativeToResources(_assetName);

			return Resources.Load<T>(_assetPath);
#endif
		}

#if UNITY_EDITOR
		public static T CreateAsset (string _assetPath)
		{
			string[]	_pathComponents		= _assetPath.Split('/');
			string		_parentFolderPath	= _pathComponents[0];
			int			_pIter				= 0;

			while (++_pIter < (_pathComponents.Length - 1))
			{
				string	_currentFolderPath	= _parentFolderPath + "/" + _pathComponents[_pIter];
				
				if (!AssetsUtility.FolderExists(_currentFolderPath))
				{
					AssetDatabase.CreateFolder(_parentFolderPath, _pathComponents[_pIter]);
				}

				// Update parent folder reference
				_parentFolderPath	= _currentFolderPath;
			}

			// Ask Unity to import changes
			AssetDatabase.Refresh();

			// Create the asset
			T 			_newAsset			= ScriptableObject.CreateInstance<T>();
			AssetDatabase.CreateAsset(_newAsset, AssetDatabase.GenerateUniqueAssetPath(_assetPath));

			// Save the changes
			(_newAsset as AdvancedScriptableObject<T>).Save();

			return _newAsset;
		}
#endif

		#endregion
	}
}