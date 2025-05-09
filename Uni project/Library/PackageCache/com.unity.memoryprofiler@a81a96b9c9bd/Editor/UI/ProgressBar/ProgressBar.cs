using System;
using UnityEditor;
using UnityEngine;

namespace Unity.MemoryProfiler.Editor.UI
{
    internal class ProgressBarDisplay
    {
        string m_Title;
        string m_CurrentStepInfo;
        float m_Progress;

        byte m_IsComplete = 0;
        byte m_DidDraw = 0;

        static ProgressBarDisplay s_Instance;
        static ProgressBarDisplay instance
        {
            get
            {
                if (s_Instance == null)
                    s_Instance = new ProgressBarDisplay();
                return s_Instance;
            }
        }

        protected ProgressBarDisplay()
        {
            m_Title = string.Empty;
            m_CurrentStepInfo = string.Empty;
            m_Progress = 0.0f;
            m_IsComplete = 1;

#if !ENABLE_CORECLR
            AppDomain.CurrentDomain.DomainUnload += OnDomainUnload;
#endif
        }

#if ENABLE_CORECLR
        [Unity.Scripting.LifecycleManagement.BeforeAssemblyUnloading]
        static void BeforeAssemblyUnloading()
        {
            ClearOnDomainUnload();
        }
#else
        // Needs to be static to be called by the domain unload event
        static void OnDomainUnload(object sender, EventArgs args)
        {
            ClearOnDomainUnload();
            AppDomain.CurrentDomain.DomainUnload -= OnDomainUnload;
        }
#endif

        static void ClearOnDomainUnload()
        {
            if (instance?.m_DidDraw == 1)
            {
                EditorUtility.ClearProgressBar();
            }
            s_Instance = null;
        }

        public static void ShowBar(string title)
        {
            Debug.Assert(instance.m_IsComplete == 1); // assert if we are trying to change the title while the bar is not done
            Debug.Assert(title != null);

            instance.m_IsComplete = 0;
            instance.m_Title = title;

            // progress bar info cannot be null or empty on osx
            // force the the info to be the same as the title if
            if (string.IsNullOrEmpty(instance.m_CurrentStepInfo))
                instance.m_CurrentStepInfo = instance.m_Title;

            UpdateProgress(0.0f);
        }

        public static void UpdateProgress(float progress, string stepInfo = null)
        {
            Debug.Assert(instance.m_IsComplete == 0);

            instance.m_Progress = progress;

            if (stepInfo != null)
            {
                instance.m_CurrentStepInfo = stepInfo;
            }
            instance.m_DidDraw = 1;
            EditorUtility.DisplayProgressBar(instance.m_Title, instance.m_CurrentStepInfo, instance.m_Progress);
        }

        public static void ClearBar()
        {
            instance.m_IsComplete = 1;
            instance.m_CurrentStepInfo = string.Empty;
            instance.m_Title = string.Empty;
            instance.m_Progress = 0.0f;
            instance.m_DidDraw = 0;
            EditorUtility.ClearProgressBar();
        }

        public static bool IsComplete() => instance.m_IsComplete == 1;
    }
}
