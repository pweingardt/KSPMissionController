using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MissionController
{
    public partial class MissionController
    {
        private Vector2 contractScrollPosition = new Vector2();
        private Vector2 previewContractScrollPosition = new Vector2();
        private Mission currentPreviewMission2 = null;

        private void drawContractsWindow(int id)
        {
           
            GUI.skin = HighLogic.Skin;
            GUILayout.BeginVertical();          

            GUILayout.BeginHorizontal();
            contractScrollPosition = GUILayout.BeginScrollView(contractScrollPosition, GUILayout.Width(500));
            foreach (Mission m in currentPackage.Missions)
            {                            
                Status s = calculateStatus(m, false, null);
                double payoutTotal = m.reward * PayoutLeveles.TechPayout;


                GUIStyle style = styleButton;                


                if (m == currentPreviewMission2)
                {
                    style = styleGreenButton;
                }
                //if (m.contractAvailable == manager.GetCurrentContract)
                //{
                //    if (GUILayout.Button(m.name, style, GUILayout.Width(450), GUILayout.Height(60)))
                //    {
                //        currentPreviewMission2 = manager.reloadMission(m, activeVessel);
                //    }
                //}
            }
            GUILayout.EndScrollView();
            GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                previewContractScrollPosition = GUILayout.BeginScrollView(previewContractScrollPosition, GUILayout.Width(500));          

                // Show the description text if no mission is currently selected
                if (currentPreviewMission2 == null)
                {
                    GUILayout.Label(currentPackage.description, styleText);
                }
                else
                {
                    // otherwise draw the mission parameters
                    drawContracts(currentPreviewMission2, calculateStatus(currentPreviewMission2, false, activeVessel));
                }
                GUILayout.EndScrollView();
                GUILayout.EndHorizontal();
            

                GUILayout.BeginHorizontal();
                if (currentPreviewMission2 != null)
                {
                    if (GUILayout.Button("Accept Contract"))
                    {
                        // we also reset the hiddenGoals field
                        hiddenGoals = new List<MissionGoal>();
                        currentMission = currentPreviewMission2;
                        currentPreviewMission2 = null;
                        packageWindow(false);
                        showContractSelection = false;
                    }

                    
                    
                }
                if (GUILayout.Button("Exit Contracts", styleButtonWordWrap))
                {
                    showContractSelection = false;
                }
                //if (manager.GetCurrentContract != 0)
                //{
                //    if (GUILayout.Button("Decline Current Contract", styleButtonWordWrap))
                //    {
                //        manager.SetCurrentContract(0);
                //        showContractSelection = false;
                //        currentMission = null;
                //        Debug.Log("MCE*** CurrentContract Reset to 0: " + manager.GetCurrentContract);
                //    }
                //}
                GUILayout.EndHorizontal();
                
                      
            GUILayout.EndVertical();
            if (!Input.GetMouseButtonDown(1))
            {
                GUI.DragWindow();
            }     
        }
    }
}
