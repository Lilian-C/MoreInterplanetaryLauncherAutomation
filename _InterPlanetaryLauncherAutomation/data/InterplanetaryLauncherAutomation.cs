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
    public class InterplanetaryLauncherAutomation: KMonoBehaviour, IActivationRangeTarget
    {
        [MyCmpReq]
        public RailGun railgun;
        private static readonly EventSystem.IntraObjectHandler<InterplanetaryLauncherAutomation> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<InterplanetaryLauncherAutomation>((System.Action<InterplanetaryLauncherAutomation, object>) ((component, data) => component.OnCopySettings(data)));
        private static readonly EventSystem.IntraObjectHandler<InterplanetaryLauncherAutomation> OnLogicValueChangedDelegate = new EventSystem.IntraObjectHandler<InterplanetaryLauncherAutomation>((System.Action<InterplanetaryLauncherAutomation, object>) ((component, data) => component.OnLogicValueChanged(data)));
        private static readonly EventSystem.IntraObjectHandler<InterplanetaryLauncherAutomation> UpdateLogicCircuitDelegate = new EventSystem.IntraObjectHandler<InterplanetaryLauncherAutomation>((System.Action<InterplanetaryLauncherAutomation, object>) ((component, data) => component.UpdateLogicCircuit(data)));
        public float PercentFull => this.railgun.CurrentEnergy / this.railgun.hepStorage.capacity;

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            this.Subscribe<InterplanetaryLauncherAutomation>(-905833192, InterplanetaryLauncherAutomation.OnCopySettingsDelegate);
            if (railgun == null) railgun = GameObject.FindObjectOfType<RailGun>();
        }

        private void OnCopySettings(object data)
        {
            InterplanetaryLauncherAutomation component = ((GameObject) data).GetComponent<InterplanetaryLauncherAutomation>();
            if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
            return;
            this.ActivateValue = component.ActivateValue;
            this.DeactivateValue = component.DeactivateValue;
        }

        //private void CreateLogicMeter() => this.logicMeter = new MeterController((KAnimControllerBase) this.GetComponent<KBatchedAnimController>(), "logicmeter_target", "logicmeter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, (string[]) Array.Empty<string>());

        protected override void OnSpawn()
        {
            base.OnSpawn();
            this.Subscribe<InterplanetaryLauncherAutomation>(-801688580, OnLogicValueChangedDelegate);
            this.Subscribe<InterplanetaryLauncherAutomation>(-592767678, UpdateLogicCircuitDelegate);
        }

        public void UpdateLogicCircuit(object data)
        {
            Debug.Log("UpdateLogicCircuit");
            float num = (float) Mathf.RoundToInt(this.PercentFull * 100f);
            if (this.activated)
            {
            if ((double) num >= (double) this.deactivateValue)
                this.activated = false;
            }
            else if ((double) num <= (double) this.activateValue)
            this.activated = true;
            bool flag = this.activated;//&& railgun.operational.IsOperational;
            railgun.GetComponent<LogicPorts>().SendSignal(SmartReservoir.PORT_ID, flag ? 1 : 0);
        }

        private void OnLogicValueChanged(object data)
        {
            Debug.Log("OnLogicValueChanged");
            LogicValueChanged logicValueChanged = (LogicValueChanged) data;
            if (!(logicValueChanged.portID == SmartReservoir.PORT_ID))
            return;
        }

        public float ActivateValue
        {
            get => (float) this.deactivateValue;
            set
            {
                this.deactivateValue = (int) value;
                this.UpdateLogicCircuit((object) null);
            }
        }

        public float DeactivateValue
        {
            get => (float) this.activateValue;
            set
            {
                this.activateValue = (int) value;
                this.UpdateLogicCircuit((object) null);
            }
        }
        [Serialize]
        private int activateValue;

        [Serialize]
        private int deactivateValue = 100;

        [Serialize]
        private bool activated;
        [MyCmpGet]
        //private MeterController logicMeter;
        
        public float MinValue => 0.0f;

        public float MaxValue => 100f;

        public bool UseWholeNumbers => true;

        public string ActivateTooltip => (string) "BUILDINGS.PREFABS.BATTERYSMART.DEACTIVATE_TOOLTIP";

        public string DeactivateTooltip => (string) "BUILDINGS.PREFABS.BATTERYSMART.ACTIVATE_TOOLTIP";

        public string ActivationRangeTitleText => (string) "BUILDINGS.PREFABS.BATTERYSMART.SIDESCREEN_TITLE";

        public string ActivateSliderLabelText => (string) "High treshold";

        public string DeactivateSliderLabelText => (string) "Low treshold";
    }
}