#if UNITY_ANDROID
using System;
using UnityEditor;
using UnityEngine;
using VoxelBusters.NativePlugins.Internal;
using VoxelBusters.Utility;
using System.Collections.Generic;

namespace VoxelBusters.NativePlugins
{
	/// <summary>
	/// Play-Services Dependencies for Cross Platform Native Plugins.
	/// </summary>
	[InitializeOnLoad]
	public class NPAndroidLibraryDependencies
	{
		/// <summary>
		/// The name of your plugin.  This is used to create a settings file
		/// which contains the dependencies specific to your plugin.
		/// </summary>
		private static readonly string PluginName = "CrossPlatformNativePlugins";
		private static readonly string DependencyFileDirectory = "ProjectSettings";

#if UNITY_2017_1_OR_NEWER && !NATIVE_PLUGINS_LITE_VERSION
		private static readonly string PlayServicesVersionString	=	"11.0+";
		private static readonly string SupportLibsVersionString		=	"26.0.1+";//26.0.1+ is must as we need to use NotificationCompat for Oreo and Up devices - wehn targetting >= 26
#else
		private static readonly string PlayServicesVersionString	=	"10.0+";
		private static readonly string SupportLibsVersionString		=	"24.2+";
#endif

		private static readonly string[] Supportv4SubLibraries = new string[]{
			"support-v4"
		};

		/// <summary>
		/// Initializes static members of the <see cref="NPAndroidLibraryDependencies"/> class.
		/// </summary>
		static NPAndroidLibraryDependencies()
		{
			EditorUtils.Invoke(()=>{
				CreateDependencies();
			}, 0.1f);
		}

		private static void CreateDependencies()
		{
			// Setup the resolver using reflection as the module may not be
		    // available at compile time.
		    Type playServicesSupport = Google.VersionHandler.FindClass(
            "Google.JarResolver", "Google.JarResolver.PlayServicesSupport");
	        if (playServicesSupport == null) {
	            return;
	        }
	        object svcSupport = Google.VersionHandler.InvokeStaticMethod(
	            playServicesSupport, "CreateInstance",
	            new object[] {
	                PluginName,
	                EditorPrefs.GetString("AndroidSdkRoot"),
	                DependencyFileDirectory}
				);

			Google.VersionHandler.InvokeInstanceMethod(
			svcSupport, "ClearDependencies", null);

			if (NPSettings.Application.SupportedFeatures.UsesGameServices)
			{
				Google.VersionHandler.InvokeInstanceMethod(
	            svcSupport, "DependOn",
	            new object[] { 	"com.google.android.gms",
								"play-services-games",
								PlayServicesVersionString },
	            namedArgs: new Dictionary<string, object>()
							{
		                		{
									"packageIds",
									new string[]
									{
		                       			"extra-google-m2repository",
		                        		"extra-android-m2repository"
									}
								}
				            }
				);

				Google.VersionHandler.InvokeInstanceMethod(
	            svcSupport, "DependOn",
	            new object[] { 	"com.google.android.gms",
								"play-services-nearby",
								PlayServicesVersionString },
	            namedArgs: null
				);
			}

			if (NPSettings.Application.SupportedFeatures.UsesNotificationService)
			{
				Google.VersionHandler.InvokeInstanceMethod(
	            svcSupport, "DependOn",
	            new object[] { 	"com.google.android.gms",
								"play-services-gcm",
								PlayServicesVersionString },
	            namedArgs: null
				);
			}

			//https://developer.android.com/topic/libraries/support-library/packages.html
			// Marshmallow permissions requires app-compat. Also used by some old API's for compatibility.
			foreach (string each in Supportv4SubLibraries)
			{
				Google.VersionHandler.InvokeInstanceMethod(
					svcSupport, "DependOn",
					new object[] { 	"com.android.support",
						each,
						SupportLibsVersionString },
					namedArgs: null
				);
			}
			
			/*Google.VersionHandler.InvokeInstanceMethod(
				svcSupport, "DependOn",
				new object[] { 	"com.android.support",
					"appcompat-v7",
					SupportLibsVersionString },
				namedArgs: null
			);*/

			/* If not enabled by default, resolve manually.
			if (!PlayServicesResolver.Resolver.AutomaticResolutionEnabled())
			{
				PlayServicesResolver.Resolver.DoResolution(svcSupport, "Assets/Plugins/Android", PlayServicesResolver.HandleOverwriteConfirmation);
				AssetDatabase.Refresh();
			}*/
		}
	}
}
#endif
