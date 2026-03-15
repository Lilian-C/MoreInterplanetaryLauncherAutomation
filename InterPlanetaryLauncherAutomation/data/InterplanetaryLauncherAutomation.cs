using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KSerialization;
using UnityEngine;
using System.Reflection;
using System.ComponentModel;
using STRINGS;

namespace InterPlanetaryLauncherAutomation
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public class InterplanetaryLauncherAutomation: KMonoBehaviour
    {
        public static readonly HashedString RADBOLTS_EMPTY_PORT_ID = (HashedString) "InterplanetaryLauncherRadboltsEmpty";
        
        [MyCmpReq]
        public RailGun railgun;
        private static readonly EventSystem.IntraObjectHandler<InterplanetaryLauncherAutomation> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<InterplanetaryLauncherAutomation>((System.Action<InterplanetaryLauncherAutomation, object>) ((component, data) => component.OnCopySettings(data)));
        private static readonly EventSystem.IntraObjectHandler<InterplanetaryLauncherAutomation> OnLogicValueChangedDelegate = new EventSystem.IntraObjectHandler<InterplanetaryLauncherAutomation>((System.Action<InterplanetaryLauncherAutomation, object>) ((component, data) => component.OnLogicValueChanged(data)));
        private static readonly EventSystem.IntraObjectHandler<InterplanetaryLauncherAutomation> UpdateLogicCircuitDelegate = new EventSystem.IntraObjectHandler<InterplanetaryLauncherAutomation>((System.Action<InterplanetaryLauncherAutomation, object>) ((component, data) => component.UpdateLogicCircuit(data)));
        [Serialize]
        private bool activated;
        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            this.Subscribe<InterplanetaryLauncherAutomation>(-905833192, InterplanetaryLauncherAutomation.OnCopySettingsDelegate);
            this.Subscribe<InterplanetaryLauncherAutomation>(-801688580, InterplanetaryLauncherAutomation.OnCopySettingsDelegate);
            if (railgun == null) railgun = GameObject.FindObjectOfType<RailGun>();
        }

        private void OnCopySettings(object data)
        {
            /*
                InterplanetaryLauncherAutomation component = ((GameObject) data).GetComponent<InterplanetaryLauncherAutomation>();
                if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
                return;
                this.ActivateValue = component.ActivateValue;
                this.DeactivateValue = component.DeactivateValue;
            */
        }

        protected override void OnSpawn()
        {
            base.OnSpawn();
            this.Subscribe<InterplanetaryLauncherAutomation>(-801688580, OnLogicValueChangedDelegate);
            this.Subscribe<InterplanetaryLauncherAutomation>(-1837862626, UpdateLogicCircuitDelegate);
            // Initialize the logic circuit state
            this.UpdateLogicCircuit(null);
        }

        public void UpdateLogicCircuit(object data)
        {
            if (railgun == null) return;
            
            RailGunPayload.StatesInstance smi = railgun.GetSMI<RailGunPayload.StatesInstance>();
            this.activated = railgun.CurrentEnergy > railgun.EnergyCost;
            bool flag = this.activated;
            
            LogicPorts logicPorts = railgun.GetComponent<LogicPorts>();
            if (logicPorts != null)
            {
                logicPorts.SendSignal(SmartReservoir.PORT_ID, flag ? 1 : 0);
                
                // Send signal for radbolts empty status (1 = empty, 0 = has radbolts)
                bool isEmpty = railgun.CurrentEnergy <= 0f;
                logicPorts.SendSignal(RADBOLTS_EMPTY_PORT_ID, isEmpty ? 1 : 0);
            }
        }

        private void OnLogicValueChanged(object data)
        {
            LogicValueChanged logicValueChanged = (LogicValueChanged) data;
            if (!(logicValueChanged.portID == SmartReservoir.PORT_ID))
            return;
        }
    }
}