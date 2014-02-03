using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace FuwanViewer.Presentation
{
    /// <summary>
    /// Contains helper methods for UI, so far just one for showing a waitcursor.
    /// </summary>
    /// <remarks>
    /// Setting busy state n-times require at least n calls to clear busy state to work properly, after
    /// which cursor will go back to normal as soon as application is in idle state.
    /// </remarks>
    public static class UiServices
    {
        private static int _busyCount;

        /// <summary>
        /// Sets cursor to 'Wait' until ClearBusyState is called
        /// </summary>
        public static void SetBusyState()
        {
            _busyCount++;
            if (_busyCount == 1)
            {
                Mouse.OverrideCursor = Cursors.Wait;
            }
        }

        /// <summary>
        /// Restores cursor to normal, as soon as application is idle
        /// </summary>
        public static void ClearBusyState()
        {
            new DispatcherTimer(TimeSpan.Zero, DispatcherPriority.ApplicationIdle, dispatcherTimer_Tick, Application.Current.Dispatcher);
        }

        /// <summary>
        /// Handles the Tick event of the dispatcherTimer control to restore old cursor, if approperiate.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private static void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            var dispatcherTimer = sender as DispatcherTimer;
            if (dispatcherTimer != null)
            {
                if (_busyCount > 0)
                    _busyCount--;

                if (_busyCount == 0)
                    Mouse.OverrideCursor = Cursors.Arrow;

                dispatcherTimer.Stop();
            }
        }
    }
}
