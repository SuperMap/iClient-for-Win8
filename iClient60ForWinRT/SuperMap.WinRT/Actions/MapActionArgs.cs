using System;

namespace SuperMap.WinRT.Actions
{
    /// <summary>
    /// 	<para>${ui_action_ActionEvent_Title}。</para>
    /// 	<para>${ui_action_ActionEvent_Description_sl}</para>
    /// </summary>
    public class MapActionArgs : EventArgs
    {
        /// <summary>${ui_action_ActionEvent_constructor_String_D}</summary>
        /// <param name="oldAction">${ui_action_ActionEvent_attribute_oldAction_D}</param>
        /// <param name="newAction">${ui_action_ActionEvent_attribute_newAction_D}</param>
        public MapActionArgs(MapAction oldAction, MapAction newAction)
        {
            this.OldAction = oldAction;
            this.NewAction = newAction;
        }

        /// <summary>${ui_action_ActionEvent_attribute_oldAction_D}</summary>
        public MapAction OldAction { get; set; }
        /// <summary>${ui_action_ActionEvent_attribute_newAction_D}</summary>
        public MapAction NewAction { get; set; }
    }
}
