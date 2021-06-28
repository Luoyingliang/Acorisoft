using System.Windows.Input;
using Acorisoft.Platform.Windows.Controls;

namespace Acorisoft.Platform.Windows
{
    public class PlatformCommands
    {
        static PlatformCommands()
        {
            ToggleIxLeft = Create("IxContentHostCommands.ToggleIxLeft");
            ToggleIxRight = Create("IxContentHostCommands.ToggleIxRight");
            ToggleIxUp = Create("IxContentHostCommands.ToggleIxUp");
            ToggleIxDown = Create("IxContentHostCommands.ToggleIxDown");
            ToggleSwipe = Create("IxContentHostCommands.ToggleSwipe");
            ToggleEnable = Create("IxContentHostCommands.ToggleEnable");
        }

        public static RoutedUICommand Create(string name)
        {
            return new RoutedUICommand(name, name, typeof(IxContentHostCommands));
        }

        /// <summary>
        /// 
        /// </summary>
        public static RoutedCommand ToggleIxLeft { get; }

        /// <summary>
        /// 
        /// </summary>
        public static RoutedCommand ToggleIxRight { get; }

        /// <summary>
        /// 
        /// </summary>
        public static RoutedCommand ToggleIxUp { get; }

        /// <summary>
        /// 
        /// </summary>
        public static RoutedCommand ToggleIxDown { get; }


        /// <summary>
        /// 
        /// </summary>
        public static RoutedCommand ToggleSwipe { get; }

        /// <summary>
        /// 
        /// </summary>
        public static RoutedCommand ToggleEnable { get; }
    }
}