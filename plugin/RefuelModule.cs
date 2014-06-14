using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MissionController
{
    class RefuelModule : PartModule
    {
        private Manager manager
        {
            get { return Manager.instance; }
        }

        Vessel vs = new Vessel();

        private List<ResourceTankList> resourceTankList = new List<ResourceTankList>();
        private void add(ResourceTankList m)
        {
            resourceTankList.Add(m);
        }

        [KSPField(isPersistant = true)]
        private double CurrentRS1 = 0;

        [KSPField(isPersistant = true)]
        private double CurrentRS2 = 0;

        [KSPField(isPersistant = true)]
        private bool orderRS1On = false;

        [KSPField(isPersistant = true)]
        private bool orderRS2On = false;
        
        private double SavedRS1 = 0;
        private double savedRS2 = 0;

        private string rS1Name = "none";
        private string rS2Name = "none";

        private double transferRate = 1;
        private double ResourceCost = 0;
        private bool priceCheck = false;
        private double surcharge = .08;
        private double deliveryCharge = .2;       

        private void getResourceCost(string name)
        {
            foreach (ConfigNode rNode in Tools.MCSettings.GetNode("RESOURCECOST").nodes)
            {
                if (rNode.name.Equals(name))
                {
                    ResourceCost = Tools.GetValueDefault(rNode, "cost", 0.0);
                }
            }
        }
        private void getAllResourcesPart()
        {
            foreach (PartResource pr in part.Resources)
            {
                Debug.LogWarning("Found this Resources In Refuel Module " + pr.resourceName.ToString());
                resourceTankList.Add(new ResourceTankList(pr.resourceName));
            }
        }
    
    
        private void chargePurchasePrice(string resource, double current, double saved)
        {
            double difference = 0;
            double totalAmount = 0;
            double SubTotal1 = 0;
            double SubTotal2 = 0;
            double TotalWithCharges = 0;
            double maxresource = this.part.Resources.Get(PartResourceLibrary.Instance.GetDefinition(resource).id).maxAmount;
            current = this.part.Resources.Get(PartResourceLibrary.Instance.GetDefinition(resource).id).amount;
            if (priceCheck != true) { priceCheck = true; saved = current; }
            this.part.RequestResource(resource, -transferRate);
            FuelPurchase = true;
            if (current == maxresource)
            {
                getResourceCost(resource);
                difference = current - saved;
                totalAmount = difference * ResourceCost;
                SubTotal1 = totalAmount * surcharge;
                SubTotal2 = totalAmount * deliveryCharge;
                TotalWithCharges = totalAmount + SubTotal1 + SubTotal2;
                manager.ModCost((int)totalAmount, "Fuel Cost Oxidizer");
                MissionController.messageEvent = "Your total fuel cost in Oxidizer Was $" + (int)totalAmount + " Surcharge of $" + (int)SubTotal1 + " And Delivery Charge of $" + (int)SubTotal2 + " Total Bill is $" + (int)TotalWithCharges; ;
                MissionController.showEventWindow = true;
                orderRS2On = false;
                if (orderRS1On != false)
                {
                    orderRS2On = true;
                }
                orderRS1On = false;
                ResourceCost = 0;
                priceCheck = false;
                FuelPurchase = false;
            }
        }

        [KSPField(isPersistant = false, guiActive = true, guiName = "Purchasing Fuel:")]
        private bool FuelPurchase = false;

        [KSPField(isPersistant = false, guiActive = true, guiName = "Refueler Landed:")]
        private bool IsLanded = false;

        [KSPEvent(guiActive = true, active = true, guiName = "Purchase Fuel")]
        public void OrderLF()
        {
            orderRS1On = true;            
        }               
        public override void OnStart(PartModule.StartState state)
        {
            this.part.force_activate();
            getAllResourcesPart();

            ResourceTankList rt1 = resourceTankList[0];
            rS1Name = rt1.resource;
            if (resourceTankList.Count > 1)
            {
                ResourceTankList rt2 = resourceTankList[1];
                rS2Name = rt2.resource;
            }
            Debug.LogWarning("rS1Name = " + rS1Name + " rS2Name = " + rS2Name);    
            
        }

        public override void OnFixedUpdate()
        {
            if (vs.situation.Equals(Vessel.Situations.LANDED))
            {
                IsLanded = true;
            }
            else IsLanded = false;
            
            if (orderRS1On.Equals(true) && vs.situation.Equals(Vessel.Situations.LANDED))
            {
                chargePurchasePrice(rS1Name, CurrentRS1, SavedRS1);
            }
            if (orderRS2On.Equals(true) && vs.situation.Equals(Vessel.Situations.LANDED))
            {
                if (rS2Name == "none") { Debug.LogWarning("No 2nd Resource found skiping Load And Purchase"); orderRS2On = false; }
                else { chargePurchasePrice(rS2Name, CurrentRS2, savedRS2); }   
            }

        }
    }
    public class ResourceTankList
    {
        public string resource;

        public ResourceTankList()
        {
        }
        public ResourceTankList(string resourceName)
        {
            this.resource = resourceName;
        }
    }
}
