##MCE API For your plugin Version 2.0 (malkuth Edits)

You can now support a few features of Mission Controller Extended in you own plugin, and its pretty easy to use all you have to do is reference the MissionLibrary.dll A version of this file is located in every download of MCE.


To access the fuctions of MCE that are avialable in MissionLibrary.dll all you have to do is access `ManagerAccessor.get`
(you may have to use the `MissionController` namespace) to access the MCE interface.

#### These Are values That Can Add Cost And Or Payments To MCE Budget

* `int modReward(int value, string Description)`: Give the Player a Reward Or Payment, Value and Description of What the payment is for.  This shows up in the Other Payments Manifest.

* `int ModCost(int value, string Description)`: Charge the player for something and have it taken out of the Budget. And add string for what the charge was for.  Shows up in the Other Cost Manifest

#### Below Values Are Not Really Needed. Suggest Only Using Above ones only.


* `int CleanReward(int value)` : This is a straight up Reward That has no % Changes from Research or others. 10 Credits will always be 10 credits.  Again better to use modReward for this.
 
* `int recyclereward(int value)` : Used for recycling Payments, and ends up in the Recycling Manifest

* `float sciencereward(float value)` : This is the value used to give science points in MCE Missions And Contracts.

* `int kerbCost(int value)` : This is a Cost and charges things to the Hire Kerbal Manifest (all as cost)

#### These Are Fuctions That you can use to Call Data From MCE to Show What Values Are

* `int IgetBudget()` : Returns the CURRENT ACTIVE MCE Budget
 
* `int Itotalbudget()` : Returns the Total Amount of Payments made through Missions and Contracts.

* `int ItotalSpentVehicles()`: returns the Total Money Spent on Launching Vehicles.

* `int ItotalRecycleMoney()`: returns the current Money Gained From Recycling.

* `int ItotalHiredKerbCost()`: returns the current Money Spent On Kerbals.

* `int ItotalModPayment()` : Returns the Total Money Added through ModPayment.

* `int ItotalModCost()` : Returns the Total Money Removed through Cost in ModCost.



## ATTENTION

Every time you call any kind of Reward or Cost the current space program will be written on the disk,
don't call it every second. In other words keep them out of GUI types that are called every frame.  Or part type updates like OnFixedUpdate().  Thanks. ;)

# Example code.

* `ManagerAccessor.get.ModCost(1000, "Place What the Cost is for in this String")`
            
