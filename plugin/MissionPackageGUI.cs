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

        private enum SortBy {NAME, REWARD};

        private SortBy currentSort = SortBy.NAME;

        /// <summary>
        /// Draws the mission package browser window
        /// </summary>
        /// <param name="id">Identifier.</param>
        private void drawPackageWindow(int id) {
            GUI.skin = HighLogic.Skin;
            GUILayout.BeginHorizontal ();

            GUILayout.BeginVertical ();
            String sort = (currentSort == SortBy.NAME ? "Sorted by name" : "Sorted by reward");
            if (GUILayout.Button (sort, styleButton)) {
                if (currentSort == SortBy.NAME) {
                    currentSort = SortBy.REWARD;
                    currentPackage.Missions.Sort (delegate(Mission x, Mission y) {
                        return x.reward.CompareTo(y.reward);
                    });
                } else if (currentSort == SortBy.REWARD) {
                    currentSort = SortBy.NAME;
                    currentPackage.Missions.Sort (delegate(Mission x, Mission y) {
                        return x.name.CompareTo(y.name);
                    });
                }
            }
            packageScrollPosition = GUILayout.BeginScrollView (packageScrollPosition, GUILayout.Width(300));

            foreach (Mission m in currentPackage.Missions) {
                GUIStyle style = styleButton;

                if (m.requiresMission != null && m.requiresMission.Length != 0 && !manager.isMissionAlreadyFinished (m.requiresMission)) {
                    style = styleRedButton;
                }

                if (m == currentPreviewMission) {
                    style = styleGreenButton;
                }

                if (GUILayout.Button (m.name + ", " + m.reward + CurrencySuffix + "\n" + m.category, style)) {
                    currentPreviewMission = manager.reloadMission(m, activeVessel);
                }
            }

            GUILayout.EndScrollView ();
            GUILayout.EndVertical ();

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
                    // we also reset the hiddenGoals field
                    hiddenGoals = new List<MissionGoal> ();
                    currentMission = currentPreviewMission;
                }

                if (currentPreviewMission.randomized && GUILayout.Button ("Discard")) {
                    manager.discardRandomMission (currentPreviewMission);
                    currentPreviewMission = manager.reloadMission (currentPreviewMission, activeVessel);
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