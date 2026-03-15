using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Database;
using STRINGS;
using UnityEngine;

namespace InterPlanetaryLauncherAutomation
{
    public class STRINGS
    {
        public class LOGIC
        {
            public class INTERPLANETARYLAUNCHERADJUST
            {
                public static LocString TITLE = "The launcher will send green signal when it's radbolt storage has enough radbolts to launch Payload.";
                public static LocString ACTIVE = (LocString) ("Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " when it's radbolt storage is full enough to launch Payload");
                public static LocString INACTIVE = (LocString) ("Sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + " when it's radbolt storage is not full enough to launch Payload");
                
                public static LocString RADBOLTS_EMPTY_TITLE = "Radbolts Empty";
                public static LocString RADBOLTS_EMPTY_ACTIVE = (LocString) ("Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " when radbolt storage is empty");
                public static LocString RADBOLTS_EMPTY_INACTIVE = (LocString) ("Sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + " when radbolt storage has radbolts");
            }
        }



        public class TRANSLATION
        {
            public class AUTHOR
            {
                public static LocString NAME = "Pouachiche";
            }
        }
    }
}
