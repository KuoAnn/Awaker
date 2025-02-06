using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace Awaker
{
    public static class SystemSleep
    {
        [DllImport("kernel32")] private static extern ExecutionState SetThreadExecutionState(ExecutionState esFlags);

        /// <summary>
        /// 設定此執行緒此時開始一直將處於執行狀態，此時計算機不應該進入睡眠狀態。
        /// 此執行緒退出後，設定將失效。
        /// 如果需要恢復，請呼叫 <see cref="RestoreForCurrentThread"/> 方法。
        /// </summary>
        /// <param name="keepDisplayOn">
        /// 表示是否應該同時保持螢幕不關閉。
        /// 對於遊戲、視訊和演示相關的任務需要保持螢幕不關閉；而對於後臺服務、下載和監控等任務則不需要。
        /// </param>
        public static void PreventForCurrentThread(bool keepDisplayOn = true)
        {
            SetThreadExecutionState(keepDisplayOn
                ? ExecutionState.Continuous | ExecutionState.SystemRequired | ExecutionState.DisplayRequired
                : ExecutionState.Continuous | ExecutionState.SystemRequired);
        }

        /// <summary>
        /// 恢復此執行緒的執行狀態，作業系統現在可以正常進入睡眠狀態和關閉螢幕。
        /// </summary>
        public static void RestoreForCurrentThread()
        {
            SetThreadExecutionState(ExecutionState.Continuous);
        }

        [Flags]
        private enum ExecutionState : uint
        {
            SystemRequired = 0x01,
            DisplayRequired = 0x02,

            [Obsolete("This value is not supported.")]
            UserPresent = 0x04,

            AwaymodeRequired = 0x40,
            Continuous = 0x80000000,
        }
    }
}