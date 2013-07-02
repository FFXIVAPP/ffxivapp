// FFXIVAPP.Plugin.Parse
// PrivatePlotModel.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2013 Ryan Wilson - All Rights Reserved

#region Usings

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using FFXIVAPP.Common.Helpers;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

#endregion

namespace FFXIVAPP.Plugin.Parse.Models
{
    public class PrivatePlotModel : INotifyPropertyChanged
    {
        #region Property Bindings

        private LinearAxis _bottomAxis;
        private LinearAxis _leftAxis;
        private PlotModel _model;

        public PlotModel Model
        {
            get { return _model ?? (_model = new PlotModel()); }
            set
            {
                if (_model == null)
                {
                    _model = new PlotModel();
                }
                _model = value;
                RaisePropertyChanged();
            }
        }

        public List<LineSeries> LineSeries
        {
            get
            {
                return Model.Series.Select(series => series as LineSeries)
                            .ToList();
            }
            set
            {
                Model.Series.Clear();
                foreach (var lineSeries in value)
                {
                    Model.Series.Add(lineSeries);
                }
            }
        }

        public LinearAxis LeftAxis
        {
            get { return _leftAxis ?? (_leftAxis = new LinearAxis(AxisPosition.Left)); }
            set
            {
                if (_leftAxis == null)
                {
                    _leftAxis = new LinearAxis(AxisPosition.Left);
                }
                _leftAxis = value;
                RaisePropertyChanged();
            }
        }

        public LinearAxis BottomAxis
        {
            get { return _bottomAxis ?? (_bottomAxis = new LinearAxis(AxisPosition.Bottom)); }
            set
            {
                if (_bottomAxis == null)
                {
                    _bottomAxis = new LinearAxis(AxisPosition.Bottom);
                }
                _bottomAxis = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region Declarations

        private Int64 Ticks = 0;

        #endregion

        /// <summary>
        /// </summary>
        /// <param name="title"></param>
        public PrivatePlotModel(string title = "")
        {
            Model.Title = title;
            Model.Axes.Add(LeftAxis);
            Model.Axes.Add(BottomAxis);
        }

        /// <summary>
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        public void AddDataPoint(int index, double value)
        {
            if (Model.Series.Count <= index)
            {
                return;
            }
            DispatcherHelper.Invoke(delegate
            {
                var points = LineSeries[index].Points;
                double lastValue = 0;
                var lastOrDefault = points.LastOrDefault();
                if (lastOrDefault != null)
                {
                    lastValue = lastOrDefault.Y;
                }
                var newValue = value - lastValue;
                points.Add(new DataPoint(Ticks, newValue < 0 ? 0 : newValue));
                ++Ticks;
                BottomAxis.Minimum = Ticks < 50 ? 0 : Ticks - 50;
                BottomAxis.Maximum = BottomAxis.Minimum + 50;
                Model.RefreshPlot(true);
            });
        }

        #region Implementation of INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private void RaisePropertyChanged([CallerMemberName] string caller = "")
        {
            PropertyChanged(this, new PropertyChangedEventArgs(caller));
        }

        #endregion
    }
}
