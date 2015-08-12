using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
//using SuperMap.WindowsPhone.Actions;
using SuperMap.WindowsPhone.Core;
using SuperMap.WindowsPhone.Utilities;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Threading;
using System.Windows.Threading;
using System.Diagnostics;

namespace SuperMap.WindowsPhone.Mapping
{
    //Map类太长，把涉及到键盘 鼠标 滚轮 等MapAction的部分移到这里来。
    public sealed partial class Map : Control, INotifyPropertyChanged
    {
        //暂时去掉Action接口，等到地图操作和Map完全分离，能独立到Action中后，再加进去。
        //private MapAction curAction;
        //private MapAction oldAction;
        ///// <summary>${WP_mapping_Map_event_actionChanged_D}</summary>
        //public event EventHandler<MapActionArgs> MapActionChanged;

        private void BuildMapAction()
        {
            //暂时去掉Action接口，等到地图操作和Map完全分离，能独立到Action中后，再加进去。
            //Pan panAction = new Pan(this);
            //curAction = panAction;//默认pan操作
            //oldAction = panAction;
        }

        protected override void OnHold(GestureEventArgs e)
        {
            base.OnHold(e);
            //if (this.Action != null)
            //{
            //    this.Action.OnHold(e);
            //}
        }

        protected override void OnDoubleTap(GestureEventArgs e)
        {
            if (IsDoubleTap == true)
            {
                base.OnDoubleTap(e);
                //if (this.Action != null)
                //{
                //    this.Action.OnDoubleTap(e);
                //}
                Point2D center = ScreenToMap(e.GetPosition(this));
                ZoomToResolution(GetNextResolution(true), center);
            }
        }

        /// <summary>${WP_ui_action_MapAction_event_onMouseDown_D}</summary>
        /// <param name="e">${WP_ui_action_MapAction_event_onMouseDown_param_e}</param>
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            //if (this.Action != null)
            //{
            //    this.Action.OnMouseLeftButtonDown(e);
            //}

        }
        /// <summary>${WP_ui_action_MapAction_event_onMouseUp_D}</summary>
        /// <param name="e">${WP_ui_action_MapAction_event_onMouseUp_param_e}</param>
        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);

            //if (this.Action != null)
            //{
            //    this.Action.OnMouseLeftButtonUp(e);
            //}
        }
        /// <summary>${WP_ui_action_MapAction_event_onMouseEnter_D}</summary>
        /// <param name="e">${WP_ui_action_MapAction_event_onMouseEnter_param_e}</param>
        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);
            //if (this.Action != null)
            //{
            //    this.Action.OnMouseEnter(e);
            //}

        }
        /// <summary>${WP_ui_action_MapAction_event_onMouseMove_D}</summary>
        /// <param name="e">${WP_ui_action_MapAction_event_onMouseMove_param_e}</param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            //if (this.Action != null)
            //{
            //    this.Action.OnMouseMove(e);
            //}
        }
        /// <summary>${WP_ui_action_MapAction_event_onMouseLeave_D}</summary>
        /// <param name="e">${WP_ui_action_MapAction_event_onMouseLeave_param_e}</param>
        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            //if (this.Action != null)
            //{
            //    this.Action.OnMouseLeave(e);
            //}
        }

        protected override void OnTap(GestureEventArgs e)
        {
            base.OnTap(e);

            //if (this.Action != null)
            //{
            //    this.Action.OnTap(e);
            //}
        }

        //private void OnMapActionChanged(MapActionArgs args)
        //{
        //    if (this.MapActionChanged != null)
        //    {
        //        this.MapActionChanged(this, args);
        //    }
        //}

        #region WP
        Point2D _centerP;
        Point _center;

        protected override void OnManipulationStarted(ManipulationStartedEventArgs e)
        {
            base.OnManipulationStarted(e);

            //if (this.Action != null)
            //{
            //    this.Action.OnManipulationStarted(e);
            //}
            _center = this.TransformToVisual(layerCollectionContainer).Transform(e.ManipulationOrigin);
            _centerP = this.ScreenToMap(_center);
        }

        protected override void OnManipulationDelta(ManipulationDeltaEventArgs e)
        {
            base.OnManipulationDelta(e);

            //if (this.Action != null)
            //{
            //    this.Action.OnManipulationDelta(e);
            //}
            Point scale = new Point(Math.Abs(e.DeltaManipulation.Scale.X), Math.Abs(e.DeltaManipulation.Scale.Y));
            _center = e.ManipulationContainer.TransformToVisual(this).Transform(e.ManipulationOrigin);

            _centerP = ScreenToMap(_center);
            if (IsPinchOrStretch(scale))
            {
                double num = (scale.X + scale.Y) / 2;
                oldResolution = this.mapResolution;
                double newResolution = this.mapResolution / num;
                newResolution = MathUtil.MinMaxCheck(newResolution, MinResolution, MaxResolution);
                num = this.mapResolution / newResolution;
                double num1 = _center.X - currentSize.Width / 2;
                double num2 = _center.Y - currentSize.Height / 2;
                ZoomToResolution(newResolution, new Point2D(_centerP.X - newResolution * num1, _centerP.Y + newResolution * num2), true);

            }
            else
            {
                this.PanHelper.DeltaPan(Convert.ToInt32(e.DeltaManipulation.Translation.X), Convert.ToInt32(e.DeltaManipulation.Translation.Y), this.PanDuration, true);
            }
        }

        private bool IsPinchOrStretch(Point scale)
        {
            return ((scale.X > 0) && (scale.Y > 0) && (scale.X != 1.0) && (scale.Y != 1.0));
        }

        #endregion

        //暂时去掉Action接口，等到地图操作和Map完全分离，能独立到Action中后，再加进去。
        /// <summary>${WP_mapping_Map_attribute_action_D}</summary>
        //public MapAction Action
        //{
        //    get { return this.curAction; }
        //    set
        //    {
        //        this.oldAction = this.curAction;
        //        if (this.oldAction != null)
        //        {
        //            this.oldAction.Deactivate();
        //        }
        //        this.curAction = value;
        //        MapActionArgs args = new MapActionArgs(this.oldAction, this.curAction);
        //        this.OnMapActionChanged(args);
        //    }
        //}

    }
}
