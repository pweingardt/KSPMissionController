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

        private SortBy currentSort = SortBy.NAME;
        private Dictionary<SortBy, String> sortStrings = new Dictionary<SortBy, string>() {
            {SortBy.NAME, "Sorted by name"},
            {SortBy.REWARD, "Sorted by reward"},
            {SortBy.PACKAGE_ORDER, "Sorted by package order"}
        };
        
        // for Geckgo mission filter
        private bool showFinishedMissions = true;
        private bool showUnavailableMissions = true;

        // Is initialized after the icons have been initialized!
        private Dictionary<Mission.Category, Texture2D> iconDictionary = new Dictionary<Mission.Category, Texture2D>();

        /// <summary>
        /// Draws the mission package browser window
        /// </summary>
        /// <param name="id">Identifier.</param>
        private void drawPackageWindow(int id) {
            GUI.skin = HighLogic.Skin;
            GUILayout.BeginHorizontal ();

            GUILayout.BeginVertical (GUILayout.Width(530));
            if (GUILayout.Button(sortStrings[currentSort], styleGreenButton, GUILayout.Width(500)))
            {
                nextSort ();
                Mission.Sort(currentPackage.Missions, currentSort);
            }
            
            // Geckgo mission filter
            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical(GUILayout.Width(260));
            showFinishedMissions = GUILayout.Toggle(showFinishedMissions, "Show Finished");
            GUILayout.EndVertical();
            GUILayout.BeginVertical();
            showUnavailableMissions = GUILayout.Toggle(showUnavailableMissions, "Show Unavailalbe");
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();

            packageScrollPosition = GUILayout.BeginScrollView (packageScrollPosition, GUILayout.Width(500));

            foreach (Mission m in currentPackage.Missions) {
                Status s = calculateStatus (m, false, null);

                GUIStyle style = styleButton;

                if (s.requiresAnotherMission) {
                    if (!showUnavailableMissions)
                        continue;
                    else
                        style = styleRedButton;
                }
                if (s.missionAlreadyFinished && !showFinishedMissions)
                    continue;

                if (m == currentPreviewMission) {
                    style = styleGreenButton;
                }

                GUILayout.BeginHorizontal(GUILayout.Width(450));
                if (m.scienceReward != 0)
                {
                    if (GUILayout.Button(m.name + "\n" + (m.reward * PayoutLeveles.TechPayout) + CurrencySuffix + " : " +m.scienceReward + " sp", style, GUILayout.Width(350)))
                    {
                        currentPreviewMission = manager.reloadMission(m, activeVessel);
                    }
                }

                else
                    if (GUILayout.Button(m.name + "\n" + (m.reward * PayoutLeveles.TechPayout) + CurrencySuffix, style, GUILayout.Width(350)))
                    {
                        currentPreviewMission = manager.reloadMission(m, activeVessel);
                    }       

                if (s.missionAlreadyFinished) {
                    GUILayout.Label(iconFinished, GUILayout.MaxWidth (50), GUILayout.MaxHeight (50), GUILayout.ExpandWidth(false),
                                    GUILayout.Width(50), GUILayout.Height(50));
                } else {
                    foreach (Mission.Category c in iconDictionary.Keys) {
                        if(m.category.Has(c)) {
                            GUILayout.Label (iconDictionary[c], GUILayout.MaxWidth (50), GUILayout.MaxHeight (50), GUILayout.ExpandWidth(false),
                                             GUILayout.Width(50), GUILayout.Height(50));
                        }
                    }
                }
                GUILayout.EndHorizontal ();
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
                drawMission (currentPreviewMission, calculateStatus (currentPreviewMission, false, activeVessel));
            }
            GUILayout.EndScrollView ();

            GUILayout.BeginHorizontal ();
            if (currentPreviewMission != null) {
                if (GUILayout.Button ("Select mission")) {
                    // we also reset the hiddenGoals field
                    hiddenGoals = new List<MissionGoal> ();
                    currentMission = currentPreviewMission;
                    currentPreviewMission = null;
                    packageWindow (false);
                }

                if (currentPreviewMission.randomized && GUILayout.Button ("New Random Mission", styleButtonYellow)) {
                    manager.discardRandomMission (currentPreviewMission);
                    currentPreviewMission = manager.reloadMission (currentPreviewMission, activeVessel);
                }
            }

            if (GUILayout.Button ("Close")) {
                packageWindow (false);
            }
            GUILayout.EndHorizontal ();

            GUILayout.EndVertical ();
            GUILayout.EndHorizontal ();

            if (!Input.GetMouseButtonDown(1))
            {
                GUI.DragWindow();
            }
            
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