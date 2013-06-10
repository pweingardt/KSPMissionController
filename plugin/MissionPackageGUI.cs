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

        /// <summary>
        /// Draws the mission package browser window
        /// </summary>
        /// <param name="id">Identifier.</param>
        private void drawPackageWindow(int id) {
            GUI.skin = HighLogic.Skin;
            GUILayout.BeginHorizontal ();

            packageScrollPosition = GUILayout.BeginScrollView (packageScrollPosition, GUILayout.Width(300));

            foreach (Mission m in currentPackage.Missions) {
                if (GUILayout.Button (m.name)) {
                    currentPreviewMission = manager.reloadMission(m, activeVessel);
                }
            }

            GUILayout.EndScrollView ();

            GUILayout.BeginVertical ();
            previewMissionScrollPosition = GUILayout.BeginScrollView (previewMissionScrollPosition);

            // Show the description text if no mission is currently selected
            if (currentPreviewMission == null) {
                GUILayout.Label (currentPackage.description, styleText);
            } else {
                // otherwise draw the mission parameters
                drawMission (currentPreviewMission, calculateStatus (currentPreviewMission));
            }
            GUILayout.EndScrollView ();

            GUILayout.BeginHorizontal ();
            if (currentPreviewMission != null) {
                if (GUILayout.Button ("Select mission")) {
                    currentMission = currentPreviewMission;
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