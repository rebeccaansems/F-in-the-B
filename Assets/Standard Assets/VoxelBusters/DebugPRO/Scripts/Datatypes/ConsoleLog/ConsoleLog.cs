using UnityEngine;
using System.Collections;
using System.Diagnostics;
using System.Text;
using System.IO;
using System.Reflection;
using UnityObject = UnityEngine.Object;

namespace VoxelBusters.DebugPRO.Internal
{
	[System.Serializable]
	public struct ConsoleLog
	{
		#region Fields

		[SerializeField]
		private 	int		m_ID;
		[SerializeField]
		private 	int		m_tagID;
		[SerializeField]
		private 	string	m_message;
		[SerializeField]
		private 	eConsoleLogType	m_type;
		[SerializeField]
		private 	string	m_callStack;
		[SerializeField]
		private 	string	m_description;
#pragma warning disable
		[SerializeField]
		private 	string	m_callerFileName;
		[SerializeField]
		private 	int		m_callerFileLineNumber;
#pragma warning restore

		#endregion

		#region Property

		public int ID
		{
			get
			{
				return m_ID;
			}
			
			private set
			{
				m_ID	= value;
			}
		}

		public int TagID
		{
			get
			{
				return m_tagID;
			}
			
			private set
			{
				m_tagID	= value;
			}
		}

		public string Message
		{
			get
			{
				return m_message;
			}
			
			private set
			{
				m_message	= value;
			}
		}

		public eConsoleLogType Type
		{
			get
			{
				return m_type;
			}
			
			private set
			{
				m_type	= value;
			}
		}
		
		public UnityObject Context
		{
			get;
			private set;
		}
		
		public string CallStack
		{
			get
			{
				return m_callStack;
			}
			
			private set
			{
				m_callStack	= value;
			}
		}

		public string Description
		{
			get
			{
				return m_description;
			}
			
			private set
			{
				m_description	= value;
			}
		}

		#endregion

		#region Constructor

		public ConsoleLog (int _logID, int _tagID, string _message, eConsoleLogType _type, UnityEngine.Object _context) : this ()
		{
			// Set console log details
			ID			= _logID;
			TagID		= _tagID;
			Message		= _message;
			Type		= _type;
			Context		= _context;

			ExtractStackTraceDescription();
		}

		#endregion

		#region Methods

		private void ExtractStackTraceDescription ()
		{
			try
			{			
#if NETFX_CORE
				throw new System.Exception("Not supported");
#else
				StringBuilder	_stringBuilder	= new StringBuilder(64);
				StackTrace		_stackTrace		= new StackTrace(true);
				int 			_totalFrames	= _stackTrace.FrameCount;
				string 			_projectPath	= Path.GetFullPath(Application.dataPath + @"/../");
				int 			_sIndex			= 0;

				// Exclude internal calls
				for (_sIndex = 0; _sIndex < _totalFrames; _sIndex++)
				{
					StackFrame 	_stackFrame		= _stackTrace.GetFrame(_sIndex);
					MethodBase 	_method			= _stackFrame.GetMethod();
					string 		_className 		= _method.DeclaringType.FullName;
					
					if (!IsInternalCall(_className))
						break;
				}

				// Build stack trace description
				int				_topFrameIndex	= _sIndex;

				for (;_sIndex < _totalFrames; _sIndex++)
				{
					StackFrame 	_stackFrame		= _stackTrace.GetFrame(_sIndex);

					// Get caller info
					MethodBase 	_method			= _stackFrame.GetMethod();
					string 		_className 		= _method.DeclaringType.Name;
					
					_stringBuilder.AppendFormat("{0}:{1}", _className, _method.ToString());
					
					// Get caller file info
					string 		_sourceFilePath	= _stackFrame.GetFileName();
					
					if (_sourceFilePath != null)
					{
						string _sourceRelativePath	= _sourceFilePath.Substring(_projectPath.Length);
						
						// Following unity standard stacktrace output "class-name:method-definition() (at relative-path:10)"
						_stringBuilder.AppendFormat("(at {0}:{1})\n", _sourceRelativePath, _stackFrame.GetFileLineNumber());
					}
				}

				// Update properties
				this.CallStack					= _stringBuilder.ToString().TrimEnd('\n');
				this.Description				= string.Format("{0}\n{1}", this.Message, this.CallStack);

				if (_topFrameIndex < _totalFrames)
				{
					StackFrame	_topFrame		= _stackTrace.GetFrame(_topFrameIndex);

					this.m_callerFileName		= _topFrame.GetFileName();
					this.m_callerFileLineNumber	= _topFrame.GetFileLineNumber();
				}
				else
				{
					this.m_callerFileName		= null;
					this.m_callerFileLineNumber	= 0;
				}
#endif
			}
			catch
			{
				// Set default values
				this.Description			= this.Message;
				this.CallStack				= string.Empty;
				this.m_callerFileName		= null;
				this.m_callerFileLineNumber	= 0;
			}
		}

		private static bool IsInternalCall (string _name)
		{
			return _name.StartsWith ("UnityEditor.") || _name.StartsWith ("UnityEngine.") || _name.StartsWith ("System.") || _name.StartsWith ("UnityScript.Lang.") || _name.StartsWith ("Boo.Lang.") || _name.StartsWith ("VoxelBusters.DebugPRO");
		}

		public bool IsValid ()
		{
			return (this.ID > 0);
		}

		public bool Equals (ConsoleLog _log)
		{
			return (this.ID == _log.ID);
		}

		public void OnSelect ()
		{
#if UNITY_EDITOR
			if (Context != null)
				UnityEditor.Selection.activeObject	= Context;
#endif
		}

		public void OnPress ()
		{
#if UNITY_EDITOR
			if (m_callerFileName != null)
				UnityEditorInternal.InternalEditorUtility.OpenFileAtLineExternal(m_callerFileName, m_callerFileLineNumber);
#endif
		}
		
		#endregion
	}
}