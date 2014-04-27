// Type: System.Timers.ElapsedEventArgs
// Assembly: System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// Assembly location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.0\System.dll

using System;
using System.Runtime;

namespace System.Timers
{
    public class ElapsedEventArgs : EventArgs
    {
        public DateTime SignalTime { [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        get; }
    }
}
