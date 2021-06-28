using System.Collections.Generic;

namespace Acorisoft.Platform.Windows.ViewModels
{
    public class ViewModelParameter : Dictionary<string, object>
    {
        private static readonly object Empty = new object();
        private const string Arg1Name = "Arg1";
        private const string Arg2Name = "Arg2";
        private const string Arg3Name = "Arg3";
        private const string Arg4Name = "Arg4";
        private const string Arg5Name = "Arg5";
        private const string Arg6Name = "Arg6";
        private const string Arg7Name = "Arg7";
        private const string Arg8Name = "Arg8";

        public object Arg1
        {
            get => TryGetValue(Arg1Name, out var val) ? val : Empty;
            set => TryAdd(Arg1Name, value);
        }

        public object Arg2
        {
            get => TryGetValue(Arg2Name, out var val) ? val : Empty;
            set => TryAdd(Arg2Name, value);
        }

        public object Arg3
        {
            get => TryGetValue(Arg3Name, out var val) ? val : Empty;
            set => TryAdd(Arg3Name, value);
        }

        public object Arg4
        {
            get => TryGetValue(Arg4Name, out var val) ? val : Empty;
            set => TryAdd(Arg4Name, value);
        }

        public object Arg5
        {
            get => TryGetValue(Arg5Name, out var val) ? val : Empty;
            set => TryAdd(Arg5Name, value);
        }

        public object Arg6
        {
            get => TryGetValue(Arg6Name, out var val) ? val : Empty;
            set => TryAdd(Arg6Name, value);
        }

        public object Arg7
        {
            get => TryGetValue(Arg7Name, out var val) ? val : Empty;
            set => TryAdd(Arg7Name, value);
        }

        public object Arg8
        {
            get => TryGetValue(Arg8Name, out var val) ? val : Empty;
            set => TryAdd(Arg8Name, value);
        }
    }
}