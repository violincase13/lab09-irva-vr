#if UNITY_EDITOR // Editor script only. Build fails if removed!

using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEditor.XR.Management;
using UnityEditor.XR.Management.Metadata;
using UnityEngine;
using UnityEngine.XR.Management;

namespace ProjectSetup.Scripts
{
    /// <summary>
    /// Logic related to XR plug-in management provider setup for either Android (Cardboard, Meta), or Standalone (SteamVR, Oculus Link).
    /// </summary>
    public class XRPluginSceneController : MonoBehaviour//, IPreprocessBuildWithReport
    {
        [SerializeField] protected Utils.TargetVR targetVR;

        private BuildTargetGroup buildTargetGroup = BuildTargetGroup.Unknown;
        private XRGeneralSettings xrGeneralSettings;
        // public int callbackOrder { get; }

        /// <summary>
        /// Called when an inspector field changes (in this script, when `targetVR` is changed).<br/>
        /// Sets the XR plug-in management provider for Standalone/PC builds (SteamVR, Quest Link).
        /// </summary>
        private void OnValidate()
        {
            if(Application.isPlaying || BuildPipeline.isBuildingPlayer) return;

            buildTargetGroup = BuildTargetGroup.Standalone;
            GetSettingsForBuildTarget();

            // Remove all loaders.
            RemoveLoader(Utils.XR_OCULUS_LOADER);
            RemoveLoader(Utils.XR_OPENVR_LOADER);
            RemoveLoader(Utils.XR_OPENXR_LOADER);
            
            // Assign only required loader.
            switch (targetVR)
            {
                case Utils.TargetVR.Meta:      AssignLoader(Utils.XR_OCULUS_LOADER); break;
                case Utils.TargetVR.Steam:     AssignLoader(Utils.XR_OPENVR_LOADER); break;
                case Utils.TargetVR.Cardboard: // default to Oculus
                default:                       AssignLoader(Utils.XR_OCULUS_LOADER); break;
            }
        }
        
        /// <summary>
        /// Called before a build is started.<br/>
        /// Sets the XR plug-in management provider for Android builds (Cardboard, Meta).
        /// </summary>
        /// <param name="report"></param>
        public void OnPreprocessBuild(BuildReport report)
        {
            buildTargetGroup = BuildTargetGroup.Android;
            GetSettingsForBuildTarget();
            
            // Remove all loaders.
            RemoveLoader(Utils.XR_ARCORE_LOADER);
            RemoveLoader(Utils.XR_OCULUS_LOADER);
            RemoveLoader(Utils.XR_CRDBRD_LOADER);
            
            // Assign only required loader.
            switch (targetVR)
            {
                case Utils.TargetVR.Cardboard: AssignLoader(Utils.XR_CRDBRD_LOADER); break;
                case Utils.TargetVR.Meta:      AssignLoader(Utils.XR_OCULUS_LOADER); break;
                case Utils.TargetVR.Steam:     // default to Oculus
                default:                       AssignLoader(Utils.XR_OCULUS_LOADER); break;
            }
        }
        
        private void GetSettingsForBuildTarget()
        {
            EditorBuildSettings.TryGetConfigObject(XRGeneralSettings.k_SettingsKey, out XRGeneralSettingsPerBuildTarget buildTargetSettings);
            xrGeneralSettings = buildTargetSettings.SettingsForBuildTarget(buildTargetGroup);
        }

        private void AssignLoader(string loader) => XRPackageMetadataStore.AssignLoader(xrGeneralSettings.Manager, loader, buildTargetGroup);
        private void RemoveLoader(string loader) => XRPackageMetadataStore.RemoveLoader(xrGeneralSettings.Manager, loader, buildTargetGroup);
    }
}

#endif