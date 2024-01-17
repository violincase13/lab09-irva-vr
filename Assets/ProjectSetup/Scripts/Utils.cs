namespace ProjectSetup.Scripts
{
    /// <summary>
    /// XR plug-in management provider helpers.
    /// </summary>
    public static class Utils
    {
        public const string XR_ARCORE_LOADER = "UnityEngine.XR.ARCore.ARCoreLoader";
        public const string XR_OPENVR_LOADER = "Unity.XR.OpenVR.OpenVRLoader";
        public const string XR_OPENXR_LOADER = "UnityEngine.XR.OpenXR.OpenXRLoader";
        public const string XR_OCULUS_LOADER = "Unity.XR.Oculus.OculusLoader";
        public const string XR_CRDBRD_LOADER = "Google.XR.Cardboard.XRLoader";
    
        public enum TargetVR
        {
            Cardboard = 0,
            Meta = 1,
            Steam = 2
        }
    }
}
