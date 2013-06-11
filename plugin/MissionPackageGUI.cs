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

        public enum SortBy {NAME, REWARD, PACKAGE_ORDER};

        private SortBy currentSort = SortBy.NAME;
        private Dictionary<SortBy, String> sortStrings = new Dictionary<SortBy, string>() {
            {SortBy.NAME, "Sorted by name"},
            {SortBy.REWARD, "Sorted by reward"},
            {SortBy.PACKAGE_ORDER, "Sorted by package order"}
        };

        /// <summary>
        /// Draws the mission package browser window
        /// </summary>
        /// <param name="id">Identifier.</param>
        private void drawPackageWindow(int id) {
            GUI.skin = HighLogic.Skin;
            GUILayout.BeginHorizontal ();

            GUILayout.BeginVertical (GUILayout.Width(450));
            if (GUILayout.Button (sortStrings[currentSort], styleButton)) {
                nextSort ();
                if (currentSort == SortBy.NAME) {
                    currentPackage.Missions.Sort (delegate(Mission x, Mission y) {
                        return x.name.CompareTo(y.name);
                    });
                } else if (currentSort == SortBy.REWARD) {
                    currentPackage.Missions.Sort (delegate(Mission x, Mission y) {
                        return x.reward.CompareTo(y.reward);
                    });
                } else if (currentSort == SortBy.PACKAGE_ORDER) {
                    currentPackage.Missions.Sort (delegate(Mission x, Mission y) {
                        return x.packageOrder.CompareTo(y.packageOrder);
                    });
                }
            }
            packageScrollPosition = GUILayout.BeginScrollView (packageScrollPosition, GUILayout.Width(450));

            foreach (Mission m in currentPackage.Missions) {
                GUIStyle style = styleButton;

                if (m.requiresMission != null && m.requiresMission.Length != 0 && !manager.isMissionAlreadyFinished (m.requiresMission)) {
                    style = styleRedButton;
                }

                if (m == currentPreviewMission) {
                    style = styleGreenButton;
                }

                if (GUILayout.Button (m.name + "\n" + m.reward + CurrencySuffix, style, GUILayout.Width(400))) {
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

        /// <summary>
        /// Selects the next sorting mechanism
        /// </summary>
        private void nextSort() {
            currentSort = currentSort.Next ();
            if (currentSort == SortBy.PACKAGE_ORDER && !currentPackage.ownOrder) {
                currentSort = currentSort.Next ();
            }
        }
    }
}