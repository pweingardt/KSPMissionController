using System;
using System.Collections.Generic;
using UnityEngine;

namespace MissionController
{
    public partial class MissionController
    {
        private Vector2 packageScrollPosition = new Vector2();
        private Vector2 previewMissionScrollPosition = new Vector2 ();
        private Mission currentPreviewMission = null;

        private void drawPackageWindow(int id) {
            GUI.skin = HighLogic.Skin;
            GUILayout.BeginHorizontal ();

            packageScrollPosition = GUILayout.BeginScrollView (packageScrollPosition, GUILayout.Width(300));

            foreach (Mission m in currentPackage.Missions) {
                if (GUILayout.Button (m.name)) {
                    currentPreviewMission = m;
                }
            }

            GUILayout.EndScrollView ();

            GUILayout.BeginVertical ();
            previewMissionScrollPosition = GUILayout.BeginScrollView (previewMissionScrollPosition);
            // Show the description text if no mission is currently selected inside the browser
            if (currentPreviewMission == null) {
                GUILayout.Label (currentPackage.description, styleText);

            } else {
                drawMission (currentPreviewMission, calculateStatus (currentPreviewMission));
            }
            GUILayout.EndScrollView ();

            GUILayout.BeginHorizontal ();
            if (currentPreviewMission != null) {
                if (GUILayout.Button ("Select mission")) {

                }
            }

            if (GUILayout.Button ("Close")) {
                showMissionPackageBrowser = false;
            }
            GUILayout.EndHorizontal ();

            GUILayout.EndVertical ();
            GUILayout.EndHorizontal ();

            GUI.DragWindow ();
        }
    }
}