﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SpongeBot.Utility
{
    class Screen
    {


        public static double getWidthScalingFactor()
        {
            return (double)Screen.execute(() => 
            { 
                // execute on UI thread due to Access restriction 
                PresentationSource _presentationSource = PresentationSource.FromVisual(Application.Current.MainWindow);
                return _presentationSource.CompositionTarget.TransformToDevice.M11;
            });
        }

        public static double getHeightScalingFactor()
        {
            return (double)Screen.execute(() =>
            {
                // execute on UI thread due to Access restriction
                PresentationSource _presentationSource = PresentationSource.FromVisual(Application.Current.MainWindow);
                return _presentationSource.CompositionTarget.TransformToDevice.M22;
            });
        }

        public static double getActualPrimaryScreenWidth()
        {
            int scaledScreenWidth = (int)System.Windows.SystemParameters.PrimaryScreenWidth;
            return scaledScreenWidth * getWidthScalingFactor();

        }

        public static double getActualPrimaryScreenHeight()
        {
            int scaledScreenHeight = (int)System.Windows.SystemParameters.PrimaryScreenHeight;
            return scaledScreenHeight * getWidthScalingFactor();
        }

        private static object execute(Func<object> someFunction)
        {
            return Application.Current.Dispatcher.Invoke(someFunction);
        }

    }
}
