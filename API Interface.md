## Interface to access the Manager from your plugin Version 2.0 (malkuth Edits)
You can now support a few features of Mission Controller Extended in you own plugin, and its pretty easy to use all you have to do is reference the MissionLibrary.dll A version of this file is located in every download of MCE.


To access the fuctions of MCE that are avialable in MissionLibrary.dll all you have to do is access `ManagerAccessor.get`
(you may have to use the `MissionController` namespace) to access the MCE interface.

Methods so far:

* `int getBudget()`: returns the current budget

* `int modReward(int value, string Description)`: Give the Player a Reward Or Payment, Value and Description of What was given in the Other Payment Manifest

* `ModCost(int value, string Description)`: Give the Player a Reward Or Payment, Value and Description of What was given in the Other Cost Manifest

#### The Below Values Are not Really Needed, but Included them incase for somereason you want to use these Manifest Versions, no Descriptions for these will be shown only Payments or Cost.


* `int CleanReward(int value);` : This is a straight up Reward That has no % Changes from Research or others. 10 Credits will always be 10 credits.  Again better to use modReward for this.
 
* `int recyclereward(int value)` : Used for recycling Payments, and ends up in the Recycling Manifest

* `float sciencereward(float value)` : This is the value used to give science points in MCE Missions And Contracts.

* `int kerbCost(int value)` : This is a Cost and charges things to the Hire Kerbal Manifest (all as cost)

## ATTENTION

Every time you call any kind of Reward or Cost the current space program will be written on the disk,
don't call it every second.

# Example code.

* if (GUILayout.Button("Testing IManager Cost")) 
            {
                ManagerAccessor.get.ModCost(1000, "Place What the Cost is for in this String"); 
            }
