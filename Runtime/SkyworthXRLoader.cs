using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Experimental.XR;
using UnityEngine.XR;
using UnityEngine.XR.Management;

namespace Unity.XR.SDK
{
    public class SkyworthXRLoader : XRLoaderHelper
    {
#if UNITY_ANDROID
        private AndroidJavaObject androidActivity;
#endif
        private static List<XRDisplaySubsystemDescriptor> s_DisplaySubsystemDescriptors =
            new List<XRDisplaySubsystemDescriptor>();
        private static List<XRInputSubsystemDescriptor> s_InputSubsystemDescriptors =
            new List<XRInputSubsystemDescriptor>();

        public override bool Initialize()
        {
#if UNITY_ANDROID
            try
            {
                using (AndroidJavaClass player = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
                {
                    androidActivity = player.GetStatic<AndroidJavaObject>("currentActivity");
                }
            }
            catch (AndroidJavaException e)
            {
                androidActivity = null;
                Debug.LogError("Exception while connecting to the Activity: " + e);
            }


            SetAndroidActivity(androidActivity.GetRawObject());
#endif
            CreateSubsystem<XRDisplaySubsystemDescriptor, XRDisplaySubsystem>(s_DisplaySubsystemDescriptors, "skyworth display");
            CreateSubsystem<XRInputSubsystemDescriptor, XRInputSubsystem>(s_InputSubsystemDescriptors, "skyworth input");
            return true;
        }

        public override bool Start()
        {
            StartSubsystem<XRDisplaySubsystem>();
            StartSubsystem<XRInputSubsystem>();
            return true;
        }

        public override bool Stop()
        {
            StopSubsystem<XRDisplaySubsystem>();
            StopSubsystem<XRInputSubsystem>();
            return true;
        }

        public override bool Deinitialize()
        {
            DestroySubsystem<XRDisplaySubsystem>();
            DestroySubsystem<XRInputSubsystem>();
            return true;
        }
		private const string pluginName = "SkyworthXRPlugin";
        [DllImport(pluginName)]
        private static extern void SetAndroidActivity(IntPtr androidActivity);
    }
}
