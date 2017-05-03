using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace HandleCSharp
{
    /// <summary>
    /// 
    /// </summary>
    public class Program
    {
        #region 基本的な宣言

        /// <summary>
        /// <c>false</c> を表します。
        /// </summary>
        public const int FALSE = 0;

        #endregion

        #region ハンドル操作

        /// <summary>
        /// 開いているオブジェクトハンドルを閉じます。
        /// </summary>
        /// <param name="hObject">開いているオブジェクトのハンドルを指定します。</param>
        /// <returns>
        /// 関数が成功すると、<c>true</c> が返ります。
        /// 関数が失敗すると、<c>false</c> が返ります。
        /// </returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool CloseHandle(IntPtr hObject);

        #endregion

        #region メモリー関連

        /// <summary>
        /// Fills a block of memory with zeros.
        /// </summary>
        /// <param name="destination">A pointer to the starting address of the block of memory to fill with zeros.</param>
        /// <param name="length">The size of the block of memory to fill with zeros, in bytes.</param>
        [DllImport("Kernel32.dll", EntryPoint = "RtlZeroMemory", SetLastError = false)]
        static extern void ZeroMemory(IntPtr destination, IntPtr length);

        #endregion

        #region プロセス関連

        /// <summary>
        /// プロセスオブジェクトで認められるアクセス方法を表します。
        /// </summary>
        [Flags]
        public enum ProcessAccessFlags : uint
        {
            /// <summary>
            /// 利用可能な範囲で、プロセスオブジェクトに対するすべてのアクセス権を指定します。
            /// </summary>
            All = 0x001F0FFF,

            /// <summary>
            /// プロセスを終了するために、TerminateProcess 関数がプロセスのハンドルを使うことを認めます。
            /// </summary>
            Terminate = 0x00000001,

            /// <summary>
            /// プロセス内にスレッドを作成するために、CreateRemoteThread 関数がプロセスのハンドルを使うことを認めます。
            /// </summary>
            CreateThread = 0x00000002,

            /// <summary>
            /// プロセスの仮想メモリを変更するために、VirtualProtectEx 関数、または WriteProcessMemory 関数がプロセスのハンドルを使うことを認めます。
            /// </summary>
            VirtualMemoryOperation = 0x00000008,

            /// <summary>
            /// プロセスの仮想メモリの内容を読み取るために、ReadProcessMemory 関数がプロセスのハンドルを使うことを認めます。
            /// </summary>
            VirtualMemoryRead = 0x00000010,

            /// <summary>
            /// プロセスの仮想メモリへの書き込みを行うために、WriteProcessMemory 関数がプロセスのハンドルを使うことを認めます。
            /// </summary>
            VirtualMemoryWrite = 0x00000020,

            /// <summary>
            /// ハンドルを複製するために、 関数が複製元または複製先としてプロセスのハンドルを使うことを認めます。
            /// </summary>
            DuplicateHandle = 0x00000040,

            /// <summary>
            /// 内部で使います。
            /// </summary>
            CreateProcess = 0x000000080,

            /// <summary>
            /// メモリの上限(クォータ)を設定するために、AssignProcessToJobObject 関数と SetProcessWorkingSetSize 関数がプロセスのハンドルを使うことを認めます。
            /// </summary>
            SetQuota = 0x00000100,

            /// <summary>
            /// このプロセスの優先順位クラスを設定するために、SetPriorityClass 関数がプロセスのハンドルを使うことを認めます。
            /// </summary>
            SetInformation = 0x00000200,

            /// <summary>
            /// プロセスオブジェクトから情報を読み取るために、GetExitCodeProcess 関数と GetPriorityClass 関数がプロセスのハンドルを使うことを認めます。
            /// </summary>
            QueryInformation = 0x00000400,

            /// <summary>
            /// Required to retrieve certain information about a process (see GetExitCodeProcess, GetPriorityClass, IsProcessInJob, QueryFullProcessImageName).
            /// </summary>
            QueryLimitedInformation = 0x00001000,

            /// <summary>
            /// このプロセスが終了するのを待つために、待機関数がこのプロセスのハンドルを使うことを認めます。
            /// </summary>
            Synchronize = 0x00100000
        }

        /// <summary>
        /// 現在のプロセスに対応する疑似ハンドルを取得します。
        /// </summary>
        /// <returns>現在のプロセスの疑似ハンドルが返ります。</returns>
        /// <remarks>
        /// 疑似ハンドルとは、現在のプロセスのハンドルと解釈される特別な定数のことです。
        /// 擬似ハンドルは、不要になっても閉じる必要がありません。
        /// </remarks>
        [DllImport("kernel32.dll")]
        public static extern IntPtr GetCurrentProcess();

        /// <summary>
        /// 既存のプロセスオブジェクトのハンドルを開きます。
        /// </summary>
        /// <param name="processAccess">プロセスオブジェクトで認められるアクセス方法を指定します。</param>
        /// <param name="bInheritHandle">現在のプロセスが新しいプロセスを作成する際に、新しいプロセスが、取得されたハンドルを継承できるかどうかを指定します。</param>
        /// <param name="processId">開くべきプロセスの識別子を指定します。</param>
        /// <returns>
        /// 関数が成功すると、指定したプロセスの既に開いているハンドルが返ります。
        /// 関数が失敗すると、<see cref="IntPtr.Zero"/> が返ります。
        /// </returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr OpenProcess(ProcessAccessFlags processAccess, bool bInheritHandle, int processId);

        #endregion

        #region ファイル関連

        /// <summary>
        /// ファイルパスの最大長さを表します。
        /// </summary>
        public const int MAX_PATH = 260;

        /// <summary>
        /// ファイルの種類を表します。
        /// </summary>
        public enum FileType : uint
        {
            /// <summary>
            /// LPT デバイスやコンソールのような文字ファイル。
            /// </summary>
            Char = 0x0002,

            /// <summary>
            /// ディスクファイル。
            /// </summary>
            Disk = 0x0001,

            /// <summary>
            /// 名前付きまたは名前なしパイプ。
            /// </summary>
            Pipe = 0x0003,

            /// <summary>
            /// 不明。
            /// </summary>
            Unknown = 0x0000,
        }

        /// <summary>
        /// 指定されたファイルの種類を取得します。
        /// </summary>
        /// <param name="hFile">開いているファイルのハンドルを指定します。</param>
        /// <returns>ファイルの種類。</returns>
        [DllImport("kernel32.dll")]
        public static extern FileType GetFileType(IntPtr hFile);

        /// <summary>
        /// アプリケーションで MS-DOS デバイス名に関する情報を取得できるようにします。
        /// </summary>
        /// <param name="lpDeviceName">照会する MS-DOS デバイス名文字列へのポインタを指定します。</param>
        /// <param name="lpTargetPath">照会結果を受け取るバッファを指定します。</param>
        /// <param name="ucchMax"><see para="lpTargetPath"/> パラメータが示すバッファに格納できる最大文字数を指定します。</param>
        /// <returns>
        /// 関数が成功すると、<see para="lpTargetPath"/> パラメータが示すバッファに格納されたデータの文字数が返ります。
        /// 関数が失敗すると、0 が返ります。
        /// </returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern uint QueryDosDevice(string lpDeviceName, StringBuilder lpTargetPath, int ucchMax);

        #endregion

        #region 特権関連

        /// <summary>
        /// <para>Required to debug and adjust the memory of a process owned by another account.</para>
        /// <para>User Right: Debug programs.</para>
        /// </summary>
        public const string SE_DEBUG_NAME = "SeDebugPrivilege";

        /// <summary>
        /// The function enables the privilege.
        /// </summary>
        public const int SE_PRIVILEGE_ENABLED = 0x00000002;

        /// <summary>
        /// Combines DELETE, READ_CONTROL, WRITE_DAC, and WRITE_OWNER access.
        /// </summary>
        public const UInt32 STANDARD_RIGHTS_REQUIRED = 0x000F0000;

        /// <summary>
        /// Currently defined to equal READ_CONTROL.
        /// </summary>
        public const UInt32 STANDARD_RIGHTS_READ = 0x00020000;

        /// <summary>
        /// Required to attach a primary token to a process. The SE_ASSIGNPRIMARYTOKEN_NAME privilege is also required to accomplish this task.
        /// </summary>
        public const UInt32 TOKEN_ASSIGN_PRIMARY = 0x0001;

        /// <summary>
        /// Required to duplicate an access token.
        /// </summary>
        public const UInt32 TOKEN_DUPLICATE = 0x0002;

        /// <summary>
        /// Required to attach an impersonation access token to a process.
        /// </summary>
        public const UInt32 TOKEN_IMPERSONATE = 0x0004;

        /// <summary>
        /// Required to query an access token.
        /// </summary>
        public const UInt32 TOKEN_QUERY = 0x0008;

        /// <summary>
        /// Required to query the source of an access token.
        /// </summary>
        public const UInt32 TOKEN_QUERY_SOURCE = 0x0010;

        /// <summary>
        /// Required to enable or disable the privileges in an access token.
        /// </summary>
        public const UInt32 TOKEN_ADJUST_PRIVILEGES = 0x0020;

        /// <summary>
        /// Required to adjust the attributes of the groups in an access token.
        /// </summary>
        public const UInt32 TOKEN_ADJUST_GROUPS = 0x0040;

        /// <summary>
        /// Required to change the default owner, primary group, or DACL of an access token.
        /// </summary>
        public const UInt32 TOKEN_ADJUST_DEFAULT = 0x0080;

        /// <summary>
        /// Required to adjust the session ID of an access token. The SE_TCB_NAME privilege is required.
        /// </summary>
        public const UInt32 TOKEN_ADJUST_SESSIONID = 0x0100;

        /// <summary>
        /// Combines STANDARD_RIGHTS_READ and TOKEN_QUERY.
        /// </summary>
        public const UInt32 TOKEN_READ = (STANDARD_RIGHTS_READ | TOKEN_QUERY);

        /// <summary>
        /// Combines all possible access rights for a token.
        /// </summary>
        public const UInt32 TOKEN_ALL_ACCESS = (STANDARD_RIGHTS_REQUIRED | TOKEN_ASSIGN_PRIMARY |
            TOKEN_DUPLICATE | TOKEN_IMPERSONATE | TOKEN_QUERY | TOKEN_QUERY_SOURCE |
            TOKEN_ADJUST_PRIVILEGES | TOKEN_ADJUST_GROUPS | TOKEN_ADJUST_DEFAULT |
            TOKEN_ADJUST_SESSIONID);

        /// <summary>
        /// Guaranteed to be unique only on the system on which it was generated. 
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct LUID
        {
            /// <summary>
            /// Low-order bits.
            /// </summary>
            public uint LowPart;

            /// <summary>
            /// High-order bits.
            /// </summary>
            public int HighPart;
        }

        /// <summary>
        /// Represents a locally unique identifier (LUID) and its attributes.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct LUID_AND_ATTRIBUTES
        {
            /// <summary>
            /// Specifies an <see cref="LUID"/> value.
            /// </summary>
            public LUID Luid;

            /// <summary>
            /// Specifies attributes of the <see cref="Luid"/>.
            /// </summary>
            public uint Attributes;
        }

        /// <summary>
        /// Contains information about a set of privileges for an access token.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct TOKEN_PRIVILEGES
        {
            /// <summary>
            /// The number of entries in the Privileges array.
            /// </summary>
            public uint PrivilegeCount;

            /// <summary>
            /// Specifies an array of <see cref="LUID_AND_ATTRIBUTES"/> structures.
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            public LUID_AND_ATTRIBUTES[] Privileges;
        }

        /// <summary>
        /// Retrieves the locally unique identifier (LUID) used on a specified system to locally represent the specified privilege name.
        /// </summary>
        /// <param name="lpSystemName">Specifies the name of the system on which the privilege name is retrieved.</param>
        /// <param name="lpName">Specifies the name of the privilege.</param>
        /// <param name="lpLuid">The LUID by which the privilege is known on the system specified by the lpSystemName parameter.</param>
        /// <returns>
        /// If the function succeeds, the function returns <c>true</c>.
        /// If the function fails, it returns <c>false</c>.
        /// </returns>
        [DllImport("advapi32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool LookupPrivilegeValue(string lpSystemName, string lpName, out LUID lpLuid);

        /// <summary>
        /// Opens the access token associated with a process.
        /// </summary>
        /// <param name="ProcessHandle">A handle to the process whose access token is opened.</param>
        /// <param name="DesiredAccess">Specifies an access mask that specifies the requested types of access to the access token.</param>
        /// <param name="TokenHandle">Handle that identifies the newly opened access token when the function returns.</param>
        /// <returns>
        /// If the function succeeds, the function returns <c>true</c>.
        /// If the function fails, it returns <c>false</c>.
        /// </returns>
        [DllImport("advapi32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool OpenProcessToken(IntPtr ProcessHandle, UInt32 DesiredAccess, out IntPtr TokenHandle);

        /// <summary>
        /// Enables or disables privileges in the specified access token. 
        /// </summary>
        /// <param name="TokenHandle">A handle to the access token that contains the privileges to be modified.</param>
        /// <param name="DisableAllPrivileges">Specifies whether the function disables all of the token's privileges.</param>
        /// <param name="NewState"><see cref="TOKEN_PRIVILEGES"/> structure that specifies an array of privileges and their attributes.</param>
        /// <param name="Bufferlength">Specifies the size, in bytes, of the buffer pointed to by the PreviousState parameter.</param>
        /// <param name="PreviousState">A pointer to a buffer that the function fills with a <see cref="TOKEN_PRIVILEGES"/> structure that contains the previous state of any privileges that the function modifies.</param>
        /// <param name="ReturnLength">A pointer to a variable that receives the required size, in bytes, of the buffer pointed to by the PreviousState parameter.</param>
        /// <returns>
        /// If the function succeeds, the function returns <c>true</c>.
        /// If the function fails, it returns <c>false</c>.
        /// </returns>
        [DllImport("advapi32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool AdjustTokenPrivileges(IntPtr TokenHandle, bool DisableAllPrivileges, ref TOKEN_PRIVILEGES NewState, uint Bufferlength, IntPtr PreviousState, IntPtr ReturnLength);

        /// <summary>
        /// Enables or disables privileges in the specified access token. 
        /// </summary>
        /// <param name="TokenHandle">Handle to the access token that contains the privileges to be modified.</param>
        /// <param name="DisableAllPrivileges">Specifies whether the function disables all of the token's privileges.</param>
        /// <param name="NewState"><see cref="TOKEN_PRIVILEGES"/> structure that specifies an array of privileges and their attributes.</param>
        /// <param name="BufferLength">Specifies the size, in bytes, of the buffer pointed to by the PreviousState parameter.</param>
        /// <param name="PreviousState"><see cref="TOKEN_PRIVILEGES"/> structure that contains the previous state of any privileges that the function modifies.</param>
        /// <param name="ReturnLength">Variable that receives the required size, in bytes, of the buffer pointed to by the PreviousState parameter.</param>
        /// <returns>
        /// If the function succeeds, the function returns <c>true</c>.
        /// If the function fails, it returns <c>false</c>.
        /// </returns>
        [DllImport("advapi32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool AdjustTokenPrivileges(IntPtr TokenHandle, bool DisableAllPrivileges, ref TOKEN_PRIVILEGES NewState, uint BufferLength, out TOKEN_PRIVILEGES PreviousState, out uint ReturnLength);

        /// <summary>
        /// 指定された特権を有効にします。
        /// </summary>
        /// <param name="privilegeName">特権名。</param>
        /// <exception cref="Win32Exception">API の呼び出しに失敗しました。</exception>
        static void EnablePrivilege(string privilegeName)
        {
            IntPtr hToken = IntPtr.Zero;

            try
            {
                TOKEN_PRIVILEGES privileges = new TOKEN_PRIVILEGES()
                {
                    PrivilegeCount = 1,
                    Privileges = new LUID_AND_ATTRIBUTES[] { new LUID_AND_ATTRIBUTES() { Attributes = SE_PRIVILEGE_ENABLED } }
                };

                if (LookupPrivilegeValue(string.Empty, privilegeName, out privileges.Privileges[0].Luid) == false)
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error().ToString());
                }

                if (OpenProcessToken(GetCurrentProcess(), TOKEN_ADJUST_PRIVILEGES, out hToken) == false)
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error().ToString());
                }

                if (AdjustTokenPrivileges(hToken, false, ref privileges, (uint)Marshal.SizeOf(privileges), IntPtr.Zero, IntPtr.Zero) == false)
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error().ToString());
                }
            }
            finally
            {
                if (hToken != IntPtr.Zero)
                {
                    CloseHandle(hToken);
                }
            }

            return;
        }

        #endregion

        #region NT カーネル API 関連

        /// <summary>
        /// Instead of using the HandleAttributes parameter, copy the attributes from the source handle to the target handle.
        /// </summary>
        public const uint DUPLICATE_SAME_ATTRIBUTES = 0x00000004;

        /// <summary>
        /// NT カーネル API のステータスコードを表します。
        /// </summary>
        /// <remarks>
        /// NTSTATUS エラーコード一覧
        /// http://accelart.jp/blog/NTSTATUSErrMsgJa.html
        /// </remarks>
        public enum NtStatus : uint
        {
            /// <summary> 
            /// 操作は正常に終了しました。
            /// </summary>
            STATUS_SUCCESS = 0x00000000,

            /// <summary> 
            /// STATUS_WAIT_0
            /// </summary>
            STATUS_WAIT_0 = 0x00000000,

            /// <summary> 
            /// STATUS_WAIT_1
            /// </summary>
            STATUS_WAIT_1 = 0x00000001,

            /// <summary> 
            /// STATUS_WAIT_2
            /// </summary>
            STATUS_WAIT_2 = 0x00000002,

            /// <summary> 
            /// STATUS_WAIT_3
            /// </summary>
            STATUS_WAIT_3 = 0x00000003,

            /// <summary> 
            /// STATUS_WAIT_63
            /// </summary>
            STATUS_WAIT_63 = 0x0000003f,

            /// <summary> 
            /// STATUS_ABANDONED
            /// </summary>
            STATUS_ABANDONED = 0x00000080,

            /// <summary> 
            /// STATUS_ABANDONED_WAIT_0
            /// </summary>
            STATUS_ABANDONED_WAIT_0 = 0x00000080,

            /// <summary> 
            /// STATUS_ABANDONED_WAIT_63
            /// </summary>
            STATUS_ABANDONED_WAIT_63 = 0x000000bf,

            /// <summary> 
            /// STATUS_USER_APC
            /// </summary>
            STATUS_USER_APC = 0x000000c0,

            /// <summary> 
            /// STATUS_KERNEL_APC
            /// </summary>
            STATUS_KERNEL_APC = 0x00000100,

            /// <summary> 
            /// STATUS_ALERTED
            /// </summary>
            STATUS_ALERTED = 0x00000101,

            /// <summary> 
            /// STATUS_TIMEOUT
            /// </summary>
            STATUS_TIMEOUT = 0x00000102,

            /// <summary> 
            /// 要求した操作は完了待ちの状態です。
            /// </summary>
            STATUS_PENDING = 0x00000103,

            /// <summary> 
            /// ファイル名の結果がシンボリック リンクになったため、オブジェクト マネージャによる再解析が必要です。
            /// </summary>
            STATUS_REPARSE = 0x00000104,

            /// <summary> 
            /// 連続呼び出しのために追加情報を利用できることを示すために、列挙 API から返されました。
            /// </summary>
            STATUS_MORE_ENTRIES = 0x00000105,

            /// <summary> 
            /// 参照したすべての特権が呼び出し側に割り当てられていないことを示します。
            /// この機能により、たとえば、割り当てられている特権を正確に知らなくても、すべての特権を無効にできます。
            /// </summary>
            STATUS_NOT_ALL_ASSIGNED = 0x00000106,

            /// <summary> 
            /// 一部の情報は変換されませんでした。
            /// </summary>
            STATUS_SOME_NOT_MAPPED = 0x00000107,

            /// <summary> 
            /// oplock ブレークの実行中に、開いたりまたは作成したりする操作が完了しました。
            /// </summary>
            STATUS_OPLOCK_BREAK_IN_PROGRESS = 0x00000108,

            /// <summary> 
            /// 新しいボリュームがファイル システムによってマウントされました。
            /// </summary>
            STATUS_VOLUME_MOUNTED = 0x00000109,

            /// <summary> 
            /// この正常終了レベル状態は、レジストリ サブツリーに対してトランザクション状態が既に存在するが、トランザクション コミットは以前に打ち切られたことを示します。
            /// コミットはこの時点で完了しました。
            /// </summary>
            STATUS_RXACT_COMMITTED = 0x0000010a,

            /// <summary> 
            /// これは、通知変更要求を出したハンドルを閉じたため、通知変更要求が終了したことを示します。
            /// </summary>
            STATUS_NOTIFY_CLEANUP = 0x0000010b,

            /// <summary> 
            /// これは、通知変更要求が終了しているため、情報が呼び出し側のバッファに返されていないことを示します。呼び出し側は、変更結果を検索するためにファイルを列挙しなければなりません。
            /// </summary>
            STATUS_NOTIFY_ENUM_DIR = 0x0000010c,

            /// <summary> 
            /// このアカウントに対してシステム クォータ制限値が設定されていません。
            /// </summary>
            STATUS_NO_QUOTAS_FOR_ACCOUNT = 0x0000010d,

            /// <summary> 
            /// プライマリ トランスポートのリモート サーバー hs に接続しようとしましたが、接続できませんでした。
            /// コンピュータはセカンダリ トランスポートでは接続できました。
            /// </summary>
            STATUS_PRIMARY_TRANSPORT_CONNECT_FAILED = 0x0000010e,

            /// <summary> 
            /// ページ フォールトはトランジション エラーでした。
            /// </summary>
            STATUS_PAGE_FAULT_TRANSITION = 0x00000110,

            /// <summary> 
            /// STATUS_PAGE_FAULT_DEMAND_ZERO
            /// </summary>
            STATUS_PAGE_FAULT_DEMAND_ZERO = 0x00000111,

            /// <summary> 
            /// STATUS_PAGE_FAULT_COPY_ON_WRITE
            /// </summary>
            STATUS_PAGE_FAULT_COPY_ON_WRITE = 0x00000112,

            /// <summary> 
            /// STATUS_PAGE_FAULT_GUARD_PAGE
            /// </summary>
            STATUS_PAGE_FAULT_GUARD_PAGE = 0x00000113,

            /// <summary> 
            /// ページ フォールトは 2 次記憶装置から読み取ることで、解決しました。
            /// </summary>
            STATUS_PAGE_FAULT_PAGING_FILE = 0x00000114,

            /// <summary> 
            /// 操作中にキャッシュ ページがロックされました。
            /// </summary>
            STATUS_CACHE_PAGE_LOCKED = 0x00000115,

            /// <summary> 
            /// クラッシュ ダンプはページング ファイルにあります。
            /// </summary>
            STATUS_CRASH_DUMP = 0x00000116,

            /// <summary> 
            /// 指定されたバッファはすべて 0 です。
            /// </summary>
            STATUS_BUFFER_ALL_ZEROS = 0x00000117,

            /// <summary> 
            /// ファイル名の結果がシンボリック リンクになったため、オブジェクト マネージャによる再解析が必要です。
            /// </summary>
            STATUS_REPARSE_OBJECT = 0x00000118,

            /// <summary> 
            /// デバイスが照会停止を行い、それに必要なリソースが変更されました。
            /// </summary>
            STATUS_RESOURCE_REQUIREMENTS_CHANGED = 0x00000119,

            /// <summary> 
            /// トランスレータは、これらのリソースをグローバル空間に翻訳しました。これ以上翻訳は実行されません。
            /// </summary>
            STATUS_TRANSLATION_COMPLETE = 0x00000120,

            /// <summary> 
            /// ディレクトリ サービスは、グループ メンバシップがグローバル カタログ サーバーと接続できなかっため、グループ メンバシップをローカルで評価しました。
            /// </summary>
            STATUS_DS_MEMBERSHIP_EVALUATED_LOCALLY = 0x00000121,

            /// <summary> 
            /// 終了中のプロセスには終了するスレッドがありません。
            /// </summary>
            STATUS_NOTHING_TO_TERMINATE = 0x00000122,

            /// <summary> 
            /// 指定されたプロセスはジョブの一部ではありません。
            /// </summary>
            STATUS_PROCESS_NOT_IN_JOB = 0x00000123,

            /// <summary> 
            /// 指定されたプロセスはジョブの一部です。
            /// </summary>
            STATUS_PROCESS_IN_JOB = 0x00000124,

            /// <summary> 
            /// STATUS_VOLSNAP_HIBERNATE_READY
            /// </summary>
            STATUS_VOLSNAP_HIBERNATE_READY = 0x00000125,

            /// <summary> 
            /// STATUS_FSFILTER_OP_COMPLETED_SUCCESSFULLY
            /// </summary>
            STATUS_FSFILTER_OP_COMPLETED_SUCCESSFULLY = 0x00000126,

            /// <summary> 
            /// STATUS_INTERRUPT_VECTOR_ALREADY_CONNECTED
            /// </summary>
            STATUS_INTERRUPT_VECTOR_ALREADY_CONNECTED = 0x00000127,

            /// <summary> 
            /// STATUS_INTERRUPT_STILL_CONNECTED
            /// </summary>
            STATUS_INTERRUPT_STILL_CONNECTED = 0x00000128,

            /// <summary> 
            /// STATUS_PROCESS_CLONED
            /// </summary>
            STATUS_PROCESS_CLONED = 0x00000129,

            /// <summary> 
            /// 1 つのセマフォに対するポストが多すぎます。
            /// </summary>
            STATUS_FILE_LOCKED_WITH_ONLY_READERS = 0x0000012a,

            /// <summary> 
            /// ReadProcessMemory 要求または WriteProcessMemory 要求の一部だけを完了しました。
            /// </summary>
            STATUS_FILE_LOCKED_WITH_WRITERS = 0x0000012b,

            /// <summary> 
            /// STATUS_RESOURCEMANAGER_READ_ONLY
            /// </summary>
            STATUS_RESOURCEMANAGER_READ_ONLY = 0x00000202,

            /// <summary> 
            /// STATUS_RING_PREVIOUSLY_EMPTY
            /// </summary>
            STATUS_RING_PREVIOUSLY_EMPTY = 0x00000210,

            /// <summary> 
            /// STATUS_RING_PREVIOUSLY_FULL
            /// </summary>
            STATUS_RING_PREVIOUSLY_FULL = 0x00000211,

            /// <summary> 
            /// STATUS_RING_PREVIOUSLY_ABOVE_QUOTA
            /// </summary>
            STATUS_RING_PREVIOUSLY_ABOVE_QUOTA = 0x00000212,

            /// <summary> 
            /// STATUS_RING_NEWLY_EMPTY
            /// </summary>
            STATUS_RING_NEWLY_EMPTY = 0x00000213,

            /// <summary> 
            /// STATUS_RING_SIGNAL_OPPOSITE_ENDPOINT
            /// </summary>
            STATUS_RING_SIGNAL_OPPOSITE_ENDPOINT = 0x00000214,

            /// <summary> 
            /// デバッガは例外を処理しました。
            /// </summary>
            DBG_EXCEPTION_HANDLED = 0x00010001,

            /// <summary> 
            /// デバッガを続行しました。
            /// </summary>
            DBG_CONTINUE = 0x00010002,

            /// <summary> 
            /// STATUS_FLT_IO_COMPLETE
            /// </summary>
            STATUS_FLT_IO_COMPLETE = 0x001c0001,

            /// <summary> 
            /// オブジェクトを作成しようとしましたが、そのオブジェクト名は既に存在します。
            /// </summary>
            STATUS_OBJECT_NAME_EXISTS = 0x40000000,

            /// <summary> 
            /// スレッドを中断していた間に、スレッドが終了しました。スレッドは再開され、終了処理が続行されました。
            /// </summary>
            STATUS_THREAD_WAS_SUSPENDED = 0x40000001,

            /// <summary> 
            /// ワーキング セットの最小値または最大値を誤った範囲の値に設定しようとしました。
            /// </summary>
            STATUS_WORKING_SET_LIMIT_RANGE = 0x40000002,

            /// <summary> 
            /// イメージ ファイル内に指定されたアドレスにイメージ ファイルを割り当てることができませんでした。ローカルでの調整はこのイメージに対して実行してください。
            /// </summary>
            STATUS_IMAGE_NOT_AT_BASE = 0x40000003,

            /// <summary> 
            /// この情報レベル状態は、指定したレジストリ サブツリー トランザクション状態が存在しないため、作成しなければならないことを示します。
            /// </summary>
            STATUS_RXACT_STATE_CREATED = 0x40000004,

            /// <summary> 
            /// 仮想 DOS コンピュータ (VDM) が MS-DOS または Win16 プログラム セグメント イメージをロード、アンロード、または移動しています。
            /// デバッガがこれらの 16 ビット セグメント内のシンボルとブレークポイントをロード、アンロード、または追跡できるように、例外のレベルを高くします。
            /// </summary>
            STATUS_SEGMENT_NOTIFICATION = 0x40000005,

            /// <summary> 
            /// ローカル RPC 接続に対してユーザー セッション キーが要求されました。返されたセッション キーは定数値であり、この接続固有の値ではありません。
            /// </summary>
            STATUS_LOCAL_USER_SESSION_KEY = 0x40000006,

            /// <summary> 
            /// プロセスはスタートアップの現在のディレクトリに切り替えることができません。
            /// </summary>
            STATUS_BAD_CURRENT_DIRECTORY = 0x40000007,

            /// <summary> 
            /// シリアル I/O 操作がほかのシリアル ポートへの別の書き込み操作によって終了しました(IOCTL_SERIAL_XOFF_COUNTER が 0 になりました)。
            /// </summary>
            STATUS_SERIAL_MORE_WRITES = 0x40000008,

            /// <summary> 
            /// ログまたは代替コピーを使用して、レジストリ データベース内のファイルの 1 つを回復しなければなりませんでした。
            /// ファイルは正しく回復されました。
            /// </summary>
            STATUS_REGISTRY_RECOVERED = 0x40000009,

            /// <summary> 
            /// 読み取り要求を満たすために、Windows NT フォールト トレラント ファイル システムは要求されたデータを冗長コピーから読み取りました。
            /// この操作を実行したのは、ファイル システムがフォールト トレラント ボリュームのメンバから障害を検出しましたが、デバイスの障害領域を再割り当てできなかったためです。
            /// </summary>
            STATUS_FT_READ_RECOVERY_FROM_BACKUP = 0x4000000a,

            /// <summary> 
            /// 書き込み要求を満足するために、Windows NT フォールト トレラント ファイル システムは情報の冗長コピーを書き込みました。
            /// この操作を実行したのは、ファイル システムがフォールト トレラント ボリュームのメンバから障害を検出しましたが、デバイスの障害領域を再割り当てできなかったためです。
            /// </summary>
            STATUS_FT_WRITE_RECOVERY = 0x4000000b,

            /// <summary> 
            /// タイムアウト期間が経過したため、シリアル I/O 操作は終了しました(IOCTL_SERIAL_XOFF_COUNTER は 0 になっていません)。
            /// </summary>
            STATUS_SERIAL_COUNTER_TIMEOUT = 0x4000000c,

            /// <summary> 
            /// Windows パスワードが複雑すぎるため、LAN Manager パスワードに変換できません。
            /// 返された LAN Manager パスワードは NULL 文字列です。
            /// </summary>
            STATUS_NULL_LM_PASSWORD = 0x4000000d,

            /// <summary> 
            /// イメージ ファイルは有効なファイルですが、コンピュータの種類が現在のコンピュータ以外のファイルです。
            /// </summary>
            STATUS_IMAGE_MACHINE_TYPE_MISMATCH = 0x4000000e,

            /// <summary> 
            /// ネットワーク トランスポートは部分的なデータをクライアントに返しました。残りのデータは後で送信されます。
            /// </summary>
            STATUS_RECEIVE_PARTIAL = 0x4000000f,

            /// <summary> 
            /// ネットワーク トランスポートは、リモート システムで優先としてマークされたクライアントにデータを返しました。
            /// </summary>
            STATUS_RECEIVE_EXPEDITED = 0x40000010,

            /// <summary> 
            /// ネットワーク トランスポートは部分的なデータをクライアントに返しました。このデータはリモート システムで優先としてマークされました。残りのデータは後で送信されます。
            /// </summary>
            STATUS_RECEIVE_PARTIAL_EXPEDITED = 0x40000011,

            /// <summary> 
            /// TDI 指示が正常終了しました。
            /// </summary>
            STATUS_EVENT_DONE = 0x40000012,

            /// <summary> 
            /// TDI 指示は保留状態になりました。
            /// </summary>
            STATUS_EVENT_PENDING = 0x40000013,

            /// <summary> 
            /// ファイル システムを確認しています
            /// </summary>
            STATUS_CHECKING_FILE_SYSTEM = 0x40000014,

            /// <summary> 
            /// 致命的なアプリケーション終了が発生しました。
            /// </summary>
            STATUS_FATAL_APP_EXIT = 0x40000015,

            /// <summary> 
            /// 指定したレジストリ キーは定義済みハンドルから参照されています。
            /// </summary>
            STATUS_PREDEFINED_HANDLE = 0x40000016,

            /// <summary> 
            /// ロックしたページのページ保護が 'アクセスなし' に変更され、ページがメモリとプロセスからロック解除されました。
            /// </summary>
            STATUS_WAS_UNLOCKED = 0x40000017,

            /// <summary> 
            /// STATUS_SERVICE_NOTIFICATION
            /// </summary>
            STATUS_SERVICE_NOTIFICATION = 0x40000018,

            /// <summary> 
            /// ロックするページの 1 つが既にロックされています。
            /// </summary>
            STATUS_WAS_LOCKED = 0x40000019,

            /// <summary> 
            /// STATUS_LOG_HARD_ERROR
            /// </summary>
            STATUS_LOG_HARD_ERROR = 0x4000001a,

            /// <summary> 
            /// STATUS_ALREADY_WIN32
            /// </summary>
            STATUS_ALREADY_WIN32 = 0x4000001b,

            /// <summary> 
            /// Win32 x86 エミュレーション サブシステムが使用する例外状態コードです。
            /// </summary>
            STATUS_WX86_UNSIMULATE = 0x4000001c,

            /// <summary> 
            /// Win32 x86 エミュレーション サブシステムが使用する例外状態コードです。
            /// </summary>
            STATUS_WX86_CONTINUE = 0x4000001d,

            /// <summary> 
            /// Win32 x86 エミュレーション サブシステムが使用する例外状態コードです。
            /// </summary>
            STATUS_WX86_SINGLE_STEP = 0x4000001e,

            /// <summary> 
            /// Win32 x86 エミュレーション サブシステムが使用する例外状態コードです。
            /// </summary>
            STATUS_WX86_BREAKPOINT = 0x4000001f,

            /// <summary> 
            /// Win32 x86 エミュレーション サブシステムが使用する例外状態コードです。
            /// </summary>
            STATUS_WX86_EXCEPTION_CONTINUE = 0x40000020,

            /// <summary> 
            /// Win32 x86 エミュレーション サブシステムが使用する例外状態コードです。
            /// </summary>
            STATUS_WX86_EXCEPTION_LASTCHANCE = 0x40000021,

            /// <summary> 
            /// Win32 x86 エミュレーション サブシステムが使用する例外状態コードです。
            /// </summary>
            STATUS_WX86_EXCEPTION_CHAIN = 0x40000022,

            /// <summary> 
            /// イメージ ファイルは有効なファイルですが、コンピュータの種類が現在のコンピュータ以外のファイルです。
            /// </summary>
            STATUS_IMAGE_MACHINE_TYPE_MISMATCH_EXE = 0x40000023,

            /// <summary> 
            /// イールドが実行されましたが、実行できるスレッドがありませんでした。
            /// </summary>
            STATUS_NO_YIELD_PERFORMED = 0x40000024,

            /// <summary> 
            /// タイマ API への再開可能フラグが無視されました。
            /// </summary>
            STATUS_TIMER_RESUME_IGNORED = 0x40000025,

            /// <summary> 
            /// 決定者がこれらのリソースの親リソースへの決定を延期しました。
            /// </summary>
            STATUS_ARBITRATION_UNHANDLED = 0x40000026,

            /// <summary> 
            /// デバイスがスロットに CardBus カードを検出しましたが、このシステムのファームウェアは CardBus モードで CardBus カード バス コントローラを実行できるように構成されていません。
            /// オペレーティング システムはこのコントローラの 16 ビット (R2) PC カードのみサポートしています。
            /// </summary>
            STATUS_CARDBUS_NOT_SUPPORTED = 0x40000027,

            /// <summary> 
            /// Win32 x86 エミュレーション サブシステムが使用する例外状態コードです。
            /// </summary>
            STATUS_WX86_CREATEWX86TIB = 0x40000028,

            /// <summary> 
            /// このマルチプロセッサ システムの CPU は、一部が同じリビジョン レベルではありません。すべてのプロセッサを使用するためにオペレーティング システムをシステムで可能な最小のプロセッサに制限します。このシステムで問題が発生する場合は、CPU 製造元に問い合わせてこの混合プロセッサがサポートされているかどうかを確認してください。
            /// </summary>
            STATUS_MP_PROCESSOR_MISMATCH = 0x40000029,

            /// <summary> 
            /// システムは休止状態に入りました。
            /// </summary>
            STATUS_HIBERNATED = 0x4000002a,

            /// <summary> 
            /// システムは休止状態から再開されました。
            /// </summary>
            STATUS_RESUME_HIBERNATION = 0x4000002b,

            /// <summary> 
            /// STATUS_FIRMWARE_UPDATED
            /// </summary>
            STATUS_FIRMWARE_UPDATED = 0x4000002c,

            /// <summary> 
            /// STATUS_DRIVERS_LEAKING_LOCKED_PAGES
            /// </summary>
            STATUS_DRIVERS_LEAKING_LOCKED_PAGES = 0x4000002d,

            /// <summary> 
            /// STATUS_MESSAGE_RETRIEVED
            /// </summary>
            STATUS_MESSAGE_RETRIEVED = 0x4000002e,

            /// <summary> 
            /// STATUS_SYSTEM_POWERSTATE_TRANSITION
            /// </summary>
            STATUS_SYSTEM_POWERSTATE_TRANSITION = 0x4000002f,

            /// <summary> 
            /// STATUS_ALPC_CHECK_COMPLETION_LIST
            /// </summary>
            STATUS_ALPC_CHECK_COMPLETION_LIST = 0x40000030,

            /// <summary> 
            /// STATUS_SYSTEM_POWERSTATE_COMPLEX_TRANSITION
            /// </summary>
            STATUS_SYSTEM_POWERSTATE_COMPLEX_TRANSITION = 0x40000031,

            /// <summary> 
            /// STATUS_ACCESS_AUDIT_BY_POLICY
            /// </summary>
            STATUS_ACCESS_AUDIT_BY_POLICY = 0x40000032,

            /// <summary> 
            /// STATUS_ABANDON_HIBERFILE
            /// </summary>
            STATUS_ABANDON_HIBERFILE = 0x40000033,

            /// <summary> 
            /// STATUS_BIZRULES_NOT_ENABLED
            /// </summary>
            STATUS_BIZRULES_NOT_ENABLED = 0x40000034,

            /// <summary> 
            /// デバッガは後で応答します。
            /// </summary>
            DBG_REPLY_LATER = 0x40010001,

            /// <summary> 
            /// デバッガはハンドルを提供できません。
            /// </summary>
            DBG_UNABLE_TO_PROVIDE_HANDLE = 0x40010002,

            /// <summary> 
            /// デバッガはスレッドを強制終了しました。
            /// </summary>
            DBG_TERMINATE_THREAD = 0x40010003,

            /// <summary> 
            /// デバッガはプロセスを強制終了しました。
            /// </summary>
            DBG_TERMINATE_PROCESS = 0x40010004,

            /// <summary> 
            /// デバッガはコントロール C を取得しました。
            /// </summary>
            DBG_CONTROL_C = 0x40010005,

            /// <summary> 
            /// デバッガはコントロール C 上で例外を印刷しました。
            /// </summary>
            DBG_PRINTEXCEPTION_C = 0x40010006,

            /// <summary> 
            /// デバッガは RIP 例外を受信しました。
            /// </summary>
            DBG_RIPEXCEPTION = 0x40010007,

            /// <summary> 
            /// デバッガはコントロール ブレークを取得しました。
            /// </summary>
            DBG_CONTROL_BREAK = 0x40010008,

            /// <summary> 
            /// DBG_COMMAND_EXCEPTION
            /// </summary>
            DBG_COMMAND_EXCEPTION = 0x40010009,

            /// <summary> 
            /// STATUS_FLT_BUFFER_TOO_SMALL
            /// </summary>
            STATUS_FLT_BUFFER_TOO_SMALL = 0x801c0001,

            /// <summary> 
            /// スタックや配列など、データ構造の最後としてマークされているメモリのページがアクセスされました。
            /// </summary>
            STATUS_GUARD_PAGE_VIOLATION = 0x80000001,

            /// <summary> 
            /// ロード命令または記憶命令でデータ型の不整列が検出されました。
            /// </summary>
            STATUS_DATATYPE_MISALIGNMENT = 0x80000002,

            /// <summary> 
            /// ブレークポイントに到達しました。
            /// </summary>
            STATUS_BREAKPOINT = 0x80000003,

            /// <summary> 
            /// シングル ステップまたはトレース操作が終了しました。
            /// </summary>
            STATUS_SINGLE_STEP = 0x80000004,

            /// <summary> 
            /// データが大きすぎるため、指定したバッファに格納できません。
            /// </summary>
            STATUS_BUFFER_OVERFLOW = 0x80000005,

            /// <summary> 
            /// ファイル指定と一致するファイルはこれ以上ありません。
            /// </summary>
            STATUS_NO_MORE_FILES = 0x80000006,

            /// <summary> 
            /// 割り込みによってシステム デバッガが起動されました。
            /// </summary>
            STATUS_WAKE_SYSTEM_DEBUGGER = 0x80000007,

            /// <summary> 
            /// 要求した操作の結果として、オブジェクトに対するハンドルが自動的に閉じました。
            /// </summary>
            STATUS_HANDLES_CLOSED = 0x8000000a,

            /// <summary> 
            /// アクセス制御リスト (ACL) に継承可能なコンポーネントが登録されていません。
            /// </summary>
            STATUS_NO_INHERITANCE = 0x8000000b,

            /// <summary> 
            /// グローバル識別子 (GUID) を Windows セキュリティ ID (SID) に変換するときに管理用に定義された GUID プレフィックスを検出できませんでした。
            /// 代替プレフィックスが使用されましたが、これによりシステム セキュリティが劣化することはありません。
            /// ただし、意図したより厳しくアクセスが制限される可能性があります。
            /// </summary>
            STATUS_GUID_SUBSTITUTION_MADE = 0x8000000c,

            /// <summary> 
            /// 保護が矛盾するため、要求した一部のバイトをコピーできませんでした。
            /// </summary>
            STATUS_PARTIAL_COPY = 0x8000000d,

            /// <summary> 
            /// プリンタは用紙切れです。
            /// </summary>
            STATUS_DEVICE_PAPER_EMPTY = 0x8000000e,

            /// <summary> 
            /// プリンタの電源が切れています。
            /// </summary>
            STATUS_DEVICE_POWERED_OFF = 0x8000000f,

            /// <summary> 
            /// プリンタがオフラインになっています。
            /// </summary>
            STATUS_DEVICE_OFF_LINE = 0x80000010,

            /// <summary> 
            /// デバイスは現在ビジー状態です。
            /// </summary>
            STATUS_DEVICE_BUSY = 0x80000011,

            /// <summary> 
            /// ファイルの拡張属性 (EA) はこれ以上見つかりませんでした。
            /// </summary>
            STATUS_NO_MORE_EAS = 0x80000012,

            /// <summary> 
            /// 指定した拡張属性 (EA) の名前に、1 文字以上の無効な文字が含まれています。
            /// </summary>
            STATUS_INVALID_EA_NAME = 0x80000013,

            /// <summary> 
            /// 拡張属性 (EA) の一覧が矛盾しています。
            /// </summary>
            STATUS_EA_LIST_INCONSISTENT = 0x80000014,

            /// <summary> 
            /// 無効な拡張属性 (EA) フラグが設定されました。
            /// </summary>
            STATUS_INVALID_EA_FLAG = 0x80000015,

            /// <summary> 
            /// メディア変更後の検査が実行中であるため、検査で使用している操作を除き、そのデバイスに対して読み取りや書き込みは実行できません。
            /// </summary>
            STATUS_VERIFY_REQUIRED = 0x80000016,

            /// <summary> 
            /// 指定されたアクセス制御リスト (ACL) には予期したより多くの情報が含まれています。
            /// </summary>
            STATUS_EXTRANEOUS_INFORMATION = 0x80000017,

            /// <summary> 
            /// この警告レベルの状態は、トランザクション状態が既にレジストリ サブツリーに対して存在するが、トランザクション コミットは以前に中止されたことを示します。コミットは完了していませんが、ロールバックもされていません (したがって、必要であればコミットできます)。
            /// この状態値はランタイム ライブラリ (RTL) のレジストリ トランザクション パッケージ (RXact) により返されます。
            /// </summary>
            STATUS_RXACT_COMMIT_NECESSARY = 0x80000018,

            /// <summary> 
            /// 列挙操作からこれ以上エントリを取得できません。
            /// </summary>
            STATUS_NO_MORE_ENTRIES = 0x8000001a,

            /// <summary> 
            /// ファイル マークを検出しました。
            /// </summary>
            STATUS_FILEMARK_DETECTED = 0x8000001b,

            /// <summary> 
            /// メディアが変更された可能性があります。
            /// </summary>
            STATUS_MEDIA_CHANGED = 0x8000001c,

            /// <summary> 
            /// I/O バスのリセットが検出されました。
            /// </summary>
            STATUS_BUS_RESET = 0x8000001d,

            /// <summary> 
            /// メディアの最後が検出されました。
            /// </summary>
            STATUS_END_OF_MEDIA = 0x8000001e,

            /// <summary> 
            /// テープまたはパーティションの先頭が検出されました。
            /// </summary>
            STATUS_BEGINNING_OF_MEDIA = 0x8000001f,

            /// <summary> 
            /// メディアが変更された可能性があります。
            /// </summary>
            STATUS_MEDIA_CHECK = 0x80000020,

            /// <summary> 
            /// テープ アクセスがセット マークに到達しました。
            /// </summary>
            STATUS_SETMARK_DETECTED = 0x80000021,

            /// <summary> 
            /// テープ アクセスの途中で、書き込まれているデータの最後に到達しました。
            /// </summary>
            STATUS_NO_DATA_DETECTED = 0x80000022,

            /// <summary> 
            /// リダイレクタは使用中です。アンロードできません。
            /// </summary>
            STATUS_REDIRECTOR_HAS_OPEN_HANDLES = 0x80000023,

            /// <summary> 
            /// サーバーは使用中です。アンロードできません。
            /// </summary>
            STATUS_SERVER_HAS_OPEN_HANDLES = 0x80000024,

            /// <summary> 
            /// 指定した接続は既に切断されています。
            /// </summary>
            STATUS_ALREADY_DISCONNECTED = 0x80000025,

            /// <summary> 
            /// ロング ジャンプが実行されました。
            /// </summary>
            STATUS_LONGJUMP = 0x80000026,

            /// <summary> 
            /// クリーナ カートリッジはテープ ライブラリにあります。
            /// </summary>
            STATUS_CLEANER_CARTRIDGE_INSTALLED = 0x80000027,

            /// <summary> 
            /// プラグ アンド プレイのクエリ操作は成功しませんでした。
            /// </summary>
            STATUS_PLUGPLAY_QUERY_VETOED = 0x80000028,

            /// <summary> 
            /// フレームの併合が実行されました。
            /// </summary>
            STATUS_UNWIND_CONSOLIDATE = 0x80000029,

            /// <summary> 
            /// STATUS_REGISTRY_HIVE_RECOVERED
            /// </summary>
            STATUS_REGISTRY_HIVE_RECOVERED = 0x8000002a,

            /// <summary> 
            /// STATUS_DLL_MIGHT_BE_INSECURE
            /// </summary>
            STATUS_DLL_MIGHT_BE_INSECURE = 0x8000002b,

            /// <summary> 
            /// STATUS_DLL_MIGHT_BE_INCOMPATIBLE
            /// </summary>
            STATUS_DLL_MIGHT_BE_INCOMPATIBLE = 0x8000002c,

            /// <summary> 
            /// STATUS_STOPPED_ON_SYMLINK
            /// </summary>
            STATUS_STOPPED_ON_SYMLINK = 0x8000002d,

            /// <summary> 
            /// デバッガはこの例外を処理できませんでした。
            /// </summary>
            DBG_EXCEPTION_NOT_HANDLED = 0x80010001,

            /// <summary> 
            /// クラスタ ノードは既にアップになっています。
            /// </summary>
            STATUS_CLUSTER_NODE_ALREADY_UP = 0x80130001,

            /// <summary> 
            /// クラスタ ノードは既にダウンしています。
            /// </summary>
            STATUS_CLUSTER_NODE_ALREADY_DOWN = 0x80130002,

            /// <summary> 
            /// クラスタ ネットワークは既にオンラインです。
            /// </summary>
            STATUS_CLUSTER_NETWORK_ALREADY_ONLINE = 0x80130003,

            /// <summary> 
            /// クラスタ ネットワークは既にオフラインです。
            /// </summary>
            STATUS_CLUSTER_NETWORK_ALREADY_OFFLINE = 0x80130004,

            /// <summary> 
            /// このクラスタ ノードは既にクラスタのメンバです。
            /// </summary>
            STATUS_CLUSTER_NODE_ALREADY_MEMBER = 0x80130005,

            /// <summary> 
            /// STATUS_FVE_PARTIAL_METADATA
            /// </summary>
            STATUS_FVE_PARTIAL_METADATA = 0x80210001,

            /// <summary> 
            /// 要求した操作が失敗しました。
            /// </summary>
            STATUS_UNSUCCESSFUL = 0xc0000001,

            /// <summary> 
            /// 要求した操作は実装されていません。
            /// </summary>
            STATUS_NOT_IMPLEMENTED = 0xc0000002,

            /// <summary> 
            /// 指定した情報クラスは指定したオブジェクトに対して有効な情報クラスではありません。
            /// </summary>
            STATUS_INVALID_INFO_CLASS = 0xc0000003,

            /// <summary> 
            /// 指定した情報レコードの長さは、指定した情報クラスに対して必要な長さと一致しません。
            /// </summary>
            STATUS_INFO_LENGTH_MISMATCH = 0xc0000004,

            /// <summary> 
            /// STATUS_ACCESS_VIOLATION
            /// </summary>
            STATUS_ACCESS_VIOLATION = 0xc0000005,

            /// <summary> 
            /// STATUS_IN_PAGE_ERROR
            /// </summary>
            STATUS_IN_PAGE_ERROR = 0xc0000006,

            /// <summary> 
            /// プロセスのページング ファイル クォータはすべて使用しました。
            /// </summary>
            STATUS_PAGEFILE_QUOTA = 0xc0000007,

            /// <summary> 
            /// 無効なハンドルを指定しました。
            /// </summary>
            STATUS_INVALID_HANDLE = 0xc0000008,

            /// <summary> 
            /// NtCreateThread の呼び出しで無効な初期スタックを指定しました。
            /// </summary>
            STATUS_BAD_INITIAL_STACK = 0xc0000009,

            /// <summary> 
            /// NtCreateThread の呼び出しに無効な初期開始アドレスを指定しました。
            /// </summary>
            STATUS_BAD_INITIAL_PC = 0xc000000a,

            /// <summary> 
            /// 無効なクライアント ID を指定しました。
            /// </summary>
            STATUS_INVALID_CID = 0xc000000b,

            /// <summary> 
            /// APC が関連付けられているタイマを取り消すか、または設定しようとしましたが、サブジェクト スレッドは、APC ルーチンに関連付けられているタイマを設定したスレッドではありません。
            /// </summary>
            STATUS_TIMER_NOT_CANCELED = 0xc000000c,

            /// <summary> 
            /// 無効なパラメータをサービスまたは関数に渡しました。
            /// </summary>
            STATUS_INVALID_PARAMETER = 0xc000000d,

            /// <summary> 
            /// 存在しないデバイスを指定しました。
            /// </summary>
            STATUS_NO_SUCH_DEVICE = 0xc000000e,

            /// <summary> 
            /// ファイルが見つかりません。
            /// </summary>
            STATUS_NO_SUCH_FILE = 0xc000000f,

            /// <summary> 
            /// 指定した要求は対象デバイスに対して有効な操作ではありません。
            /// </summary>
            STATUS_INVALID_DEVICE_REQUEST = 0xc0000010,

            /// <summary> 
            /// ファイルの終わり (EOF) マークに到達しました。ファイル内でこのマークの後に有効なデータはありません。
            /// </summary>
            STATUS_END_OF_FILE = 0xc0000011,

            /// <summary> 
            /// 間違ったボリュームがドライブに挿入されています。
            /// </summary>
            STATUS_WRONG_VOLUME = 0xc0000012,

            /// <summary> 
            /// ドライブにディスクがありません。
            /// </summary>
            STATUS_NO_MEDIA_IN_DEVICE = 0xc0000013,

            /// <summary> 
            /// ドライブのディスクが正しくフォーマットされていません。
            /// ディスクを調べ、必要に応じて再フォーマットしてください。
            /// </summary>
            STATUS_UNRECOGNIZED_MEDIA = 0xc0000014,

            /// <summary> 
            /// 指定したセクタが存在しません。
            /// </summary>
            STATUS_NONEXISTENT_SECTOR = 0xc0000015,

            /// <summary> 
            /// 指定した I/O 要求パケット (IRP) は、I/O 操作が完了していないため、後処理できません。
            /// </summary>
            STATUS_MORE_PROCESSING_REQUIRED = 0xc0000016,

            /// <summary> 
            /// 仮想メモリまたはページング ファイルのクォータが不足するため、指定した操作を完了できません。
            /// </summary>
            STATUS_NO_MEMORY = 0xc0000017,

            /// <summary> 
            /// 指定したアドレス範囲がアドレス空間と矛盾します。
            /// </summary>
            STATUS_CONFLICTING_ADDRESSES = 0xc0000018,

            /// <summary> 
            /// 切断するアドレス範囲は割り当てられた表示ではありません。
            /// </summary>
            STATUS_NOT_MAPPED_VIEW = 0xc0000019,

            /// <summary> 
            /// 仮想メモリを解放できません。
            /// </summary>
            STATUS_UNABLE_TO_FREE_VM = 0xc000001a,

            /// <summary> 
            /// 指定したセクションを削除できません。
            /// </summary>
            STATUS_UNABLE_TO_DELETE_SECTION = 0xc000001b,

            /// <summary> 
            /// 無効なシステム サービスをシステム サービス呼び出しに指定しました。
            /// </summary>
            STATUS_INVALID_SYSTEM_SERVICE = 0xc000001c,

            /// <summary> 
            /// 不正命令を実行しようとしました。
            /// </summary>
            STATUS_ILLEGAL_INSTRUCTION = 0xc000001d,

            /// <summary> 
            /// 無効なロック シーケンスを実行しようとしました。
            /// </summary>
            STATUS_INVALID_LOCK_SEQUENCE = 0xc000001e,

            /// <summary> 
            /// セクションより大きいセクション表示を作成しようとしました。
            /// </summary>
            STATUS_INVALID_VIEW_SIZE = 0xc000001f,

            /// <summary> 
            /// メモリ セクションに対して指定したマッピング ファイルの属性を読み取ることができません。
            /// </summary>
            STATUS_INVALID_FILE_FOR_SECTION = 0xc0000020,

            /// <summary> 
            /// 指定したアドレス範囲は既にコミットされています。
            /// </summary>
            STATUS_ALREADY_COMMITTED = 0xc0000021,

            /// <summary> 
            /// プロセスはオブジェクトのアクセスを要求しましたが、アクセス権が与えられていません。
            /// </summary>
            STATUS_ACCESS_DENIED = 0xc0000022,

            /// <summary> 
            /// バッファの容量不足のため、エントリを格納できません。情報はバッファに書き込まれませんでした。
            /// </summary>
            STATUS_BUFFER_TOO_SMALL = 0xc0000023,

            /// <summary> 
            /// 要求した操作で必要なオブジェクトの種類と要求に指定したオブジェクトの種類が一致しません。
            /// </summary>
            STATUS_OBJECT_TYPE_MISMATCH = 0xc0000024,

            /// <summary> 
            /// Windows はこの例外から続行できません。
            /// </summary>
            STATUS_NONCONTINUABLE_EXCEPTION = 0xc0000025,

            /// <summary> 
            /// 無効な例外後処理が例外ハンドラから返されました。
            /// </summary>
            STATUS_INVALID_DISPOSITION = 0xc0000026,

            /// <summary> 
            /// 例外コードをアンワインドします。
            /// </summary>
            STATUS_UNWIND = 0xc0000027,

            /// <summary> 
            /// 無効なスタックまたは境界不整列なスタックがアンワインド操作で検出されました。
            /// </summary>
            STATUS_BAD_STACK = 0xc0000028,

            /// <summary> 
            /// 無効なアンワインド対象がアンワインド操作で検出されました。
            /// </summary>
            STATUS_INVALID_UNWIND_TARGET = 0xc0000029,

            /// <summary> 
            /// ロックされていないメモリ ページのロックを解除しようとしました。
            /// </summary>
            STATUS_NOT_LOCKED = 0xc000002a,

            /// <summary> 
            /// I/O 操作でデバイス パリティ エラーが発生しました。
            /// </summary>
            STATUS_PARITY_ERROR = 0xc000002b,

            /// <summary> 
            /// アンコミットした仮想メモリをデコミットしようとしました。
            /// </summary>
            STATUS_UNABLE_TO_DECOMMIT_VM = 0xc000002c,

            /// <summary> 
            /// コミットされていない属性を変更しようとしました。
            /// </summary>
            STATUS_NOT_COMMITTED = 0xc000002d,

            /// <summary> 
            /// NtCreatePort に対して無効なオブジェクト属性を指定したか、NtConnectPort に対して無効なポート属性を指定しました。
            /// </summary>
            STATUS_INVALID_PORT_ATTRIBUTES = 0xc000002e,

            /// <summary> 
            /// NtRequestPort または NtRequestWaitReplyPort に渡したメッセージの長さがポートで可能な最大メッセージより長すぎます。
            /// </summary>
            STATUS_PORT_MESSAGE_TOO_LONG = 0xc000002f,

            /// <summary> 
            /// 無効な組み合わせのパラメータを指定しました。
            /// </summary>
            STATUS_INVALID_PARAMETER_MIX = 0xc0000030,

            /// <summary> 
            /// クォータ制限を現在の使用量より小さい値に変更しようとしました。
            /// </summary>
            STATUS_INVALID_QUOTA_LOWER = 0xc0000031,

            /// <summary> 
            /// ディスクのファイル システム構造が壊れており、使用できません。
            /// ボリュームに対して CHKDSK ユーティリティを実行してください。
            /// </summary>
            STATUS_DISK_CORRUPT_ERROR = 0xc0000032,

            /// <summary> 
            /// オブジェクト名が無効です。
            /// </summary>
            STATUS_OBJECT_NAME_INVALID = 0xc0000033,

            /// <summary> 
            /// オブジェクト名が見つかりません。
            /// </summary>
            STATUS_OBJECT_NAME_NOT_FOUND = 0xc0000034,

            /// <summary> 
            /// オブジェクト名は既に存在します。
            /// </summary>
            STATUS_OBJECT_NAME_COLLISION = 0xc0000035,

            /// <summary> 
            /// 切断された通信ポートにメッセージを送信しようとしました。
            /// </summary>
            STATUS_PORT_DISCONNECTED = 0xc0000037,

            /// <summary> 
            /// 既に別のデバイスに接続されているデバイスに接続しようとしました。
            /// </summary>
            STATUS_DEVICE_ALREADY_ATTACHED = 0xc0000038,

            /// <summary> 
            /// オブジェクト パス コンポーネントがディレクトリ オブジェクトではありません。
            /// </summary>
            STATUS_OBJECT_PATH_INVALID = 0xc0000039,

            /// <summary> 
            /// パスが見つかりません。
            /// </summary>
            STATUS_OBJECT_PATH_NOT_FOUND = 0xc000003a,

            /// <summary> 
            /// オブジェクト パス コンポーネントがディレクトリ オブジェクトではありません。
            /// </summary>
            STATUS_OBJECT_PATH_SYNTAX_BAD = 0xc000003b,

            /// <summary> 
            /// データ オーバーランが発生しました。
            /// </summary>
            STATUS_DATA_OVERRUN = 0xc000003c,

            /// <summary> 
            /// データ遅延エラーが発生しました。
            /// </summary>
            STATUS_DATA_LATE_ERROR = 0xc000003d,

            /// <summary> 
            /// データの読み取り中または書き込み中にエラーが発生しました。
            /// </summary>
            STATUS_DATA_ERROR = 0xc000003e,

            /// <summary> 
            /// 巡回冗長検査 (CRC) チェックサム エラーが発生しました。
            /// </summary>
            STATUS_CRC_ERROR = 0xc000003f,

            /// <summary> 
            /// 指定したセクションが大きすぎるため、ファイルを割り当てられません。
            /// </summary>
            STATUS_SECTION_TOO_BIG = 0xc0000040,

            /// <summary> 
            /// NtConnectPort 要求が拒否されました。
            /// </summary>
            STATUS_PORT_CONNECTION_REFUSED = 0xc0000041,

            /// <summary> 
            /// 要求した操作に対するポート ハンドルの種類が無効です。
            /// </summary>
            STATUS_INVALID_PORT_HANDLE = 0xc0000042,

            /// <summary> 
            /// 共有アクセス フラグに互換性がないため、ファイルを開けません。
            /// </summary>
            STATUS_SHARING_VIOLATION = 0xc0000043,

            /// <summary> 
            /// クォータ不足のため、操作を終了できません。
            /// </summary>
            STATUS_QUOTA_EXCEEDED = 0xc0000044,

            /// <summary> 
            /// 指定したページ保護は無効です。
            /// </summary>
            STATUS_INVALID_PAGE_PROTECTION = 0xc0000045,

            /// <summary> 
            /// ミュータント オブジェクトの所有者でないスレッドがミュータント オブジェクトを解放しようとしました。
            /// </summary>
            STATUS_MUTANT_NOT_OWNED = 0xc0000046,

            /// <summary> 
            /// 最大カウントを超過するため、セマフォを解放できませんでした。
            /// </summary>
            STATUS_SEMAPHORE_LIMIT_EXCEEDED = 0xc0000047,

            /// <summary> 
            /// プロセス DebugPort または ExceptionPort を設定しようとしましたが、プロセス内に既にポートが存在します。
            /// </summary>
            STATUS_PORT_ALREADY_SET = 0xc0000048,

            /// <summary> 
            /// イメージが割り当てられていないセクションに対してイメージ情報を照会しようとしました。
            /// </summary>
            STATUS_SECTION_NOT_IMAGE = 0xc0000049,

            /// <summary> 
            /// 中断カウントが最大値になったスレッドを中断しようとしました。
            /// </summary>
            STATUS_SUSPEND_COUNT_EXCEEDED = 0xc000004a,

            /// <summary> 
            /// 終了を開始したスレッドを中断しようとしました。
            /// </summary>
            STATUS_THREAD_IS_TERMINATING = 0xc000004b,

            /// <summary> 
            /// ワーキング セットの制限を無効な値 (最小値が最大値より大きいなど) に設定しようとしました。
            /// </summary>
            STATUS_BAD_WORKING_SET_LIMIT = 0xc000004c,

            /// <summary> 
            /// ファイルを割り当てるためにセクションを作成しましたが、同じファイルが割り当てられている既存のセクションと互換性がありません。
            /// </summary>
            STATUS_INCOMPATIBLE_FILE_MAP = 0xc000004d,

            /// <summary> 
            /// セクションに対する表示は初期表示保護と互換性のない保護を指定します。
            /// </summary>
            STATUS_SECTION_PROTECTION = 0xc000004e,

            /// <summary> 
            /// ファイル システムが EA をサポートしないため、EA に関係する操作に失敗しました。
            /// </summary>
            STATUS_EAS_NOT_SUPPORTED = 0xc000004f,

            /// <summary> 
            /// EA セットが大きすぎるため、EA 操作に失敗しました。
            /// </summary>
            STATUS_EA_TOO_LARGE = 0xc0000050,

            /// <summary> 
            /// 名前または EA インデックスが無効であるため、EA 操作に失敗しました。
            /// </summary>
            STATUS_NONEXISTENT_EA_ENTRY = 0xc0000051,

            /// <summary> 
            /// EA を要求したファイルに EA が登録されていません。
            /// </summary>
            STATUS_NO_EAS_ON_FILE = 0xc0000052,

            /// <summary> 
            /// EA は壊れており、読み取ることができません。
            /// </summary>
            STATUS_EA_CORRUPT_ERROR = 0xc0000053,

            /// <summary> 
            /// ファイル ロックの矛盾のため、要求した読み取りまたは書き込みを許可できません。
            /// </summary>
            STATUS_FILE_LOCK_CONFLICT = 0xc0000054,

            /// <summary> 
            /// ほかのロックが存在するため、要求したファイル ロックを許可できません。
            /// </summary>
            STATUS_LOCK_NOT_GRANTED = 0xc0000055,

            /// <summary> 
            /// 削除保留のファイルに対してクローズ以外の操作が要求されました。
            /// </summary>
            STATUS_DELETE_PENDING = 0xc0000056,

            /// <summary> 
            /// ファイルの制御属性を設定しようとしました。この属性は対象のファイル システムでサポートされません。
            /// </summary>
            STATUS_CTL_FILE_NOT_SUPPORTED = 0xc0000057,

            /// <summary> 
            /// リビジョン番号がサービスで認識されないことを示します。サービスが認識するリビジョンより新しいリビジョンである可能性があります。
            /// </summary>
            STATUS_UNKNOWN_REVISION = 0xc0000058,

            /// <summary> 
            /// 2 つのリビジョン レベルに互換性がないことを示します。
            /// </summary>
            STATUS_REVISION_MISMATCH = 0xc0000059,

            /// <summary> 
            /// 特定のセキュリティ ID をオブジェクトの所有者として割り当てられないことを示します。
            /// </summary>
            STATUS_INVALID_OWNER = 0xc000005a,

            /// <summary> 
            /// 特定のセキュリティ ID をオブジェクトの所有者として割り当てられないことを示します。
            /// </summary>
            STATUS_INVALID_PRIMARY_GROUP = 0xc000005b,

            /// <summary> 
            /// 現在、クライアントを偽装していないスレッドが偽装トークンを操作しようとしました。
            /// </summary>
            STATUS_NO_IMPERSONATION_TOKEN = 0xc000005c,

            /// <summary> 
            /// 固定グループが無効になっていない可能性があります。
            /// </summary>
            STATUS_CANT_DISABLE_MANDATORY = 0xc000005d,

            /// <summary> 
            /// 現在、ログオン要求を処理できるログオン サーバーはありません。
            /// </summary>
            STATUS_NO_LOGON_SERVERS = 0xc000005e,

            /// <summary> 
            /// 指定されたログオン セッションは存在しません。そのセッションは既に終了している可能性があります。
            /// </summary>
            STATUS_NO_SUCH_LOGON_SESSION = 0xc000005f,

            /// <summary> 
            /// 指定された特権は存在しません。
            /// </summary>
            STATUS_NO_SUCH_PRIVILEGE = 0xc0000060,

            /// <summary> 
            /// クライアントは要求された特権を保有していません。
            /// </summary>
            STATUS_PRIVILEGE_NOT_HELD = 0xc0000061,

            /// <summary> 
            /// 指定された名前は正しい形式のアカウント名ではありません。
            /// </summary>
            STATUS_INVALID_ACCOUNT_NAME = 0xc0000062,

            /// <summary> 
            /// 指定されたユーザーは既に存在します。
            /// </summary>
            STATUS_USER_EXISTS = 0xc0000063,

            /// <summary> 
            /// 指定されたユーザーは存在しません。
            /// </summary>
            STATUS_NO_SUCH_USER = 0xc0000064,

            /// <summary> 
            /// 指定されたグループは既に存在します。
            /// </summary>
            STATUS_GROUP_EXISTS = 0xc0000065,

            /// <summary> 
            /// 指定されたグループは存在しません。
            /// </summary>
            STATUS_NO_SUCH_GROUP = 0xc0000066,

            /// <summary> 
            /// 指定されたユーザー アカウントは指定されたグループ アカウントに既に属しています。
            /// グループにメンバが属しているため、そのグループを削除できないことを示すためにも使用されます。
            /// </summary>
            STATUS_MEMBER_IN_GROUP = 0xc0000067,

            /// <summary> 
            /// 指定されたユーザー アカウントは指定されたグループ アカウントのメンバではありません。
            /// </summary>
            STATUS_MEMBER_NOT_IN_GROUP = 0xc0000068,

            /// <summary> 
            /// 要求した操作によって、最後に残った管理アカウントが無効になるか、または削除されることを示します。
            /// システムを管理できない状況が発生するのを防止するために、この操作は許可されません。
            /// </summary>
            STATUS_LAST_ADMIN = 0xc0000069,

            /// <summary> 
            /// パスワードを更新しようとしたときに、このリターン状態は、現在のパスワードとして指定した値が正しくないことを示します。
            /// </summary>
            STATUS_WRONG_PASSWORD = 0xc000006a,

            /// <summary> 
            /// パスワードを更新しようとしたときに、このリターン状態は、新しいパスワードとして指定した値がパスワードで許可されない値であることを示します。
            /// </summary>
            STATUS_ILL_FORMED_PASSWORD = 0xc000006b,

            /// <summary> 
            /// パスワードを更新しようとしたときに、この状態は一部のパスワード更新規則に違反したことを示します。たとえば、パスワードが長さが条件に合っていない可能性があります。
            /// </summary>
            STATUS_PASSWORD_RESTRICTION = 0xc000006c,

            /// <summary> 
            /// 実行しようとしたログオンは無効です。ユーザー名または認証情報に誤りがあります。
            /// </summary>
            STATUS_LOGON_FAILURE = 0xc000006d,

            /// <summary> 
            /// 参照したユーザー名と認証情報は正しいが、一部のユーザー アカウント制限 (時間帯の制限など) によって認証が失敗したことを示します。
            /// </summary>
            STATUS_ACCOUNT_RESTRICTION = 0xc000006e,

            /// <summary> 
            /// ユーザー アカウントでログオン時間が制限されているため、現在はログオンできません。
            /// </summary>
            STATUS_INVALID_LOGON_HOURS = 0xc000006f,

            /// <summary> 
            /// ユーザー アカウントは、要求元ワークステーションからのログオンには使用できないように制限されています。
            /// </summary>
            STATUS_INVALID_WORKSTATION = 0xc0000070,

            /// <summary> 
            /// ユーザー アカウントのパスワードの有効期限が切れています。
            /// </summary>
            STATUS_PASSWORD_EXPIRED = 0xc0000071,

            /// <summary> 
            /// 参照したアカウントは現在無効であり、ログオンできません。
            /// </summary>
            STATUS_ACCOUNT_DISABLED = 0xc0000072,

            /// <summary> 
            /// 情報はまったく変換されませんでした。
            /// </summary>
            STATUS_NONE_MAPPED = 0xc0000073,

            /// <summary> 
            /// 要求した数の LUID は 1 回の割り当てで割り当てることができません。
            /// </summary>
            STATUS_TOO_MANY_LUIDS_REQUESTED = 0xc0000074,

            /// <summary> 
            /// LUID をこれ以上割り当てることができないことを示します。
            /// </summary>
            STATUS_LUIDS_EXHAUSTED = 0xc0000075,

            /// <summary> 
            /// サブ機関の値が特定の使用目的に対して無効であることを示します。
            /// </summary>
            STATUS_INVALID_SUB_AUTHORITY = 0xc0000076,

            /// <summary> 
            /// ACL 構造体が正しくないことを示します。
            /// </summary>
            STATUS_INVALID_ACL = 0xc0000077,

            /// <summary> 
            /// SID 構造体が正しくないことを示します。
            /// </summary>
            STATUS_INVALID_SID = 0xc0000078,

            /// <summary> 
            /// SECURITY_DESCRIPTOR 構造体が正しくないことを示します。
            /// </summary>
            STATUS_INVALID_SECURITY_DESCR = 0xc0000079,

            /// <summary> 
            /// 指定したプロシージャ アドレスを DLL から見つけられないことを示します。
            /// </summary>
            STATUS_PROCEDURE_NOT_FOUND = 0xc000007a,

            /// <summary> 
            /// アプリケーションまたは DLL は正しい Windows イメージではありません。これをインストール ディスクのファイルと照合してください。
            /// </summary>
            STATUS_INVALID_IMAGE_FORMAT = 0xc000007b,

            /// <summary> 
            /// 存在しないトークンを参照しようとしました。
            /// この操作は通常、スレッドがクライアントを偽装していないときに、スレッドに関連付けたトークンを参照することにより実行されます。
            /// </summary>
            STATUS_NO_TOKEN = 0xc000007c,

            /// <summary> 
            /// 継承した ACL または ACE を作成する操作が失敗したことを示します。
            /// これは多くの原因で発生する可能性があります。可能性の高い原因の 1 つとして、CreatorId を、ACE または ACL に格納できない SID と置換したことが考えられます。
            /// </summary>
            STATUS_BAD_INHERITANCE_ACL = 0xc000007d,

            /// <summary> 
            /// NtUnlockFile に指定した範囲がロックされていません。
            /// </summary>
            STATUS_RANGE_NOT_LOCKED = 0xc000007e,

            /// <summary> 
            /// ディスクがいっぱいであるため、操作に失敗しました。
            /// </summary>
            STATUS_DISK_FULL = 0xc000007f,

            /// <summary> 
            /// GUID 割り当てサーバーは既に無効になっています。
            /// </summary>
            STATUS_SERVER_DISABLED = 0xc0000080,

            /// <summary> 
            /// GUID 割り当てサーバーは既に有効になっています。
            /// </summary>
            STATUS_SERVER_NOT_DISABLED = 0xc0000081,

            /// <summary> 
            /// 割り当てサーバーから一度に要求された GUID の数が多すぎます。
            /// </summary>
            STATUS_TOO_MANY_GUIDS_REQUESTED = 0xc0000082,

            /// <summary> 
            /// 機関のエージェントをすべて使用してしまったため、GUID を割り当てることができません。
            /// </summary>
            STATUS_GUIDS_EXHAUSTED = 0xc0000083,

            /// <summary> 
            /// 指定された値は識別子機関にとって無効な値です。
            /// </summary>
            STATUS_INVALID_ID_AUTHORITY = 0xc0000084,

            /// <summary> 
            /// 指定した識別子機関に対して使用できる機関のエージェント値はこれ以上ありません。
            /// </summary>
            STATUS_AGENTS_EXHAUSTED = 0xc0000085,

            /// <summary> 
            /// 無効なボリューム ラベルを指定しました。
            /// </summary>
            STATUS_INVALID_VOLUME_LABEL = 0xc0000086,

            /// <summary> 
            /// 割り当てられたセクションを拡張できませんでした。
            /// </summary>
            STATUS_SECTION_NOT_EXTENDED = 0xc0000087,

            /// <summary> 
            /// 消去するように指定されたセクションが、データ ファイルの割り当てを行っていません。
            /// </summary>
            STATUS_NOT_MAPPED_DATA = 0xc0000088,

            /// <summary> 
            /// 指定されたイメージ ファイルにリソース セクションが含まれていなかったことを示します。
            /// </summary>
            STATUS_RESOURCE_DATA_NOT_FOUND = 0xc0000089,

            /// <summary> 
            /// 指定されたリソースの種類がイメージ ファイルから見つからないことを示します。
            /// </summary>
            STATUS_RESOURCE_TYPE_NOT_FOUND = 0xc000008a,

            /// <summary> 
            /// 指定されたリソース名がイメージ ファイルから見つからないことを示します。
            /// </summary>
            STATUS_RESOURCE_NAME_NOT_FOUND = 0xc000008b,

            /// <summary> 
            /// 配列境界を超えました。
            /// </summary>
            STATUS_ARRAY_BOUNDS_EXCEEDED = 0xc000008c,

            /// <summary> 
            /// 浮動小数点非正規化オペランド。
            /// </summary>
            STATUS_FLOAT_DENORMAL_OPERAND = 0xc000008d,

            /// <summary> 
            /// 0 による浮動小数点除算。
            /// </summary>
            STATUS_FLOAT_DIVIDE_BY_ZERO = 0xc000008e,

            /// <summary> 
            /// 浮動小数点の不正確な結果。
            /// </summary>
            STATUS_FLOAT_INEXACT_RESULT = 0xc000008f,

            /// <summary> 
            /// 浮動小数点の無効な演算。
            /// </summary>
            STATUS_FLOAT_INVALID_OPERATION = 0xc0000090,

            /// <summary> 
            /// 浮動小数点オーバーフロー。
            /// </summary>
            STATUS_FLOAT_OVERFLOW = 0xc0000091,

            /// <summary> 
            /// 浮動小数点スタック チェック。
            /// </summary>
            STATUS_FLOAT_STACK_CHECK = 0xc0000092,

            /// <summary> 
            /// 浮動小数点アンダーフロー。
            /// </summary>
            STATUS_FLOAT_UNDERFLOW = 0xc0000093,

            /// <summary> 
            /// 0 による整数除算。
            /// </summary>
            STATUS_INTEGER_DIVIDE_BY_ZERO = 0xc0000094,

            /// <summary> 
            /// 整数オーバーフロー。
            /// </summary>
            STATUS_INTEGER_OVERFLOW = 0xc0000095,

            /// <summary> 
            /// 特権のある命令。
            /// </summary>
            STATUS_PRIVILEGED_INSTRUCTION = 0xc0000096,

            /// <summary> 
            /// システムがサポートする数より多くのページング ファイルをインストールしようとしました。
            /// </summary>
            STATUS_TOO_MANY_PAGING_FILES = 0xc0000097,

            /// <summary> 
            /// ファイルを格納しているボリュームが外部的に変更されたため、開かれているファイルが無効になりました。
            /// </summary>
            STATUS_FILE_INVALID = 0xc0000098,

            /// <summary> 
            /// 任意のアクセス制御情報やプライマリ グループ情報を格納するために割り当てたメモリなど、将来の更新のためにメモリ ブロックを割り当てる場合、更新操作を連続して実行すると、割り当てたメモリ容量を超過する可能性があります。
            /// クォータは既に、オブジェクトに対するハンドルを持つ複数のプロセスに与えられている可能性があるため、割り当てたメモリのサイズを変更することは妥当ではありません。
            /// この場合には、割り当てた容量より多くのメモリを必要とする要求は失敗し、STATUS_ALLOTED_SPACE_EXCEEDED エラーを返さなければなりません。
            /// </summary>
            STATUS_ALLOTTED_SPACE_EXCEEDED = 0xc0000099,

            /// <summary> 
            /// システム リソースが不足するため、API を終了できません。
            /// </summary>
            STATUS_INSUFFICIENT_RESOURCES = 0xc000009a,

            /// <summary> 
            /// DFS 終了パス制御ファイルを開こうとしました。
            /// </summary>
            STATUS_DFS_EXIT_PATH_FOUND = 0xc000009b,

            /// <summary> 
            /// STATUS_DEVICE_DATA_ERROR
            /// </summary>
            STATUS_DEVICE_DATA_ERROR = 0xc000009c,

            /// <summary> 
            /// STATUS_DEVICE_NOT_CONNECTED
            /// </summary>
            STATUS_DEVICE_NOT_CONNECTED = 0xc000009d,

            /// <summary> 
            /// STATUS_DEVICE_POWER_FAILURE
            /// </summary>
            STATUS_DEVICE_POWER_FAILURE = 0xc000009e,

            /// <summary> 
            /// ベース アドレスが領域のベースではなく、領域サイズとして 0 を指定したため、仮想メモリを解放できません。
            /// </summary>
            STATUS_FREE_VM_NOT_AT_BASE = 0xc000009f,

            /// <summary> 
            /// 割り当てられていない仮想メモリを解放しようとしました。
            /// </summary>
            STATUS_MEMORY_NOT_ALLOCATED = 0xc00000a0,

            /// <summary> 
            /// ワーキング セットの容量不足のため、要求したページをロックできません。
            /// </summary>
            STATUS_WORKING_SET_QUOTA = 0xc00000a1,

            /// <summary> 
            /// ディスクは書き込み禁止です。
            /// 書き込み禁止を解除してください。
            /// </summary>
            STATUS_MEDIA_WRITE_PROTECTED = 0xc00000a2,

            /// <summary> 
            /// ドライブは使用できる状態ではありません。ドアが開いている可能性があります。
            /// ドライブを調べ、ディスクが挿入されているかと、ドライブのドアが閉じているか調べてください。
            /// </summary>
            STATUS_DEVICE_NOT_READY = 0xc00000a3,

            /// <summary> 
            /// 指定された属性が無効であるか、またはグループ全体の属性と矛盾します。
            /// </summary>
            STATUS_INVALID_GROUP_ATTRIBUTES = 0xc00000a4,

            /// <summary> 
            /// 指定した偽装レベルは無効です。
            /// また、必要な偽装レベルが提供されなかったことも示すためにも使用されます。
            /// </summary>
            STATUS_BAD_IMPERSONATION_LEVEL = 0xc00000a5,

            /// <summary> 
            /// 匿名レベル トークンを開こうとしました。
            /// 匿名トークンを開けません。
            /// </summary>
            STATUS_CANT_OPEN_ANONYMOUS = 0xc00000a6,

            /// <summary> 
            /// 要求された妥当性検査情報クラスが無効です。
            /// </summary>
            STATUS_BAD_VALIDATION_CLASS = 0xc00000a7,

            /// <summary> 
            /// この種類のトークンはこの方法で使用するのに不適切です。
            /// </summary>
            STATUS_BAD_TOKEN_TYPE = 0xc00000a8,

            /// <summary> 
            /// この種類のトークンはこの方法で使用するのに不適切です。
            /// </summary>
            STATUS_BAD_MASTER_BOOT_RECORD = 0xc00000a9,

            /// <summary> 
            /// 境界不整列アドレスで命令を実行しようとしましたが、ホスト システムは不整列な命令参照をサポートしません。
            /// </summary>
            STATUS_INSTRUCTION_MISALIGNMENT = 0xc00000aa,

            /// <summary> 
            /// 名前付きパイプ インスタンスの最大数に到達しました。
            /// </summary>
            STATUS_INSTANCE_NOT_AVAILABLE = 0xc00000ab,

            /// <summary> 
            /// 受信状態の名前付きパイプのインスタンスが見つかりません。
            /// </summary>
            STATUS_PIPE_NOT_AVAILABLE = 0xc00000ac,

            /// <summary> 
            /// 名前付きパイプは接続状態または終了状態ではありません。
            /// </summary>
            STATUS_INVALID_PIPE_STATE = 0xc00000ad,

            /// <summary> 
            /// 指定したパイプは操作を完了するように設定されており、現在の I/O 操作はキューに登録されているため、パイプをキュー操作に変更することはできません。
            /// </summary>
            STATUS_PIPE_BUSY = 0xc00000ae,

            /// <summary> 
            /// 指定したハンドルは、名前付きパイプのサーバーに対して開かれていません。
            /// </summary>
            STATUS_ILLEGAL_FUNCTION = 0xc00000af,

            /// <summary> 
            /// 指定した名前付きパイプは切断状態です。
            /// </summary>
            STATUS_PIPE_DISCONNECTED = 0xc00000b0,

            /// <summary> 
            /// 指定した名前付きパイプは終了状態です。
            /// </summary>
            STATUS_PIPE_CLOSING = 0xc00000b1,

            /// <summary> 
            /// 指定した名前付きパイプは接続状態です。
            /// </summary>
            STATUS_PIPE_CONNECTED = 0xc00000b2,

            /// <summary> 
            /// 指定した名前付きパイプは受信状態です。
            /// </summary>
            STATUS_PIPE_LISTENING = 0xc00000b3,

            /// <summary> 
            /// 指定した名前付きパイプはメッセージ モードではありません。
            /// </summary>
            STATUS_INVALID_READ_MODE = 0xc00000b4,

            /// <summary> 
            /// タイムアウト期間内に、指定した I/O 操作が完了しませんでした。
            /// </summary>
            STATUS_IO_TIMEOUT = 0xc00000b5,

            /// <summary> 
            /// 指定したファイルは別のプロセスによって閉じられました。
            /// </summary>
            STATUS_FILE_FORCED_CLOSED = 0xc00000b6,

            /// <summary> 
            /// プロファイルは起動されていません。
            /// </summary>
            STATUS_PROFILING_NOT_STARTED = 0xc00000b7,

            /// <summary> 
            /// プロファイルは停止されていません。
            /// </summary>
            STATUS_PROFILING_NOT_STOPPED = 0xc00000b8,

            /// <summary> 
            /// 渡した ACL には必要最低限の情報が登録されていません。
            /// </summary>
            STATUS_COULD_NOT_INTERPRET = 0xc00000b9,

            /// <summary> 
            /// 操作対象として指定したファイルはディレクトリですが、呼び出し側はディレクトリ以外のファイルであると指定しました。
            /// </summary>
            STATUS_FILE_IS_A_DIRECTORY = 0xc00000ba,

            /// <summary> 
            /// この要求はサポートされていません。
            /// </summary>
            STATUS_NOT_SUPPORTED = 0xc00000bb,

            /// <summary> 
            /// このリモート コンピュータは受信状態ではありません。
            /// </summary>
            STATUS_REMOTE_NOT_LISTENING = 0xc00000bc,

            /// <summary> 
            /// ネットワーク上に同じ名前があります。
            /// </summary>
            STATUS_DUPLICATE_NAME = 0xc00000bd,

            /// <summary> 
            /// ネットワーク パスが見つかりません。
            /// </summary>
            STATUS_BAD_NETWORK_PATH = 0xc00000be,

            /// <summary> 
            /// ネットワークがビジーです。
            /// </summary>
            STATUS_NETWORK_BUSY = 0xc00000bf,

            /// <summary> 
            /// このデバイスは存在しません。
            /// </summary>
            STATUS_DEVICE_DOES_NOT_EXIST = 0xc00000c0,

            /// <summary> 
            /// ネットワーク BIOS コマンドが制限値に達しました。
            /// </summary>
            STATUS_TOO_MANY_COMMANDS = 0xc00000c1,

            /// <summary> 
            /// I/O アダプタ ハードウェア エラーが発生しました。
            /// </summary>
            STATUS_ADAPTER_HARDWARE_ERROR = 0xc00000c2,

            /// <summary> 
            /// ネットワークが正しく応答しませんでした。
            /// </summary>
            STATUS_INVALID_NETWORK_RESPONSE = 0xc00000c3,

            /// <summary> 
            /// 予期しないネットワーク エラーが発生しました。
            /// </summary>
            STATUS_UNEXPECTED_NETWORK_ERROR = 0xc00000c4,

            /// <summary> 
            /// リモート アダプタは互換性がありません。
            /// </summary>
            STATUS_BAD_REMOTE_ADAPTER = 0xc00000c5,

            /// <summary> 
            /// プリンタ キューがいっぱいです。
            /// </summary>
            STATUS_PRINT_QUEUE_FULL = 0xc00000c6,

            /// <summary> 
            /// サーバー上の印刷待ちファイルを格納するためのディスク領域がありません。
            /// </summary>
            STATUS_NO_SPOOL_SPACE = 0xc00000c7,

            /// <summary> 
            /// 要求した印刷ファイルは取り消されました。
            /// </summary>
            STATUS_PRINT_CANCELLED = 0xc00000c8,

            /// <summary> 
            /// ネットワーク名は削除されました。
            /// </summary>
            STATUS_NETWORK_NAME_DELETED = 0xc00000c9,

            /// <summary> 
            /// ネットワーク アクセスは拒否されました。
            /// </summary>
            STATUS_NETWORK_ACCESS_DENIED = 0xc00000ca,

            /// <summary> 
            /// 指定したデバイスの種類 (たとえば LPT) は実際のリモート リソースのデバイスの種類と矛盾します。
            /// </summary>
            STATUS_BAD_DEVICE_TYPE = 0xc00000cb,

            /// <summary> 
            /// 指定した共有名がリモート サーバーで見つかりません。
            /// </summary>
            STATUS_BAD_NETWORK_NAME = 0xc00000cc,

            /// <summary> 
            /// ローカル コンピュータのネットワーク アダプタ カードに対する名前の数が制限値を超えました。
            /// </summary>
            STATUS_TOO_MANY_NAMES = 0xc00000cd,

            /// <summary> 
            /// ネットワーク BIOS セッションの数が制限値を超えました。
            /// </summary>
            STATUS_TOO_MANY_SESSIONS = 0xc00000ce,

            /// <summary> 
            /// ファイルの共有は一時的に中断されました。
            /// </summary>
            STATUS_SHARING_PAUSED = 0xc00000cf,

            /// <summary> 
            /// 既に接続数が最大に達しているため、これ以上このリモート コンピュータに接続できません。
            /// </summary>
            STATUS_REQUEST_NOT_ACCEPTED = 0xc00000d0,

            /// <summary> 
            /// プリンタまたはディスクのリダイレクトを一時的に中断しています。
            /// </summary>
            STATUS_REDIRECTOR_PAUSED = 0xc00000d1,

            /// <summary> 
            /// ネットワーク データ フォールトが発生しました。
            /// </summary>
            STATUS_NET_WRITE_FAULT = 0xc00000d2,

            /// <summary> 
            /// アクティブ プロファイル オブジェクトの数が最大数になったため、これ以上起動できません。
            /// </summary>
            STATUS_PROFILING_AT_LIMIT = 0xc00000d3,

            /// <summary> 
            /// 名前変更要求の対象となるファイルが名前要求の変更元以外のデバイスに登録されています。
            /// </summary>
            STATUS_NOT_SAME_DEVICE = 0xc00000d4,

            /// <summary> 
            /// 指定したファイルの名前が変更されたため、ファイルの内容を変更できません。
            /// </summary>
            STATUS_FILE_RENAMED = 0xc00000d5,

            /// <summary> 
            /// 要求のタイムアウトが経過したため、リモート サーバーとのセッションが切断されました。
            /// </summary>
            STATUS_VIRTUAL_CIRCUIT_CLOSED = 0xc00000d6,

            /// <summary> 
            /// セキュリティが関連付けられていないオブジェクトのセキュリティを操作しようとしました。
            /// </summary>
            STATUS_NO_SECURITY_ON_OBJECT = 0xc00000d7,

            /// <summary> 
            /// I/O をブロックせずに操作を続行できないことを示すために使用されます。
            /// </summary>
            STATUS_CANT_WAIT = 0xc00000d8,

            /// <summary> 
            /// 空のパイプに対して読み取り操作を実行したことを示すために使用されます。
            /// </summary>
            STATUS_PIPE_EMPTY = 0xc00000d9,

            /// <summary> 
            /// ドメイン コントローラから構成情報を読み取れませんでした。コンピュータが有効ではないか、またはアクセスが拒否されました。
            /// </summary>
            STATUS_CANT_ACCESS_DOMAIN_INFO = 0xc00000da,

            /// <summary> 
            /// 既定の設定によりスレッドがそれ自身を終了しようとし(NULL を使って NtTerminateThread を呼び出しました)、それが現在のプロセス内の最後のスレッドであったことを示します。
            /// </summary>
            STATUS_CANT_TERMINATE_SELF = 0xc00000db,

            /// <summary> 
            /// SAM サーバーが正しくない状態であるため、必要な操作を実行できないことを示します。
            /// </summary>
            STATUS_INVALID_SERVER_STATE = 0xc00000dc,

            /// <summary> 
            /// ドメインが正しくない状態であるため、必要な操作を実行できないことを示します。
            /// </summary>
            STATUS_INVALID_DOMAIN_STATE = 0xc00000dd,

            /// <summary> 
            /// この操作はドメインのプライマリ ドメイン コントローラに対してのみ実行できます。
            /// </summary>
            STATUS_INVALID_DOMAIN_ROLE = 0xc00000de,

            /// <summary> 
            /// 指定されたドメインは存在しません。
            /// </summary>
            STATUS_NO_SUCH_DOMAIN = 0xc00000df,

            /// <summary> 
            /// 指定されたドメインは既に存在します。
            /// </summary>
            STATUS_DOMAIN_EXISTS = 0xc00000e0,

            /// <summary> 
            /// このリリースで設定されている各サーバーのドメイン数の制限を超過します。
            /// </summary>
            STATUS_DOMAIN_LIMIT_EXCEEDED = 0xc00000e1,

            /// <summary> 
            /// oplock 要求が拒否されたときに、エラー状態が返されました。
            /// </summary>
            STATUS_OPLOCK_NOT_GRANTED = 0xc00000e2,

            /// <summary> 
            /// 誤った oplock 受信確認をファイル システムで受信したときに、エラー状態が返されました。
            /// </summary>
            STATUS_INVALID_OPLOCK_PROTOCOL = 0xc00000e3,

            /// <summary> 
            /// このエラーは、致命的なメディア障害が発生したか、またはディスクでデータ構造が破壊されたため、要求した操作を終了できないことを示します。
            /// </summary>
            STATUS_INTERNAL_DB_CORRUPTION = 0xc00000e4,

            /// <summary> 
            /// 内部エラーが発生しました。
            /// </summary>
            STATUS_INTERNAL_ERROR = 0xc00000e5,

            /// <summary> 
            /// 既に非ジェネリック タイプにマッピングされているアクセス マスクにジェネリック アクセス タイプが含まれていたことを示します。
            /// </summary>
            STATUS_GENERIC_NOT_MAPPED = 0xc00000e6,

            /// <summary> 
            /// セキュリティ記述子の形式 (絶対または自己相対) が誤っていることを示します。
            /// </summary>
            STATUS_BAD_DESCRIPTOR_FORMAT = 0xc00000e7,

            /// <summary> 
            /// ユーザー バッファへのアクセスが ""予測"" ポイントで失敗しました。
            /// このコードが定義されているのは、呼び出し側がフィルタで STATUS_ACCESS_VIOLATION を受け付けたくないからです。
            /// </summary>
            STATUS_INVALID_USER_BUFFER = 0xc00000e8,

            /// <summary> 
            /// 標準の FsRtl フィルタに定義されていない I/O エラーが返された場合には、フィルタに必ず定義されていると保証される次のエラーに変換されます。
            /// この場合、情報は失われますが、フィルタは例外を正しく処理します。
            /// </summary>
            STATUS_UNEXPECTED_IO_ERROR = 0xc00000e9,

            /// <summary> 
            /// 標準の FsRtl フィルタに定義されていない MM エラーが返された場合には、フィルタに定義されていることが保証されている次のいずれかのエラーに変換されます。
            /// この場合、情報は失われますが、フィルタは例外を正しく処理します。
            /// </summary>
            STATUS_UNEXPECTED_MM_CREATE_ERR = 0xc00000ea,

            /// <summary> 
            /// 標準の FsRtl フィルタに定義されていない MM エラーが返された場合には、フィルタに定義されていることが保証されている次のいずれかのエラーに変換されます。
            /// この場合、情報は失われますが、フィルタは例外を正しく処理します。
            /// </summary>
            STATUS_UNEXPECTED_MM_MAP_ERROR = 0xc00000eb,

            /// <summary> 
            /// 標準の FsRtl フィルタに定義されていない MM エラーが返された場合には、フィルタに定義されていることが保証されている次のいずれかのエラーに変換されます。
            /// この場合、情報は失われますが、フィルタは例外を正しく処理します。
            /// </summary>
            STATUS_UNEXPECTED_MM_EXTEND_ERR = 0xc00000ec,

            /// <summary> 
            /// 要求された操作はログオン プロセスだけが使用できます。呼び出し側プロセスはログオン プロセスとして登録されていません。
            /// </summary>
            STATUS_NOT_LOGON_PROCESS = 0xc00000ed,

            /// <summary> 
            /// 既に使用されている ID を使用して LSA ログオン セッションまたは新しいセッション マネージャを起動しようとしました。
            /// </summary>
            STATUS_LOGON_SESSION_EXISTS = 0xc00000ee,

            /// <summary> 
            /// 無効なパラメータを最初の引数としてサービスまたは関数に渡しました。
            /// </summary>
            STATUS_INVALID_PARAMETER_1 = 0xc00000ef,

            /// <summary> 
            /// 無効なパラメータを 2 番目の引数としてサービスまたは関数に渡しました。
            /// </summary>
            STATUS_INVALID_PARAMETER_2 = 0xc00000f0,

            /// <summary> 
            /// 無効なパラメータを 3 番目の引数としてサービスまたは関数に渡しました。
            /// </summary>
            STATUS_INVALID_PARAMETER_3 = 0xc00000f1,

            /// <summary> 
            /// 無効なパラメータを 4 番目の引数としてサービスまたは関数に渡しました。
            /// </summary>
            STATUS_INVALID_PARAMETER_4 = 0xc00000f2,

            /// <summary> 
            /// 無効なパラメータを 5 番目の引数としてサービスまたは関数に渡しました。
            /// </summary>
            STATUS_INVALID_PARAMETER_5 = 0xc00000f3,

            /// <summary> 
            /// 無効なパラメータを 6 番目の引数としてサービスまたは関数に渡しました。
            /// </summary>
            STATUS_INVALID_PARAMETER_6 = 0xc00000f4,

            /// <summary> 
            /// 無効なパラメータを 7 番目の引数としてサービスまたは関数に渡しました。
            /// </summary>
            STATUS_INVALID_PARAMETER_7 = 0xc00000f5,

            /// <summary> 
            /// 無効なパラメータを 8 番目の引数としてサービスまたは関数に渡しました。
            /// </summary>
            STATUS_INVALID_PARAMETER_8 = 0xc00000f6,

            /// <summary> 
            /// 無効なパラメータを 9 番目の引数としてサービスまたは関数に渡しました。
            /// </summary>
            STATUS_INVALID_PARAMETER_9 = 0xc00000f7,

            /// <summary> 
            /// 無効なパラメータを 10 番目の引数としてサービスまたは関数に渡しました。
            /// </summary>
            STATUS_INVALID_PARAMETER_10 = 0xc00000f8,

            /// <summary> 
            /// 無効なパラメータを 11 番目の引数としてサービスまたは関数に渡しました。
            /// </summary>
            STATUS_INVALID_PARAMETER_11 = 0xc00000f9,

            /// <summary> 
            /// 無効なパラメータを 12 番目の引数としてサービスまたは関数に渡しました。
            /// </summary>
            STATUS_INVALID_PARAMETER_12 = 0xc00000fa,

            /// <summary> 
            /// ネットワーク ファイルにアクセスしようとしましたが、ネットワーク ソフトウェアがまだ起動されていません。
            /// </summary>
            STATUS_REDIRECTOR_NOT_STARTED = 0xc00000fb,

            /// <summary> 
            /// リダイレクタを起動しようとしましたが、リダイレクタは既に起動されています。
            /// </summary>
            STATUS_REDIRECTOR_STARTED = 0xc00000fc,

            /// <summary> 
            /// スタックの新しいガード ページを作成できません。
            /// </summary>
            STATUS_STACK_OVERFLOW = 0xc00000fd,

            /// <summary> 
            /// 指定された認証パッケージは認識されません。
            /// </summary>
            STATUS_NO_SUCH_PACKAGE = 0xc00000fe,

            /// <summary> 
            /// 誤った形式の機能テーブルがアンワインド操作で検出されました。
            /// </summary>
            STATUS_BAD_FUNCTION_TABLE = 0xc00000ff,

            /// <summary> 
            /// 指定した環境変数名が、指定した環境ブロックから見つからないことを示します。
            /// </summary>
            STATUS_VARIABLE_NOT_FOUND = 0xc0000100,

            /// <summary> 
            /// 削除対象のディレクトリが空でないことを示します。
            /// </summary>
            STATUS_DIRECTORY_NOT_EMPTY = 0xc0000101,

            /// <summary> 
            /// ファイルまたはディレクトリが壊れており、読み取ることができません。
            /// CHKDSK ユーティリティを実行してください。
            /// </summary>
            STATUS_FILE_CORRUPT_ERROR = 0xc0000102,

            /// <summary> 
            /// 要求したファイルはディレクトリではありません。
            /// </summary>
            STATUS_NOT_A_DIRECTORY = 0xc0000103,

            /// <summary> 
            /// ログオン セッションは、要求された操作と矛盾する状態です。
            /// </summary>
            STATUS_BAD_LOGON_SESSION_STATE = 0xc0000104,

            /// <summary> 
            /// 内部 LSA エラーが発生しました。認証パッケージはログオン セッションの作成を要求しましたが、既存のログオン セッションの ID を指定しました。
            /// </summary>
            STATUS_LOGON_SESSION_COLLISION = 0xc0000105,

            /// <summary> 
            /// 指定した名前の文字列が長すぎるため、使用できません。
            /// </summary>
            STATUS_NAME_TOO_LONG = 0xc0000106,

            /// <summary> 
            /// ユーザーはリダイレクトされたドライブでファイルを強制的に閉じようとしましたが、ドライブでファイルが開かれているため、その操作を強制的に実行できませんでした。
            /// </summary>
            STATUS_FILES_OPEN = 0xc0000107,

            /// <summary> 
            /// ユーザーはリダイレクトされたドライブでファイルを強制的に閉じようとしましたが、ドライブでディレクトリが開かれているため、その操作を強制的に実行できませんでした。
            /// </summary>
            STATUS_CONNECTION_IN_USE = 0xc0000108,

            /// <summary> 
            /// RtlFindMessage は要求されたメッセージ ID をメッセージ テーブル リソースから見つけることができませんでした。
            /// </summary>
            STATUS_MESSAGE_NOT_FOUND = 0xc0000109,

            /// <summary> 
            /// 既存のプロセスとの間でオブジェクト ハンドルを複製しようとしました。
            /// </summary>
            STATUS_PROCESS_IS_TERMINATING = 0xc000010a,

            /// <summary> 
            /// 要求した LogonType に対して、無効な値が指定されていることを示します。
            /// </summary>
            STATUS_INVALID_LOGON_TYPE = 0xc000010b,

            /// <summary> 
            /// ファイル システムのファイルまたはディレクトリに保護を割り当てようとしたときに、セキュリティ記述子内の SID を、HPFS に格納できる GUID に変換できなかったことを示します。
            /// この結果、保護の割り当ては失敗し、ファイルの作成も失敗する可能性があります。
            /// </summary>
            STATUS_NO_GUID_TRANSLATION = 0xc000010c,

            /// <summary> 
            /// まだ読み取られていない名前付きパイプを経由して偽装しようとしたことを示します。
            /// </summary>
            STATUS_CANNOT_IMPERSONATE = 0xc000010d,

            /// <summary> 
            /// 指定したイメージが既に読み込まれていることを示します。
            /// </summary>
            STATUS_IMAGE_ALREADY_LOADED = 0xc000010e,

            /// <summary> 
            /// STATUS_ABIOS_NOT_PRESENT
            /// </summary>
            STATUS_ABIOS_NOT_PRESENT = 0xc000010f,

            /// <summary> 
            /// STATUS_ABIOS_LID_NOT_EXIST
            /// </summary>
            STATUS_ABIOS_LID_NOT_EXIST = 0xc0000110,

            /// <summary> 
            /// STATUS_ABIOS_LID_ALREADY_OWNED
            /// </summary>
            STATUS_ABIOS_LID_ALREADY_OWNED = 0xc0000111,

            /// <summary> 
            /// STATUS_ABIOS_NOT_LID_OWNER
            /// </summary>
            STATUS_ABIOS_NOT_LID_OWNER = 0xc0000112,

            /// <summary> 
            /// STATUS_ABIOS_INVALID_COMMAND
            /// </summary>
            STATUS_ABIOS_INVALID_COMMAND = 0xc0000113,

            /// <summary> 
            /// STATUS_ABIOS_INVALID_LID
            /// </summary>
            STATUS_ABIOS_INVALID_LID = 0xc0000114,

            /// <summary> 
            /// STATUS_ABIOS_SELECTOR_NOT_AVAILABLE
            /// </summary>
            STATUS_ABIOS_SELECTOR_NOT_AVAILABLE = 0xc0000115,

            /// <summary> 
            /// STATUS_ABIOS_INVALID_SELECTOR
            /// </summary>
            STATUS_ABIOS_INVALID_SELECTOR = 0xc0000116,

            /// <summary> 
            /// LDT のないプロセスの LDT のサイズを変更しようとしたことを示します。
            /// </summary>
            STATUS_NO_LDT = 0xc0000117,

            /// <summary> 
            /// サイズを設定することにより、LDT を拡大しようとしたか、サイズが偶数個のセレクタでないことを示します。
            /// </summary>
            STATUS_INVALID_LDT_SIZE = 0xc0000118,

            /// <summary> 
            /// LDT 情報の初期値がセレクタ サイズの整数倍でないことを示します。
            /// </summary>
            STATUS_INVALID_LDT_OFFSET = 0xc0000119,

            /// <summary> 
            /// LDT 記述子を設定するときに、ユーザーが無効な記述子を指定したことを示します。
            /// </summary>
            STATUS_INVALID_LDT_DESCRIPTOR = 0xc000011a,

            /// <summary> 
            /// 指定したイメージ ファイルは正しい形式でなく、NE 形式です。
            /// </summary>
            STATUS_INVALID_IMAGE_NE_FORMAT = 0xc000011b,

            /// <summary> 
            /// レジストリ サブツリーのトランザクション状態と要求された操作との間に互換性のないことを示します。
            /// たとえば、既にトランザクションが実行中のときに新しいトランザクションを起動する要求を出したり、実行中でないときにトランザクションを適用する要求を出しました。
            /// </summary>
            STATUS_RXACT_INVALID_STATE = 0xc000011c,

            /// <summary> 
            /// レジストリ トランザクション コミットでエラーが発生したことを示します。
            /// データベースが不明な (矛盾した) 状態です。レジストリ トランザクションの状態は COMMITTING として残されます。
            /// </summary>
            STATUS_RXACT_COMMIT_FAILURE = 0xc000011d,

            /// <summary> 
            /// 最大サイズを 0 として指定して、サイズが 0 のファイルを割り当てようとしました。
            /// </summary>
            STATUS_MAPPED_FILE_SIZE_ZERO = 0xc000011e,

            /// <summary> 
            /// リモート サーバーで開かれているファイルの数が多すぎます。
            /// このエラーはリモート ドライブの Windows リダイレクタからのみ返されます。
            /// </summary>
            STATUS_TOO_MANY_OPENED_FILES = 0xc000011f,

            /// <summary> 
            /// I/O 要求が取り消されました。
            /// </summary>
            STATUS_CANCELLED = 0xc0000120,

            /// <summary> 
            /// 削除できないファイルまたはディレクトリを削除しようとしました。
            /// </summary>
            STATUS_CANNOT_DELETE = 0xc0000121,

            /// <summary> 
            /// リモート コンピュータ名として指定した名前の構文が無効であることを示します。
            /// </summary>
            STATUS_INVALID_COMPUTER_NAME = 0xc0000122,

            /// <summary> 
            /// ファイルを削除した後、そのファイルに対してクローズ以外の I/O 要求を実行しました。
            /// この状況は、NtClose を使用して最後のハンドルを終了する前に完了していない要求に対してのみ発生します。
            /// </summary>
            STATUS_FILE_DELETED = 0xc0000123,

            /// <summary> 
            /// 組み込みアカウントと互換性のない操作を組み込み (特殊) SAM アカウントに対して実行しようとしました。
            /// たとえば、組み込みアカウントを削除することはできません。
            /// </summary>
            STATUS_SPECIAL_ACCOUNT = 0xc0000124,

            /// <summary> 
            /// 指定したグループは組み込み特殊グループであるため、要求した操作はそのグループに対して実行されません。
            /// </summary>
            STATUS_SPECIAL_GROUP = 0xc0000125,

            /// <summary> 
            /// 指定したユーザーは組み込み特殊ユーザーであるため、要求した操作はそのユーザーに対して実行されません。
            /// </summary>
            STATUS_SPECIAL_USER = 0xc0000126,

            /// <summary> 
            /// グループは現在、メンバのプライマリ グループであるため、そのメンバをグループから削除できないことを示します。
            /// </summary>
            STATUS_MEMBERS_PRIMARY_GROUP = 0xc0000127,

            /// <summary> 
            /// 既に閉じているファイル オブジェクトを使用してクローズ以外の I/O 要求または、ほかの特殊な操作を実行しようとしました。
            /// </summary>
            STATUS_FILE_CLOSED = 0xc0000128,

            /// <summary> 
            /// プロセスのスレッドの数が多すぎるため、要求した操作を実行できないことを示します。たとえば、プライマリ トークンの割り当ては、プロセスのスレッドの数が 0 または 1 の場合にだけ実行できます。
            /// </summary>
            STATUS_TOO_MANY_THREADS = 0xc0000129,

            /// <summary> 
            /// 特定のプロセス内でスレッドを操作しようとしましたが、指定したスレッドが指定したプロセス内にありません。
            /// </summary>
            STATUS_THREAD_NOT_IN_PROCESS = 0xc000012a,

            /// <summary> 
            /// プライマリ トークンとして使用するためにトークンを設定しようとしましたが、そのトークンは既に使用されています。各トークンは一度に 1 つだけのプロセスのプライマリ トークンになることができます。
            /// </summary>
            STATUS_TOKEN_ALREADY_IN_USE = 0xc000012b,

            /// <summary> 
            /// ページング ファイル クォータを超えました。
            /// </summary>
            STATUS_PAGEFILE_QUOTA_EXCEEDED = 0xc000012c,

            /// <summary> 
            /// システムの仮想メモリが少なくなってきています。Windows が正しく動作するために、仮想メモリ ページ ファイルのサイズを増やしてください。詳細はヘルプを参照してください。
            /// </summary>
            STATUS_COMMITMENT_LIMIT = 0xc000012d,

            /// <summary> 
            /// 指定したイメージ ファイルは正しい形式でなく、LE 形式です。
            /// </summary>
            STATUS_INVALID_IMAGE_LE_FORMAT = 0xc000012e,

            /// <summary> 
            /// 指定したイメージ ファイルは正しい形式でなく、初期 MZ が登録されていません。
            /// </summary>
            STATUS_INVALID_IMAGE_NOT_MZ = 0xc000012f,

            /// <summary> 
            /// 指定したイメージ ファイルは正しい形式でなく、MZ ヘッダーに正しい e_lfarlc が登録されていません。
            /// </summary>
            STATUS_INVALID_IMAGE_PROTECT = 0xc0000130,

            /// <summary> 
            /// 指定したイメージ ファイルは正しい形式でなく、16 ビットの Windows イメージです。
            /// </summary>
            STATUS_INVALID_IMAGE_WIN_16 = 0xc0000131,

            /// <summary> 
            /// ドメインで実行中の別の Netlogon サービスが指定した役割と競合するため、Netlogon サービスを開始できません。
            /// </summary>
            STATUS_LOGON_SERVER_CONFLICT = 0xc0000132,

            /// <summary> 
            /// プライマリ ドメイン コントローラの時刻がバックアップ ドメイン コントローラまたはメンバ サーバーの時刻と大幅に違います。
            /// </summary>
            STATUS_TIME_DIFFERENCE_AT_DC = 0xc0000133,

            /// <summary> 
            /// Windows サーバーの SAM データベースはドメイン コントローラのコピーと大幅に同期がとれていません。完全な同期をとることが必要です。
            /// </summary>
            STATUS_SYNCHRONIZATION_REQUIRED = 0xc0000134,

            /// <summary> 
            /// コンポーネントが見つからなかったため、このアプリケーションを開始できませんでした。アプリケーションをインストールし直すとこの問題は解決される場合があります。
            /// </summary>
            STATUS_DLL_NOT_FOUND = 0xc0000135,

            /// <summary> 
            /// NtCreateFile API が失敗しました。このエラーがアプリケーションに返されることはなく、Windows LAN Manager リダイレクタが内部エラーの割り当てルーチンでプレースホルダとして使用します。
            /// </summary>
            STATUS_OPEN_FAILED = 0xc0000136,

            /// <summary> 
            /// プロセスの I/O アクセス許可を変更できませんでした。
            /// </summary>
            STATUS_IO_PRIVILEGE_FAILED = 0xc0000137,

            /// <summary> 
            /// 序数がダイナミック ライブラリから見つかりませんでした。
            /// </summary>
            STATUS_ORDINAL_NOT_FOUND = 0xc0000138,

            /// <summary> 
            /// プロシージャ エントリ ポイントがダイナミック リンク ライブラリから見つかりませんでした。
            /// </summary>
            STATUS_ENTRYPOINT_NOT_FOUND = 0xc0000139,

            /// <summary> 
            /// Ctrl+C キーでアプリケーションが終了しました。
            /// </summary>
            STATUS_CONTROL_C_EXIT = 0xc000013a,

            /// <summary> 
            /// コンピュータのネットワーク トランスポートはネットワーク接続を終了しました。処理待ちの I/O 要求が残されているかどうかは不明です。
            /// </summary>
            STATUS_LOCAL_DISCONNECT = 0xc000013b,

            /// <summary> 
            /// リモート コンピュータのネットワーク トランスポートはネットワーク接続を終了しました。処理待ちの I/O 要求が残されているかどうかは不明です。
            /// </summary>
            STATUS_REMOTE_DISCONNECT = 0xc000013c,

            /// <summary> 
            /// リモート コンピュータのリソースが足りないため、ネットワーク要求を終了できません。たとえば、リモート コンピュータで使用できるメモリが不足しているため、現在、要求を実行できません。
            /// </summary>
            STATUS_REMOTE_RESOURCES = 0xc000013d,

            /// <summary> 
            /// 既存の接続 (仮想回線) がリモート コンピュータで終了しました。リモート コンピュータのネットワーク ソフトウェア プロトコルまたはネットワーク ハードウェアに問題のある可能性があります。
            /// </summary>
            STATUS_LINK_FAILED = 0xc000013e,

            /// <summary> 
            /// コンピュータのネットワーク トランスポートがネットワーク接続を終了しました。リモート コンピュータからの応答待ち時間が限界を超えました。ネットワーク接続を終了しました。
            /// </summary>
            STATUS_LINK_TIMEOUT = 0xc000013f,

            /// <summary> 
            /// トランスポートに与えた接続ハンドルは無効です。
            /// </summary>
            STATUS_INVALID_CONNECTION = 0xc0000140,

            /// <summary> 
            /// トランスポートに与えたアドレス ハンドルは無効です。
            /// </summary>
            STATUS_INVALID_ADDRESS = 0xc0000141,

            /// <summary> 
            /// ダイナミック リンク ライブラリの初期化に失敗しました。プロセスは異常終了します。
            /// </summary>
            STATUS_DLL_INIT_FAILED = 0xc0000142,

            /// <summary> 
            /// 必要なシステム ファイルが正しくない、または紛失しています。
            /// </summary>
            STATUS_MISSING_SYSTEMFILE = 0xc0000143,

            /// <summary> 
            /// ハンドルされていない例外が発生しました。
            /// </summary>
            STATUS_UNHANDLED_EXCEPTION = 0xc0000144,

            /// <summary> 
            /// アプリケーションを正しく初期化できませんでした。
            /// </summary>
            STATUS_APP_INIT_FAILURE = 0xc0000145,

            /// <summary> 
            /// ページング ファイルを作成できません。
            /// </summary>
            STATUS_PAGEFILE_CREATE_FAILED = 0xc0000146,

            /// <summary> 
            /// システム構成にページング ファイルが指定されていません。
            /// </summary>
            STATUS_NO_PAGEFILE = 0xc0000147,

            /// <summary> 
            /// 指定したシステム コールに無効なレベルが渡されました。
            /// </summary>
            STATUS_INVALID_LEVEL = 0xc0000148,

            /// <summary> 
            /// LAN Manager 2.x または MS-NET サーバーに誤ったパスワードを指定しました。
            /// </summary>
            STATUS_WRONG_PASSWORD_CORE = 0xc0000149,

            /// <summary> 
            /// リアル モード アプリケーションが浮動小数点命令を実行しましたが、浮動小数点ハードウェアがありません。
            /// </summary>
            STATUS_ILLEGAL_FLOAT_CONTEXT = 0xc000014a,

            /// <summary> 
            /// パイプの他端が閉じているため、パイプ操作が失敗しました。
            /// </summary>
            STATUS_PIPE_BROKEN = 0xc000014b,

            /// <summary> 
            /// レジストリ データを格納しているファイルの構造が壊れているか、メモリ内のファイル イメージが壊れているか、代替コピーまたはログがないか壊れているため、ファイルを回復できません。
            /// </summary>
            STATUS_REGISTRY_CORRUPT = 0xc000014c,

            /// <summary> 
            /// レジストリが開始した I/O 操作で回復不可能なエラーが発生しました。
            /// レジストリのシステム イメージを登録しているファイルの 1 つをレジストリが読み取ることができないか、書き込むことができないか、または消去できません。
            /// </summary>
            STATUS_REGISTRY_IO_FAILED = 0xc000014d,

            /// <summary> 
            /// スレッド固有のクライアント/サーバー イベント ペア オブジェクトを使用してイベント ペア同期操作を実行しましたが、イベント ペア オブジェクトがスレッドに関連付けられていません。
            /// </summary>
            STATUS_NO_EVENT_PAIR = 0xc000014e,

            /// <summary> 
            /// このボリュームは認識可能なファイル システムではありません。
            /// 必要なファイル システム ドライバがすべて読み込まれているか、ボリュームが壊れていないか確認してください。
            /// </summary>
            STATUS_UNRECOGNIZED_VOLUME = 0xc000014f,

            /// <summary> 
            /// シリアル デバイスを正しく初期化できませんでした。シリアル ドライバはアンロードされます。
            /// </summary>
            STATUS_SERIAL_NO_DEVICE_INITED = 0xc0000150,

            /// <summary> 
            /// 指定されたローカル グループは存在しません。
            /// </summary>
            STATUS_NO_SUCH_ALIAS = 0xc0000151,

            /// <summary> 
            /// 指定されたアカウント名はローカル グループのメンバではありません。
            /// </summary>
            STATUS_MEMBER_NOT_IN_ALIAS = 0xc0000152,

            /// <summary> 
            /// 指定されたアカウント名は既にローカル グループのメンバです。
            /// </summary>
            STATUS_MEMBER_IN_ALIAS = 0xc0000153,

            /// <summary> 
            /// 指定されたローカル グループは既に存在します。
            /// </summary>
            STATUS_ALIAS_EXISTS = 0xc0000154,

            /// <summary> 
            /// 要求したログオンの種類 (たとえば、対話型、ネットワーク、サービスなど) は対象システムのローカル セキュリティ ポリシーで許可されていません。
            /// システム管理者に必要なログオンの種類を与えてもらってください。
            /// </summary>
            STATUS_LOGON_NOT_GRANTED = 0xc0000155,

            /// <summary> 
            /// 1 つのシステムに格納できるシークレットの最大数を超えました。シークレットの長さと数は、米国防総省の輸出規制によって制限されます。
            /// </summary>
            STATUS_TOO_MANY_SECRETS = 0xc0000156,

            /// <summary> 
            /// シークレットの長さが最大長を超えました。シークレットの長さと数は米国防総省の輸出規制によって制限されます。
            /// </summary>
            STATUS_SECRET_TOO_LONG = 0xc0000157,

            /// <summary> 
            /// ローカル セキュリティ機関 (LSA) データベースの内部に矛盾があります。
            /// </summary>
            STATUS_INTERNAL_DB_ERROR = 0xc0000158,

            /// <summary> 
            /// 要求された操作は全画面モードでは実行できません。
            /// </summary>
            STATUS_FULLSCREEN_MODE = 0xc0000159,

            /// <summary> 
            /// ログオンの実行中に、ユーザーのセキュリティ コンテキストで蓄積されたセキュリティ ID が多すぎます。これはきわめてまれな状況です。
            /// グローバルまたはローカル グループからユーザーを削除して、セキュリティ コンテキストに組み込むセキュリティ ID の数を削減してください。
            /// </summary>
            STATUS_TOO_MANY_CONTEXT_IDS = 0xc000015a,

            /// <summary> 
            /// ユーザーが許可されていないログオンの種類 (たとえば、対話型やネットワーク) を要求しました。管理者は対話方式またはネットワークを介してログオンできるユーザーを制御しています。
            /// </summary>
            STATUS_LOGON_TYPE_NOT_GRANTED = 0xc000015b,

            /// <summary> 
            /// レジストリにファイルを読み込みまたは復元しようとしましたが、指定されたファイルはレジストリ ファイルの形式ではありません。
            /// </summary>
            STATUS_NOT_REGISTRY_FILE = 0xc000015c,

            /// <summary> 
            /// Windows 形式で暗号化したパスワードを指定せずにセキュリティ アカウント マネージャでユーザー パスワードを変更しようとしました。
            /// </summary>
            STATUS_NT_CROSS_ENCRYPTION_REQUIRED = 0xc000015d,

            /// <summary> 
            /// Windows サーバーの構成が誤っています。
            /// </summary>
            STATUS_DOMAIN_CTRLR_CONFIG_ERROR = 0xc000015e,

            /// <summary> 
            /// フォールト トレランス ドライバに対するデバイス制御を介して情報のセカンダリ コピーにアクセスしようとしましたが、セカンダリ コピーがシステムにありません。
            /// </summary>
            STATUS_FT_MISSING_MEMBER = 0xc000015f,

            /// <summary> 
            /// ドライバ サービス エントリを表現する構成レジストリ ノードの形式が誤っており、必要な値エントリが指定されていません。
            /// </summary>
            STATUS_ILL_FORMED_SERVICE_ENTRY = 0xc0000160,

            /// <summary> 
            /// 無効な文字が検出されました。マルチバイト文字セットの場合、1 バイトのみで後続のバイトのない文字もこのエラーになります。Unicode 文字セットの場合は、0xFFFF と 0xFFFE は誤った文字であると判断されます。
            /// </summary>
            STATUS_ILLEGAL_CHARACTER = 0xc0000161,

            /// <summary> 
            /// Unicode 文字のマッピングがターゲットのマルチバイト コード ページにありません。
            /// </summary>
            STATUS_UNMAPPABLE_CHARACTER = 0xc0000162,

            /// <summary> 
            /// システムにインストールされている Unicode 文字セットに Unicode 文字が定義されていません。
            /// </summary>
            STATUS_UNDEFINED_CHARACTER = 0xc0000163,

            /// <summary> 
            /// ページング ファイルをフロッピーに作成することはできません。
            /// </summary>
            STATUS_FLOPPY_VOLUME = 0xc0000164,

            /// <summary> 
            /// フロッピーのアクセスで ID アドレス マークが見つかりませんでした。
            /// </summary>
            STATUS_FLOPPY_ID_MARK_NOT_FOUND = 0xc0000165,

            /// <summary> 
            /// フロッピーのアクセスでセクタ ID フィールドのトラック アドレスがコントローラで管理されているトラック アドレスと違います。
            /// </summary>
            STATUS_FLOPPY_WRONG_CYLINDER = 0xc0000166,

            /// <summary> 
            /// フロッピー ディスク ドライバが認識しないエラーがフロッピー ディスク コントローラから報告されました。
            /// </summary>
            STATUS_FLOPPY_UNKNOWN_ERROR = 0xc0000167,

            /// <summary> 
            /// フロッピーのアクセスで、レジスタを介してコントローラが矛盾する結果を返しました。
            /// </summary>
            STATUS_FLOPPY_BAD_REGISTERS = 0xc0000168,

            /// <summary> 
            /// ハード ディスクにアクセスするときに、再試行の後も再補正操作を正しく実行できませんでした。
            /// </summary>
            STATUS_DISK_RECALIBRATE_FAILED = 0xc0000169,

            /// <summary> 
            /// ハード ディスクにアクセスするときに、再試行の後もディスク操作を正しく実行できませんでした。
            /// </summary>
            STATUS_DISK_OPERATION_FAILED = 0xc000016a,

            /// <summary> 
            /// ハード ディスクのアクセスでディスク コントローラのリセットが必要でしたが、リセットが失敗しました。
            /// </summary>
            STATUS_DISK_RESET_FAILED = 0xc000016b,

            /// <summary> 
            /// ほかのデバイスと IRQ を共有しているデバイスを開こうとしました。IRQ を使用している 1 つ以上のほかのデバイスが既に開かれています。
            /// IRQ を共有し、割り込みによってのみ動作するデバイスを 2 つ以上同時に開くことは、デバイスが使用している特定のバスの種類ではサポートされません。
            /// </summary>
            STATUS_SHARED_IRQ_BUSY = 0xc000016c,

            /// <summary> 
            /// フォールト トレラント ボリュームの一部であるディスクにアクセスできません。
            /// </summary>
            STATUS_FT_ORPHANING = 0xc000016d,

            /// <summary> 
            /// システム BIOS は、システム割り込みをデバイスまたはデバイスに接続されているバスに接続できませんでした。
            /// </summary>
            STATUS_BIOS_FAILED_TO_CONNECT_INTERRUPT = 0xc000016e,

            /// <summary> 
            /// テープをパーティションに分割することはできません。
            /// </summary>
            STATUS_PARTITION_FAILURE = 0xc0000172,

            /// <summary> 
            /// マルチボリューム パーティションの新しいテープにアクセスするときに、現在のブロック サイズが誤っています。
            /// </summary>
            STATUS_INVALID_BLOCK_LENGTH = 0xc0000173,

            /// <summary> 
            /// テープを読み込むときに、テープ パーティション情報を見つけることができませんでした。
            /// </summary>
            STATUS_DEVICE_NOT_PARTITIONED = 0xc0000174,

            /// <summary> 
            /// メディア取り出し機構をロックできませんでした。
            /// </summary>
            STATUS_UNABLE_TO_LOCK_MEDIA = 0xc0000175,

            /// <summary> 
            /// メディアをアンロードできません。
            /// </summary>
            STATUS_UNABLE_TO_UNLOAD_MEDIA = 0xc0000176,

            /// <summary> 
            /// 物理的なテープの最後が検出されました。
            /// </summary>
            STATUS_EOM_OVERFLOW = 0xc0000177,

            /// <summary> 
            /// ドライブにメディアがありません。
            /// </summary>
            STATUS_NO_MEDIA = 0xc0000178,

            /// <summary> 
            /// メンバが存在しないため、メンバをローカル グループに追加または削除できませんでした。
            /// </summary>
            STATUS_NO_SUCH_MEMBER = 0xc000017a,

            /// <summary> 
            /// メンバのアカウントの種類が間違っているため、新しいメンバをローカル グループに追加できませんでした。
            /// </summary>
            STATUS_INVALID_MEMBER = 0xc000017b,

            /// <summary> 
            /// 削除対象としてマークされているレジストリ キーに対して不正操作を実行しようとしました。
            /// </summary>
            STATUS_KEY_DELETED = 0xc000017c,

            /// <summary> 
            /// 要求された空間をレジストリ ログに割り当てることができませんでした。
            /// </summary>
            STATUS_NO_LOG_SPACE = 0xc000017d,

            /// <summary> 
            /// 指定された SID の数が多すぎます。
            /// </summary>
            STATUS_TOO_MANY_SIDS = 0xc000017e,

            /// <summary> 
            /// LM 形式で暗号化したパスワードを指定せずに、セキュリティ アカウント マネージャでユーザー パスワードを変更しようとしました。
            /// </summary>
            STATUS_LM_CROSS_ENCRYPTION_REQUIRED = 0xc000017f,

            /// <summary> 
            /// 既にサブキーまたは値が割り当てられているレジストリ キーにシンボリック リンクを作成しようとしました。
            /// </summary>
            STATUS_KEY_HAS_CHILDREN = 0xc0000180,

            /// <summary> 
            /// 揮発性の親キーの下に安定したサブキーを作成しようとしました。
            /// </summary>
            STATUS_CHILD_MUST_BE_VOLATILE = 0xc0000181,

            /// <summary> 
            /// I/O デバイスの構成が誤っているか、ドライバに対する構成パラメータが誤っています。
            /// </summary>
            STATUS_DEVICE_CONFIGURATION_ERROR = 0xc0000182,

            /// <summary> 
            /// 2 つのドライバの間または I/O ドライバ内でエラーが検出されました。
            /// </summary>
            STATUS_DRIVER_INTERNAL_ERROR = 0xc0000183,

            /// <summary> 
            /// デバイスが正しい状態でないため、この要求を実行できません。
            /// </summary>
            STATUS_INVALID_DEVICE_STATE = 0xc0000184,

            /// <summary> 
            /// I/O デバイスが I/O エラーを報告しました。
            /// </summary>
            STATUS_IO_DEVICE_ERROR = 0xc0000185,

            /// <summary> 
            /// ドライバとデバイスの間でプロトコル エラーが検出されました。
            /// </summary>
            STATUS_DEVICE_PROTOCOL_ERROR = 0xc0000186,

            /// <summary> 
            /// この操作はドメインのプライマリ ドメイン コントローラに対してのみ実行できます。
            /// </summary>
            STATUS_BACKUP_CONTROLLER = 0xc0000187,

            /// <summary> 
            /// ログ ファイルの容量不足のため、この操作はサポートされません。
            /// </summary>
            STATUS_LOG_FILE_FULL = 0xc0000188,

            /// <summary> 
            /// マウント解除した後のボリュームに対して書き込み操作を実行しようとしました。
            /// </summary>
            STATUS_TOO_LATE = 0xc0000189,

            /// <summary> 
            /// ワークステーションのローカル LSA データベースにはプライマリ ドメインの信頼関係シークレットがありません。
            /// </summary>
            STATUS_NO_TRUST_LSA_SECRET = 0xc000018a,

            /// <summary> 
            /// Windows Server 上の SAM データベースがこのワークステーションの信頼関係に対するコンピュータ アカウントを持っていません。
            /// </summary>
            STATUS_NO_TRUST_SAM_ACCOUNT = 0xc000018b,

            /// <summary> 
            /// プライマリ ドメインと信頼される側のドメインの間の信頼関係の確立に失敗したため、ログオン要求は失敗しました。
            /// </summary>
            STATUS_TRUSTED_DOMAIN_FAILURE = 0xc000018c,

            /// <summary> 
            /// このワークステーションとプライマリ ドメインの間で信頼関係を結ぶことができなかったため、ログオン要求は失敗しました。
            /// </summary>
            STATUS_TRUSTED_RELATIONSHIP_FAILURE = 0xc000018d,

            /// <summary> 
            /// Eventlog ログ ファイルが壊れています。
            /// </summary>
            STATUS_EVENTLOG_FILE_CORRUPT = 0xc000018e,

            /// <summary> 
            /// Eventlog ログ ファイルを開けません。Eventlog サービスは開始されませんでした。
            /// </summary>
            STATUS_EVENTLOG_CANT_START = 0xc000018f,

            /// <summary> 
            /// ネットワーク ログオンが失敗しました。これは検査機関に到達できなかったためです。
            /// </summary>
            STATUS_TRUST_FAILURE = 0xc0000190,

            /// <summary> 
            /// 最大アカウントを超過するようなミュータントを要求しようとしました。
            /// </summary>
            STATUS_MUTANT_LIMIT_EXCEEDED = 0xc0000191,

            /// <summary> 
            /// ログオンしようとしましたが、Netlogon サービスが開始されていませんでした。
            /// </summary>
            STATUS_NETLOGON_NOT_STARTED = 0xc0000192,

            /// <summary> 
            /// ユーザーのアカウントは有効期限が切れています。
            /// </summary>
            STATUS_ACCOUNT_EXPIRED = 0xc0000193,

            /// <summary> 
            /// デッドロック状態の可能性があります。
            /// </summary>
            STATUS_POSSIBLE_DEADLOCK = 0xc0000194,

            /// <summary> 
            /// 同じユーザーによる、サーバーまたは共有リソースへの複数のユーザー名での複数の接続は許可されません。サーバーまたは共有リソースへの以前の接続をすべて切断してから、再試行してください。
            /// </summary>
            STATUS_NETWORK_CREDENTIAL_CONFLICT = 0xc0000195,

            /// <summary> 
            /// ネットワーク サーバーとの間でセッションを確立しようとしましたが、既にそのサーバーとの間に確立されているセッションが多すぎます。
            /// </summary>
            STATUS_REMOTE_SESSION_LIMIT = 0xc0000196,

            /// <summary> 
            /// 各読み取り操作の間でログ ファイルが変更されました。
            /// </summary>
            STATUS_EVENTLOG_FILE_CHANGED = 0xc0000197,

            /// <summary> 
            /// 使用されているアカウントはインタードメイン間信頼アカウントです。このサーバーにアクセスするには、ローカル ユーザー アカウントまたはグローバル ユーザー アカウントを使用してください。
            /// </summary>
            STATUS_NOLOGON_INTERDOMAIN_TRUST_ACCOUNT = 0xc0000198,

            /// <summary> 
            /// 使用されているアカウントはコンピュータ アカウントです。このサーバーにアクセスするには、グローバル ユーザー アカウントまたはローカル ユーザー アカウントを使用してください。
            /// </summary>
            STATUS_NOLOGON_WORKSTATION_TRUST_ACCOUNT = 0xc0000199,

            /// <summary> 
            /// 使用されているアカウントはサーバー信頼アカウントです。このサーバーにアクセスするには、グローバル ユーザー アカウントまたはローカル ユーザー アカウントを使用してください。
            /// </summary>
            STATUS_NOLOGON_SERVER_TRUST_ACCOUNT = 0xc000019a,

            /// <summary> 
            /// 指定されたドメインの名前またはセキュリティ ID (SID) とそのドメインの信頼情報が矛盾します。
            /// </summary>
            STATUS_DOMAIN_TRUST_INCONSISTENT = 0xc000019b,

            /// <summary> 
            /// ファイル システム ドライバがまだ読み込まれていないボリュームにアクセスしました。
            /// </summary>
            STATUS_FS_DRIVER_REQUIRED = 0xc000019c,

            /// <summary> 
            /// STATUS_IMAGE_ALREADY_LOADED_AS_DLL
            /// </summary>
            STATUS_IMAGE_ALREADY_LOADED_AS_DLL = 0xc000019d,

            /// <summary> 
            /// STATUS_NETWORK_OPEN_RESTRICTION
            /// </summary>
            STATUS_NETWORK_OPEN_RESTRICTION = 0xc0000201,

            /// <summary> 
            /// 指定されたログオン セッションに対して、ユーザー セッション キーがありません。
            /// </summary>
            STATUS_NO_USER_SESSION_KEY = 0xc0000202,

            /// <summary> 
            /// リモート ユーザー セッションが削除されました。
            /// </summary>
            STATUS_USER_SESSION_DELETED = 0xc0000203,

            /// <summary> 
            /// 指定したリソース ランゲージ ID がイメージ ファイルから見つからないことを示します。
            /// </summary>
            STATUS_RESOURCE_LANG_NOT_FOUND = 0xc0000204,

            /// <summary> 
            /// サーバー リソースが不足するため、要求を終了できません。
            /// </summary>
            STATUS_INSUFF_SERVER_RESOURCES = 0xc0000205,

            /// <summary> 
            /// 指定した操作に対するバッファのサイズが無効です。
            /// </summary>
            STATUS_INVALID_BUFFER_SIZE = 0xc0000206,

            /// <summary> 
            /// トランスポートは指定したネットワーク アドレスを無効なアドレスとして拒否しました。
            /// </summary>
            STATUS_INVALID_ADDRESS_COMPONENT = 0xc0000207,

            /// <summary> 
            /// ワイルドカードの使用法が誤っているため、トランスポートは指定したネットワーク アドレスを拒否しました。
            /// </summary>
            STATUS_INVALID_ADDRESS_WILDCARD = 0xc0000208,

            /// <summary> 
            /// 使用可能なすべてのアドレスが使用されているため、トランスポート アドレスを開くことができません。
            /// </summary>
            STATUS_TOO_MANY_ADDRESSES = 0xc0000209,

            /// <summary> 
            /// 既に存在するため、トランスポート アドレスを開くことができません。
            /// </summary>
            STATUS_ADDRESS_ALREADY_EXISTS = 0xc000020a,

            /// <summary> 
            /// トランスポート アドレスは現在閉じられています。
            /// </summary>
            STATUS_ADDRESS_CLOSED = 0xc000020b,

            /// <summary> 
            /// トランスポート接続は現在切断されています。
            /// </summary>
            STATUS_CONNECTION_DISCONNECTED = 0xc000020c,

            /// <summary> 
            /// トランスポート接続はリセットされました。
            /// </summary>
            STATUS_CONNECTION_RESET = 0xc000020d,

            /// <summary> 
            /// トランスポートはこれ以上のノードを動的に獲得できません。
            /// </summary>
            STATUS_TOO_MANY_NODES = 0xc000020e,

            /// <summary> 
            /// トランスポートは保留状態のトランザクションを打ち切りました。
            /// </summary>
            STATUS_TRANSACTION_ABORTED = 0xc000020f,

            /// <summary> 
            /// トランスポートは応答待ちの要求をタイムアウトにしました。
            /// </summary>
            STATUS_TRANSACTION_TIMED_OUT = 0xc0000210,

            /// <summary> 
            /// トランスポートは応答待ちの解除を受信しませんでした。
            /// </summary>
            STATUS_TRANSACTION_NO_RELEASE = 0xc0000211,

            /// <summary> 
            /// トランスポートは特定のトークンと一致するトランザクションを見つけることができませんでした。
            /// </summary>
            STATUS_TRANSACTION_NO_MATCH = 0xc0000212,

            /// <summary> 
            /// トランスポートはトランザクション要求に既に応答しています。
            /// </summary>
            STATUS_TRANSACTION_RESPONDED = 0xc0000213,

            /// <summary> 
            /// トランスポートは指定されたトランザクション要求識別子を認識しません。
            /// </summary>
            STATUS_TRANSACTION_INVALID_ID = 0xc0000214,

            /// <summary> 
            /// トランスポートは指定されたトランザクション要求の種類を認識しません。
            /// </summary>
            STATUS_TRANSACTION_INVALID_TYPE = 0xc0000215,

            /// <summary> 
            /// トランスポートは指定された要求をセッションのクライアント側でのみ処理できます。
            /// </summary>
            STATUS_NOT_SERVER_SESSION = 0xc0000216,

            /// <summary> 
            /// トランスポートは指定された要求をセッションのクライアント側でのみ処理できます。
            /// </summary>
            STATUS_NOT_CLIENT_SESSION = 0xc0000217,

            /// <summary> 
            /// レジストリは、ハイブ (ファイル) 、そのログ、または代替ファイルを読み込めません。
            /// 壊れているか、紛失したか、または書き込み不可能です。
            /// </summary>
            STATUS_CANNOT_LOAD_REGISTRY_FILE = 0xc0000218,

            /// <summary> 
            /// DebugActiveProcess API 要求の処理で予期しないエラーが発生しました。
            /// </summary>
            STATUS_DEBUG_ATTACH_FAILED = 0xc0000219,

            /// <summary> 
            /// システム プロセスが異常終了しました。
            /// </summary>
            STATUS_SYSTEM_PROCESS_TERMINATED = 0xc000021a,

            /// <summary> 
            /// TDI クライアントは指示中に受信したデータを処理できませんでした。
            /// </summary>
            STATUS_DATA_NOT_ACCEPTED = 0xc000021b,

            /// <summary> 
            /// このワークグループのサーバー一覧は現在利用できません。
            /// </summary>
            STATUS_NO_BROWSER_SERVERS_FOUND = 0xc000021c,

            /// <summary> 
            /// NTVDM がハード エラーを検出しました。
            /// </summary>
            STATUS_VDM_HARD_ERROR = 0xc000021d,

            /// <summary> 
            /// ドライバが割り当てられた時間内に取り消された I/O 要求を完了できませんでした。
            /// </summary>
            STATUS_DRIVER_CANCEL_TIMEOUT = 0xc000021e,

            /// <summary> 
            /// LPC メッセージに返信しようとしましたが、メッセージ内のクライアント ID によって指定されたスレッドがそのメッセージを待っていませんでした。
            /// </summary>
            STATUS_REPLY_MESSAGE_MISMATCH = 0xc000021f,

            /// <summary> 
            /// ファイルの表示を割り当てようとしましたが、指定したベース アドレスまたはファイルへのオフセットが正しい割り当て粒度 (整列境界) に整列されていませんでした。
            /// </summary>
            STATUS_MAPPED_ALIGNMENT = 0xc0000220,

            /// <summary> 
            /// イメージが壊れている可能性があります。ヘッダーのチェックサムが計算で求めたチェックサムと一致しません。
            /// </summary>
            STATUS_IMAGE_CHECKSUM_MISMATCH = 0xc0000221,

            /// <summary> 
            /// ファイルのためのデータを一部保存できませんでした。データを損失しました。
            /// このエラーは、コンピュータのハードウェアまたはネットワーク接続の障害によって発生した可能性があります。このファイルをどこか別の所に保存してください。
            /// </summary>
            STATUS_LOST_WRITEBEHIND_DATA = 0xc0000222,

            /// <summary> 
            /// クライアント/サーバー共有メモリ ウィンドウのサーバーに渡されたパラメータが無効です。共有メモリ ウィンドウに渡したデータが多すぎます。
            /// </summary>
            STATUS_CLIENT_SERVER_PARAMETERS_INVALID = 0xc0000223,

            /// <summary> 
            /// ユーザーのパスワードを最初にログオンする前に変更しなければなりません。
            /// </summary>
            STATUS_PASSWORD_MUST_CHANGE = 0xc0000224,

            /// <summary> 
            /// オブジェクトが見つかりませんでした。
            /// </summary>
            STATUS_NOT_FOUND = 0xc0000225,

            /// <summary> 
            /// このストリームは小さなストリームではありません。
            /// </summary>
            STATUS_NOT_TINY_STREAM = 0xc0000226,

            /// <summary> 
            /// トランザクションの回復に失敗しました。
            /// </summary>
            STATUS_RECOVERY_FAILURE = 0xc0000227,

            /// <summary> 
            /// この要求はスタック オーバーフロー コードが実行します。
            /// </summary>
            STATUS_STACK_OVERFLOW_READ = 0xc0000228,

            /// <summary> 
            /// 整合性チェックが失敗しました。
            /// </summary>
            STATUS_FAIL_CHECK = 0xc0000229,

            /// <summary> 
            /// 既にインデックスに ID が存在するため、ID の挿入に失敗しました。
            /// </summary>
            STATUS_DUPLICATE_OBJECTID = 0xc000022a,

            /// <summary> 
            /// オブジェクトには既に ID が設定されているため、ID の設定に失敗しました。
            /// </summary>
            STATUS_OBJECTID_EXISTS = 0xc000022b,

            /// <summary> 
            /// 内部 OFS 状態コードはどのように割り当て操作が実行されるかを示しています。onode が移された後に再実行されるか、ストリームが大きなストリームに変換された後に再実行されるかのいずれかです。
            /// </summary>
            STATUS_CONVERT_TO_LARGE = 0xc000022c,

            /// <summary> 
            /// 要求の再実行が必要です。
            /// </summary>
            STATUS_RETRY = 0xc000022d,

            /// <summary> 
            /// このボリュームに同一の ID を持つオブジェクトを見つけましたが、この操作で使用するハンドルの範囲外にあります。
            /// </summary>
            STATUS_FOUND_OUT_OF_SCOPE = 0xc000022e,

            /// <summary> 
            /// バケット配列を大きくしてください。その後でトランザクションを再実行してください。
            /// </summary>
            STATUS_ALLOCATE_BUCKET = 0xc000022f,

            /// <summary> 
            /// 指定したプロパティ セットがこのオブジェクトに存在しません。
            /// </summary>
            STATUS_PROPSET_NOT_FOUND = 0xc0000230,

            /// <summary> 
            /// ユーザー/カーネル マーシャリング バッファがオーバーフローしました。
            /// </summary>
            STATUS_MARSHALL_OVERFLOW = 0xc0000231,

            /// <summary> 
            /// 指定した可変構造に無効なデータがあります。
            /// </summary>
            STATUS_INVALID_VARIANT = 0xc0000232,

            /// <summary> 
            /// このドメインのドメイン コントローラが見つかりませんでした。
            /// </summary>
            STATUS_DOMAIN_CONTROLLER_NOT_FOUND = 0xc0000233,

            /// <summary> 
            /// 無効なログオンまたはパスワードの変更の要求が多すぎたため、このユーザー アカウントは自動的にロックされました。
            /// </summary>
            STATUS_ACCOUNT_LOCKED_OUT = 0xc0000234,

            /// <summary> 
            /// NtClose は NtSetInformationObject 経由のクローズから保護されているハンドルにコールされました。
            /// </summary>
            STATUS_HANDLE_NOT_CLOSABLE = 0xc0000235,

            /// <summary> 
            /// トランスポート接続はリモート システムに拒否されました。
            /// </summary>
            STATUS_CONNECTION_REFUSED = 0xc0000236,

            /// <summary> 
            /// トランスポート接続は終了しました。
            /// </summary>
            STATUS_GRACEFUL_DISCONNECT = 0xc0000237,

            /// <summary> 
            /// トランスポートのエンドポイントには既に関連付けられたアドレスがあります。
            /// </summary>
            STATUS_ADDRESS_ALREADY_ASSOCIATED = 0xc0000238,

            /// <summary> 
            /// アドレスはまだトランスポートのエンドポイントに関連付けられていません。
            /// </summary>
            STATUS_ADDRESS_NOT_ASSOCIATED = 0xc0000239,

            /// <summary> 
            /// 存在しないトランスポート接続で操作を実行しようとしました。
            /// </summary>
            STATUS_CONNECTION_INVALID = 0xc000023a,

            /// <summary> 
            /// 無効な操作をトランスポート接続で実行しようとしました。
            /// </summary>
            STATUS_CONNECTION_ACTIVE = 0xc000023b,

            /// <summary> 
            /// トランスポートからリモート ネットワークへ接続できません。
            /// </summary>
            STATUS_NETWORK_UNREACHABLE = 0xc000023c,

            /// <summary> 
            /// トランスポートからリモート システムへ接続できません。
            /// </summary>
            STATUS_HOST_UNREACHABLE = 0xc000023d,

            /// <summary> 
            /// リモート システムはこのトランスポート プロトコルをサポートしていません。
            /// </summary>
            STATUS_PROTOCOL_UNREACHABLE = 0xc000023e,

            /// <summary> 
            /// リモート システムのトランスポートの転送先ポートでサービスが開始されていません。
            /// </summary>
            STATUS_PORT_UNREACHABLE = 0xc000023f,

            /// <summary> 
            /// 要求は中止されました。
            /// </summary>
            STATUS_REQUEST_ABORTED = 0xc0000240,

            /// <summary> 
            /// トランスポート接続がローカル システムによって中止されました。
            /// </summary>
            STATUS_CONNECTION_ABORTED = 0xc0000241,

            /// <summary> 
            /// 指定したバッファに誤った形式のデータが含まれています。
            /// </summary>
            STATUS_BAD_COMPRESSION_BUFFER = 0xc0000242,

            /// <summary> 
            /// 要求された操作はユーザーに割り当てられたセクションで開いたファイルでは実行できません。
            /// </summary>
            STATUS_USER_MAPPED_FILE = 0xc0000243,

            /// <summary> 
            /// セキュリティ監査の生成に失敗しました。
            /// </summary>
            STATUS_AUDIT_FAILED = 0xc0000244,

            /// <summary> 
            /// タイマ刻みは既に現在のプロセスによって設定されています。
            /// </summary>
            STATUS_TIMER_RESOLUTION_NOT_SET = 0xc0000245,

            /// <summary> 
            /// このアカウントに対する同時接続数が上限に達したため、サーバーに接続できませんでした。
            /// </summary>
            STATUS_CONNECTION_COUNT_LIMIT = 0xc0000246,

            /// <summary> 
            /// このアカウントに許可されていない時刻にログインしようとしています。
            /// </summary>
            STATUS_LOGIN_TIME_RESTRICTION = 0xc0000247,

            /// <summary> 
            /// そのアカウントは、このワークステーションからのログインを許可されていません。
            /// </summary>
            STATUS_LOGIN_WKSTA_RESTRICTION = 0xc0000248,

            /// <summary> 
            /// イメージはユニプロセッサ システムで使用できるように修正されていますが、現在、マルチプロセッサ コンピュータ上で実行しています。
            /// イメージ ファイルをインストールし直してください。
            /// </summary>
            STATUS_IMAGE_MP_UP_MISMATCH = 0xc0000249,

            /// <summary> 
            /// ログオンのためのアカウント情報が不足しています。
            /// </summary>
            STATUS_INSUFFICIENT_LOGON_INFO = 0xc0000250,

            /// <summary> 
            /// 動的リンク ライブラリは正しく記述されていません。スタック ポインタが矛盾する状態にあります。
            /// エントリ ポイントは WINAPI または STDCALL として宣言されなければなりません。
            /// </summary>
            STATUS_BAD_DLL_ENTRYPOINT = 0xc0000251,

            /// <summary> 
            /// サービスは正しく記述されていません。スタック ポインタが矛盾する状態にあります。
            /// コールバック エントリ ポイントは WINAPI または STDCALL として宣言されなければなりません。
            /// </summary>
            STATUS_BAD_SERVICE_ENTRYPOINT = 0xc0000252,

            /// <summary> 
            /// サーバーはメッセージを受信しましたが、返信できませんでした。
            /// </summary>
            STATUS_LPC_REPLY_LOST = 0xc0000253,

            /// <summary> 
            /// ネットワーク上の別のシステムと競合する IP アドレスがあります。
            /// </summary>
            STATUS_IP_ADDRESS_CONFLICT1 = 0xc0000254,

            /// <summary> 
            /// ネットワーク上の別のシステムと競合する IP アドレスがあります。
            /// </summary>
            STATUS_IP_ADDRESS_CONFLICT2 = 0xc0000255,

            /// <summary> 
            /// このシステムはレジストリのシステムの部分に割り当てられた最大サイズに達しました。記憶域の追加要求は無視されます。
            /// </summary>
            STATUS_REGISTRY_QUOTA_LIMIT = 0xc0000256,

            /// <summary> 
            /// 接続したサーバーは DFS 名前空間の一部をサポートしません。
            /// </summary>
            STATUS_PATH_NOT_COVERED = 0xc0000257,

            /// <summary> 
            /// コールバックがアクティブでないときは、コールバック リターン システム サービスを実行できません。
            /// </summary>
            STATUS_NO_CALLBACK_ACTIVE = 0xc0000258,

            /// <summary> 
            /// アクセスされているサービスは、ライセンスされる接続数が特定されています。
            /// これ以上そのサービスに接続できません。
            /// </summary>
            STATUS_LICENSE_QUOTA_EXCEEDED = 0xc0000259,

            /// <summary> 
            /// 指定されたパスワードが短すぎるため、ユーザー アカウント ポリシーに適合しません。
            /// もっと長いパスワードを入力してください。
            /// </summary>
            STATUS_PWD_TOO_SHORT = 0xc000025a,

            /// <summary> 
            /// 使用しているユーザー アカウントのポリシーでは、パスワードの頻繁な変更は許可されていません。
            /// これはユーザーが、第三者に知られてしまった可能性のある以前のパスワードに変更するのを防止するためです。
            /// 自分のパスワードがありふれていると感じているユーザーは、管理者に問い合わせて、新しいパスワードを割り当ててもらってください。
            /// </summary>
            STATUS_PWD_TOO_RECENT = 0xc000025b,

            /// <summary> 
            /// 以前に使用していたパスワードに変更しようとしました。
            /// これはユーザー アカウント ポリシーに違反します。使用したことのないパスワードを入力してください。
            /// </summary>
            STATUS_PWD_HISTORY_CONFLICT = 0xc000025c,

            /// <summary> 
            /// デバイス インスタンスが無効になっている間に、レガシ デバイス ドライバを読み込もうとしました。
            /// </summary>
            STATUS_PLUGPLAY_NO_DEVICE = 0xc000025e,

            /// <summary> 
            /// 指定された圧縮形式はサポートされていません。
            /// </summary>
            STATUS_UNSUPPORTED_COMPRESSION = 0xc000025f,

            /// <summary> 
            /// 指定されたハードウェア プロファイル構成が無効です。
            /// </summary>
            STATUS_INVALID_HW_PROFILE = 0xc0000260,

            /// <summary> 
            /// 指定されたプラグ アンド プレイのレジストリ デバイス パスが無効です。
            /// </summary>
            STATUS_INVALID_PLUGPLAY_DEVICE_PATH = 0xc0000261,

            /// <summary> 
            /// デバイス ドライバが序数を見つけられませんでした。
            /// </summary>
            STATUS_DRIVER_ORDINAL_NOT_FOUND = 0xc0000262,

            /// <summary> 
            /// デバイス ドライバがエントリ ポイントを見つけられませんでした。
            /// </summary>
            STATUS_DRIVER_ENTRYPOINT_NOT_FOUND = 0xc0000263,

            /// <summary> 
            /// アプリケーションは、所有権のないリソースを解放しようとしました。
            /// </summary>
            STATUS_RESOURCE_NOT_OWNED = 0xc0000264,

            /// <summary> 
            /// ファイルに対して、ファイル システムがサポートする以上の数のリンクを作成しようとしました。
            /// </summary>
            STATUS_TOO_MANY_LINKS = 0xc0000265,

            /// <summary> 
            /// 指定されたクォータの一覧は、記述子と内部的に矛盾しています。
            /// </summary>
            STATUS_QUOTA_LIST_INCONSISTENT = 0xc0000266,

            /// <summary> 
            /// 指定されたファイルは、オフライン記憶域に再配置されました。
            /// </summary>
            STATUS_FILE_IS_OFFLINE = 0xc0000267,

            /// <summary> 
            /// 現在インストールされている Windows の評価期間が終了しました。このシステムは、1 時間以内にシャットダウンします。この Windows へのアクセスを回復するには、この製品のライセンス付きバージョンを使ってアップグレードしてください。
            /// </summary>
            STATUS_EVALUATION_EXPIRATION = 0xc0000268,

            /// <summary> 
            /// システム DLL がメモリ内で再配置されました。アプリケーションは正常に動作しません。
            /// 再配置が起きたのは、DLL が、Windows システム DLL のために予約されているアドレス範囲を使用していたためです。この DLL のベンダに連絡して、新しい DLL を入手してください。
            /// </summary>
            STATUS_ILLEGAL_DLL_RELOCATION = 0xc0000269,

            /// <summary> 
            /// システムは、登録されている製品の種類が変更されていることを検出しました。これは、ソフトウェア ライセンスの違反になります。製品の種類の変更は禁止されています。
            /// </summary>
            STATUS_LICENSE_VIOLATION = 0xc000026a,

            /// <summary> 
            /// ウィンドウ ステーションがシャットダウン中であるため、アプリケーションが初期化に失敗しました。
            /// </summary>
            STATUS_DLL_INIT_FAILED_LOGOFF = 0xc000026b,

            /// <summary> 
            /// デバイス ドライバを読み込めませんでした。
            /// </summary>
            STATUS_DRIVER_UNABLE_TO_LOAD = 0xc000026c,

            /// <summary> 
            /// 接続したサーバー上で、DFS を利用できません。
            /// </summary>
            STATUS_DFS_UNAVAILABLE = 0xc000026d,

            /// <summary> 
            /// マウント解除した後のボリュームに対して操作を実行しようとしました。
            /// </summary>
            STATUS_VOLUME_DISMOUNTED = 0xc000026e,

            /// <summary> 
            /// Win32 x86 エミュレーション サブシステムで内部エラーが発生しました。
            /// </summary>
            STATUS_WX86_INTERNAL_ERROR = 0xc000026f,

            /// <summary> 
            /// Win32 x86 エミュレーション サブシステム浮動小数点スタック チェック。
            /// </summary>
            STATUS_WX86_FLOAT_STACK_CHECK = 0xc0000270,

            /// <summary> 
            /// 検証プロセスは次のステップへ続行させる必要があります。
            /// </summary>
            STATUS_VALIDATE_CONTINUE = 0xc0000271,

            /// <summary> 
            /// インデックス内には指定されたキーに一致するものがありませんでした。
            /// </summary>
            STATUS_NO_MATCH = 0xc0000272,

            /// <summary> 
            /// 現在のインデックス列挙には一致するものがありません。
            /// </summary>
            STATUS_NO_MORE_MATCHES = 0xc0000273,

            /// <summary> 
            /// NTFS ファイルまたはディレクトリは再解析ポイントではありません。
            /// </summary>
            STATUS_NOT_A_REPARSE_POINT = 0xc0000275,

            /// <summary> 
            /// NTFS 再解析ポイントに渡された Windows I/O 再解析タグが無効です。
            /// </summary>
            STATUS_IO_REPARSE_TAG_INVALID = 0xc0000276,

            /// <summary> 
            /// Windows I/O 再解析タグは NTFS 再解析ポイントに存在するものと一致しません。
            /// </summary>
            STATUS_IO_REPARSE_TAG_MISMATCH = 0xc0000277,

            /// <summary> 
            /// NTFS 再解析ポイントに渡されたユーザー データが無効です。
            /// </summary>
            STATUS_IO_REPARSE_DATA_INVALID = 0xc0000278,

            /// <summary> 
            /// この IO タグのための複数層のファイル システム ドライバは必要なときにタグを処理しませんでした。
            /// </summary>
            STATUS_IO_REPARSE_TAG_NOT_HANDLED = 0xc0000279,

            /// <summary> 
            /// 最初のファイル名は有効ですが、 NTFS シンボリック リンクを解決できませんでした。
            /// </summary>
            STATUS_REPARSE_POINT_NOT_RESOLVED = 0xc0000280,

            /// <summary> 
            /// NTFS ディレクトリは再解析ポイントです。
            /// </summary>
            STATUS_DIRECTORY_IS_A_REPARSE_POINT = 0xc0000281,

            /// <summary> 
            /// 競合しているため、範囲一覧に範囲を追加できませんでした。
            /// </summary>
            STATUS_RANGE_LIST_CONFLICT = 0xc0000282,

            /// <summary> 
            /// 指定されたメディア チェンジャのソースにメディアが入っていません。
            /// </summary>
            STATUS_SOURCE_ELEMENT_EMPTY = 0xc0000283,

            /// <summary> 
            /// 指定されたメディア チャンジャ先には既にメディアが入っています。
            /// </summary>
            STATUS_DESTINATION_ELEMENT_FULL = 0xc0000284,

            /// <summary> 
            /// 指定されたメディア チェンジャが存在しません。
            /// </summary>
            STATUS_ILLEGAL_ELEMENT_ADDRESS = 0xc0000285,

            /// <summary> 
            /// 指定された要素は既に存在しないマガジン内に含まれています。
            /// </summary>
            STATUS_MAGAZINE_NOT_PRESENT = 0xc0000286,

            /// <summary> 
            /// ハードウェア エラーのためデバイスは再初期化が必要です。
            /// </summary>
            STATUS_REINITIALIZATION_NEEDED = 0xc0000287,

            /// <summary> 
            /// クリーニングが必要であるとデバイスが示しています。
            /// </summary>
            STATUS_DEVICE_REQUIRES_CLEANING = 0x80000288,

            /// <summary> 
            /// ドアが開いているとデバイスが示しています。今後の操作はドアを閉めてから行ってください。
            /// </summary>
            STATUS_DEVICE_DOOR_OPEN = 0x80000289,

            /// <summary> 
            /// ファイルの暗号化に失敗しました。
            /// </summary>
            STATUS_ENCRYPTION_FAILED = 0xc000028a,

            /// <summary> 
            /// ファイルの解読に失敗しました。
            /// </summary>
            STATUS_DECRYPTION_FAILED = 0xc000028b,

            /// <summary> 
            /// 指定された範囲が範囲一覧で見つかりませんでした。
            /// </summary>
            STATUS_RANGE_NOT_FOUND = 0xc000028c,

            /// <summary> 
            /// このシステムのために構成された暗号化の回復ポリシーがありません。
            /// </summary>
            STATUS_NO_RECOVERY_POLICY = 0xc000028d,

            /// <summary> 
            /// 必要な暗号化ドライバがこのシステムに読み込まれていません。
            /// </summary>
            STATUS_NO_EFS = 0xc000028e,

            /// <summary> 
            /// 現在読み込まれているドライバとは別の暗号化ドライバでファイルが暗号化されていました。
            /// </summary>
            STATUS_WRONG_EFS = 0xc000028f,

            /// <summary> 
            /// ユーザー用に定義された EFS キーがありません。
            /// </summary>
            STATUS_NO_USER_KEYS = 0xc0000290,

            /// <summary> 
            /// 指定されたファイルは暗号化されていません。
            /// </summary>
            STATUS_FILE_NOT_ENCRYPTED = 0xc0000291,

            /// <summary> 
            /// 指定されたファイルは定義された EFS エクスポート形式ではありません。
            /// </summary>
            STATUS_NOT_EXPORT_FORMAT = 0xc0000292,

            /// <summary> 
            /// 指定されたファイルは暗号化されており、ユーザーは暗号解読することができません。
            /// </summary>
            STATUS_FILE_ENCRYPTED = 0xc0000293,

            /// <summary> 
            /// システムが起動しました。
            /// </summary>
            STATUS_WAKE_SYSTEM = 0x40000294,

            /// <summary> 
            /// 渡された guid は WMI データ プロバイダによって有効なものとして認識されませんでした。
            /// </summary>
            STATUS_WMI_GUID_NOT_FOUND = 0xc0000295,

            /// <summary> 
            /// 渡されたインスタンス名は WMI データ プロバイダによって有効なものとして認識されませんでした。
            /// </summary>
            STATUS_WMI_INSTANCE_NOT_FOUND = 0xc0000296,

            /// <summary> 
            /// 渡されたデータ項目 ID は WMI データ プロバイダによって有効なものとして認識されませんでした。
            /// </summary>
            STATUS_WMI_ITEMID_NOT_FOUND = 0xc0000297,

            /// <summary> 
            /// WMI 要求を完了できませんでした。もう一度やり直してください。
            /// </summary>
            STATUS_WMI_TRY_AGAIN = 0xc0000298,

            /// <summary> 
            /// ポリシー オブジェクトが共有されているので、ルートでのみ変更できます。
            /// </summary>
            STATUS_SHARED_POLICY = 0xc0000299,

            /// <summary> 
            /// ポリシー オブジェクトは存在しなければならないときに存在していません。
            /// </summary>
            STATUS_POLICY_OBJECT_NOT_FOUND = 0xc000029a,

            /// <summary> 
            /// 要求されたポリシー情報は Ds のみにあります。
            /// </summary>
            STATUS_POLICY_ONLY_IN_DS = 0xc000029b,

            /// <summary> 
            /// この機能を有効にするには、ボリュームをアップグレードする必要があります。
            /// </summary>
            STATUS_VOLUME_NOT_UPGRADED = 0xc000029c,

            /// <summary> 
            /// 現在、リモート記憶域サービスは使用可能ではありません。
            /// </summary>
            STATUS_REMOTE_STORAGE_NOT_ACTIVE = 0xc000029d,

            /// <summary> 
            /// リモート記憶域サービスでメディア エラーが発生しました。
            /// </summary>
            STATUS_REMOTE_STORAGE_MEDIA_ERROR = 0xc000029e,

            /// <summary> 
            /// トラッキング (ワークステーション) サービスは、実行されていません。
            /// </summary>
            STATUS_NO_TRACKING_SERVICE = 0xc000029f,

            /// <summary> 
            /// サーバー プロセスは、クライアントが必要なものとは異なる SID のもとで実行されています。
            /// </summary>
            STATUS_SERVER_SID_MISMATCH = 0xc00002a0,

            /// <summary> 
            /// 指定したディレクトリ サービスの属性または値が存在しません。
            /// </summary>
            STATUS_DS_NO_ATTRIBUTE_OR_VALUE = 0xc00002a1,

            /// <summary> 
            /// ディレクトリ サービスに指定された属性構文は無効です。
            /// </summary>
            STATUS_DS_INVALID_ATTRIBUTE_SYNTAX = 0xc00002a2,

            /// <summary> 
            /// ディレクトリ サービスに指定された属性の種類は、定義されていません。
            /// </summary>
            STATUS_DS_ATTRIBUTE_TYPE_UNDEFINED = 0xc00002a3,

            /// <summary> 
            /// 指定されたディレクトリ サービス属性または値は、既に存在します。
            /// </summary>
            STATUS_DS_ATTRIBUTE_OR_VALUE_EXISTS = 0xc00002a4,

            /// <summary> 
            /// ディレクトリ サービスは、ビジーです。
            /// </summary>
            STATUS_DS_BUSY = 0xc00002a5,

            /// <summary> 
            /// ディレクトリ サービスを利用できません。
            /// </summary>
            STATUS_DS_UNAVAILABLE = 0xc00002a6,

            /// <summary> 
            /// ディレクトリ サービスは、相対 ID を割り当てられませんでした。
            /// </summary>
            STATUS_DS_NO_RIDS_ALLOCATED = 0xc00002a7,

            /// <summary> 
            /// ディレクトリ サービスは、相対 ID のプールを使い果たしました。
            /// </summary>
            STATUS_DS_NO_MORE_RIDS = 0xc00002a8,

            /// <summary> 
            /// 要求された操作は、ディレクトリ サービスが、その種類の操作のマスタではないため、許可されませんでした。
            /// </summary>
            STATUS_DS_INCORRECT_ROLE_OWNER = 0xc00002a9,

            /// <summary> 
            /// ディレクトリ サービスは、相対 ID を割り当てるサブシステムを初期化できませんでした。
            /// </summary>
            STATUS_DS_RIDMGR_INIT_ERROR = 0xc00002aa,

            /// <summary> 
            /// 要求された操作は、オブジェトのクラスに関連付けられているいくつかの制約を満たしませんでした。
            /// </summary>
            STATUS_DS_OBJ_CLASS_VIOLATION = 0xc00002ab,

            /// <summary> 
            /// ディレクトリ サービスは、要求された操作をリーフ オブジェクトでのみ実行できます。
            /// </summary>
            STATUS_DS_CANT_ON_NON_LEAF = 0xc00002ac,

            /// <summary> 
            /// ディレクトリ サービスは、要求された操作を、オブジェクトの相対定義名 (RDN) 属性に対して実行できません。
            /// </summary>
            STATUS_DS_CANT_ON_RDN = 0xc00002ad,

            /// <summary> 
            /// ディレクトリ サービスは、オブジェクトのオブジェクト クラスを変更しようとしたことを検出しました。
            /// </summary>
            STATUS_DS_CANT_MOD_OBJ_CLASS = 0xc00002ae,

            /// <summary> 
            /// ドメインをまたがる移動操作の実行中にエラーが発生しました。
            /// </summary>
            STATUS_DS_CROSS_DOM_MOVE_FAILED = 0xc00002af,

            /// <summary> 
            /// グローバル カタログ サーバーに接続できません。
            /// </summary>
            STATUS_DS_GC_NOT_AVAILABLE = 0xc00002b0,

            /// <summary> 
            /// 要求した操作は、ディレクトリ サービスを必要としますが、どれも利用できません。
            /// </summary>
            STATUS_DIRECTORY_SERVICE_REQUIRED = 0xc00002b1,

            /// <summary> 
            /// 再解析属性は、既存の属性と互換性がないため、設定することができません。
            /// </summary>
            STATUS_REPARSE_ATTRIBUTE_CONFLICT = 0xc00002b2,

            /// <summary> 
            /// 拒否だけに使用するようにマークされたグループは、有効にできません。
            /// </summary>
            STATUS_CANT_ENABLE_DENY_ONLY = 0xc00002b3,

            /// <summary> 
            /// 複数浮動小数点エラーです。
            /// </summary>
            STATUS_FLOAT_MULTIPLE_FAULTS = 0xc00002b4,

            /// <summary> 
            /// 複数浮動小数点トラップです。
            /// </summary>
            STATUS_FLOAT_MULTIPLE_TRAPS = 0xc00002b5,

            /// <summary> 
            /// デバイスは削除されました。
            /// </summary>
            STATUS_DEVICE_REMOVED = 0xc00002b6,

            /// <summary> 
            /// ボリューム変更ジャーナルを削除しています。
            /// </summary>
            STATUS_JOURNAL_DELETE_IN_PROGRESS = 0xc00002b7,

            /// <summary> 
            /// ボリューム変更ジャーナルは、アクティブではありません。
            /// </summary>
            STATUS_JOURNAL_NOT_ACTIVE = 0xc00002b8,

            /// <summary> 
            /// 要求されたインターフェイスは、サポートされません。
            /// </summary>
            STATUS_NOINTERFACE = 0xc00002b9,

            /// <summary> 
            /// ディレクトリ サービス リソースは制限を超えました。
            /// </summary>
            STATUS_DS_ADMIN_LIMIT_EXCEEDED = 0xc00002c1,

            /// <summary> 
            /// ドライバはスタンバイ モードをサポートしません。このドライバを更新するとシステムがスタンバイ モードになることを許可する場合があります。
            /// </summary>
            STATUS_DRIVER_FAILED_SLEEP = 0xc00002c2,

            /// <summary> 
            /// 相互認証が失敗しました。ドメイン コントローラのサーバーのパスワードの有効期限が切れています。
            /// </summary>
            STATUS_MUTUAL_AUTHENTICATION_FAILED = 0xc00002c3,

            /// <summary> 
            /// STATUS_CORRUPT_SYSTEM_FILE
            /// </summary>
            STATUS_CORRUPT_SYSTEM_FILE = 0xc00002c4,

            /// <summary> 
            /// ロード命令または記憶命令でデータ型の不整列が検出されました。
            /// </summary>
            STATUS_DATATYPE_MISALIGNMENT_ERROR = 0xc00002c5,

            /// <summary> 
            /// WMI データ項目またはデータ ブロックは読み取り専用です。
            /// </summary>
            STATUS_WMI_READ_ONLY = 0xc00002c6,

            /// <summary> 
            /// WNI データ項目またはデータブロックは変更できません。
            /// </summary>
            STATUS_WMI_SET_FAILURE = 0xc00002c7,

            /// <summary> 
            /// システムの仮想メモリがなくなって来ています。仮想メモリ ページ ファイルのサイズを増やしています。
            /// この処理の間、いくつかのアプリケーションのメモリ要求が拒否されることがあります。詳細情報に関してはヘルプを参照してください。
            /// </summary>
            STATUS_COMMITMENT_MINIMUM = 0xc00002c8,

            /// <summary> 
            /// NaT 登録の消費障害が発生しました。
            /// NaT の値は不確実な命令に基づいて消費されています。
            /// </summary>
            STATUS_REG_NAT_CONSUMPTION = 0xc00002c9,

            /// <summary> 
            /// メディア チェンジャのトランスポートにメディアが含まれてるため操作が失敗しています。
            /// </summary>
            STATUS_TRANSPORT_FULL = 0xc00002ca,

            /// <summary> 
            /// セキュリティ アカウント マネージャの初期化に失敗しました。
            /// </summary>
            STATUS_DS_SAM_INIT_FAILURE = 0xc00002cb,

            /// <summary> 
            /// この操作はサーバーに接続されているときのみサポートされています。
            /// </summary>
            STATUS_ONLY_IF_CONNECTED = 0xc00002cc,

            /// <summary> 
            /// 管理者のみが Administrator グループのメンバシップの一覧を変更することができます。
            /// </summary>
            STATUS_DS_SENSITIVE_GROUP_VIOLATION = 0xc00002cd,

            /// <summary> 
            /// デバイスが削除されたため、問い合わせを再実行しなければなりません。
            /// </summary>
            STATUS_PNP_RESTART_ENUMERATION = 0xc00002ce,

            /// <summary> 
            /// ジャーナルのエントリがジャーナルから削除されました。
            /// </summary>
            STATUS_JOURNAL_ENTRY_DELETED = 0xc00002cf,

            /// <summary> 
            /// ドメイン コントローラ アカウントのプライマリ グループ ID を変更できません。
            /// </summary>
            STATUS_DS_CANT_MOD_PRIMARYGROUPID = 0xc00002d0,

            /// <summary> 
            /// システム イメージは正しく署名されていません。
            /// ファイルが署名されたファイルと置き換えられています。
            /// システムがシャットダウンされています。
            /// </summary>
            STATUS_SYSTEM_IMAGE_BAD_SIGNATURE = 0xc00002d1,

            /// <summary> 
            /// デバイスは再起動しないと開始されません。
            /// </summary>
            STATUS_PNP_REBOOT_REQUIRED = 0xc00002d2,

            /// <summary> 
            /// デバイスの現在の電源状態では、この要求をサポートできません。
            /// </summary>
            STATUS_POWER_STATE_INVALID = 0xc00002d3,

            /// <summary> 
            /// 指定されたグループの種類が無効です。
            /// </summary>
            STATUS_DS_INVALID_GROUP_TYPE = 0xc00002d4,

            /// <summary> 
            /// 混在したドメインでは、グループのセキュリティが有効の場合にグローバル グループを入れ子にすることはできません。
            /// </summary>
            STATUS_DS_NO_NEST_GLOBALGROUP_IN_MIXEDDOMAIN = 0xc00002d5,

            /// <summary> 
            /// 混在したドメインでは、グループのセキュリティが有効の場合にローカル グループをほかのローカル グループと入れ子にすることはできません。
            /// </summary>
            STATUS_DS_NO_NEST_LOCALGROUP_IN_MIXEDDOMAIN = 0xc00002d6,

            /// <summary> 
            /// グローバル グループはローカル グループをメンバに含むことはできません。
            /// </summary>
            STATUS_DS_GLOBAL_CANT_HAVE_LOCAL_MEMBER = 0xc00002d7,

            /// <summary> 
            /// グローバル グループはユニバーサル グループをメンバに含むことはできません。
            /// </summary>
            STATUS_DS_GLOBAL_CANT_HAVE_UNIVERSAL_MEMBER = 0xc00002d8,

            /// <summary> 
            /// ユニバーサル グループはローカル グループをメンバに含むことはできません。
            /// </summary>
            STATUS_DS_UNIVERSAL_CANT_HAVE_LOCAL_MEMBER = 0xc00002d9,

            /// <summary> 
            /// グローバル グループはドメインを越えたメンバを含むことはできません。
            /// </summary>
            STATUS_DS_GLOBAL_CANT_HAVE_CROSSDOMAIN_MEMBER = 0xc00002da,

            /// <summary> 
            /// ローカル グループはドメインを越えた別のローカル グループをメンバに含むことはできません。
            /// </summary>
            STATUS_DS_LOCAL_CANT_HAVE_CROSSDOMAIN_LOCAL_MEMBER = 0xc00002db,

            /// <summary> 
            /// このグループにプライマリ メンバが含まれているため、セキュリティが無効のグループには変更できません。
            /// </summary>
            STATUS_DS_HAVE_PRIMARY_MEMBERS = 0xc00002dc,

            /// <summary> 
            /// WMI 操作はデータ ブロックまたはメソッドによってサポートされていません。
            /// </summary>
            STATUS_WMI_NOT_SUPPORTED = 0xc00002dd,

            /// <summary> 
            /// 要求された操作を完了するのに必要な電力がありません。
            /// </summary>
            STATUS_INSUFFICIENT_POWER = 0xc00002de,

            /// <summary> 
            /// セキュリティ アカウント マネージャはブート パスワードが必要です。
            /// </summary>
            STATUS_SAM_NEED_BOOTKEY_PASSWORD = 0xc00002df,

            /// <summary> 
            /// セキュリティ アカウント マネージャはフロッピー ディスクからブート キーが必要です。
            /// </summary>
            STATUS_SAM_NEED_BOOTKEY_FLOPPY = 0xc00002e0,

            /// <summary> 
            /// ディレクトリ サービスを開始できません。
            /// </summary>
            STATUS_DS_CANT_START = 0xc00002e1,

            /// <summary> 
            /// ディレクトリ サービスを開始できませんでした。
            /// </summary>
            STATUS_DS_INIT_FAILURE = 0xc00002e2,

            /// <summary> 
            /// セキュリティ アカウント マネージャを初期化できませんでした。
            /// </summary>
            STATUS_SAM_INIT_FAILURE = 0xc00002e3,

            /// <summary> 
            /// 要求された操作はグローバル カタログ サーバー上のみで実行されます。
            /// </summary>
            STATUS_DS_GC_REQUIRED = 0xc00002e4,

            /// <summary> 
            /// ローカル グループは同じドメインでほかのローカル グループのメンバのみになれます。
            /// </summary>
            STATUS_DS_LOCAL_MEMBER_OF_LOCAL_ONLY = 0xc00002e5,

            /// <summary> 
            /// 外部のセキュリティ プリンシパルをユニバーサル グループのメンバにできません。
            /// </summary>
            STATUS_DS_NO_FPO_IN_UNIVERSAL_GROUPS = 0xc00002e6,

            /// <summary> 
            /// コンピュータをドメインに参加できませんでした。このドメインに作成できるコンピュータ アカウントの最大数を超えています。システム管理者に問い合わせて制限をリセットするか、または増やしてください。
            /// </summary>
            STATUS_DS_MACHINE_ACCOUNT_QUOTA_EXCEEDED = 0xc00002e7,

            /// <summary> 
            /// STATUS_MULTIPLE_FAULT_VIOLATION
            /// </summary>
            STATUS_MULTIPLE_FAULT_VIOLATION = 0xc00002e8,

            /// <summary> 
            /// この操作は現在のドメインで実行できません。
            /// </summary>
            STATUS_CURRENT_DOMAIN_NOT_ALLOWED = 0xc00002e9,

            /// <summary> 
            /// ディレクトリまたはファイルを作成できません。
            /// </summary>
            STATUS_CANNOT_MAKE = 0xc00002ea,

            /// <summary> 
            /// シャットダウン中です。
            /// </summary>
            STATUS_SYSTEM_SHUTDOWN = 0xc00002eb,

            /// <summary> 
            /// ディレクトリ サービスを開始できませんでした。
            /// </summary>
            STATUS_DS_INIT_FAILURE_CONSOLE = 0xc00002ec,

            /// <summary> 
            /// セキュリティ アカウント マネージャの初期化に失敗しました。
            /// </summary>
            STATUS_DS_SAM_INIT_FAILURE_CONSOLE = 0xc00002ed,

            /// <summary> 
            /// セキュリティ コンテキストは完了前に削除されました。これはログオン エラーとみなされます。
            /// </summary>
            STATUS_UNFINISHED_CONTEXT_DELETED = 0xc00002ee,

            /// <summary> 
            /// クライアントがコンテキストをネゴシエートしようとし、サーバーがユーザー対ユーザーを要求していますが、サーバーが TGT 応答を送信しませんでした。
            /// </summary>
            STATUS_NO_TGT_REPLY = 0xc00002ef,

            /// <summary> 
            /// このファイルでオブジェクト ID が見つかりませんでした。
            /// </summary>
            STATUS_OBJECTID_NOT_FOUND = 0xc00002f0,

            /// <summary> 
            /// ローカル コンピュータに IP アドレスがないため、要求されたタスクを完了できません。
            /// </summary>
            STATUS_NO_IP_ADDRESSES = 0xc00002f1,

            /// <summary> 
            /// 指定された資格情報ハンドルはセキュリティ コンテキストに関連付けられた資格情報と一致しません。
            /// </summary>
            STATUS_WRONG_CREDENTIAL_HANDLE = 0xc00002f2,

            /// <summary> 
            /// 要求された関数が利用できないため、crypto システムまたはチェックサム関数が無効です。
            /// </summary>
            STATUS_CRYPTO_SYSTEM_INVALID = 0xc00002f3,

            /// <summary> 
            /// チケットの紹介の最大数を超えました。
            /// </summary>
            STATUS_MAX_REFERRALS_EXCEEDED = 0xc00002f4,

            /// <summary> 
            /// ローカルコンピュータは Kerberos KDC (ドメイン コントローラ) でなければなりませんが、Kerberos KDC ではありません。
            /// </summary>
            STATUS_MUST_BE_KDC = 0xc00002f5,

            /// <summary> 
            /// 相手側のセキュリティ ネゴシエーションでは強力な crypto が必要ですが、ローカル コンピュータではサポートされていません。
            /// </summary>
            STATUS_STRONG_CRYPTO_NOT_SUPPORTED = 0xc00002f6,

            /// <summary> 
            /// KDC からの返答にプリンシパル名が複数含まれています。
            /// </summary>
            STATUS_TOO_MANY_PRINCIPALS = 0xc00002f7,

            /// <summary> 
            /// 使用する etype のヒントとして PA データが検出されることを予期していましたが、見つかりませんでした。
            /// </summary>
            STATUS_NO_PA_DATA = 0xc00002f8,

            /// <summary> 
            /// クライアントの証明書名がユーザー名と一致しないか、または KDC 名が間違っています。
            /// </summary>
            STATUS_PKINIT_NAME_MISMATCH = 0xc00002f9,

            /// <summary> 
            /// スマート カード ログオンが必要ですが、使用されませんでした。
            /// </summary>
            STATUS_SMARTCARD_LOGON_REQUIRED = 0xc00002fa,

            /// <summary> 
            /// 無効な要求が KDC に送信されました。
            /// </summary>
            STATUS_KDC_INVALID_REQUEST = 0xc00002fb,

            /// <summary> 
            /// KDC は要求されたサービスのための紹介を生成できませんでした。
            /// </summary>
            STATUS_KDC_UNABLE_TO_REFER = 0xc00002fc,

            /// <summary> 
            /// 要求された暗号化の種類は KDC によってサポートされていません。
            /// </summary>
            STATUS_KDC_UNKNOWN_ETYPE = 0xc00002fd,

            /// <summary> 
            /// システム シャットダウンが実行中です。
            /// </summary>
            STATUS_SHUTDOWN_IN_PROGRESS = 0xc00002fe,

            /// <summary> 
            /// サーバー コンピュータをシャットダウンしています。
            /// </summary>
            STATUS_SERVER_SHUTDOWN_IN_PROGRESS = 0xc00002ff,

            /// <summary> 
            /// この操作は Microsoft Small Business Server ではサポートされていません。
            /// </summary>
            STATUS_NOT_SUPPORTED_ON_SBS = 0xc0000300,

            /// <summary> 
            /// WMI GUID を利用できません。
            /// </summary>
            STATUS_WMI_GUID_DISCONNECTED = 0xc0000301,

            /// <summary> 
            /// WMI GUID のコレクションまたはイベントは既に無効になっています。
            /// </summary>
            STATUS_WMI_ALREADY_DISABLED = 0xc0000302,

            /// <summary> 
            /// WMI GUID のコレクションまたはイベントは既に有効になっています。
            /// </summary>
            STATUS_WMI_ALREADY_ENABLED = 0xc0000303,

            /// <summary> 
            /// このボリュームのマスタ ファイル テーブルは断片化されすぎているため、この操作を完了できません。
            /// </summary>
            STATUS_MFT_TOO_FRAGMENTED = 0xc0000304,

            /// <summary> 
            /// コピー防止エラーです。
            /// </summary>
            STATUS_COPY_PROTECTION_FAILURE = 0xc0000305,

            /// <summary> 
            /// コピー防止エラー - DVD CSS 認証に失敗しました。
            /// </summary>
            STATUS_CSS_AUTHENTICATION_FAILURE = 0xc0000306,

            /// <summary> 
            /// コピー防止エラー - 指定されたセクタには有効なキーがありません。
            /// </summary>
            STATUS_CSS_KEY_NOT_PRESENT = 0xc0000307,

            /// <summary> 
            /// コピー防止エラー - DVD セッション キーが確立されていません。
            /// </summary>
            STATUS_CSS_KEY_NOT_ESTABLISHED = 0xc0000308,

            /// <summary> 
            /// コピー防止エラー - セクタが暗号化されているため、読み取りに失敗しました。
            /// </summary>
            STATUS_CSS_SCRAMBLED_SECTOR = 0xc0000309,

            /// <summary> 
            /// コピー防止エラー - 指定された DVD の地域はドライブの地域設定に一致しません。
            /// </summary>
            STATUS_CSS_REGION_MISMATCH = 0xc000030a,

            /// <summary> 
            /// コピー防止エラー - ドライブの地域設定は変更できない可能性があります。
            /// </summary>
            STATUS_CSS_RESETS_EXHAUSTED = 0xc000030b,

            /// <summary> 
            /// スマート カードのログオン中に KDC 証明書を検証するときに、kerberos プロトコルによりエラーが検出されました。
            /// </summary>
            STATUS_PKINIT_FAILURE = 0xc0000320,

            /// <summary> 
            /// スマート カード サブシステムを利用するときに、kerberos プロトコルによりエラーが検出されました。
            /// </summary>
            STATUS_SMARTCARD_SUBSYSTEM_FAILURE = 0xc0000321,

            /// <summary> 
            /// 対象のサーバーには適切な kerberos 資格情報がありません。
            /// </summary>
            STATUS_NO_KERB_KEY = 0xc0000322,

            /// <summary> 
            /// トランスポートによりリモート システムがダウンしていることが判明しました。
            /// </summary>
            STATUS_HOST_DOWN = 0xc0000350,

            /// <summary> 
            /// サポートされていない事前認証機構が kerberos パッケージに提供されました。
            /// </summary>
            STATUS_UNSUPPORTED_PREAUTH = 0xc0000351,

            /// <summary> 
            /// 送り側ファイルで使用された暗号化アルゴリズムでは、受け側ファイルで使用されたものより大きいキー バッファが必要です。
            /// </summary>
            STATUS_EFS_ALG_BLOB_TOO_BIG = 0xc0000352,

            /// <summary> 
            /// プロセス DebugPort を削除しようとしましたが、ポートはプロセスに既に関連付けられていました。
            /// </summary>
            STATUS_PORT_NOT_SET = 0xc0000353,

            /// <summary> 
            /// このポートは削除中のため、デバッグ ポート上で操作を実行しようとしましたが失敗しました。
            /// </summary>
            STATUS_DEBUGGER_INACTIVE = 0xc0000354,

            /// <summary> 
            /// このバージョンの Windows はディレクトリ フォレスト、ドメインまたはドメイン コントローラの動作バージョンと互換性がありません。
            /// </summary>
            STATUS_DS_VERSION_CHECK_FAILURE = 0xc0000355,

            /// <summary> 
            /// 指定されたイベントは現在監査されていません。
            /// </summary>
            STATUS_AUDITING_DISABLED = 0xc0000356,

            /// <summary> 
            /// このコンピュータ アカウントは NT4 より前に作成されました。アカウントを作成し直す必要があります。
            /// </summary>
            STATUS_PRENT4_MACHINE_ACCOUNT = 0xc0000357,

            /// <summary> 
            /// アカウント グループはユニバーサル グループをメンバとして持つことはできません。
            /// </summary>
            STATUS_DS_AG_CANT_HAVE_UNIVERSAL_MEMBER = 0xc0000358,

            /// <summary> 
            /// 指定したイメージ ファイルは正しい形式でなく、32 ビットの Windows イメージです。
            /// </summary>
            STATUS_INVALID_IMAGE_WIN_32 = 0xc0000359,

            /// <summary> 
            /// 指定したイメージ ファイルは正しい形式でなく、64 ビットの Windows イメージです。
            /// </summary>
            STATUS_INVALID_IMAGE_WIN_64 = 0xc000035a,

            /// <summary> 
            /// クライアントによって提供された SSPI チャネル バインドが無効でした。
            /// </summary>
            STATUS_BAD_BINDINGS = 0xc000035b,

            /// <summary> 
            /// クライアントのセッションの期限が切れています。引き続きリモート リソースにアクセスするにはクライアントは再度認証する必要があります。
            /// </summary>
            STATUS_NETWORK_SESSION_EXPIRED = 0xc000035c,

            /// <summary> 
            /// AppHelp ダイアログが取り消されたため、アプリケーションを開始できませんでした。
            /// </summary>
            STATUS_APPHELP_BLOCK = 0xc000035d,

            /// <summary> 
            /// SID のフィルタ処理操作により、SID はすべて削除されました。
            /// </summary>
            STATUS_ALL_SIDS_FILTERED = 0xc000035e,

            /// <summary> 
            /// システムがセーフ モードで起動中のため、ドライバは読み込まれませんでした。
            /// </summary>
            STATUS_NOT_SAFE_MODE_DRIVER = 0xc000035f,

            /// <summary> 
            /// STATUS_ACCESS_DISABLED_BY_POLICY_DEFAULT
            /// </summary>
            STATUS_ACCESS_DISABLED_BY_POLICY_DEFAULT = 0xc0000361,

            /// <summary> 
            /// STATUS_ACCESS_DISABLED_BY_POLICY_PATH
            /// </summary>
            STATUS_ACCESS_DISABLED_BY_POLICY_PATH = 0xc0000362,

            /// <summary> 
            /// STATUS_ACCESS_DISABLED_BY_POLICY_PUBLISHER
            /// </summary>
            STATUS_ACCESS_DISABLED_BY_POLICY_PUBLISHER = 0xc0000363,

            /// <summary> 
            /// STATUS_ACCESS_DISABLED_BY_POLICY_OTHER
            /// </summary>
            STATUS_ACCESS_DISABLED_BY_POLICY_OTHER = 0xc0000364,

            /// <summary> 
            /// 初期化の呼び出しで失敗したため、ドライバは読み込まれませんでした。
            /// </summary>
            STATUS_FAILED_DRIVER_ENTRY = 0xc0000365,

            /// <summary> 
            /// 電源を適用しているときまたはデバイス構成を読み取っているときに、エラーが発生しました。
            /// これは、ハードウェア障害または不完全な接続によって発生した可能性があります。
            /// </summary>
            STATUS_DEVICE_ENUMERATION_ERROR = 0xc0000366,

            /// <summary> 
            /// oplock の待機中に操作はブロックされました。
            /// </summary>
            STATUS_WAIT_FOR_OPLOCK = 0x00000367,

            /// <summary> 
            /// この名前には、指定したデバイス オブジェクトが接続されていないボリュームに解決されているマウント ポイントが少なくとも 1 つ含まれているため、作成操作に失敗しました。
            /// </summary>
            STATUS_MOUNT_POINT_NOT_RESOLVED = 0xc0000368,

            /// <summary> 
            /// デバイス オブジェクトのパラメータが有効なデバイス オブジェクトでないか、またはファイル名で指定されたボリュームに接続されていません。
            /// </summary>
            STATUS_INVALID_DEVICE_OBJECT_PARAMETER = 0xc0000369,

            /// <summary> 
            /// Machine Check エラーが発生しました。システムのイベント ログで詳細情報を確認してください。
            /// </summary>
            STATUS_MCA_OCCURED = 0xc000036a,

            /// <summary> 
            /// STATUS_DRIVER_BLOCKED_CRITICAL
            /// </summary>
            STATUS_DRIVER_BLOCKED_CRITICAL = 0xc000036b,

            /// <summary> 
            /// STATUS_DRIVER_BLOCKED
            /// </summary>
            STATUS_DRIVER_BLOCKED = 0xc000036c,

            /// <summary> 
            /// STATUS_DRIVER_DATABASE_ERROR
            /// </summary>
            STATUS_DRIVER_DATABASE_ERROR = 0xc000036d,

            /// <summary> 
            /// システム ハイブのサイズが制限を超えました。
            /// </summary>
            STATUS_SYSTEM_HIVE_TOO_LARGE = 0xc000036e,

            /// <summary> 
            /// ダイナミック リンク ライブラリ (DLL) が、DLL でもプロセスの実行イメージでもないモジュールを参照しました。
            /// </summary>
            STATUS_INVALID_IMPORT_OF_NON_DLL = 0xc000036f,

            /// <summary> 
            /// ディレクトリ サービスをシャットダウンしています。
            /// </summary>
            STATUS_DS_SHUTTING_DOWN = 0x40000370,

            /// <summary> 
            /// STATUS_NO_SECRETS
            /// </summary>
            STATUS_NO_SECRETS = 0xc0000371,

            /// <summary> 
            /// STATUS_ACCESS_DISABLED_NO_SAFER_UI_BY_POLICY
            /// </summary>
            STATUS_ACCESS_DISABLED_NO_SAFER_UI_BY_POLICY = 0xc0000372,

            /// <summary> 
            /// STATUS_FAILED_STACK_SWITCH
            /// </summary>
            STATUS_FAILED_STACK_SWITCH = 0xc0000373,

            /// <summary> 
            /// STATUS_HEAP_CORRUPTION
            /// </summary>
            STATUS_HEAP_CORRUPTION = 0xc0000374,

            /// <summary> 
            /// スマート カードに正しくない PIN が提示されました。
            /// </summary>
            STATUS_SMARTCARD_WRONG_PIN = 0xc0000380,

            /// <summary> 
            /// スマート カードはブロックされています。
            /// </summary>
            STATUS_SMARTCARD_CARD_BLOCKED = 0xc0000381,

            /// <summary> 
            /// スマート カードに PIN が提示されませんでした。
            /// </summary>
            STATUS_SMARTCARD_CARD_NOT_AUTHENTICATED = 0xc0000382,

            /// <summary> 
            /// 利用できるスマート カードがありません。
            /// </summary>
            STATUS_SMARTCARD_NO_CARD = 0xc0000383,

            /// <summary> 
            /// スマート カードに要求されたキー コンテナが存在しません。
            /// </summary>
            STATUS_SMARTCARD_NO_KEY_CONTAINER = 0xc0000384,

            /// <summary> 
            /// 要求された証明書がスマート カードにありません。
            /// </summary>
            STATUS_SMARTCARD_NO_CERTIFICATE = 0xc0000385,

            /// <summary> 
            /// 要求されたキーがありません。
            /// </summary>
            STATUS_SMARTCARD_NO_KEYSET = 0xc0000386,

            /// <summary> 
            /// スマート カードの通信エラーが検出されました。
            /// </summary>
            STATUS_SMARTCARD_IO_ERROR = 0xc0000387,

            /// <summary> 
            /// セキュリティに危害を与える試みが検出されました。認証したサーバーに連絡してください。
            /// </summary>
            STATUS_DOWNGRADE_DETECTED = 0xc0000388,

            /// <summary> 
            /// 認証に使用されたスマート カード証明書は失効しています。
            /// システム管理者に問い合わせてください。イベント ログに追加情報がある場合があります。
            /// </summary>
            STATUS_SMARTCARD_CERT_REVOKED = 0xc0000389,

            /// <summary> 
            /// 認証に使用されたスマート カード証明書の処理中に、信頼されていない証明機関が検出されました。
            /// システム管理者に問い合わせてください。
            /// </summary>
            STATUS_ISSUING_CA_UNTRUSTED = 0xc000038a,

            /// <summary> 
            /// 認証に使用されたスマート カード証明書の失効化の状態を判断することができません。
            /// システム管理者に問い合わせてください。
            /// </summary>
            STATUS_REVOCATION_OFFLINE_C = 0xc000038b,

            /// <summary> 
            /// 認証に使用されたスマート カード証明書を信頼できませんでした。
            /// システム管理者に問い合わせてください。
            /// </summary>
            STATUS_PKINIT_CLIENT_FAILURE = 0xc000038c,

            /// <summary> 
            /// 認証に使用されたスマート カード証明書は有効期限が切れています。
            /// システム管理者に問い合わせてください。
            /// </summary>
            STATUS_SMARTCARD_CERT_EXPIRED = 0xc000038d,

            /// <summary> 
            /// 以前のバージョンのドライバがメモリに残っているため、ドライバを読み込むことができませんでした。
            /// </summary>
            STATUS_DRIVER_FAILED_PRIOR_UNLOAD = 0xc000038e,

            /// <summary> 
            /// STATUS_SMARTCARD_SILENT_CONTEXT
            /// </summary>
            STATUS_SMARTCARD_SILENT_CONTEXT = 0xc000038f,

            /// <summary> 
            /// STATUS_PER_USER_TRUST_QUOTA_EXCEEDED
            /// </summary>
            STATUS_PER_USER_TRUST_QUOTA_EXCEEDED = 0xc0000401,

            /// <summary> 
            /// STATUS_ALL_USER_TRUST_QUOTA_EXCEEDED
            /// </summary>
            STATUS_ALL_USER_TRUST_QUOTA_EXCEEDED = 0xc0000402,

            /// <summary> 
            /// STATUS_USER_DELETE_TRUST_QUOTA_EXCEEDED
            /// </summary>
            STATUS_USER_DELETE_TRUST_QUOTA_EXCEEDED = 0xc0000403,

            /// <summary> 
            /// STATUS_DS_NAME_NOT_UNIQUE
            /// </summary>
            STATUS_DS_NAME_NOT_UNIQUE = 0xc0000404,

            /// <summary> 
            /// STATUS_DS_DUPLICATE_ID_FOUND
            /// </summary>
            STATUS_DS_DUPLICATE_ID_FOUND = 0xc0000405,

            /// <summary> 
            /// STATUS_DS_GROUP_CONVERSION_ERROR
            /// </summary>
            STATUS_DS_GROUP_CONVERSION_ERROR = 0xc0000406,

            /// <summary> 
            /// STATUS_VOLSNAP_PREPARE_HIBERNATE
            /// </summary>
            STATUS_VOLSNAP_PREPARE_HIBERNATE = 0xc0000407,

            /// <summary> 
            /// STATUS_USER2USER_REQUIRED
            /// </summary>
            STATUS_USER2USER_REQUIRED = 0xc0000408,

            /// <summary> 
            /// STATUS_STACK_BUFFER_OVERRUN
            /// </summary>
            STATUS_STACK_BUFFER_OVERRUN = 0xc0000409,

            /// <summary> 
            /// STATUS_NO_S4U_PROT_SUPPORT
            /// </summary>
            STATUS_NO_S4U_PROT_SUPPORT = 0xc000040a,

            /// <summary> 
            /// STATUS_CROSSREALM_DELEGATION_FAILURE
            /// </summary>
            STATUS_CROSSREALM_DELEGATION_FAILURE = 0xc000040b,

            /// <summary> 
            /// STATUS_REVOCATION_OFFLINE_KDC
            /// </summary>
            STATUS_REVOCATION_OFFLINE_KDC = 0xc000040c,

            /// <summary> 
            /// STATUS_ISSUING_CA_UNTRUSTED_KDC
            /// </summary>
            STATUS_ISSUING_CA_UNTRUSTED_KDC = 0xc000040d,

            /// <summary> 
            /// STATUS_KDC_CERT_EXPIRED
            /// </summary>
            STATUS_KDC_CERT_EXPIRED = 0xc000040e,

            /// <summary> 
            /// STATUS_KDC_CERT_REVOKED
            /// </summary>
            STATUS_KDC_CERT_REVOKED = 0xc000040f,

            /// <summary> 
            /// STATUS_PARAMETER_QUOTA_EXCEEDED
            /// </summary>
            STATUS_PARAMETER_QUOTA_EXCEEDED = 0xc0000410,

            /// <summary> 
            /// STATUS_HIBERNATION_FAILURE
            /// </summary>
            STATUS_HIBERNATION_FAILURE = 0xc0000411,

            /// <summary> 
            /// STATUS_DELAY_LOAD_FAILED
            /// </summary>
            STATUS_DELAY_LOAD_FAILED = 0xc0000412,

            /// <summary> 
            /// STATUS_AUTHENTICATION_FIREWALL_FAILED
            /// </summary>
            STATUS_AUTHENTICATION_FIREWALL_FAILED = 0xc0000413,

            /// <summary> 
            /// STATUS_VDM_DISALLOWED
            /// </summary>
            STATUS_VDM_DISALLOWED = 0xc0000414,

            /// <summary> 
            /// STATUS_HUNG_DISPLAY_DRIVER_THREAD
            /// </summary>
            STATUS_HUNG_DISPLAY_DRIVER_THREAD = 0xc0000415,

            /// <summary> 
            /// STATUS_INSUFFICIENT_RESOURCE_FOR_SPECIFIED_SHARED_SECTION_SIZE
            /// </summary>
            STATUS_INSUFFICIENT_RESOURCE_FOR_SPECIFIED_SHARED_SECTION_SIZE = 0xc0000416,

            /// <summary> 
            /// STATUS_INVALID_CRUNTIME_PARAMETER
            /// </summary>
            STATUS_INVALID_CRUNTIME_PARAMETER = 0xc0000417,

            /// <summary> 
            /// STATUS_NTLM_BLOCKED
            /// </summary>
            STATUS_NTLM_BLOCKED = 0xc0000418,

            /// <summary> 
            /// STATUS_ASSERTION_FAILURE
            /// </summary>
            STATUS_ASSERTION_FAILURE = 0xc0000420,

            /// <summary> 
            /// STATUS_VERIFIER_STOP
            /// </summary>
            STATUS_VERIFIER_STOP = 0xc0000421,

            /// <summary> 
            /// STATUS_CALLBACK_POP_STACK
            /// </summary>
            STATUS_CALLBACK_POP_STACK = 0xc0000423,

            /// <summary> 
            /// STATUS_INCOMPATIBLE_DRIVER_BLOCKED
            /// </summary>
            STATUS_INCOMPATIBLE_DRIVER_BLOCKED = 0xc0000424,

            /// <summary> 
            /// STATUS_HIVE_UNLOADED
            /// </summary>
            STATUS_HIVE_UNLOADED = 0xc0000425,

            /// <summary> 
            /// STATUS_COMPRESSION_DISABLED
            /// </summary>
            STATUS_COMPRESSION_DISABLED = 0xc0000426,

            /// <summary> 
            /// STATUS_FILE_SYSTEM_LIMITATION
            /// </summary>
            STATUS_FILE_SYSTEM_LIMITATION = 0xc0000427,

            /// <summary> 
            /// STATUS_INVALID_IMAGE_HASH
            /// </summary>
            STATUS_INVALID_IMAGE_HASH = 0xc0000428,

            /// <summary> 
            /// STATUS_NOT_CAPABLE
            /// </summary>
            STATUS_NOT_CAPABLE = 0xc0000429,

            /// <summary> 
            /// STATUS_REQUEST_OUT_OF_SEQUENCE
            /// </summary>
            STATUS_REQUEST_OUT_OF_SEQUENCE = 0xc000042a,

            /// <summary> 
            /// STATUS_IMPLEMENTATION_LIMIT
            /// </summary>
            STATUS_IMPLEMENTATION_LIMIT = 0xc000042b,

            /// <summary> 
            /// STATUS_ELEVATION_REQUIRED
            /// </summary>
            STATUS_ELEVATION_REQUIRED = 0xc000042c,

            /// <summary> 
            /// STATUS_BEYOND_VDL
            /// </summary>
            STATUS_BEYOND_VDL = 0xc0000432,

            /// <summary> 
            /// STATUS_ENCOUNTERED_WRITE_IN_PROGRESS
            /// </summary>
            STATUS_ENCOUNTERED_WRITE_IN_PROGRESS = 0xc0000433,

            /// <summary> 
            /// STATUS_PTE_CHANGED
            /// </summary>
            STATUS_PTE_CHANGED = 0xc0000434,

            /// <summary> 
            /// STATUS_PURGE_FAILED
            /// </summary>
            STATUS_PURGE_FAILED = 0xc0000435,

            /// <summary> 
            /// STATUS_CRED_REQUIRES_CONFIRMATION
            /// </summary>
            STATUS_CRED_REQUIRES_CONFIRMATION = 0xc0000440,

            /// <summary> 
            /// STATUS_CS_ENCRYPTION_INVALID_SERVER_RESPONSE
            /// </summary>
            STATUS_CS_ENCRYPTION_INVALID_SERVER_RESPONSE = 0xc0000441,

            /// <summary> 
            /// STATUS_CS_ENCRYPTION_UNSUPPORTED_SERVER
            /// </summary>
            STATUS_CS_ENCRYPTION_UNSUPPORTED_SERVER = 0xc0000442,

            /// <summary> 
            /// STATUS_CS_ENCRYPTION_EXISTING_ENCRYPTED_FILE
            /// </summary>
            STATUS_CS_ENCRYPTION_EXISTING_ENCRYPTED_FILE = 0xc0000443,

            /// <summary> 
            /// STATUS_CS_ENCRYPTION_NEW_ENCRYPTED_FILE
            /// </summary>
            STATUS_CS_ENCRYPTION_NEW_ENCRYPTED_FILE = 0xc0000444,

            /// <summary> 
            /// STATUS_CS_ENCRYPTION_FILE_NOT_CSE
            /// </summary>
            STATUS_CS_ENCRYPTION_FILE_NOT_CSE = 0xc0000445,

            /// <summary> 
            /// STATUS_INVALID_LABEL
            /// </summary>
            STATUS_INVALID_LABEL = 0xc0000446,

            /// <summary> 
            /// STATUS_DRIVER_PROCESS_TERMINATED
            /// </summary>
            STATUS_DRIVER_PROCESS_TERMINATED = 0xc0000450,

            /// <summary> 
            /// STATUS_AMBIGUOUS_SYSTEM_DEVICE
            /// </summary>
            STATUS_AMBIGUOUS_SYSTEM_DEVICE = 0xc0000451,

            /// <summary> 
            /// STATUS_SYSTEM_DEVICE_NOT_FOUND
            /// </summary>
            STATUS_SYSTEM_DEVICE_NOT_FOUND = 0xc0000452,

            /// <summary> 
            /// STATUS_RESTART_BOOT_APPLICATION
            /// </summary>
            STATUS_RESTART_BOOT_APPLICATION = 0xc0000453,

            /// <summary> 
            /// STATUS_INVALID_TASK_NAME
            /// </summary>
            STATUS_INVALID_TASK_NAME = 0xc0000500,

            /// <summary> 
            /// STATUS_INVALID_TASK_INDEX
            /// </summary>
            STATUS_INVALID_TASK_INDEX = 0xc0000501,

            /// <summary> 
            /// STATUS_THREAD_ALREADY_IN_TASK
            /// </summary>
            STATUS_THREAD_ALREADY_IN_TASK = 0xc0000502,

            /// <summary> 
            /// STATUS_CALLBACK_BYPASS
            /// </summary>
            STATUS_CALLBACK_BYPASS = 0xc0000503,

            /// <summary> 
            /// STATUS_PORT_CLOSED
            /// </summary>
            STATUS_PORT_CLOSED = 0xc0000700,

            /// <summary> 
            /// STATUS_MESSAGE_LOST
            /// </summary>
            STATUS_MESSAGE_LOST = 0xc0000701,

            /// <summary> 
            /// STATUS_INVALID_MESSAGE
            /// </summary>
            STATUS_INVALID_MESSAGE = 0xc0000702,

            /// <summary> 
            /// STATUS_REQUEST_CANCELED
            /// </summary>
            STATUS_REQUEST_CANCELED = 0xc0000703,

            /// <summary> 
            /// STATUS_RECURSIVE_DISPATCH
            /// </summary>
            STATUS_RECURSIVE_DISPATCH = 0xc0000704,

            /// <summary> 
            /// STATUS_LPC_RECEIVE_BUFFER_EXPECTED
            /// </summary>
            STATUS_LPC_RECEIVE_BUFFER_EXPECTED = 0xc0000705,

            /// <summary> 
            /// STATUS_LPC_INVALID_CONNECTION_USAGE
            /// </summary>
            STATUS_LPC_INVALID_CONNECTION_USAGE = 0xc0000706,

            /// <summary> 
            /// STATUS_LPC_REQUESTS_NOT_ALLOWED
            /// </summary>
            STATUS_LPC_REQUESTS_NOT_ALLOWED = 0xc0000707,

            /// <summary> 
            /// STATUS_RESOURCE_IN_USE
            /// </summary>
            STATUS_RESOURCE_IN_USE = 0xc0000708,

            /// <summary> 
            /// STATUS_HARDWARE_MEMORY_ERROR
            /// </summary>
            STATUS_HARDWARE_MEMORY_ERROR = 0xc0000709,

            /// <summary> 
            /// STATUS_THREADPOOL_HANDLE_EXCEPTION
            /// </summary>
            STATUS_THREADPOOL_HANDLE_EXCEPTION = 0xc000070a,

            /// <summary> 
            /// STATUS_THREADPOOL_SET_EVENT_ON_COMPLETION_FAILED
            /// </summary>
            STATUS_THREADPOOL_SET_EVENT_ON_COMPLETION_FAILED = 0xc000070b,

            /// <summary> 
            /// STATUS_THREADPOOL_RELEASE_SEMAPHORE_ON_COMPLETION_FAILED
            /// </summary>
            STATUS_THREADPOOL_RELEASE_SEMAPHORE_ON_COMPLETION_FAILED = 0xc000070c,

            /// <summary> 
            /// STATUS_THREADPOOL_RELEASE_MUTEX_ON_COMPLETION_FAILED
            /// </summary>
            STATUS_THREADPOOL_RELEASE_MUTEX_ON_COMPLETION_FAILED = 0xc000070d,

            /// <summary> 
            /// STATUS_THREADPOOL_FREE_LIBRARY_ON_COMPLETION_FAILED
            /// </summary>
            STATUS_THREADPOOL_FREE_LIBRARY_ON_COMPLETION_FAILED = 0xc000070e,

            /// <summary> 
            /// STATUS_THREADPOOL_RELEASED_DURING_OPERATION
            /// </summary>
            STATUS_THREADPOOL_RELEASED_DURING_OPERATION = 0xc000070f,

            /// <summary> 
            /// STATUS_CALLBACK_RETURNED_WHILE_IMPERSONATING
            /// </summary>
            STATUS_CALLBACK_RETURNED_WHILE_IMPERSONATING = 0xc0000710,

            /// <summary> 
            /// STATUS_APC_RETURNED_WHILE_IMPERSONATING
            /// </summary>
            STATUS_APC_RETURNED_WHILE_IMPERSONATING = 0xc0000711,

            /// <summary> 
            /// STATUS_PROCESS_IS_PROTECTED
            /// </summary>
            STATUS_PROCESS_IS_PROTECTED = 0xc0000712,

            /// <summary> 
            /// STATUS_MCA_EXCEPTION
            /// </summary>
            STATUS_MCA_EXCEPTION = 0xc0000713,

            /// <summary> 
            /// STATUS_CERTIFICATE_MAPPING_NOT_UNIQUE
            /// </summary>
            STATUS_CERTIFICATE_MAPPING_NOT_UNIQUE = 0xc0000714,

            /// <summary> 
            /// STATUS_SYMLINK_CLASS_DISABLED
            /// </summary>
            STATUS_SYMLINK_CLASS_DISABLED = 0xc0000715,

            /// <summary> 
            /// STATUS_INVALID_IDN_NORMALIZATION
            /// </summary>
            STATUS_INVALID_IDN_NORMALIZATION = 0xc0000716,

            /// <summary> 
            /// STATUS_NO_UNICODE_TRANSLATION
            /// </summary>
            STATUS_NO_UNICODE_TRANSLATION = 0xc0000717,

            /// <summary> 
            /// STATUS_ALREADY_REGISTERED
            /// </summary>
            STATUS_ALREADY_REGISTERED = 0xc0000718,

            /// <summary> 
            /// STATUS_CONTEXT_MISMATCH
            /// </summary>
            STATUS_CONTEXT_MISMATCH = 0xc0000719,

            /// <summary> 
            /// STATUS_PORT_ALREADY_HAS_COMPLETION_LIST
            /// </summary>
            STATUS_PORT_ALREADY_HAS_COMPLETION_LIST = 0xc000071a,

            /// <summary> 
            /// STATUS_CALLBACK_RETURNED_THREAD_PRIORITY
            /// </summary>
            STATUS_CALLBACK_RETURNED_THREAD_PRIORITY = 0xc000071b,

            /// <summary> 
            /// STATUS_INVALID_THREAD
            /// </summary>
            STATUS_INVALID_THREAD = 0xc000071c,

            /// <summary> 
            /// STATUS_CALLBACK_RETURNED_TRANSACTION
            /// </summary>
            STATUS_CALLBACK_RETURNED_TRANSACTION = 0xc000071d,

            /// <summary> 
            /// STATUS_CALLBACK_RETURNED_LDR_LOCK
            /// </summary>
            STATUS_CALLBACK_RETURNED_LDR_LOCK = 0xc000071e,

            /// <summary> 
            /// STATUS_CALLBACK_RETURNED_LANG
            /// </summary>
            STATUS_CALLBACK_RETURNED_LANG = 0xc000071f,

            /// <summary> 
            /// STATUS_CALLBACK_RETURNED_PRI_BACK
            /// </summary>
            STATUS_CALLBACK_RETURNED_PRI_BACK = 0xc0000720,

            /// <summary> 
            /// STATUS_CALLBACK_RETURNED_THREAD_AFFINITY
            /// </summary>
            STATUS_CALLBACK_RETURNED_THREAD_AFFINITY = 0xc0000721,

            /// <summary> 
            /// STATUS_DISK_REPAIR_DISABLED
            /// </summary>
            STATUS_DISK_REPAIR_DISABLED = 0xc0000800,

            /// <summary> 
            /// STATUS_DS_DOMAIN_RENAME_IN_PROGRESS
            /// </summary>
            STATUS_DS_DOMAIN_RENAME_IN_PROGRESS = 0xc0000801,

            /// <summary> 
            /// STATUS_DISK_QUOTA_EXCEEDED
            /// </summary>
            STATUS_DISK_QUOTA_EXCEEDED = 0xc0000802,

            /// <summary> 
            /// STATUS_DATA_LOST_REPAIR
            /// </summary>
            STATUS_DATA_LOST_REPAIR = 0x80000803,

            /// <summary> 
            /// STATUS_CONTENT_BLOCKED
            /// </summary>
            STATUS_CONTENT_BLOCKED = 0xc0000804,

            /// <summary> 
            /// STATUS_BAD_CLUSTERS
            /// </summary>
            STATUS_BAD_CLUSTERS = 0xc0000805,

            /// <summary> 
            /// STATUS_VOLUME_DIRTY
            /// </summary>
            STATUS_VOLUME_DIRTY = 0xc0000806,

            /// <summary> 
            /// STATUS_FILE_CHECKED_OUT
            /// </summary>
            STATUS_FILE_CHECKED_OUT = 0xc0000901,

            /// <summary> 
            /// STATUS_CHECKOUT_REQUIRED
            /// </summary>
            STATUS_CHECKOUT_REQUIRED = 0xc0000902,

            /// <summary> 
            /// STATUS_BAD_FILE_TYPE
            /// </summary>
            STATUS_BAD_FILE_TYPE = 0xc0000903,

            /// <summary> 
            /// STATUS_FILE_TOO_LARGE
            /// </summary>
            STATUS_FILE_TOO_LARGE = 0xc0000904,

            /// <summary> 
            /// STATUS_FORMS_AUTH_REQUIRED
            /// </summary>
            STATUS_FORMS_AUTH_REQUIRED = 0xc0000905,

            /// <summary> 
            /// STATUS_VIRUS_INFECTED
            /// </summary>
            STATUS_VIRUS_INFECTED = 0xc0000906,

            /// <summary> 
            /// STATUS_VIRUS_DELETED
            /// </summary>
            STATUS_VIRUS_DELETED = 0xc0000907,

            /// <summary> 
            /// STATUS_BAD_MCFG_TABLE
            /// </summary>
            STATUS_BAD_MCFG_TABLE = 0xc0000908,

            /// <summary> 
            /// WOW アサーション エラーです。
            /// </summary>
            STATUS_WOW_ASSERTION = 0xc0009898,

            /// <summary> 
            /// STATUS_INVALID_SIGNATURE
            /// </summary>
            STATUS_INVALID_SIGNATURE = 0xc000a000,

            /// <summary> 
            /// STATUS_HMAC_NOT_SUPPORTED
            /// </summary>
            STATUS_HMAC_NOT_SUPPORTED = 0xc000a001,

            /// <summary> 
            /// STATUS_AUTH_TAG_MISMATCH
            /// </summary>
            STATUS_AUTH_TAG_MISMATCH = 0xc000a002,

            /// <summary> 
            /// STATUS_IPSEC_QUEUE_OVERFLOW
            /// </summary>
            STATUS_IPSEC_QUEUE_OVERFLOW = 0xc000a010,

            /// <summary> 
            /// STATUS_ND_QUEUE_OVERFLOW
            /// </summary>
            STATUS_ND_QUEUE_OVERFLOW = 0xc000a011,

            /// <summary> 
            /// STATUS_HOPLIMIT_EXCEEDED
            /// </summary>
            STATUS_HOPLIMIT_EXCEEDED = 0xc000a012,

            /// <summary> 
            /// STATUS_PROTOCOL_NOT_SUPPORTED
            /// </summary>
            STATUS_PROTOCOL_NOT_SUPPORTED = 0xc000a013,

            /// <summary> 
            /// STATUS_FASTPATH_REJECTED
            /// </summary>
            STATUS_FASTPATH_REJECTED = 0xc000a014,

            /// <summary> 
            /// STATUS_LOST_WRITEBEHIND_DATA_NETWORK_DISCONNECTED
            /// </summary>
            STATUS_LOST_WRITEBEHIND_DATA_NETWORK_DISCONNECTED = 0xc000a080,

            /// <summary> 
            /// STATUS_LOST_WRITEBEHIND_DATA_NETWORK_SERVER_ERROR
            /// </summary>
            STATUS_LOST_WRITEBEHIND_DATA_NETWORK_SERVER_ERROR = 0xc000a081,

            /// <summary> 
            /// STATUS_LOST_WRITEBEHIND_DATA_LOCAL_DISK_ERROR
            /// </summary>
            STATUS_LOST_WRITEBEHIND_DATA_LOCAL_DISK_ERROR = 0xc000a082,

            /// <summary> 
            /// STATUS_XML_PARSE_ERROR
            /// </summary>
            STATUS_XML_PARSE_ERROR = 0xc000a083,

            /// <summary> 
            /// STATUS_XMLDSIG_ERROR
            /// </summary>
            STATUS_XMLDSIG_ERROR = 0xc000a084,

            /// <summary> 
            /// STATUS_WRONG_COMPARTMENT
            /// </summary>
            STATUS_WRONG_COMPARTMENT = 0xc000a085,

            /// <summary> 
            /// STATUS_AUTHIP_FAILURE
            /// </summary>
            STATUS_AUTHIP_FAILURE = 0xc000a086,

            /// <summary> 
            /// デバッガは状態変更を実行しませんでした。
            /// </summary>
            DBG_NO_STATE_CHANGE = 0xc0010001,

            /// <summary> 
            /// デバッガはアプリケーションがアイドルでないことを検出しました。
            /// </summary>
            DBG_APP_NOT_IDLE = 0xc0010002,

            /// <summary> 
            /// その文字列結合は無効です。
            /// </summary>
            RPC_NT_INVALID_STRING_BINDING = 0xc0020001,

            /// <summary> 
            /// 結合ハンドルの種類が誤っています。
            /// </summary>
            RPC_NT_WRONG_KIND_OF_BINDING = 0xc0020002,

            /// <summary> 
            /// 結合ハンドルが無効です。
            /// </summary>
            RPC_NT_INVALID_BINDING = 0xc0020003,

            /// <summary> 
            /// RPC プロトコル シーケンスはサポートされません。
            /// </summary>
            RPC_NT_PROTSEQ_NOT_SUPPORTED = 0xc0020004,

            /// <summary> 
            /// RPC プロトコル シーケンスが無効です。
            /// </summary>
            RPC_NT_INVALID_RPC_PROTSEQ = 0xc0020005,

            /// <summary> 
            /// 文字列 UUID が無効です。
            /// </summary>
            RPC_NT_INVALID_STRING_UUID = 0xc0020006,

            /// <summary> 
            /// エンドポイントの形式が無効です。
            /// </summary>
            RPC_NT_INVALID_ENDPOINT_FORMAT = 0xc0020007,

            /// <summary> 
            /// ネットワーク アドレスが無効です。
            /// </summary>
            RPC_NT_INVALID_NET_ADDR = 0xc0020008,

            /// <summary> 
            /// エンドポイントが見つかりませんでした。
            /// </summary>
            RPC_NT_NO_ENDPOINT_FOUND = 0xc0020009,

            /// <summary> 
            /// タイムアウト値が無効です。
            /// </summary>
            RPC_NT_INVALID_TIMEOUT = 0xc002000a,

            /// <summary> 
            /// オブジェクト UUID が見つかりませんでした。
            /// </summary>
            RPC_NT_OBJECT_NOT_FOUND = 0xc002000b,

            /// <summary> 
            /// オブジェクト UUID は既に登録されています。
            /// </summary>
            RPC_NT_ALREADY_REGISTERED = 0xc002000c,

            /// <summary> 
            /// タイプ UUID は既に登録されています。
            /// </summary>
            RPC_NT_TYPE_ALREADY_REGISTERED = 0xc002000d,

            /// <summary> 
            /// RPC サーバーは既にリッスン状態です。
            /// </summary>
            RPC_NT_ALREADY_LISTENING = 0xc002000e,

            /// <summary> 
            /// プロトコル シーケンスが登録されていません。
            /// </summary>
            RPC_NT_NO_PROTSEQS_REGISTERED = 0xc002000f,

            /// <summary> 
            /// RPC サーバーはリッスン状態ではありません。
            /// </summary>
            RPC_NT_NOT_LISTENING = 0xc0020010,

            /// <summary> 
            /// マネージャの種類を認識できません。
            /// </summary>
            RPC_NT_UNKNOWN_MGR_TYPE = 0xc0020011,

            /// <summary> 
            /// そのインターフェイスは認識されません。
            /// </summary>
            RPC_NT_UNKNOWN_IF = 0xc0020012,

            /// <summary> 
            /// 結合がありません。
            /// </summary>
            RPC_NT_NO_BINDINGS = 0xc0020013,

            /// <summary> 
            /// プロトコル シーケンスがありません。
            /// </summary>
            RPC_NT_NO_PROTSEQS = 0xc0020014,

            /// <summary> 
            /// エンドポイントを作成できません。
            /// </summary>
            RPC_NT_CANT_CREATE_ENDPOINT = 0xc0020015,

            /// <summary> 
            /// リソースが不足しているため、この操作を完了できません。
            /// </summary>
            RPC_NT_OUT_OF_RESOURCES = 0xc0020016,

            /// <summary> 
            /// RPC サーバーが使用不可能な状態です。
            /// </summary>
            RPC_NT_SERVER_UNAVAILABLE = 0xc0020017,

            /// <summary> 
            /// RPC サーバーが非常にビジーであるため、この操作を終了できません。
            /// </summary>
            RPC_NT_SERVER_TOO_BUSY = 0xc0020018,

            /// <summary> 
            /// ネットワーク オプションが無効です。
            /// </summary>
            RPC_NT_INVALID_NETWORK_OPTIONS = 0xc0020019,

            /// <summary> 
            /// このスレッドにアクティブなリモート プロシージャ コールはありません。
            /// </summary>
            RPC_NT_NO_CALL_ACTIVE = 0xc002001a,

            /// <summary> 
            /// リモート プロシージャ コールに失敗しました。
            /// </summary>
            RPC_NT_CALL_FAILED = 0xc002001b,

            /// <summary> 
            /// リモート プロシージャ コールに失敗し、実行されませんでした。
            /// </summary>
            RPC_NT_CALL_FAILED_DNE = 0xc002001c,

            /// <summary> 
            /// リモート プロシージャ コール (RPC) でプロトコル エラーが発生しました。
            /// </summary>
            RPC_NT_PROTOCOL_ERROR = 0xc002001d,

            /// <summary> 
            /// その転送構文は RPC サーバーでサポートされません。
            /// </summary>
            RPC_NT_UNSUPPORTED_TRANS_SYN = 0xc002001f,

            /// <summary> 
            /// タイプ UUID はサポートされません。
            /// </summary>
            RPC_NT_UNSUPPORTED_TYPE = 0xc0020021,

            /// <summary> 
            /// タグが無効です。
            /// </summary>
            RPC_NT_INVALID_TAG = 0xc0020022,

            /// <summary> 
            /// 配列の範囲が無効です。
            /// </summary>
            RPC_NT_INVALID_BOUND = 0xc0020023,

            /// <summary> 
            /// 結合にエントリ名が指定されていません。
            /// </summary>
            RPC_NT_NO_ENTRY_NAME = 0xc0020024,

            /// <summary> 
            /// 名前の構文が無効です。
            /// </summary>
            RPC_NT_INVALID_NAME_SYNTAX = 0xc0020025,

            /// <summary> 
            /// その名前の構文はサポートされません。
            /// </summary>
            RPC_NT_UNSUPPORTED_NAME_SYNTAX = 0xc0020026,

            /// <summary> 
            /// UUID を作成するために使用できるネットワーク アドレスがありません。
            /// </summary>
            RPC_NT_UUID_NO_ADDRESS = 0xc0020028,

            /// <summary> 
            /// そのエンドポイントは重複しています。
            /// </summary>
            RPC_NT_DUPLICATE_ENDPOINT = 0xc0020029,

            /// <summary> 
            /// 認証の種類が認識されません。
            /// </summary>
            RPC_NT_UNKNOWN_AUTHN_TYPE = 0xc002002a,

            /// <summary> 
            /// コールの最大数が小さすぎます。
            /// </summary>
            RPC_NT_MAX_CALLS_TOO_SMALL = 0xc002002b,

            /// <summary> 
            /// 文字列が長すぎます。
            /// </summary>
            RPC_NT_STRING_TOO_LONG = 0xc002002c,

            /// <summary> 
            /// RPC プロトコル シーケンスを見つけることができませんでした。
            /// </summary>
            RPC_NT_PROTSEQ_NOT_FOUND = 0xc002002d,

            /// <summary> 
            /// プロシージャ番号は範囲外です。
            /// </summary>
            RPC_NT_PROCNUM_OUT_OF_RANGE = 0xc002002e,

            /// <summary> 
            /// 結合に認証情報が指定されていません。
            /// </summary>
            RPC_NT_BINDING_HAS_NO_AUTH = 0xc002002f,

            /// <summary> 
            /// その認証サービスは認識されません。
            /// </summary>
            RPC_NT_UNKNOWN_AUTHN_SERVICE = 0xc0020030,

            /// <summary> 
            /// その認証レベルは認識されません。
            /// </summary>
            RPC_NT_UNKNOWN_AUTHN_LEVEL = 0xc0020031,

            /// <summary> 
            /// セキュリティ コンテキストが無効です。
            /// </summary>
            RPC_NT_INVALID_AUTH_IDENTITY = 0xc0020032,

            /// <summary> 
            /// その認証サービスは認識されません。
            /// </summary>
            RPC_NT_UNKNOWN_AUTHZ_SERVICE = 0xc0020033,

            /// <summary> 
            /// そのエントリは無効です。
            /// </summary>
            EPT_NT_INVALID_ENTRY = 0xc0020034,

            /// <summary> 
            /// 操作を実行できません。
            /// </summary>
            EPT_NT_CANT_PERFORM_OP = 0xc0020035,

            /// <summary> 
            /// エンドポイント マッパーから使用できるエンドポイントはこれ以上ありません。
            /// </summary>
            EPT_NT_NOT_REGISTERED = 0xc0020036,

            /// <summary> 
            /// インターフェイスはエクスポートされませんでした。
            /// </summary>
            RPC_NT_NOTHING_TO_EXPORT = 0xc0020037,

            /// <summary> 
            /// そのエントリ名は不完全です。
            /// </summary>
            RPC_NT_INCOMPLETE_NAME = 0xc0020038,

            /// <summary> 
            /// バージョン オプションが無効です。
            /// </summary>
            RPC_NT_INVALID_VERS_OPTION = 0xc0020039,

            /// <summary> 
            /// これ以上メンバはありません。
            /// </summary>
            RPC_NT_NO_MORE_MEMBERS = 0xc002003a,

            /// <summary> 
            /// アンエクスポートするものは何もありません。
            /// </summary>
            RPC_NT_NOT_ALL_OBJS_UNEXPORTED = 0xc002003b,

            /// <summary> 
            /// インターフェイスが見つかりませんでした。
            /// </summary>
            RPC_NT_INTERFACE_NOT_FOUND = 0xc002003c,

            /// <summary> 
            /// そのエントリは既に存在します。
            /// </summary>
            RPC_NT_ENTRY_ALREADY_EXISTS = 0xc002003d,

            /// <summary> 
            /// エントリが見つかりません。
            /// </summary>
            RPC_NT_ENTRY_NOT_FOUND = 0xc002003e,

            /// <summary> 
            /// ネーム サービスを利用できません。
            /// </summary>
            RPC_NT_NAME_SERVICE_UNAVAILABLE = 0xc002003f,

            /// <summary> 
            /// ネットワーク アドレス ファミリが無効です。
            /// </summary>
            RPC_NT_INVALID_NAF_ID = 0xc0020040,

            /// <summary> 
            /// 要求された操作はサポートされません。
            /// </summary>
            RPC_NT_CANNOT_SUPPORT = 0xc0020041,

            /// <summary> 
            /// 偽装を可能にするために使用できるセキュリティ コンテキストはありません。
            /// </summary>
            RPC_NT_NO_CONTEXT_AVAILABLE = 0xc0020042,

            /// <summary> 
            /// リモート プロシージャ コール (RPC) で内部エラーが発生しました。
            /// </summary>
            RPC_NT_INTERNAL_ERROR = 0xc0020043,

            /// <summary> 
            /// RPC サーバーで 0 による整数除算を実行しようとしました。
            /// </summary>
            RPC_NT_ZERO_DIVIDE = 0xc0020044,

            /// <summary> 
            /// アドレス指定エラーが RPC サーバーで発生しました。
            /// </summary>
            RPC_NT_ADDRESS_ERROR = 0xc0020045,

            /// <summary> 
            /// RPC サーバーの浮動小数点演算で 0 による除算が実行されました。
            /// </summary>
            RPC_NT_FP_DIV_ZERO = 0xc0020046,

            /// <summary> 
            /// RPC サーバーで浮動小数点アンダーフローが発生しました。
            /// </summary>
            RPC_NT_FP_UNDERFLOW = 0xc0020047,

            /// <summary> 
            /// RPC サーバーで浮動小数点オーバーフローが発生しました。
            /// </summary>
            RPC_NT_FP_OVERFLOW = 0xc0020048,

            /// <summary> 
            /// 自動ハンドルの結合のために使用できる RPC サーバーの一覧はすべて使用されました。
            /// </summary>
            RPC_NT_NO_MORE_ENTRIES = 0xc0030001,

            /// <summary> 
            /// DCERPCCHARTRANS によって指定されたファイルを開くことができません。
            /// </summary>
            RPC_NT_SS_CHAR_TRANS_OPEN_FAIL = 0xc0030002,

            /// <summary> 
            /// 文字変換テーブルが登録されているファイルのサイズが 512 バイト未満です。
            /// </summary>
            RPC_NT_SS_CHAR_TRANS_SHORT_FILE = 0xc0030003,

            /// <summary> 
            /// NULL コンテキスト ハンドルが [in] パラメータとして渡されました。
            /// </summary>
            RPC_NT_SS_IN_NULL_CONTEXT = 0xc0030004,

            /// <summary> 
            /// コンテキスト ハンドルが、認識されるどのコンテキスト ハンドルとも一致しません。
            /// </summary>
            RPC_NT_SS_CONTEXT_MISMATCH = 0xc0030005,

            /// <summary> 
            /// 呼び出しの途中でコンテキスト ハンドルが変化しました。
            /// </summary>
            RPC_NT_SS_CONTEXT_DAMAGED = 0xc0030006,

            /// <summary> 
            /// リモート プロシージャ コールに渡された結合ハンドルが一致しません。
            /// </summary>
            RPC_NT_SS_HANDLES_MISMATCH = 0xc0030007,

            /// <summary> 
            /// スタブは呼び出しハンドルを入手できません。
            /// </summary>
            RPC_NT_SS_CANNOT_GET_CALL_HANDLE = 0xc0030008,

            /// <summary> 
            /// NULL 参照ポインタがスタブに渡されました。
            /// </summary>
            RPC_NT_NULL_REF_POINTER = 0xc0030009,

            /// <summary> 
            /// 問い合わせの値は範囲外です。
            /// </summary>
            RPC_NT_ENUM_VALUE_OUT_OF_RANGE = 0xc003000a,

            /// <summary> 
            /// バイト カウントが小さすぎます。
            /// </summary>
            RPC_NT_BYTE_COUNT_TOO_SMALL = 0xc003000b,

            /// <summary> 
            /// スタブは正しくないデータを受信しました。
            /// </summary>
            RPC_NT_BAD_STUB_DATA = 0xc003000c,

            /// <summary> 
            /// リモート プロシージャ コールは既にこのスレッドに対して処理中です。
            /// </summary>
            RPC_NT_CALL_IN_PROGRESS = 0xc0020049,

            /// <summary> 
            /// これ以上バインディングはありません。
            /// </summary>
            RPC_NT_NO_MORE_BINDINGS = 0xc002004a,

            /// <summary> 
            /// グループ メンバが見つかりませんでした。
            /// </summary>
            RPC_NT_GROUP_MEMBER_NOT_FOUND = 0xc002004b,

            /// <summary> 
            /// エンドポイント マッパー データベース エントリを作成できませんでした。
            /// </summary>
            EPT_NT_CANT_CREATE = 0xc002004c,

            /// <summary> 
            /// オブジェクト UUID が nil UUID です。
            /// </summary>
            RPC_NT_INVALID_OBJECT = 0xc002004d,

            /// <summary> 
            /// インターフェイスが登録されていません。
            /// </summary>
            RPC_NT_NO_INTERFACES = 0xc002004f,

            /// <summary> 
            /// リモート プロシージャ コールを取り消しました。
            /// </summary>
            RPC_NT_CALL_CANCELLED = 0xc0020050,

            /// <summary> 
            /// 結合ハンドルには、要求したすべての情報が含まれていません。
            /// </summary>
            RPC_NT_BINDING_INCOMPLETE = 0xc0020051,

            /// <summary> 
            /// リモート プロシージャ コール中に通信エラーが発生しました。
            /// </summary>
            RPC_NT_COMM_FAILURE = 0xc0020052,

            /// <summary> 
            /// 要求した認証レベルはサポートされていません。
            /// </summary>
            RPC_NT_UNSUPPORTED_AUTHN_LEVEL = 0xc0020053,

            /// <summary> 
            /// プリンシパル名が登録されていません。
            /// </summary>
            RPC_NT_NO_PRINC_NAME = 0xc0020054,

            /// <summary> 
            /// 指定されたエラーは有効な Windows RPC エラー コードではありません。
            /// </summary>
            RPC_NT_NOT_RPC_ERROR = 0xc0020055,

            /// <summary> 
            /// このコンピュータでのみ有効な UUID が割り当てられています。
            /// </summary>
            RPC_NT_UUID_LOCAL_ONLY = 0x40020056,

            /// <summary> 
            /// セキュリティ パッケージ固有エラーが発生しました。
            /// </summary>
            RPC_NT_SEC_PKG_ERROR = 0xc0020057,

            /// <summary> 
            /// スレッドは取り消されていません。
            /// </summary>
            RPC_NT_NOT_CANCELLED = 0xc0020058,

            /// <summary> 
            /// 暗号化または暗号解読のハンドルに対する無効な操作です。
            /// </summary>
            RPC_NT_INVALID_ES_ACTION = 0xc0030059,

            /// <summary> 
            /// シリアル パッケージと互換性のないバージョンです。
            /// </summary>
            RPC_NT_WRONG_ES_VERSION = 0xc003005a,

            /// <summary> 
            /// RPC スタブと互換性のないバージョンです。
            /// </summary>
            RPC_NT_WRONG_STUB_VERSION = 0xc003005b,

            /// <summary> 
            /// RPC パイプ オブジェクトが無効か、または壊れています。
            /// </summary>
            RPC_NT_INVALID_PIPE_OBJECT = 0xc003005c,

            /// <summary> 
            /// RPC パイプ オブジェクトで無効な操作を行おうとしました。
            /// </summary>
            RPC_NT_INVALID_PIPE_OPERATION = 0xc003005d,

            /// <summary> 
            /// サポートされていない RPC パイプ バージョンです。
            /// </summary>
            RPC_NT_WRONG_PIPE_VERSION = 0xc003005e,

            /// <summary> 
            /// RPC パイプ オブジェクトは既に閉じられていました。
            /// </summary>
            RPC_NT_PIPE_CLOSED = 0xc003005f,

            /// <summary> 
            /// すべてのパイプが処理される前に RPC の呼び出しが完了しました。
            /// </summary>
            RPC_NT_PIPE_DISCIPLINE_ERROR = 0xc0030060,

            /// <summary> 
            /// RPC パイプからはデータを利用できません。
            /// </summary>
            RPC_NT_PIPE_EMPTY = 0xc0030061,

            /// <summary> 
            /// 無効な非同期リモート プロシージャ コール ハンドルです。
            /// </summary>
            RPC_NT_INVALID_ASYNC_HANDLE = 0xc0020062,

            /// <summary> 
            /// この操作には無効な非同期 RPC の呼び出しハンドルです。
            /// </summary>
            RPC_NT_INVALID_ASYNC_CALL = 0xc0020063,

            /// <summary> 
            /// RPC_NT_PROXY_ACCESS_DENIED
            /// </summary>
            RPC_NT_PROXY_ACCESS_DENIED = 0xc0020064,

            /// <summary> 
            /// RPC_NT_COOKIE_AUTH_FAILED
            /// </summary>
            RPC_NT_COOKIE_AUTH_FAILED = 0xc0020065,

            /// <summary> 
            /// 要求バッファに未送信のデータが残っています。
            /// </summary>
            RPC_NT_SEND_INCOMPLETE = 0x400200af,

            /// <summary> 
            /// 無効な AML オペコードを実行しようとしました。
            /// </summary>
            STATUS_ACPI_INVALID_OPCODE = 0xc0140001,

            /// <summary> 
            /// AML インタープリタ スタックは、オバーフローしました。
            /// </summary>
            STATUS_ACPI_STACK_OVERFLOW = 0xc0140002,

            /// <summary> 
            /// 矛盾した状態が発生しました。
            /// </summary>
            STATUS_ACPI_ASSERT_FAILED = 0xc0140003,

            /// <summary> 
            /// 配列の境界の外側にアクセスしようとしました。
            /// </summary>
            STATUS_ACPI_INVALID_INDEX = 0xc0140004,

            /// <summary> 
            /// 必要な引数が指定されませんでした。
            /// </summary>
            STATUS_ACPI_INVALID_ARGUMENT = 0xc0140005,

            /// <summary> 
            /// 致命的なエラーが発生しました。
            /// </summary>
            STATUS_ACPI_FATAL = 0xc0140006,

            /// <summary> 
            /// 無効な SuperName が指定されました。
            /// </summary>
            STATUS_ACPI_INVALID_SUPERNAME = 0xc0140007,

            /// <summary> 
            /// 正しくない種類の引数が指定されました。
            /// </summary>
            STATUS_ACPI_INVALID_ARGTYPE = 0xc0140008,

            /// <summary> 
            /// 正しくない種類のオブジェクトが指定されました。
            /// </summary>
            STATUS_ACPI_INVALID_OBJTYPE = 0xc0140009,

            /// <summary> 
            /// 正しくない種類のターゲットが指定されました。
            /// </summary>
            STATUS_ACPI_INVALID_TARGETTYPE = 0xc014000a,

            /// <summary> 
            /// 正しくない引数の数が指定されました。
            /// </summary>
            STATUS_ACPI_INCORRECT_ARGUMENT_COUNT = 0xc014000b,

            /// <summary> 
            /// アドレスを変換できませんでした。
            /// </summary>
            STATUS_ACPI_ADDRESS_NOT_MAPPED = 0xc014000c,

            /// <summary> 
            /// 正しくないイベントの種類が指定されました。
            /// </summary>
            STATUS_ACPI_INVALID_EVENTTYPE = 0xc014000d,

            /// <summary> 
            /// ターゲットのハンドルは、既に存在します。
            /// </summary>
            STATUS_ACPI_HANDLER_COLLISION = 0xc014000e,

            /// <summary> 
            /// ターゲットの無効なデータが指定されました。
            /// </summary>
            STATUS_ACPI_INVALID_DATA = 0xc014000f,

            /// <summary> 
            /// ターゲットの無効な領域が指定されました。
            /// </summary>
            STATUS_ACPI_INVALID_REGION = 0xc0140010,

            /// <summary> 
            /// 定義された範囲の外側のフィールドにアクセスしようとしました。
            /// </summary>
            STATUS_ACPI_INVALID_ACCESS_SIZE = 0xc0140011,

            /// <summary> 
            /// グローバル システム ロックを取得できませんでした。
            /// </summary>
            STATUS_ACPI_ACQUIRE_GLOBAL_LOCK = 0xc0140012,

            /// <summary> 
            /// ACPI サブシステムを再初期化しようとしました。
            /// </summary>
            STATUS_ACPI_ALREADY_INITIALIZED = 0xc0140013,

            /// <summary> 
            /// ACPI サブシステムは、初期化されました。
            /// </summary>
            STATUS_ACPI_NOT_INITIALIZED = 0xc0140014,

            /// <summary> 
            /// 正しくないミューテックスが指定されました。
            /// </summary>
            STATUS_ACPI_INVALID_MUTEX_LEVEL = 0xc0140015,

            /// <summary> 
            /// 現在、ミューテックスを所有していません。
            /// </summary>
            STATUS_ACPI_MUTEX_NOT_OWNED = 0xc0140016,

            /// <summary> 
            /// 所有者でないプロセスがミューテックスをアクセスしようとしました。
            /// </summary>
            STATUS_ACPI_MUTEX_NOT_OWNER = 0xc0140017,

            /// <summary> 
            /// 領域にアクセスしている間にエラーが発生しました。
            /// </summary>
            STATUS_ACPI_RS_ACCESS = 0xc0140018,

            /// <summary> 
            /// 正しくないテーブルを使おうとしました。
            /// </summary>
            STATUS_ACPI_INVALID_TABLE = 0xc0140019,

            /// <summary> 
            /// ACPI イベントの登録に失敗しました。
            /// </summary>
            STATUS_ACPI_REG_HANDLER_FAILED = 0xc0140020,

            /// <summary> 
            /// ACPI パワー オブジェクトは、状態の移行に失敗しました。
            /// </summary>
            STATUS_ACPI_POWER_REQUEST_FAILED = 0xc0140021,

            /// <summary> 
            /// STATUS_CTX_WINSTATION_NAME_INVALID
            /// </summary>
            STATUS_CTX_WINSTATION_NAME_INVALID = 0xc00a0001,

            /// <summary> 
            /// STATUS_CTX_INVALID_PD
            /// </summary>
            STATUS_CTX_INVALID_PD = 0xc00a0002,

            /// <summary> 
            /// STATUS_CTX_PD_NOT_FOUND
            /// </summary>
            STATUS_CTX_PD_NOT_FOUND = 0xc00a0003,

            /// <summary> 
            /// クライアント ドライブ マッピング サービスは、ターミナル コネクション上で接続されています。
            /// </summary>
            STATUS_CTX_CDM_CONNECT = 0x400a0004,

            /// <summary> 
            /// クライアント ドライブ マッピング サービスは、ターミナル コネクション上で切断されています。
            /// </summary>
            STATUS_CTX_CDM_DISCONNECT = 0x400a0005,

            /// <summary> 
            /// 閉じる操作は、ターミナル コネクションで待ちになっています。
            /// </summary>
            STATUS_CTX_CLOSE_PENDING = 0xc00a0006,

            /// <summary> 
            /// 利用できる空き出力バッファがありません。
            /// </summary>
            STATUS_CTX_NO_OUTBUF = 0xc00a0007,

            /// <summary> 
            /// MODEM.INF ファイルが見つかりませんでした。
            /// </summary>
            STATUS_CTX_MODEM_INF_NOT_FOUND = 0xc00a0008,

            /// <summary> 
            /// STATUS_CTX_INVALID_MODEMNAME
            /// </summary>
            STATUS_CTX_INVALID_MODEMNAME = 0xc00a0009,

            /// <summary> 
            /// モデムは、モデムに送信されたコマンドを受け取りませんでした。
            /// モデム名が接続されているモデムと一致しているかを確認してください。
            /// </summary>
            STATUS_CTX_RESPONSE_ERROR = 0xc00a000a,

            /// <summary> 
            /// モデムは、モデムに送信されたコマンドに応答しませんでした。
            /// モデムが正しく接続されていて電源が入っているかを確認してください。
            /// </summary>
            STATUS_CTX_MODEM_RESPONSE_TIMEOUT = 0xc00a000b,

            /// <summary> 
            /// 切断されたため、キャリア検出に失敗したか、またはキャリアは、ドロップされました。
            /// </summary>
            STATUS_CTX_MODEM_RESPONSE_NO_CARRIER = 0xc00a000c,

            /// <summary> 
            /// 発信音が要求された時間以内に検出されませんでした。
            /// 電話線が正しく接続されていて、機能しているかを確認してください。
            /// </summary>
            STATUS_CTX_MODEM_RESPONSE_NO_DIALTONE = 0xc00a000d,

            /// <summary> 
            /// ビジー シグナルが、コールバックのリモート サイトで検出されました。
            /// </summary>
            STATUS_CTX_MODEM_RESPONSE_BUSY = 0xc00a000e,

            /// <summary> 
            /// 音声が、コールバックのリモート サイトで検出されました。
            /// </summary>
            STATUS_CTX_MODEM_RESPONSE_VOICE = 0xc00a000f,

            /// <summary> 
            /// 転送ドライバ エラー。
            /// </summary>
            STATUS_CTX_TD_ERROR = 0xc00a0010,

            /// <summary> 
            /// 使用しているクライアントは、このシステムで使えるようにライセンスされていません。ログオン要求は拒否されます。
            /// </summary>
            STATUS_CTX_LICENSE_CLIENT_INVALID = 0xc00a0012,

            /// <summary> 
            /// ライセンスされたログオンの制限値に達しました。
            /// しばらくしてから、やり直してください。
            /// </summary>
            STATUS_CTX_LICENSE_NOT_AVAILABLE = 0xc00a0013,

            /// <summary> 
            /// システム ライセンスの有効期限が切れています。ログオンの要求は拒否されました。
            /// </summary>
            STATUS_CTX_LICENSE_EXPIRED = 0xc00a0014,

            /// <summary> 
            /// 指定されたセッションが見つかりませんでした。
            /// </summary>
            STATUS_CTX_WINSTATION_NOT_FOUND = 0xc00a0015,

            /// <summary> 
            /// 指定されたセッション名は、既に使用されています。
            /// </summary>
            STATUS_CTX_WINSTATION_NAME_COLLISION = 0xc00a0016,

            /// <summary> 
            /// ターミナル接続が現在接続、切断、リセット、または削除操作でビジーのため、要求された操作を完了できません。
            /// </summary>
            STATUS_CTX_WINSTATION_BUSY = 0xc00a0017,

            /// <summary> 
            /// 現在のクライアントによって、サポートされていないビデオ モードのセッションへ接続しようとしました。
            /// </summary>
            STATUS_CTX_BAD_VIDEO_MODE = 0xc00a0018,

            /// <summary> 
            /// アプリケーションは、DOS グラフィック モードを有効にしようとしましが、DOS グラフィック モードは、サポートされていません。
            /// </summary>
            STATUS_CTX_GRAPHICS_INVALID = 0xc00a0022,

            /// <summary> 
            /// 要求された操作は、システム コンソールのみで実行できます。
            /// これは、多くの場合、ドライバまたはシステム DLL が直接コンソールにアクセスを要求した結果発生します。
            /// </summary>
            STATUS_CTX_NOT_CONSOLE = 0xc00a0024,

            /// <summary> 
            /// クライアントは、サーバー接続メッセージの応答に失敗しました。
            /// </summary>
            STATUS_CTX_CLIENT_QUERY_TIMEOUT = 0xc00a0026,

            /// <summary> 
            /// コンソール セッションの切断は、サポートされていません。
            /// </summary>
            STATUS_CTX_CONSOLE_DISCONNECT = 0xc00a0027,

            /// <summary> 
            /// 切断されたセッションからコンソールへ再接続することは、サポートされていません。
            /// </summary>
            STATUS_CTX_CONSOLE_CONNECT = 0xc00a0028,

            /// <summary> 
            /// 別のセッションのリモート制御の要求は拒否されました。
            /// </summary>
            STATUS_CTX_SHADOW_DENIED = 0xc00a002a,

            /// <summary> 
            /// プロセスがセッションにアクセスするように要求しましたが、これらのアクセス権が与えられていません。
            /// </summary>
            STATUS_CTX_WINSTATION_ACCESS_DENIED = 0xc00a002b,

            /// <summary> 
            /// STATUS_CTX_INVALID_WD
            /// </summary>
            STATUS_CTX_INVALID_WD = 0xc00a002e,

            /// <summary> 
            /// STATUS_CTX_WD_NOT_FOUND
            /// </summary>
            STATUS_CTX_WD_NOT_FOUND = 0xc00a002f,

            /// <summary> 
            /// 要求されたセッションをリモートで制御できません。
            /// 自分のセッション、自分のセッションを制御しようとしているセッションまたはユーザーがログオンしていないセッションを制御したり、コンソールからほかのセッションを制御したりすることはできません。
            /// </summary>
            STATUS_CTX_SHADOW_INVALID = 0xc00a0030,

            /// <summary> 
            /// 要求されたセッションは、リモート制御を許可するように構成されていません。
            /// </summary>
            STATUS_CTX_SHADOW_DISABLED = 0xc00a0031,

            /// <summary> 
            /// STATUS_RDP_PROTOCOL_ERROR
            /// </summary>
            STATUS_RDP_PROTOCOL_ERROR = 0xc00a0032,

            /// <summary> 
            /// このターミナル サーバーへの接続要求が拒否されました。
            /// このターミナル クライアント コピーのためのターミナル サーバー クライアントのライセンス番号が入力されていません。
            /// システム管理者に連絡をしてターミナル サーバー クライアントの有効な一意のライセンス番号値を入力してください。
            /// </summary>
            STATUS_CTX_CLIENT_LICENSE_NOT_SET = 0xc00a0033,

            /// <summary> 
            /// このターミナル サーバーへの接続要求が拒否されました。
            /// このターミナル サーバー クライアントのライセンス番号は現在別のユーザーによって使われています。
            /// システム管理者に連絡をして新しい有効で一意のターミナル サーバー クライアントのライセンス番号を入手してください。
            /// </summary>
            STATUS_CTX_CLIENT_LICENSE_IN_USE = 0xc00a0034,

            /// <summary> 
            /// 表示モードが変更されたため、コンソールのリモート制御は終了されました。リモート制御セッションでの表示モードの変更はサポートされていません。
            /// </summary>
            STATUS_CTX_SHADOW_ENDED_BY_MODE_CHANGE = 0xc00a0035,

            /// <summary> 
            /// 指定されたセッションは現在リモートで制御されていないため、リモート制御を終了できませんでした。
            /// </summary>
            STATUS_CTX_SHADOW_NOT_RUNNING = 0xc00a0036,

            /// <summary> 
            /// STATUS_CTX_LOGON_DISABLED
            /// </summary>
            STATUS_CTX_LOGON_DISABLED = 0xc00a0037,

            /// <summary> 
            /// STATUS_CTX_SECURITY_LAYER_ERROR
            /// </summary>
            STATUS_CTX_SECURITY_LAYER_ERROR = 0xc00a0038,

            /// <summary> 
            /// STATUS_TS_INCOMPATIBLE_SESSIONS
            /// </summary>
            STATUS_TS_INCOMPATIBLE_SESSIONS = 0xc00a0039,

            /// <summary> 
            /// デバイスがシステム BIOS MPS テーブルで見つかりません。このデバイスを使用しません。
            /// システム BIOS の更新についてはシステム ベンダに問い合わせてください。
            /// </summary>
            STATUS_PNP_BAD_MPS_TABLE = 0xc0040035,

            /// <summary> 
            /// トランスレータはリソースを翻訳できませんでした。
            /// </summary>
            STATUS_PNP_TRANSLATION_FAILED = 0xc0040036,

            /// <summary> 
            /// IRQ トランスレータはリソースを翻訳できませんでした。
            /// </summary>
            STATUS_PNP_IRQ_TRANSLATION_FAILED = 0xc0040037,

            /// <summary> 
            /// STATUS_PNP_INVALID_ID
            /// </summary>
            STATUS_PNP_INVALID_ID = 0xc0040038,

            /// <summary> 
            /// STATUS_IO_REISSUE_AS_CACHED
            /// </summary>
            STATUS_IO_REISSUE_AS_CACHED = 0xc0040039,

            /// <summary> 
            /// STATUS_MUI_FILE_NOT_FOUND
            /// </summary>
            STATUS_MUI_FILE_NOT_FOUND = 0xc00b0001,

            /// <summary> 
            /// STATUS_MUI_INVALID_FILE
            /// </summary>
            STATUS_MUI_INVALID_FILE = 0xc00b0002,

            /// <summary> 
            /// STATUS_MUI_INVALID_RC_CONFIG
            /// </summary>
            STATUS_MUI_INVALID_RC_CONFIG = 0xc00b0003,

            /// <summary> 
            /// STATUS_MUI_INVALID_LOCALE_NAME
            /// </summary>
            STATUS_MUI_INVALID_LOCALE_NAME = 0xc00b0004,

            /// <summary> 
            /// STATUS_MUI_INVALID_ULTIMATEFALLBACK_NAME
            /// </summary>
            STATUS_MUI_INVALID_ULTIMATEFALLBACK_NAME = 0xc00b0005,

            /// <summary> 
            /// STATUS_MUI_FILE_NOT_LOADED
            /// </summary>
            STATUS_MUI_FILE_NOT_LOADED = 0xc00b0006,

            /// <summary> 
            /// STATUS_RESOURCE_ENUM_USER_STOP
            /// </summary>
            STATUS_RESOURCE_ENUM_USER_STOP = 0xc00b0007,

            /// <summary> 
            /// STATUS_FLT_NO_HANDLER_DEFINED
            /// </summary>
            STATUS_FLT_NO_HANDLER_DEFINED = 0xc01c0001,

            /// <summary> 
            /// STATUS_FLT_CONTEXT_ALREADY_DEFINED
            /// </summary>
            STATUS_FLT_CONTEXT_ALREADY_DEFINED = 0xc01c0002,

            /// <summary> 
            /// STATUS_FLT_INVALID_ASYNCHRONOUS_REQUEST
            /// </summary>
            STATUS_FLT_INVALID_ASYNCHRONOUS_REQUEST = 0xc01c0003,

            /// <summary> 
            /// STATUS_FLT_DISALLOW_FAST_IO
            /// </summary>
            STATUS_FLT_DISALLOW_FAST_IO = 0xc01c0004,

            /// <summary> 
            /// STATUS_FLT_INVALID_NAME_REQUEST
            /// </summary>
            STATUS_FLT_INVALID_NAME_REQUEST = 0xc01c0005,

            /// <summary> 
            /// STATUS_FLT_NOT_SAFE_TO_POST_OPERATION
            /// </summary>
            STATUS_FLT_NOT_SAFE_TO_POST_OPERATION = 0xc01c0006,

            /// <summary> 
            /// STATUS_FLT_NOT_INITIALIZED
            /// </summary>
            STATUS_FLT_NOT_INITIALIZED = 0xc01c0007,

            /// <summary> 
            /// STATUS_FLT_FILTER_NOT_READY
            /// </summary>
            STATUS_FLT_FILTER_NOT_READY = 0xc01c0008,

            /// <summary> 
            /// STATUS_FLT_POST_OPERATION_CLEANUP
            /// </summary>
            STATUS_FLT_POST_OPERATION_CLEANUP = 0xc01c0009,

            /// <summary> 
            /// STATUS_FLT_INTERNAL_ERROR
            /// </summary>
            STATUS_FLT_INTERNAL_ERROR = 0xc01c000a,

            /// <summary> 
            /// STATUS_FLT_DELETING_OBJECT
            /// </summary>
            STATUS_FLT_DELETING_OBJECT = 0xc01c000b,

            /// <summary> 
            /// STATUS_FLT_MUST_BE_NONPAGED_POOL
            /// </summary>
            STATUS_FLT_MUST_BE_NONPAGED_POOL = 0xc01c000c,

            /// <summary> 
            /// STATUS_FLT_DUPLICATE_ENTRY
            /// </summary>
            STATUS_FLT_DUPLICATE_ENTRY = 0xc01c000d,

            /// <summary> 
            /// STATUS_FLT_CBDQ_DISABLED
            /// </summary>
            STATUS_FLT_CBDQ_DISABLED = 0xc01c000e,

            /// <summary> 
            /// STATUS_FLT_DO_NOT_ATTACH
            /// </summary>
            STATUS_FLT_DO_NOT_ATTACH = 0xc01c000f,

            /// <summary> 
            /// STATUS_FLT_DO_NOT_DETACH
            /// </summary>
            STATUS_FLT_DO_NOT_DETACH = 0xc01c0010,

            /// <summary> 
            /// STATUS_FLT_INSTANCE_ALTITUDE_COLLISION
            /// </summary>
            STATUS_FLT_INSTANCE_ALTITUDE_COLLISION = 0xc01c0011,

            /// <summary> 
            /// STATUS_FLT_INSTANCE_NAME_COLLISION
            /// </summary>
            STATUS_FLT_INSTANCE_NAME_COLLISION = 0xc01c0012,

            /// <summary> 
            /// STATUS_FLT_FILTER_NOT_FOUND
            /// </summary>
            STATUS_FLT_FILTER_NOT_FOUND = 0xc01c0013,

            /// <summary> 
            /// STATUS_FLT_VOLUME_NOT_FOUND
            /// </summary>
            STATUS_FLT_VOLUME_NOT_FOUND = 0xc01c0014,

            /// <summary> 
            /// STATUS_FLT_INSTANCE_NOT_FOUND
            /// </summary>
            STATUS_FLT_INSTANCE_NOT_FOUND = 0xc01c0015,

            /// <summary> 
            /// STATUS_FLT_CONTEXT_ALLOCATION_NOT_FOUND
            /// </summary>
            STATUS_FLT_CONTEXT_ALLOCATION_NOT_FOUND = 0xc01c0016,

            /// <summary> 
            /// STATUS_FLT_INVALID_CONTEXT_REGISTRATION
            /// </summary>
            STATUS_FLT_INVALID_CONTEXT_REGISTRATION = 0xc01c0017,

            /// <summary> 
            /// STATUS_FLT_NAME_CACHE_MISS
            /// </summary>
            STATUS_FLT_NAME_CACHE_MISS = 0xc01c0018,

            /// <summary> 
            /// STATUS_FLT_NO_DEVICE_OBJECT
            /// </summary>
            STATUS_FLT_NO_DEVICE_OBJECT = 0xc01c0019,

            /// <summary> 
            /// STATUS_FLT_VOLUME_ALREADY_MOUNTED
            /// </summary>
            STATUS_FLT_VOLUME_ALREADY_MOUNTED = 0xc01c001a,

            /// <summary> 
            /// STATUS_FLT_ALREADY_ENLISTED
            /// </summary>
            STATUS_FLT_ALREADY_ENLISTED = 0xc01c001b,

            /// <summary> 
            /// STATUS_FLT_CONTEXT_ALREADY_LINKED
            /// </summary>
            STATUS_FLT_CONTEXT_ALREADY_LINKED = 0xc01c001c,

            /// <summary> 
            /// STATUS_FLT_NO_WAITER_FOR_REPLY
            /// </summary>
            STATUS_FLT_NO_WAITER_FOR_REPLY = 0xc01c0020,

            /// <summary> 
            /// 要求されたセクションはアクティブ化コンテキストにありません。
            /// </summary>
            STATUS_SXS_SECTION_NOT_FOUND = 0xc0150001,

            /// <summary> 
            /// アプリケーション バインド情報を処理できませんでした。
            /// 詳細はシステム イベント ログを参照してください。
            /// </summary>
            STATUS_SXS_CANT_GEN_ACTCTX = 0xc0150002,

            /// <summary> 
            /// アプリケーション バインド データ形式が無効です。
            /// </summary>
            STATUS_SXS_INVALID_ACTCTXDATA_FORMAT = 0xc0150003,

            /// <summary> 
            /// 参照されたアセンブリはこのシステムにインストールされていません。
            /// </summary>
            STATUS_SXS_ASSEMBLY_NOT_FOUND = 0xc0150004,

            /// <summary> 
            /// manifest ファイルは要求されたタグおよび形式で開始されていません。
            /// </summary>
            STATUS_SXS_MANIFEST_FORMAT_ERROR = 0xc0150005,

            /// <summary> 
            /// manifest ファイルには構文エラーが含まれています。
            /// </summary>
            STATUS_SXS_MANIFEST_PARSE_ERROR = 0xc0150006,

            /// <summary> 
            /// アプリケーションにより、無効にされたアクティブ化コンテキストのアクティブ化が試行されました。
            /// </summary>
            STATUS_SXS_ACTIVATION_CONTEXT_DISABLED = 0xc0150007,

            /// <summary> 
            /// 要求された参照キーはアクティブなアクティブ化コンテキストで見つかりませんでした。
            /// </summary>
            STATUS_SXS_KEY_NOT_FOUND = 0xc0150008,

            /// <summary> 
            /// このアプリケーションによって要求されたコンポーネントのバージョンは、既にアクティブな別のコンポーネントのバージョンと競合しています。
            /// </summary>
            STATUS_SXS_VERSION_CONFLICT = 0xc0150009,

            /// <summary> 
            /// アクティブ化コンテキスト セクションを要求した種類が API が使ったクエリと一致しません。
            /// </summary>
            STATUS_SXS_WRONG_SECTION_TYPE = 0xc015000a,

            /// <summary> 
            /// システム リソースの不足により、実行の現在のスレッドに対して、分離されたアクティブ化を無効にすることが必要になりました。
            /// </summary>
            STATUS_SXS_THREAD_QUERIES_DISABLED = 0xc015000b,

            /// <summary> 
            /// 参照されたアセンブリは見つかりませんでした。
            /// </summary>
            STATUS_SXS_ASSEMBLY_MISSING = 0xc015000c,

            /// <summary> 
            /// kernel モード コンポーネントはアクティブ化コンテキストへの参照を解除しています。
            /// </summary>
            STATUS_SXS_RELEASE_ACTIVATION_CONTEXT = 0x4015000d,

            /// <summary> 
            /// プロセスの既定のアクティブ化コンテキストは既に設定されているため、設定できませんでした。
            /// </summary>
            STATUS_SXS_PROCESS_DEFAULT_ALREADY_SET = 0xc015000e,

            /// <summary> 
            /// アクティブ化を解除しているアクティブ化コンテキストは、最近アクティブ化されたものではありません。
            /// </summary>
            STATUS_SXS_EARLY_DEACTIVATION = 0xc015000f,

            /// <summary> 
            /// アクティブ化を解除しているアクティブ化コンテキストは、現在の実行のスレッドでアクティブではありません。
            /// </summary>
            STATUS_SXS_INVALID_DEACTIVATION = 0xc0150010,

            /// <summary> 
            /// アクティブ化を解除しているアクティブ化コンテキストは、既にアクティブ化が解除されています。
            /// </summary>
            STATUS_SXS_MULTIPLE_DEACTIVATION = 0xc0150011,

            /// <summary> 
            /// システムの既定アセンブリのアクティブ化コンテキストを生成できませんでした。
            /// </summary>
            STATUS_SXS_SYSTEM_DEFAULT_ACTIVATION_CONTEXT_EMPTY = 0xc0150012,

            /// <summary> 
            /// 分離機能によって使用されているコンポーネントが、プロセスの中断を要求しました。
            /// </summary>
            STATUS_SXS_PROCESS_TERMINATION_REQUESTED = 0xc0150013,

            /// <summary> 
            /// STATUS_SXS_CORRUPT_ACTIVATION_STACK
            /// </summary>
            STATUS_SXS_CORRUPT_ACTIVATION_STACK = 0xc0150014,

            /// <summary> 
            /// STATUS_SXS_CORRUPTION
            /// </summary>
            STATUS_SXS_CORRUPTION = 0xc0150015,

            /// <summary> 
            /// STATUS_SXS_INVALID_IDENTITY_ATTRIBUTE_VALUE
            /// </summary>
            STATUS_SXS_INVALID_IDENTITY_ATTRIBUTE_VALUE = 0xc0150016,

            /// <summary> 
            /// STATUS_SXS_INVALID_IDENTITY_ATTRIBUTE_NAME
            /// </summary>
            STATUS_SXS_INVALID_IDENTITY_ATTRIBUTE_NAME = 0xc0150017,

            /// <summary> 
            /// STATUS_SXS_IDENTITY_DUPLICATE_ATTRIBUTE
            /// </summary>
            STATUS_SXS_IDENTITY_DUPLICATE_ATTRIBUTE = 0xc0150018,

            /// <summary> 
            /// STATUS_SXS_IDENTITY_PARSE_ERROR
            /// </summary>
            STATUS_SXS_IDENTITY_PARSE_ERROR = 0xc0150019,

            /// <summary> 
            /// STATUS_SXS_COMPONENT_STORE_CORRUPT
            /// </summary>
            STATUS_SXS_COMPONENT_STORE_CORRUPT = 0xc015001a,

            /// <summary> 
            /// STATUS_SXS_FILE_HASH_MISMATCH
            /// </summary>
            STATUS_SXS_FILE_HASH_MISMATCH = 0xc015001b,

            /// <summary> 
            /// STATUS_SXS_MANIFEST_IDENTITY_SAME_BUT_CONTENTS_DIFFERENT
            /// </summary>
            STATUS_SXS_MANIFEST_IDENTITY_SAME_BUT_CONTENTS_DIFFERENT = 0xc015001c,

            /// <summary> 
            /// STATUS_SXS_IDENTITIES_DIFFERENT
            /// </summary>
            STATUS_SXS_IDENTITIES_DIFFERENT = 0xc015001d,

            /// <summary> 
            /// STATUS_SXS_ASSEMBLY_IS_NOT_A_DEPLOYMENT
            /// </summary>
            STATUS_SXS_ASSEMBLY_IS_NOT_A_DEPLOYMENT = 0xc015001e,

            /// <summary> 
            /// STATUS_SXS_FILE_NOT_PART_OF_ASSEMBLY
            /// </summary>
            STATUS_SXS_FILE_NOT_PART_OF_ASSEMBLY = 0xc015001f,

            /// <summary> 
            /// STATUS_ADVANCED_INSTALLER_FAILED
            /// </summary>
            STATUS_ADVANCED_INSTALLER_FAILED = 0xc0150020,

            /// <summary> 
            /// STATUS_XML_ENCODING_MISMATCH
            /// </summary>
            STATUS_XML_ENCODING_MISMATCH = 0xc0150021,

            /// <summary> 
            /// STATUS_SXS_MANIFEST_TOO_BIG
            /// </summary>
            STATUS_SXS_MANIFEST_TOO_BIG = 0xc0150022,

            /// <summary> 
            /// STATUS_SXS_SETTING_NOT_REGISTERED
            /// </summary>
            STATUS_SXS_SETTING_NOT_REGISTERED = 0xc0150023,

            /// <summary> 
            /// STATUS_SXS_TRANSACTION_CLOSURE_INCOMPLETE
            /// </summary>
            STATUS_SXS_TRANSACTION_CLOSURE_INCOMPLETE = 0xc0150024,

            /// <summary> 
            /// STATUS_SMI_PRIMITIVE_INSTALLER_FAILED
            /// </summary>
            STATUS_SMI_PRIMITIVE_INSTALLER_FAILED = 0xc0150025,

            /// <summary> 
            /// STATUS_GENERIC_COMMAND_FAILED
            /// </summary>
            STATUS_GENERIC_COMMAND_FAILED = 0xc0150026,

            /// <summary> 
            /// STATUS_SXS_FILE_HASH_MISSING
            /// </summary>
            STATUS_SXS_FILE_HASH_MISSING = 0xc0150027,

            /// <summary> 
            /// クラスタ ノードが無効です。
            /// </summary>
            STATUS_CLUSTER_INVALID_NODE = 0xc0130001,

            /// <summary> 
            /// クラスタ ノードが既に存在します。
            /// </summary>
            STATUS_CLUSTER_NODE_EXISTS = 0xc0130002,

            /// <summary> 
            /// ノードはクラスタへの参加の処理中です。
            /// </summary>
            STATUS_CLUSTER_JOIN_IN_PROGRESS = 0xc0130003,

            /// <summary> 
            /// クラスタ ノードが見つかりませんでした。
            /// </summary>
            STATUS_CLUSTER_NODE_NOT_FOUND = 0xc0130004,

            /// <summary> 
            /// クラスタ ローカル ノードの情報が見つかりませんでした。
            /// </summary>
            STATUS_CLUSTER_LOCAL_NODE_NOT_FOUND = 0xc0130005,

            /// <summary> 
            /// クラスタ ネットワークが既に存在します。
            /// </summary>
            STATUS_CLUSTER_NETWORK_EXISTS = 0xc0130006,

            /// <summary> 
            /// クラスタ ネットワークが見つかりません。
            /// </summary>
            STATUS_CLUSTER_NETWORK_NOT_FOUND = 0xc0130007,

            /// <summary> 
            /// クラスタ ネットワーク インターフェイスが既に存在します。
            /// </summary>
            STATUS_CLUSTER_NETINTERFACE_EXISTS = 0xc0130008,

            /// <summary> 
            /// クラスタ ネットワーク インターフェイスが見つかりません。
            /// </summary>
            STATUS_CLUSTER_NETINTERFACE_NOT_FOUND = 0xc0130009,

            /// <summary> 
            /// クラスタの要求はオブジェクトに対して無効です。
            /// </summary>
            STATUS_CLUSTER_INVALID_REQUEST = 0xc013000a,

            /// <summary> 
            /// クラスタ ネットワーク プロバイダが無効です。
            /// </summary>
            STATUS_CLUSTER_INVALID_NETWORK_PROVIDER = 0xc013000b,

            /// <summary> 
            /// クラスタ ノードがダウンしています。
            /// </summary>
            STATUS_CLUSTER_NODE_DOWN = 0xc013000c,

            /// <summary> 
            /// クラスタ ノードに到達できません。
            /// </summary>
            STATUS_CLUSTER_NODE_UNREACHABLE = 0xc013000d,

            /// <summary> 
            /// そのクラスタ ノードはクラスタのメンバではありません。
            /// </summary>
            STATUS_CLUSTER_NODE_NOT_MEMBER = 0xc013000e,

            /// <summary> 
            /// クラスタの参加操作が実行されていません。
            /// </summary>
            STATUS_CLUSTER_JOIN_NOT_IN_PROGRESS = 0xc013000f,

            /// <summary> 
            /// クラスタ ネットワークが無効です。
            /// </summary>
            STATUS_CLUSTER_INVALID_NETWORK = 0xc0130010,

            /// <summary> 
            /// 利用できるネットワーク アダプタがありません。
            /// </summary>
            STATUS_CLUSTER_NO_NET_ADAPTERS = 0xc0130011,

            /// <summary> 
            /// クラスタ ノードはアップになっています。
            /// </summary>
            STATUS_CLUSTER_NODE_UP = 0xc0130012,

            /// <summary> 
            /// クラスタ ノードは一時停止しています。
            /// </summary>
            STATUS_CLUSTER_NODE_PAUSED = 0xc0130013,

            /// <summary> 
            /// クラスタ ノードは停止されていません。
            /// </summary>
            STATUS_CLUSTER_NODE_NOT_PAUSED = 0xc0130014,

            /// <summary> 
            /// クラスタ セキュリティの状況が利用できません。
            /// </summary>
            STATUS_CLUSTER_NO_SECURITY_CONTEXT = 0xc0130015,

            /// <summary> 
            /// クラスタ ネットワークは内部クラスタ通信用に構成されていません。
            /// </summary>
            STATUS_CLUSTER_NETWORK_NOT_INTERNAL = 0xc0130016,

            /// <summary> 
            /// クラスタ ノードが侵害されています。
            /// </summary>
            STATUS_CLUSTER_POISONED = 0xc0130017,

            /// <summary> 
            /// STATUS_TRANSACTIONAL_CONFLICT
            /// </summary>
            STATUS_TRANSACTIONAL_CONFLICT = 0xc0190001,

            /// <summary> 
            /// STATUS_INVALID_TRANSACTION
            /// </summary>
            STATUS_INVALID_TRANSACTION = 0xc0190002,

            /// <summary> 
            /// STATUS_TRANSACTION_NOT_ACTIVE
            /// </summary>
            STATUS_TRANSACTION_NOT_ACTIVE = 0xc0190003,

            /// <summary> 
            /// STATUS_TM_INITIALIZATION_FAILED
            /// </summary>
            STATUS_TM_INITIALIZATION_FAILED = 0xc0190004,

            /// <summary> 
            /// STATUS_RM_NOT_ACTIVE
            /// </summary>
            STATUS_RM_NOT_ACTIVE = 0xc0190005,

            /// <summary> 
            /// STATUS_RM_METADATA_CORRUPT
            /// </summary>
            STATUS_RM_METADATA_CORRUPT = 0xc0190006,

            /// <summary> 
            /// STATUS_TRANSACTION_NOT_JOINED
            /// </summary>
            STATUS_TRANSACTION_NOT_JOINED = 0xc0190007,

            /// <summary> 
            /// STATUS_DIRECTORY_NOT_RM
            /// </summary>
            STATUS_DIRECTORY_NOT_RM = 0xc0190008,

            /// <summary> 
            /// STATUS_COULD_NOT_RESIZE_LOG
            /// </summary>
            STATUS_COULD_NOT_RESIZE_LOG = 0x80190009,

            /// <summary> 
            /// STATUS_TRANSACTIONS_UNSUPPORTED_REMOTE
            /// </summary>
            STATUS_TRANSACTIONS_UNSUPPORTED_REMOTE = 0xc019000a,

            /// <summary> 
            /// STATUS_LOG_RESIZE_INVALID_SIZE
            /// </summary>
            STATUS_LOG_RESIZE_INVALID_SIZE = 0xc019000b,

            /// <summary> 
            /// STATUS_REMOTE_FILE_VERSION_MISMATCH
            /// </summary>
            STATUS_REMOTE_FILE_VERSION_MISMATCH = 0xc019000c,

            /// <summary> 
            /// STATUS_CRM_PROTOCOL_ALREADY_EXISTS
            /// </summary>
            STATUS_CRM_PROTOCOL_ALREADY_EXISTS = 0xc019000f,

            /// <summary> 
            /// STATUS_TRANSACTION_PROPAGATION_FAILED
            /// </summary>
            STATUS_TRANSACTION_PROPAGATION_FAILED = 0xc0190010,

            /// <summary> 
            /// STATUS_CRM_PROTOCOL_NOT_FOUND
            /// </summary>
            STATUS_CRM_PROTOCOL_NOT_FOUND = 0xc0190011,

            /// <summary> 
            /// STATUS_TRANSACTION_SUPERIOR_EXISTS
            /// </summary>
            STATUS_TRANSACTION_SUPERIOR_EXISTS = 0xc0190012,

            /// <summary> 
            /// STATUS_TRANSACTION_REQUEST_NOT_VALID
            /// </summary>
            STATUS_TRANSACTION_REQUEST_NOT_VALID = 0xc0190013,

            /// <summary> 
            /// STATUS_TRANSACTION_NOT_REQUESTED
            /// </summary>
            STATUS_TRANSACTION_NOT_REQUESTED = 0xc0190014,

            /// <summary> 
            /// STATUS_TRANSACTION_ALREADY_ABORTED
            /// </summary>
            STATUS_TRANSACTION_ALREADY_ABORTED = 0xc0190015,

            /// <summary> 
            /// STATUS_TRANSACTION_ALREADY_COMMITTED
            /// </summary>
            STATUS_TRANSACTION_ALREADY_COMMITTED = 0xc0190016,

            /// <summary> 
            /// STATUS_TRANSACTION_INVALID_MARSHALL_BUFFER
            /// </summary>
            STATUS_TRANSACTION_INVALID_MARSHALL_BUFFER = 0xc0190017,

            /// <summary> 
            /// STATUS_CURRENT_TRANSACTION_NOT_VALID
            /// </summary>
            STATUS_CURRENT_TRANSACTION_NOT_VALID = 0xc0190018,

            /// <summary> 
            /// STATUS_LOG_GROWTH_FAILED
            /// </summary>
            STATUS_LOG_GROWTH_FAILED = 0xc0190019,

            /// <summary> 
            /// STATUS_OBJECT_NO_LONGER_EXISTS
            /// </summary>
            STATUS_OBJECT_NO_LONGER_EXISTS = 0xc0190021,

            /// <summary> 
            /// STATUS_STREAM_MINIVERSION_NOT_FOUND
            /// </summary>
            STATUS_STREAM_MINIVERSION_NOT_FOUND = 0xc0190022,

            /// <summary> 
            /// STATUS_STREAM_MINIVERSION_NOT_VALID
            /// </summary>
            STATUS_STREAM_MINIVERSION_NOT_VALID = 0xc0190023,

            /// <summary> 
            /// STATUS_MINIVERSION_INACCESSIBLE_FROM_SPECIFIED_TRANSACTION
            /// </summary>
            STATUS_MINIVERSION_INACCESSIBLE_FROM_SPECIFIED_TRANSACTION = 0xc0190024,

            /// <summary> 
            /// STATUS_CANT_OPEN_MINIVERSION_WITH_MODIFY_INTENT
            /// </summary>
            STATUS_CANT_OPEN_MINIVERSION_WITH_MODIFY_INTENT = 0xc0190025,

            /// <summary> 
            /// STATUS_CANT_CREATE_MORE_STREAM_MINIVERSIONS
            /// </summary>
            STATUS_CANT_CREATE_MORE_STREAM_MINIVERSIONS = 0xc0190026,

            /// <summary> 
            /// STATUS_HANDLE_NO_LONGER_VALID
            /// </summary>
            STATUS_HANDLE_NO_LONGER_VALID = 0xc0190028,

            /// <summary> 
            /// STATUS_NO_TXF_METADATA
            /// </summary>
            STATUS_NO_TXF_METADATA = 0x80190029,

            /// <summary> 
            /// STATUS_LOG_CORRUPTION_DETECTED
            /// </summary>
            STATUS_LOG_CORRUPTION_DETECTED = 0xc0190030,

            /// <summary> 
            /// STATUS_CANT_RECOVER_WITH_HANDLE_OPEN
            /// </summary>
            STATUS_CANT_RECOVER_WITH_HANDLE_OPEN = 0x80190031,

            /// <summary> 
            /// STATUS_RM_DISCONNECTED
            /// </summary>
            STATUS_RM_DISCONNECTED = 0xc0190032,

            /// <summary> 
            /// STATUS_ENLISTMENT_NOT_SUPERIOR
            /// </summary>
            STATUS_ENLISTMENT_NOT_SUPERIOR = 0xc0190033,

            /// <summary> 
            /// STATUS_RECOVERY_NOT_NEEDED
            /// </summary>
            STATUS_RECOVERY_NOT_NEEDED = 0x40190034,

            /// <summary> 
            /// STATUS_RM_ALREADY_STARTED
            /// </summary>
            STATUS_RM_ALREADY_STARTED = 0x40190035,

            /// <summary> 
            /// STATUS_FILE_IDENTITY_NOT_PERSISTENT
            /// </summary>
            STATUS_FILE_IDENTITY_NOT_PERSISTENT = 0xc0190036,

            /// <summary> 
            /// STATUS_CANT_BREAK_TRANSACTIONAL_DEPENDENCY
            /// </summary>
            STATUS_CANT_BREAK_TRANSACTIONAL_DEPENDENCY = 0xc0190037,

            /// <summary> 
            /// STATUS_CANT_CROSS_RM_BOUNDARY
            /// </summary>
            STATUS_CANT_CROSS_RM_BOUNDARY = 0xc0190038,

            /// <summary> 
            /// STATUS_TXF_DIR_NOT_EMPTY
            /// </summary>
            STATUS_TXF_DIR_NOT_EMPTY = 0xc0190039,

            /// <summary> 
            /// STATUS_INDOUBT_TRANSACTIONS_EXIST
            /// </summary>
            STATUS_INDOUBT_TRANSACTIONS_EXIST = 0xc019003a,

            /// <summary> 
            /// STATUS_TM_VOLATILE
            /// </summary>
            STATUS_TM_VOLATILE = 0xc019003b,

            /// <summary> 
            /// STATUS_ROLLBACK_TIMER_EXPIRED
            /// </summary>
            STATUS_ROLLBACK_TIMER_EXPIRED = 0xc019003c,

            /// <summary> 
            /// STATUS_TXF_ATTRIBUTE_CORRUPT
            /// </summary>
            STATUS_TXF_ATTRIBUTE_CORRUPT = 0xc019003d,

            /// <summary> 
            /// STATUS_EFS_NOT_ALLOWED_IN_TRANSACTION
            /// </summary>
            STATUS_EFS_NOT_ALLOWED_IN_TRANSACTION = 0xc019003e,

            /// <summary> 
            /// STATUS_TRANSACTIONAL_OPEN_NOT_ALLOWED
            /// </summary>
            STATUS_TRANSACTIONAL_OPEN_NOT_ALLOWED = 0xc019003f,

            /// <summary> 
            /// STATUS_TRANSACTED_MAPPING_UNSUPPORTED_REMOTE
            /// </summary>
            STATUS_TRANSACTED_MAPPING_UNSUPPORTED_REMOTE = 0xc0190040,

            /// <summary> 
            /// STATUS_TXF_METADATA_ALREADY_PRESENT
            /// </summary>
            STATUS_TXF_METADATA_ALREADY_PRESENT = 0x80190041,

            /// <summary> 
            /// STATUS_TRANSACTION_SCOPE_CALLBACKS_NOT_SET
            /// </summary>
            STATUS_TRANSACTION_SCOPE_CALLBACKS_NOT_SET = 0x80190042,

            /// <summary> 
            /// STATUS_TRANSACTION_REQUIRED_PROMOTION
            /// </summary>
            STATUS_TRANSACTION_REQUIRED_PROMOTION = 0xc0190043,

            /// <summary> 
            /// STATUS_CANNOT_EXECUTE_FILE_IN_TRANSACTION
            /// </summary>
            STATUS_CANNOT_EXECUTE_FILE_IN_TRANSACTION = 0xc0190044,

            /// <summary> 
            /// STATUS_TRANSACTIONS_NOT_FROZEN
            /// </summary>
            STATUS_TRANSACTIONS_NOT_FROZEN = 0xc0190045,

            /// <summary> 
            /// STATUS_TRANSACTION_FREEZE_IN_PROGRESS
            /// </summary>
            STATUS_TRANSACTION_FREEZE_IN_PROGRESS = 0xc0190046,

            /// <summary> 
            /// STATUS_NOT_SNAPSHOT_VOLUME
            /// </summary>
            STATUS_NOT_SNAPSHOT_VOLUME = 0xc0190047,

            /// <summary> 
            /// STATUS_NO_SAVEPOINT_WITH_OPEN_FILES
            /// </summary>
            STATUS_NO_SAVEPOINT_WITH_OPEN_FILES = 0xc0190048,

            /// <summary> 
            /// STATUS_SPARSE_NOT_ALLOWED_IN_TRANSACTION
            /// </summary>
            STATUS_SPARSE_NOT_ALLOWED_IN_TRANSACTION = 0xc0190049,

            /// <summary> 
            /// STATUS_TM_IDENTITY_MISMATCH
            /// </summary>
            STATUS_TM_IDENTITY_MISMATCH = 0xc019004a,

            /// <summary> 
            /// STATUS_FLOATED_SECTION
            /// </summary>
            STATUS_FLOATED_SECTION = 0xc019004b,

            /// <summary> 
            /// STATUS_CANNOT_ACCEPT_TRANSACTED_WORK
            /// </summary>
            STATUS_CANNOT_ACCEPT_TRANSACTED_WORK = 0xc019004c,

            /// <summary> 
            /// STATUS_CANNOT_ABORT_TRANSACTIONS
            /// </summary>
            STATUS_CANNOT_ABORT_TRANSACTIONS = 0xc019004d,

            /// <summary> 
            /// STATUS_TRANSACTION_NOT_FOUND
            /// </summary>
            STATUS_TRANSACTION_NOT_FOUND = 0xc019004e,

            /// <summary> 
            /// STATUS_RESOURCEMANAGER_NOT_FOUND
            /// </summary>
            STATUS_RESOURCEMANAGER_NOT_FOUND = 0xc019004f,

            /// <summary> 
            /// STATUS_ENLISTMENT_NOT_FOUND
            /// </summary>
            STATUS_ENLISTMENT_NOT_FOUND = 0xc0190050,

            /// <summary> 
            /// STATUS_TRANSACTIONMANAGER_NOT_FOUND
            /// </summary>
            STATUS_TRANSACTIONMANAGER_NOT_FOUND = 0xc0190051,

            /// <summary> 
            /// STATUS_TRANSACTIONMANAGER_NOT_ONLINE
            /// </summary>
            STATUS_TRANSACTIONMANAGER_NOT_ONLINE = 0xc0190052,

            /// <summary> 
            /// STATUS_TRANSACTIONMANAGER_RECOVERY_NAME_COLLISION
            /// </summary>
            STATUS_TRANSACTIONMANAGER_RECOVERY_NAME_COLLISION = 0xc0190053,

            /// <summary> 
            /// STATUS_TRANSACTION_NOT_ROOT
            /// </summary>
            STATUS_TRANSACTION_NOT_ROOT = 0xc0190054,

            /// <summary> 
            /// STATUS_TRANSACTION_OBJECT_EXPIRED
            /// </summary>
            STATUS_TRANSACTION_OBJECT_EXPIRED = 0xc0190055,

            /// <summary> 
            /// STATUS_COMPRESSION_NOT_ALLOWED_IN_TRANSACTION
            /// </summary>
            STATUS_COMPRESSION_NOT_ALLOWED_IN_TRANSACTION = 0xc0190056,

            /// <summary> 
            /// STATUS_TRANSACTION_RESPONSE_NOT_ENLISTED
            /// </summary>
            STATUS_TRANSACTION_RESPONSE_NOT_ENLISTED = 0xc0190057,

            /// <summary> 
            /// STATUS_TRANSACTION_RECORD_TOO_LONG
            /// </summary>
            STATUS_TRANSACTION_RECORD_TOO_LONG = 0xc0190058,

            /// <summary> 
            /// STATUS_NO_LINK_TRACKING_IN_TRANSACTION
            /// </summary>
            STATUS_NO_LINK_TRACKING_IN_TRANSACTION = 0xc0190059,

            /// <summary> 
            /// STATUS_OPERATION_NOT_SUPPORTED_IN_TRANSACTION
            /// </summary>
            STATUS_OPERATION_NOT_SUPPORTED_IN_TRANSACTION = 0xc019005a,

            /// <summary> 
            /// STATUS_TRANSACTION_INTEGRITY_VIOLATED
            /// </summary>
            STATUS_TRANSACTION_INTEGRITY_VIOLATED = 0xc019005b,

            /// <summary> 
            /// STATUS_TRANSACTIONMANAGER_IDENTITY_MISMATCH
            /// </summary>
            STATUS_TRANSACTIONMANAGER_IDENTITY_MISMATCH = 0xc019005c,

            /// <summary> 
            /// STATUS_RM_CANNOT_BE_FROZEN_FOR_SNAPSHOT
            /// </summary>
            STATUS_RM_CANNOT_BE_FROZEN_FOR_SNAPSHOT = 0xc019005d,

            /// <summary> 
            /// STATUS_TRANSACTION_MUST_WRITETHROUGH
            /// </summary>
            STATUS_TRANSACTION_MUST_WRITETHROUGH = 0xc019005e,

            /// <summary> 
            /// STATUS_TRANSACTION_NO_SUPERIOR
            /// </summary>
            STATUS_TRANSACTION_NO_SUPERIOR = 0xc019005f,

            /// <summary> 
            /// STATUS_LOG_SECTOR_INVALID
            /// </summary>
            STATUS_LOG_SECTOR_INVALID = 0xc01a0001,

            /// <summary> 
            /// STATUS_LOG_SECTOR_PARITY_INVALID
            /// </summary>
            STATUS_LOG_SECTOR_PARITY_INVALID = 0xc01a0002,

            /// <summary> 
            /// STATUS_LOG_SECTOR_REMAPPED
            /// </summary>
            STATUS_LOG_SECTOR_REMAPPED = 0xc01a0003,

            /// <summary> 
            /// STATUS_LOG_BLOCK_INCOMPLETE
            /// </summary>
            STATUS_LOG_BLOCK_INCOMPLETE = 0xc01a0004,

            /// <summary> 
            /// STATUS_LOG_INVALID_RANGE
            /// </summary>
            STATUS_LOG_INVALID_RANGE = 0xc01a0005,

            /// <summary> 
            /// STATUS_LOG_BLOCKS_EXHAUSTED
            /// </summary>
            STATUS_LOG_BLOCKS_EXHAUSTED = 0xc01a0006,

            /// <summary> 
            /// STATUS_LOG_READ_CONTEXT_INVALID
            /// </summary>
            STATUS_LOG_READ_CONTEXT_INVALID = 0xc01a0007,

            /// <summary> 
            /// STATUS_LOG_RESTART_INVALID
            /// </summary>
            STATUS_LOG_RESTART_INVALID = 0xc01a0008,

            /// <summary> 
            /// STATUS_LOG_BLOCK_VERSION
            /// </summary>
            STATUS_LOG_BLOCK_VERSION = 0xc01a0009,

            /// <summary> 
            /// STATUS_LOG_BLOCK_INVALID
            /// </summary>
            STATUS_LOG_BLOCK_INVALID = 0xc01a000a,

            /// <summary> 
            /// STATUS_LOG_READ_MODE_INVALID
            /// </summary>
            STATUS_LOG_READ_MODE_INVALID = 0xc01a000b,

            /// <summary> 
            /// STATUS_LOG_NO_RESTART
            /// </summary>
            STATUS_LOG_NO_RESTART = 0x401a000c,

            /// <summary> 
            /// STATUS_LOG_METADATA_CORRUPT
            /// </summary>
            STATUS_LOG_METADATA_CORRUPT = 0xc01a000d,

            /// <summary> 
            /// STATUS_LOG_METADATA_INVALID
            /// </summary>
            STATUS_LOG_METADATA_INVALID = 0xc01a000e,

            /// <summary> 
            /// STATUS_LOG_METADATA_INCONSISTENT
            /// </summary>
            STATUS_LOG_METADATA_INCONSISTENT = 0xc01a000f,

            /// <summary> 
            /// STATUS_LOG_RESERVATION_INVALID
            /// </summary>
            STATUS_LOG_RESERVATION_INVALID = 0xc01a0010,

            /// <summary> 
            /// STATUS_LOG_CANT_DELETE
            /// </summary>
            STATUS_LOG_CANT_DELETE = 0xc01a0011,

            /// <summary> 
            /// STATUS_LOG_CONTAINER_LIMIT_EXCEEDED
            /// </summary>
            STATUS_LOG_CONTAINER_LIMIT_EXCEEDED = 0xc01a0012,

            /// <summary> 
            /// STATUS_LOG_START_OF_LOG
            /// </summary>
            STATUS_LOG_START_OF_LOG = 0xc01a0013,

            /// <summary> 
            /// STATUS_LOG_POLICY_ALREADY_INSTALLED
            /// </summary>
            STATUS_LOG_POLICY_ALREADY_INSTALLED = 0xc01a0014,

            /// <summary> 
            /// STATUS_LOG_POLICY_NOT_INSTALLED
            /// </summary>
            STATUS_LOG_POLICY_NOT_INSTALLED = 0xc01a0015,

            /// <summary> 
            /// STATUS_LOG_POLICY_INVALID
            /// </summary>
            STATUS_LOG_POLICY_INVALID = 0xc01a0016,

            /// <summary> 
            /// STATUS_LOG_POLICY_CONFLICT
            /// </summary>
            STATUS_LOG_POLICY_CONFLICT = 0xc01a0017,

            /// <summary> 
            /// STATUS_LOG_PINNED_ARCHIVE_TAIL
            /// </summary>
            STATUS_LOG_PINNED_ARCHIVE_TAIL = 0xc01a0018,

            /// <summary> 
            /// STATUS_LOG_RECORD_NONEXISTENT
            /// </summary>
            STATUS_LOG_RECORD_NONEXISTENT = 0xc01a0019,

            /// <summary> 
            /// STATUS_LOG_RECORDS_RESERVED_INVALID
            /// </summary>
            STATUS_LOG_RECORDS_RESERVED_INVALID = 0xc01a001a,

            /// <summary> 
            /// STATUS_LOG_SPACE_RESERVED_INVALID
            /// </summary>
            STATUS_LOG_SPACE_RESERVED_INVALID = 0xc01a001b,

            /// <summary> 
            /// STATUS_LOG_TAIL_INVALID
            /// </summary>
            STATUS_LOG_TAIL_INVALID = 0xc01a001c,

            /// <summary> 
            /// STATUS_LOG_FULL
            /// </summary>
            STATUS_LOG_FULL = 0xc01a001d,

            /// <summary> 
            /// STATUS_LOG_MULTIPLEXED
            /// </summary>
            STATUS_LOG_MULTIPLEXED = 0xc01a001e,

            /// <summary> 
            /// STATUS_LOG_DEDICATED
            /// </summary>
            STATUS_LOG_DEDICATED = 0xc01a001f,

            /// <summary> 
            /// STATUS_LOG_ARCHIVE_NOT_IN_PROGRESS
            /// </summary>
            STATUS_LOG_ARCHIVE_NOT_IN_PROGRESS = 0xc01a0020,

            /// <summary> 
            /// STATUS_LOG_ARCHIVE_IN_PROGRESS
            /// </summary>
            STATUS_LOG_ARCHIVE_IN_PROGRESS = 0xc01a0021,

            /// <summary> 
            /// STATUS_LOG_EPHEMERAL
            /// </summary>
            STATUS_LOG_EPHEMERAL = 0xc01a0022,

            /// <summary> 
            /// STATUS_LOG_NOT_ENOUGH_CONTAINERS
            /// </summary>
            STATUS_LOG_NOT_ENOUGH_CONTAINERS = 0xc01a0023,

            /// <summary> 
            /// STATUS_LOG_CLIENT_ALREADY_REGISTERED
            /// </summary>
            STATUS_LOG_CLIENT_ALREADY_REGISTERED = 0xc01a0024,

            /// <summary> 
            /// STATUS_LOG_CLIENT_NOT_REGISTERED
            /// </summary>
            STATUS_LOG_CLIENT_NOT_REGISTERED = 0xc01a0025,

            /// <summary> 
            /// STATUS_LOG_FULL_HANDLER_IN_PROGRESS
            /// </summary>
            STATUS_LOG_FULL_HANDLER_IN_PROGRESS = 0xc01a0026,

            /// <summary> 
            /// STATUS_LOG_CONTAINER_READ_FAILED
            /// </summary>
            STATUS_LOG_CONTAINER_READ_FAILED = 0xc01a0027,

            /// <summary> 
            /// STATUS_LOG_CONTAINER_WRITE_FAILED
            /// </summary>
            STATUS_LOG_CONTAINER_WRITE_FAILED = 0xc01a0028,

            /// <summary> 
            /// STATUS_LOG_CONTAINER_OPEN_FAILED
            /// </summary>
            STATUS_LOG_CONTAINER_OPEN_FAILED = 0xc01a0029,

            /// <summary> 
            /// STATUS_LOG_CONTAINER_STATE_INVALID
            /// </summary>
            STATUS_LOG_CONTAINER_STATE_INVALID = 0xc01a002a,

            /// <summary> 
            /// STATUS_LOG_STATE_INVALID
            /// </summary>
            STATUS_LOG_STATE_INVALID = 0xc01a002b,

            /// <summary> 
            /// STATUS_LOG_PINNED
            /// </summary>
            STATUS_LOG_PINNED = 0xc01a002c,

            /// <summary> 
            /// STATUS_LOG_METADATA_FLUSH_FAILED
            /// </summary>
            STATUS_LOG_METADATA_FLUSH_FAILED = 0xc01a002d,

            /// <summary> 
            /// STATUS_LOG_INCONSISTENT_SECURITY
            /// </summary>
            STATUS_LOG_INCONSISTENT_SECURITY = 0xc01a002e,

            /// <summary> 
            /// STATUS_LOG_APPENDED_FLUSH_FAILED
            /// </summary>
            STATUS_LOG_APPENDED_FLUSH_FAILED = 0xc01a002f,

            /// <summary> 
            /// STATUS_LOG_PINNED_RESERVATION
            /// </summary>
            STATUS_LOG_PINNED_RESERVATION = 0xc01a0030,

            /// <summary> 
            /// STATUS_VIDEO_HUNG_DISPLAY_DRIVER_THREAD
            /// </summary>
            STATUS_VIDEO_HUNG_DISPLAY_DRIVER_THREAD = 0xc01b00ea,

            /// <summary> 
            /// STATUS_VIDEO_HUNG_DISPLAY_DRIVER_THREAD_RECOVERED
            /// </summary>
            STATUS_VIDEO_HUNG_DISPLAY_DRIVER_THREAD_RECOVERED = 0x801b00eb,

            /// <summary> 
            /// STATUS_VIDEO_DRIVER_DEBUG_REPORT_REQUEST
            /// </summary>
            STATUS_VIDEO_DRIVER_DEBUG_REPORT_REQUEST = 0x401b00ec,

            /// <summary> 
            /// STATUS_MONITOR_NO_DESCRIPTOR
            /// </summary>
            STATUS_MONITOR_NO_DESCRIPTOR = 0xc01d0001,

            /// <summary> 
            /// STATUS_MONITOR_UNKNOWN_DESCRIPTOR_FORMAT
            /// </summary>
            STATUS_MONITOR_UNKNOWN_DESCRIPTOR_FORMAT = 0xc01d0002,

            /// <summary> 
            /// STATUS_MONITOR_INVALID_DESCRIPTOR_CHECKSUM
            /// </summary>
            STATUS_MONITOR_INVALID_DESCRIPTOR_CHECKSUM = 0xc01d0003,

            /// <summary> 
            /// STATUS_MONITOR_INVALID_STANDARD_TIMING_BLOCK
            /// </summary>
            STATUS_MONITOR_INVALID_STANDARD_TIMING_BLOCK = 0xc01d0004,

            /// <summary> 
            /// STATUS_MONITOR_WMI_DATABLOCK_REGISTRATION_FAILED
            /// </summary>
            STATUS_MONITOR_WMI_DATABLOCK_REGISTRATION_FAILED = 0xc01d0005,

            /// <summary> 
            /// STATUS_MONITOR_INVALID_SERIAL_NUMBER_MONDSC_BLOCK
            /// </summary>
            STATUS_MONITOR_INVALID_SERIAL_NUMBER_MONDSC_BLOCK = 0xc01d0006,

            /// <summary> 
            /// STATUS_MONITOR_INVALID_USER_FRIENDLY_MONDSC_BLOCK
            /// </summary>
            STATUS_MONITOR_INVALID_USER_FRIENDLY_MONDSC_BLOCK = 0xc01d0007,

            /// <summary> 
            /// STATUS_MONITOR_NO_MORE_DESCRIPTOR_DATA
            /// </summary>
            STATUS_MONITOR_NO_MORE_DESCRIPTOR_DATA = 0xc01d0008,

            /// <summary> 
            /// STATUS_MONITOR_INVALID_DETAILED_TIMING_BLOCK
            /// </summary>
            STATUS_MONITOR_INVALID_DETAILED_TIMING_BLOCK = 0xc01d0009,

            /// <summary> 
            /// STATUS_GRAPHICS_NOT_EXCLUSIVE_MODE_OWNER
            /// </summary>
            STATUS_GRAPHICS_NOT_EXCLUSIVE_MODE_OWNER = 0xc01e0000,

            /// <summary> 
            /// STATUS_GRAPHICS_INSUFFICIENT_DMA_BUFFER
            /// </summary>
            STATUS_GRAPHICS_INSUFFICIENT_DMA_BUFFER = 0xc01e0001,

            /// <summary> 
            /// STATUS_GRAPHICS_INVALID_DISPLAY_ADAPTER
            /// </summary>
            STATUS_GRAPHICS_INVALID_DISPLAY_ADAPTER = 0xc01e0002,

            /// <summary> 
            /// STATUS_GRAPHICS_ADAPTER_WAS_RESET
            /// </summary>
            STATUS_GRAPHICS_ADAPTER_WAS_RESET = 0xc01e0003,

            /// <summary> 
            /// STATUS_GRAPHICS_INVALID_DRIVER_MODEL
            /// </summary>
            STATUS_GRAPHICS_INVALID_DRIVER_MODEL = 0xc01e0004,

            /// <summary> 
            /// STATUS_GRAPHICS_PRESENT_MODE_CHANGED
            /// </summary>
            STATUS_GRAPHICS_PRESENT_MODE_CHANGED = 0xc01e0005,

            /// <summary> 
            /// STATUS_GRAPHICS_PRESENT_OCCLUDED
            /// </summary>
            STATUS_GRAPHICS_PRESENT_OCCLUDED = 0xc01e0006,

            /// <summary> 
            /// STATUS_GRAPHICS_PRESENT_DENIED
            /// </summary>
            STATUS_GRAPHICS_PRESENT_DENIED = 0xc01e0007,

            /// <summary> 
            /// STATUS_GRAPHICS_CANNOTCOLORCONVERT
            /// </summary>
            STATUS_GRAPHICS_CANNOTCOLORCONVERT = 0xc01e0008,

            /// <summary> 
            /// STATUS_GRAPHICS_DRIVER_MISMATCH
            /// </summary>
            STATUS_GRAPHICS_DRIVER_MISMATCH = 0xc01e0009,

            /// <summary> 
            /// STATUS_GRAPHICS_PARTIAL_DATA_POPULATED
            /// </summary>
            STATUS_GRAPHICS_PARTIAL_DATA_POPULATED = 0x401e000a,

            /// <summary> 
            /// STATUS_GRAPHICS_NO_VIDEO_MEMORY
            /// </summary>
            STATUS_GRAPHICS_NO_VIDEO_MEMORY = 0xc01e0100,

            /// <summary> 
            /// STATUS_GRAPHICS_CANT_LOCK_MEMORY
            /// </summary>
            STATUS_GRAPHICS_CANT_LOCK_MEMORY = 0xc01e0101,

            /// <summary> 
            /// STATUS_GRAPHICS_ALLOCATION_BUSY
            /// </summary>
            STATUS_GRAPHICS_ALLOCATION_BUSY = 0xc01e0102,

            /// <summary> 
            /// STATUS_GRAPHICS_TOO_MANY_REFERENCES
            /// </summary>
            STATUS_GRAPHICS_TOO_MANY_REFERENCES = 0xc01e0103,

            /// <summary> 
            /// STATUS_GRAPHICS_TRY_AGAIN_LATER
            /// </summary>
            STATUS_GRAPHICS_TRY_AGAIN_LATER = 0xc01e0104,

            /// <summary> 
            /// STATUS_GRAPHICS_TRY_AGAIN_NOW
            /// </summary>
            STATUS_GRAPHICS_TRY_AGAIN_NOW = 0xc01e0105,

            /// <summary> 
            /// STATUS_GRAPHICS_ALLOCATION_INVALID
            /// </summary>
            STATUS_GRAPHICS_ALLOCATION_INVALID = 0xc01e0106,

            /// <summary> 
            /// STATUS_GRAPHICS_UNSWIZZLING_APERTURE_UNAVAILABLE
            /// </summary>
            STATUS_GRAPHICS_UNSWIZZLING_APERTURE_UNAVAILABLE = 0xc01e0107,

            /// <summary> 
            /// STATUS_GRAPHICS_UNSWIZZLING_APERTURE_UNSUPPORTED
            /// </summary>
            STATUS_GRAPHICS_UNSWIZZLING_APERTURE_UNSUPPORTED = 0xc01e0108,

            /// <summary> 
            /// STATUS_GRAPHICS_CANT_EVICT_PINNED_ALLOCATION
            /// </summary>
            STATUS_GRAPHICS_CANT_EVICT_PINNED_ALLOCATION = 0xc01e0109,

            /// <summary> 
            /// STATUS_GRAPHICS_INVALID_ALLOCATION_USAGE
            /// </summary>
            STATUS_GRAPHICS_INVALID_ALLOCATION_USAGE = 0xc01e0110,

            /// <summary> 
            /// STATUS_GRAPHICS_CANT_RENDER_LOCKED_ALLOCATION
            /// </summary>
            STATUS_GRAPHICS_CANT_RENDER_LOCKED_ALLOCATION = 0xc01e0111,

            /// <summary> 
            /// STATUS_GRAPHICS_ALLOCATION_CLOSED
            /// </summary>
            STATUS_GRAPHICS_ALLOCATION_CLOSED = 0xc01e0112,

            /// <summary> 
            /// STATUS_GRAPHICS_INVALID_ALLOCATION_INSTANCE
            /// </summary>
            STATUS_GRAPHICS_INVALID_ALLOCATION_INSTANCE = 0xc01e0113,

            /// <summary> 
            /// STATUS_GRAPHICS_INVALID_ALLOCATION_HANDLE
            /// </summary>
            STATUS_GRAPHICS_INVALID_ALLOCATION_HANDLE = 0xc01e0114,

            /// <summary> 
            /// STATUS_GRAPHICS_WRONG_ALLOCATION_DEVICE
            /// </summary>
            STATUS_GRAPHICS_WRONG_ALLOCATION_DEVICE = 0xc01e0115,

            /// <summary> 
            /// STATUS_GRAPHICS_ALLOCATION_CONTENT_LOST
            /// </summary>
            STATUS_GRAPHICS_ALLOCATION_CONTENT_LOST = 0xc01e0116,

            /// <summary> 
            /// STATUS_GRAPHICS_GPU_EXCEPTION_ON_DEVICE
            /// </summary>
            STATUS_GRAPHICS_GPU_EXCEPTION_ON_DEVICE = 0xc01e0200,

            /// <summary> 
            /// STATUS_GRAPHICS_INVALID_VIDPN_TOPOLOGY
            /// </summary>
            STATUS_GRAPHICS_INVALID_VIDPN_TOPOLOGY = 0xc01e0300,

            /// <summary> 
            /// STATUS_GRAPHICS_VIDPN_TOPOLOGY_NOT_SUPPORTED
            /// </summary>
            STATUS_GRAPHICS_VIDPN_TOPOLOGY_NOT_SUPPORTED = 0xc01e0301,

            /// <summary> 
            /// STATUS_GRAPHICS_VIDPN_TOPOLOGY_CURRENTLY_NOT_SUPPORTED
            /// </summary>
            STATUS_GRAPHICS_VIDPN_TOPOLOGY_CURRENTLY_NOT_SUPPORTED = 0xc01e0302,

            /// <summary> 
            /// STATUS_GRAPHICS_INVALID_VIDPN
            /// </summary>
            STATUS_GRAPHICS_INVALID_VIDPN = 0xc01e0303,

            /// <summary> 
            /// STATUS_GRAPHICS_INVALID_VIDEO_PRESENT_SOURCE
            /// </summary>
            STATUS_GRAPHICS_INVALID_VIDEO_PRESENT_SOURCE = 0xc01e0304,

            /// <summary> 
            /// STATUS_GRAPHICS_INVALID_VIDEO_PRESENT_TARGET
            /// </summary>
            STATUS_GRAPHICS_INVALID_VIDEO_PRESENT_TARGET = 0xc01e0305,

            /// <summary> 
            /// STATUS_GRAPHICS_VIDPN_MODALITY_NOT_SUPPORTED
            /// </summary>
            STATUS_GRAPHICS_VIDPN_MODALITY_NOT_SUPPORTED = 0xc01e0306,

            /// <summary> 
            /// STATUS_GRAPHICS_MODE_NOT_PINNED
            /// </summary>
            STATUS_GRAPHICS_MODE_NOT_PINNED = 0x401e0307,

            /// <summary> 
            /// STATUS_GRAPHICS_INVALID_VIDPN_SOURCEMODESET
            /// </summary>
            STATUS_GRAPHICS_INVALID_VIDPN_SOURCEMODESET = 0xc01e0308,

            /// <summary> 
            /// STATUS_GRAPHICS_INVALID_VIDPN_TARGETMODESET
            /// </summary>
            STATUS_GRAPHICS_INVALID_VIDPN_TARGETMODESET = 0xc01e0309,

            /// <summary> 
            /// STATUS_GRAPHICS_INVALID_FREQUENCY
            /// </summary>
            STATUS_GRAPHICS_INVALID_FREQUENCY = 0xc01e030a,

            /// <summary> 
            /// STATUS_GRAPHICS_INVALID_ACTIVE_REGION
            /// </summary>
            STATUS_GRAPHICS_INVALID_ACTIVE_REGION = 0xc01e030b,

            /// <summary> 
            /// STATUS_GRAPHICS_INVALID_TOTAL_REGION
            /// </summary>
            STATUS_GRAPHICS_INVALID_TOTAL_REGION = 0xc01e030c,

            /// <summary> 
            /// STATUS_GRAPHICS_INVALID_VIDEO_PRESENT_SOURCE_MODE
            /// </summary>
            STATUS_GRAPHICS_INVALID_VIDEO_PRESENT_SOURCE_MODE = 0xc01e0310,

            /// <summary> 
            /// STATUS_GRAPHICS_INVALID_VIDEO_PRESENT_TARGET_MODE
            /// </summary>
            STATUS_GRAPHICS_INVALID_VIDEO_PRESENT_TARGET_MODE = 0xc01e0311,

            /// <summary> 
            /// STATUS_GRAPHICS_PINNED_MODE_MUST_REMAIN_IN_SET
            /// </summary>
            STATUS_GRAPHICS_PINNED_MODE_MUST_REMAIN_IN_SET = 0xc01e0312,

            /// <summary> 
            /// STATUS_GRAPHICS_PATH_ALREADY_IN_TOPOLOGY
            /// </summary>
            STATUS_GRAPHICS_PATH_ALREADY_IN_TOPOLOGY = 0xc01e0313,

            /// <summary> 
            /// STATUS_GRAPHICS_MODE_ALREADY_IN_MODESET
            /// </summary>
            STATUS_GRAPHICS_MODE_ALREADY_IN_MODESET = 0xc01e0314,

            /// <summary> 
            /// STATUS_GRAPHICS_INVALID_VIDEOPRESENTSOURCESET
            /// </summary>
            STATUS_GRAPHICS_INVALID_VIDEOPRESENTSOURCESET = 0xc01e0315,

            /// <summary> 
            /// STATUS_GRAPHICS_INVALID_VIDEOPRESENTTARGETSET
            /// </summary>
            STATUS_GRAPHICS_INVALID_VIDEOPRESENTTARGETSET = 0xc01e0316,

            /// <summary> 
            /// STATUS_GRAPHICS_SOURCE_ALREADY_IN_SET
            /// </summary>
            STATUS_GRAPHICS_SOURCE_ALREADY_IN_SET = 0xc01e0317,

            /// <summary> 
            /// STATUS_GRAPHICS_TARGET_ALREADY_IN_SET
            /// </summary>
            STATUS_GRAPHICS_TARGET_ALREADY_IN_SET = 0xc01e0318,

            /// <summary> 
            /// STATUS_GRAPHICS_INVALID_VIDPN_PRESENT_PATH
            /// </summary>
            STATUS_GRAPHICS_INVALID_VIDPN_PRESENT_PATH = 0xc01e0319,

            /// <summary> 
            /// STATUS_GRAPHICS_NO_RECOMMENDED_VIDPN_TOPOLOGY
            /// </summary>
            STATUS_GRAPHICS_NO_RECOMMENDED_VIDPN_TOPOLOGY = 0xc01e031a,

            /// <summary> 
            /// STATUS_GRAPHICS_INVALID_MONITOR_FREQUENCYRANGESET
            /// </summary>
            STATUS_GRAPHICS_INVALID_MONITOR_FREQUENCYRANGESET = 0xc01e031b,

            /// <summary> 
            /// STATUS_GRAPHICS_INVALID_MONITOR_FREQUENCYRANGE
            /// </summary>
            STATUS_GRAPHICS_INVALID_MONITOR_FREQUENCYRANGE = 0xc01e031c,

            /// <summary> 
            /// STATUS_GRAPHICS_FREQUENCYRANGE_NOT_IN_SET
            /// </summary>
            STATUS_GRAPHICS_FREQUENCYRANGE_NOT_IN_SET = 0xc01e031d,

            /// <summary> 
            /// STATUS_GRAPHICS_NO_PREFERRED_MODE
            /// </summary>
            STATUS_GRAPHICS_NO_PREFERRED_MODE = 0x401e031e,

            /// <summary> 
            /// STATUS_GRAPHICS_FREQUENCYRANGE_ALREADY_IN_SET
            /// </summary>
            STATUS_GRAPHICS_FREQUENCYRANGE_ALREADY_IN_SET = 0xc01e031f,

            /// <summary> 
            /// STATUS_GRAPHICS_STALE_MODESET
            /// </summary>
            STATUS_GRAPHICS_STALE_MODESET = 0xc01e0320,

            /// <summary> 
            /// STATUS_GRAPHICS_INVALID_MONITOR_SOURCEMODESET
            /// </summary>
            STATUS_GRAPHICS_INVALID_MONITOR_SOURCEMODESET = 0xc01e0321,

            /// <summary> 
            /// STATUS_GRAPHICS_INVALID_MONITOR_SOURCE_MODE
            /// </summary>
            STATUS_GRAPHICS_INVALID_MONITOR_SOURCE_MODE = 0xc01e0322,

            /// <summary> 
            /// STATUS_GRAPHICS_NO_RECOMMENDED_FUNCTIONAL_VIDPN
            /// </summary>
            STATUS_GRAPHICS_NO_RECOMMENDED_FUNCTIONAL_VIDPN = 0xc01e0323,

            /// <summary> 
            /// STATUS_GRAPHICS_MODE_ID_MUST_BE_UNIQUE
            /// </summary>
            STATUS_GRAPHICS_MODE_ID_MUST_BE_UNIQUE = 0xc01e0324,

            /// <summary> 
            /// STATUS_GRAPHICS_EMPTY_ADAPTER_MONITOR_MODE_SUPPORT_INTERSECTION
            /// </summary>
            STATUS_GRAPHICS_EMPTY_ADAPTER_MONITOR_MODE_SUPPORT_INTERSECTION = 0xc01e0325,

            /// <summary> 
            /// STATUS_GRAPHICS_VIDEO_PRESENT_TARGETS_LESS_THAN_SOURCES
            /// </summary>
            STATUS_GRAPHICS_VIDEO_PRESENT_TARGETS_LESS_THAN_SOURCES = 0xc01e0326,

            /// <summary> 
            /// STATUS_GRAPHICS_PATH_NOT_IN_TOPOLOGY
            /// </summary>
            STATUS_GRAPHICS_PATH_NOT_IN_TOPOLOGY = 0xc01e0327,

            /// <summary> 
            /// STATUS_GRAPHICS_ADAPTER_MUST_HAVE_AT_LEAST_ONE_SOURCE
            /// </summary>
            STATUS_GRAPHICS_ADAPTER_MUST_HAVE_AT_LEAST_ONE_SOURCE = 0xc01e0328,

            /// <summary> 
            /// STATUS_GRAPHICS_ADAPTER_MUST_HAVE_AT_LEAST_ONE_TARGET
            /// </summary>
            STATUS_GRAPHICS_ADAPTER_MUST_HAVE_AT_LEAST_ONE_TARGET = 0xc01e0329,

            /// <summary> 
            /// STATUS_GRAPHICS_INVALID_MONITORDESCRIPTORSET
            /// </summary>
            STATUS_GRAPHICS_INVALID_MONITORDESCRIPTORSET = 0xc01e032a,

            /// <summary> 
            /// STATUS_GRAPHICS_INVALID_MONITORDESCRIPTOR
            /// </summary>
            STATUS_GRAPHICS_INVALID_MONITORDESCRIPTOR = 0xc01e032b,

            /// <summary> 
            /// STATUS_GRAPHICS_MONITORDESCRIPTOR_NOT_IN_SET
            /// </summary>
            STATUS_GRAPHICS_MONITORDESCRIPTOR_NOT_IN_SET = 0xc01e032c,

            /// <summary> 
            /// STATUS_GRAPHICS_MONITORDESCRIPTOR_ALREADY_IN_SET
            /// </summary>
            STATUS_GRAPHICS_MONITORDESCRIPTOR_ALREADY_IN_SET = 0xc01e032d,

            /// <summary> 
            /// STATUS_GRAPHICS_MONITORDESCRIPTOR_ID_MUST_BE_UNIQUE
            /// </summary>
            STATUS_GRAPHICS_MONITORDESCRIPTOR_ID_MUST_BE_UNIQUE = 0xc01e032e,

            /// <summary> 
            /// STATUS_GRAPHICS_INVALID_VIDPN_TARGET_SUBSET_TYPE
            /// </summary>
            STATUS_GRAPHICS_INVALID_VIDPN_TARGET_SUBSET_TYPE = 0xc01e032f,

            /// <summary> 
            /// STATUS_GRAPHICS_RESOURCES_NOT_RELATED
            /// </summary>
            STATUS_GRAPHICS_RESOURCES_NOT_RELATED = 0xc01e0330,

            /// <summary> 
            /// STATUS_GRAPHICS_SOURCE_ID_MUST_BE_UNIQUE
            /// </summary>
            STATUS_GRAPHICS_SOURCE_ID_MUST_BE_UNIQUE = 0xc01e0331,

            /// <summary> 
            /// STATUS_GRAPHICS_TARGET_ID_MUST_BE_UNIQUE
            /// </summary>
            STATUS_GRAPHICS_TARGET_ID_MUST_BE_UNIQUE = 0xc01e0332,

            /// <summary> 
            /// STATUS_GRAPHICS_NO_AVAILABLE_VIDPN_TARGET
            /// </summary>
            STATUS_GRAPHICS_NO_AVAILABLE_VIDPN_TARGET = 0xc01e0333,

            /// <summary> 
            /// STATUS_GRAPHICS_MONITOR_COULD_NOT_BE_ASSOCIATED_WITH_ADAPTER
            /// </summary>
            STATUS_GRAPHICS_MONITOR_COULD_NOT_BE_ASSOCIATED_WITH_ADAPTER = 0xc01e0334,

            /// <summary> 
            /// STATUS_GRAPHICS_NO_VIDPNMGR
            /// </summary>
            STATUS_GRAPHICS_NO_VIDPNMGR = 0xc01e0335,

            /// <summary> 
            /// STATUS_GRAPHICS_NO_ACTIVE_VIDPN
            /// </summary>
            STATUS_GRAPHICS_NO_ACTIVE_VIDPN = 0xc01e0336,

            /// <summary> 
            /// STATUS_GRAPHICS_STALE_VIDPN_TOPOLOGY
            /// </summary>
            STATUS_GRAPHICS_STALE_VIDPN_TOPOLOGY = 0xc01e0337,

            /// <summary> 
            /// STATUS_GRAPHICS_MONITOR_NOT_CONNECTED
            /// </summary>
            STATUS_GRAPHICS_MONITOR_NOT_CONNECTED = 0xc01e0338,

            /// <summary> 
            /// STATUS_GRAPHICS_SOURCE_NOT_IN_TOPOLOGY
            /// </summary>
            STATUS_GRAPHICS_SOURCE_NOT_IN_TOPOLOGY = 0xc01e0339,

            /// <summary> 
            /// STATUS_GRAPHICS_INVALID_PRIMARYSURFACE_SIZE
            /// </summary>
            STATUS_GRAPHICS_INVALID_PRIMARYSURFACE_SIZE = 0xc01e033a,

            /// <summary> 
            /// STATUS_GRAPHICS_INVALID_VISIBLEREGION_SIZE
            /// </summary>
            STATUS_GRAPHICS_INVALID_VISIBLEREGION_SIZE = 0xc01e033b,

            /// <summary> 
            /// STATUS_GRAPHICS_INVALID_STRIDE
            /// </summary>
            STATUS_GRAPHICS_INVALID_STRIDE = 0xc01e033c,

            /// <summary> 
            /// STATUS_GRAPHICS_INVALID_PIXELFORMAT
            /// </summary>
            STATUS_GRAPHICS_INVALID_PIXELFORMAT = 0xc01e033d,

            /// <summary> 
            /// STATUS_GRAPHICS_INVALID_COLORBASIS
            /// </summary>
            STATUS_GRAPHICS_INVALID_COLORBASIS = 0xc01e033e,

            /// <summary> 
            /// STATUS_GRAPHICS_INVALID_PIXELVALUEACCESSMODE
            /// </summary>
            STATUS_GRAPHICS_INVALID_PIXELVALUEACCESSMODE = 0xc01e033f,

            /// <summary> 
            /// STATUS_GRAPHICS_TARGET_NOT_IN_TOPOLOGY
            /// </summary>
            STATUS_GRAPHICS_TARGET_NOT_IN_TOPOLOGY = 0xc01e0340,

            /// <summary> 
            /// STATUS_GRAPHICS_NO_DISPLAY_MODE_MANAGEMENT_SUPPORT
            /// </summary>
            STATUS_GRAPHICS_NO_DISPLAY_MODE_MANAGEMENT_SUPPORT = 0xc01e0341,

            /// <summary> 
            /// STATUS_GRAPHICS_VIDPN_SOURCE_IN_USE
            /// </summary>
            STATUS_GRAPHICS_VIDPN_SOURCE_IN_USE = 0xc01e0342,

            /// <summary> 
            /// STATUS_GRAPHICS_CANT_ACCESS_ACTIVE_VIDPN
            /// </summary>
            STATUS_GRAPHICS_CANT_ACCESS_ACTIVE_VIDPN = 0xc01e0343,

            /// <summary> 
            /// STATUS_GRAPHICS_INVALID_PATH_IMPORTANCE_ORDINAL
            /// </summary>
            STATUS_GRAPHICS_INVALID_PATH_IMPORTANCE_ORDINAL = 0xc01e0344,

            /// <summary> 
            /// STATUS_GRAPHICS_INVALID_PATH_CONTENT_GEOMETRY_TRANSFORMATION
            /// </summary>
            STATUS_GRAPHICS_INVALID_PATH_CONTENT_GEOMETRY_TRANSFORMATION = 0xc01e0345,

            /// <summary> 
            /// STATUS_GRAPHICS_PATH_CONTENT_GEOMETRY_TRANSFORMATION_NOT_SUPPORTED
            /// </summary>
            STATUS_GRAPHICS_PATH_CONTENT_GEOMETRY_TRANSFORMATION_NOT_SUPPORTED = 0xc01e0346,

            /// <summary> 
            /// STATUS_GRAPHICS_INVALID_GAMMA_RAMP
            /// </summary>
            STATUS_GRAPHICS_INVALID_GAMMA_RAMP = 0xc01e0347,

            /// <summary> 
            /// STATUS_GRAPHICS_GAMMA_RAMP_NOT_SUPPORTED
            /// </summary>
            STATUS_GRAPHICS_GAMMA_RAMP_NOT_SUPPORTED = 0xc01e0348,

            /// <summary> 
            /// STATUS_GRAPHICS_MULTISAMPLING_NOT_SUPPORTED
            /// </summary>
            STATUS_GRAPHICS_MULTISAMPLING_NOT_SUPPORTED = 0xc01e0349,

            /// <summary> 
            /// STATUS_GRAPHICS_MODE_NOT_IN_MODESET
            /// </summary>
            STATUS_GRAPHICS_MODE_NOT_IN_MODESET = 0xc01e034a,

            /// <summary> 
            /// STATUS_GRAPHICS_DATASET_IS_EMPTY
            /// </summary>
            STATUS_GRAPHICS_DATASET_IS_EMPTY = 0x401e034b,

            /// <summary> 
            /// STATUS_GRAPHICS_NO_MORE_ELEMENTS_IN_DATASET
            /// </summary>
            STATUS_GRAPHICS_NO_MORE_ELEMENTS_IN_DATASET = 0x401e034c,

            /// <summary> 
            /// STATUS_GRAPHICS_INVALID_VIDPN_TOPOLOGY_RECOMMENDATION_REASON
            /// </summary>
            STATUS_GRAPHICS_INVALID_VIDPN_TOPOLOGY_RECOMMENDATION_REASON = 0xc01e034d,

            /// <summary> 
            /// STATUS_GRAPHICS_INVALID_PATH_CONTENT_TYPE
            /// </summary>
            STATUS_GRAPHICS_INVALID_PATH_CONTENT_TYPE = 0xc01e034e,

            /// <summary> 
            /// STATUS_GRAPHICS_INVALID_COPYPROTECTION_TYPE
            /// </summary>
            STATUS_GRAPHICS_INVALID_COPYPROTECTION_TYPE = 0xc01e034f,

            /// <summary> 
            /// STATUS_GRAPHICS_UNASSIGNED_MODESET_ALREADY_EXISTS
            /// </summary>
            STATUS_GRAPHICS_UNASSIGNED_MODESET_ALREADY_EXISTS = 0xc01e0350,

            /// <summary> 
            /// STATUS_GRAPHICS_PATH_CONTENT_GEOMETRY_TRANSFORMATION_NOT_PINNED
            /// </summary>
            STATUS_GRAPHICS_PATH_CONTENT_GEOMETRY_TRANSFORMATION_NOT_PINNED = 0x401e0351,

            /// <summary> 
            /// STATUS_GRAPHICS_INVALID_SCANLINE_ORDERING
            /// </summary>
            STATUS_GRAPHICS_INVALID_SCANLINE_ORDERING = 0xc01e0352,

            /// <summary> 
            /// STATUS_GRAPHICS_TOPOLOGY_CHANGES_NOT_ALLOWED
            /// </summary>
            STATUS_GRAPHICS_TOPOLOGY_CHANGES_NOT_ALLOWED = 0xc01e0353,

            /// <summary> 
            /// STATUS_GRAPHICS_NO_AVAILABLE_IMPORTANCE_ORDINALS
            /// </summary>
            STATUS_GRAPHICS_NO_AVAILABLE_IMPORTANCE_ORDINALS = 0xc01e0354,

            /// <summary> 
            /// STATUS_GRAPHICS_INCOMPATIBLE_PRIVATE_FORMAT
            /// </summary>
            STATUS_GRAPHICS_INCOMPATIBLE_PRIVATE_FORMAT = 0xc01e0355,

            /// <summary> 
            /// STATUS_GRAPHICS_INVALID_MODE_PRUNING_ALGORITHM
            /// </summary>
            STATUS_GRAPHICS_INVALID_MODE_PRUNING_ALGORITHM = 0xc01e0356,

            /// <summary> 
            /// STATUS_GRAPHICS_INVALID_MONITOR_CAPABILITY_ORIGIN
            /// </summary>
            STATUS_GRAPHICS_INVALID_MONITOR_CAPABILITY_ORIGIN = 0xc01e0357,

            /// <summary> 
            /// STATUS_GRAPHICS_INVALID_MONITOR_FREQUENCYRANGE_CONSTRAINT
            /// </summary>
            STATUS_GRAPHICS_INVALID_MONITOR_FREQUENCYRANGE_CONSTRAINT = 0xc01e0358,

            /// <summary> 
            /// STATUS_GRAPHICS_MAX_NUM_PATHS_REACHED
            /// </summary>
            STATUS_GRAPHICS_MAX_NUM_PATHS_REACHED = 0xc01e0359,

            /// <summary> 
            /// STATUS_GRAPHICS_CANCEL_VIDPN_TOPOLOGY_AUGMENTATION
            /// </summary>
            STATUS_GRAPHICS_CANCEL_VIDPN_TOPOLOGY_AUGMENTATION = 0xc01e035a,

            /// <summary> 
            /// STATUS_GRAPHICS_INVALID_CLIENT_TYPE
            /// </summary>
            STATUS_GRAPHICS_INVALID_CLIENT_TYPE = 0xc01e035b,

            /// <summary> 
            /// STATUS_GRAPHICS_CLIENTVIDPN_NOT_SET
            /// </summary>
            STATUS_GRAPHICS_CLIENTVIDPN_NOT_SET = 0xc01e035c,

            /// <summary> 
            /// STATUS_GRAPHICS_SPECIFIED_CHILD_ALREADY_CONNECTED
            /// </summary>
            STATUS_GRAPHICS_SPECIFIED_CHILD_ALREADY_CONNECTED = 0xc01e0400,

            /// <summary> 
            /// STATUS_GRAPHICS_CHILD_DESCRIPTOR_NOT_SUPPORTED
            /// </summary>
            STATUS_GRAPHICS_CHILD_DESCRIPTOR_NOT_SUPPORTED = 0xc01e0401,

            /// <summary> 
            /// STATUS_GRAPHICS_UNKNOWN_CHILD_STATUS
            /// </summary>
            STATUS_GRAPHICS_UNKNOWN_CHILD_STATUS = 0x401e042f,

            /// <summary> 
            /// STATUS_GRAPHICS_NOT_A_LINKED_ADAPTER
            /// </summary>
            STATUS_GRAPHICS_NOT_A_LINKED_ADAPTER = 0xc01e0430,

            /// <summary> 
            /// STATUS_GRAPHICS_LEADLINK_NOT_ENUMERATED
            /// </summary>
            STATUS_GRAPHICS_LEADLINK_NOT_ENUMERATED = 0xc01e0431,

            /// <summary> 
            /// STATUS_GRAPHICS_CHAINLINKS_NOT_ENUMERATED
            /// </summary>
            STATUS_GRAPHICS_CHAINLINKS_NOT_ENUMERATED = 0xc01e0432,

            /// <summary> 
            /// STATUS_GRAPHICS_ADAPTER_CHAIN_NOT_READY
            /// </summary>
            STATUS_GRAPHICS_ADAPTER_CHAIN_NOT_READY = 0xc01e0433,

            /// <summary> 
            /// STATUS_GRAPHICS_CHAINLINKS_NOT_STARTED
            /// </summary>
            STATUS_GRAPHICS_CHAINLINKS_NOT_STARTED = 0xc01e0434,

            /// <summary> 
            /// STATUS_GRAPHICS_CHAINLINKS_NOT_POWERED_ON
            /// </summary>
            STATUS_GRAPHICS_CHAINLINKS_NOT_POWERED_ON = 0xc01e0435,

            /// <summary> 
            /// STATUS_GRAPHICS_INCONSISTENT_DEVICE_LINK_STATE
            /// </summary>
            STATUS_GRAPHICS_INCONSISTENT_DEVICE_LINK_STATE = 0xc01e0436,

            /// <summary> 
            /// STATUS_GRAPHICS_LEADLINK_START_DEFERRED
            /// </summary>
            STATUS_GRAPHICS_LEADLINK_START_DEFERRED = 0x401e0437,

            /// <summary> 
            /// STATUS_GRAPHICS_NOT_POST_DEVICE_DRIVER
            /// </summary>
            STATUS_GRAPHICS_NOT_POST_DEVICE_DRIVER = 0xc01e0438,

            /// <summary> 
            /// STATUS_GRAPHICS_POLLING_TOO_FREQUENTLY
            /// </summary>
            STATUS_GRAPHICS_POLLING_TOO_FREQUENTLY = 0x401e0439,

            /// <summary> 
            /// STATUS_GRAPHICS_START_DEFERRED
            /// </summary>
            STATUS_GRAPHICS_START_DEFERRED = 0x401e043a,

            /// <summary> 
            /// STATUS_GRAPHICS_ADAPTER_ACCESS_NOT_EXCLUDED
            /// </summary>
            STATUS_GRAPHICS_ADAPTER_ACCESS_NOT_EXCLUDED = 0xc01e043b,

            /// <summary> 
            /// STATUS_GRAPHICS_OPM_NOT_SUPPORTED
            /// </summary>
            STATUS_GRAPHICS_OPM_NOT_SUPPORTED = 0xc01e0500,

            /// <summary> 
            /// STATUS_GRAPHICS_COPP_NOT_SUPPORTED
            /// </summary>
            STATUS_GRAPHICS_COPP_NOT_SUPPORTED = 0xc01e0501,

            /// <summary> 
            /// STATUS_GRAPHICS_UAB_NOT_SUPPORTED
            /// </summary>
            STATUS_GRAPHICS_UAB_NOT_SUPPORTED = 0xc01e0502,

            /// <summary> 
            /// STATUS_GRAPHICS_OPM_INVALID_ENCRYPTED_PARAMETERS
            /// </summary>
            STATUS_GRAPHICS_OPM_INVALID_ENCRYPTED_PARAMETERS = 0xc01e0503,

            /// <summary> 
            /// STATUS_GRAPHICS_OPM_NO_PROTECTED_OUTPUTS_EXIST
            /// </summary>
            STATUS_GRAPHICS_OPM_NO_PROTECTED_OUTPUTS_EXIST = 0xc01e0505,

            /// <summary> 
            /// STATUS_GRAPHICS_OPM_INTERNAL_ERROR
            /// </summary>
            STATUS_GRAPHICS_OPM_INTERNAL_ERROR = 0xc01e050b,

            /// <summary> 
            /// STATUS_GRAPHICS_OPM_INVALID_HANDLE
            /// </summary>
            STATUS_GRAPHICS_OPM_INVALID_HANDLE = 0xc01e050c,

            /// <summary> 
            /// STATUS_GRAPHICS_PVP_INVALID_CERTIFICATE_LENGTH
            /// </summary>
            STATUS_GRAPHICS_PVP_INVALID_CERTIFICATE_LENGTH = 0xc01e050e,

            /// <summary> 
            /// STATUS_GRAPHICS_OPM_SPANNING_MODE_ENABLED
            /// </summary>
            STATUS_GRAPHICS_OPM_SPANNING_MODE_ENABLED = 0xc01e050f,

            /// <summary> 
            /// STATUS_GRAPHICS_OPM_THEATER_MODE_ENABLED
            /// </summary>
            STATUS_GRAPHICS_OPM_THEATER_MODE_ENABLED = 0xc01e0510,

            /// <summary> 
            /// STATUS_GRAPHICS_PVP_HFS_FAILED
            /// </summary>
            STATUS_GRAPHICS_PVP_HFS_FAILED = 0xc01e0511,

            /// <summary> 
            /// STATUS_GRAPHICS_OPM_INVALID_SRM
            /// </summary>
            STATUS_GRAPHICS_OPM_INVALID_SRM = 0xc01e0512,

            /// <summary> 
            /// STATUS_GRAPHICS_OPM_OUTPUT_DOES_NOT_SUPPORT_HDCP
            /// </summary>
            STATUS_GRAPHICS_OPM_OUTPUT_DOES_NOT_SUPPORT_HDCP = 0xc01e0513,

            /// <summary> 
            /// STATUS_GRAPHICS_OPM_OUTPUT_DOES_NOT_SUPPORT_ACP
            /// </summary>
            STATUS_GRAPHICS_OPM_OUTPUT_DOES_NOT_SUPPORT_ACP = 0xc01e0514,

            /// <summary> 
            /// STATUS_GRAPHICS_OPM_OUTPUT_DOES_NOT_SUPPORT_CGMSA
            /// </summary>
            STATUS_GRAPHICS_OPM_OUTPUT_DOES_NOT_SUPPORT_CGMSA = 0xc01e0515,

            /// <summary> 
            /// STATUS_GRAPHICS_OPM_HDCP_SRM_NEVER_SET
            /// </summary>
            STATUS_GRAPHICS_OPM_HDCP_SRM_NEVER_SET = 0xc01e0516,

            /// <summary> 
            /// STATUS_GRAPHICS_OPM_RESOLUTION_TOO_HIGH
            /// </summary>
            STATUS_GRAPHICS_OPM_RESOLUTION_TOO_HIGH = 0xc01e0517,

            /// <summary> 
            /// STATUS_GRAPHICS_OPM_ALL_HDCP_HARDWARE_ALREADY_IN_USE
            /// </summary>
            STATUS_GRAPHICS_OPM_ALL_HDCP_HARDWARE_ALREADY_IN_USE = 0xc01e0518,

            /// <summary> 
            /// STATUS_GRAPHICS_OPM_PROTECTED_OUTPUT_NO_LONGER_EXISTS
            /// </summary>
            STATUS_GRAPHICS_OPM_PROTECTED_OUTPUT_NO_LONGER_EXISTS = 0xc01e051a,

            /// <summary> 
            /// STATUS_GRAPHICS_OPM_PROTECTED_OUTPUT_DOES_NOT_HAVE_COPP_SEMANTICS
            /// </summary>
            STATUS_GRAPHICS_OPM_PROTECTED_OUTPUT_DOES_NOT_HAVE_COPP_SEMANTICS = 0xc01e051c,

            /// <summary> 
            /// STATUS_GRAPHICS_OPM_INVALID_INFORMATION_REQUEST
            /// </summary>
            STATUS_GRAPHICS_OPM_INVALID_INFORMATION_REQUEST = 0xc01e051d,

            /// <summary> 
            /// STATUS_GRAPHICS_OPM_DRIVER_INTERNAL_ERROR
            /// </summary>
            STATUS_GRAPHICS_OPM_DRIVER_INTERNAL_ERROR = 0xc01e051e,

            /// <summary> 
            /// STATUS_GRAPHICS_OPM_PROTECTED_OUTPUT_DOES_NOT_HAVE_OPM_SEMANTICS
            /// </summary>
            STATUS_GRAPHICS_OPM_PROTECTED_OUTPUT_DOES_NOT_HAVE_OPM_SEMANTICS = 0xc01e051f,

            /// <summary> 
            /// STATUS_GRAPHICS_OPM_SIGNALING_NOT_SUPPORTED
            /// </summary>
            STATUS_GRAPHICS_OPM_SIGNALING_NOT_SUPPORTED = 0xc01e0520,

            /// <summary> 
            /// STATUS_GRAPHICS_OPM_INVALID_CONFIGURATION_REQUEST
            /// </summary>
            STATUS_GRAPHICS_OPM_INVALID_CONFIGURATION_REQUEST = 0xc01e0521,

            /// <summary> 
            /// STATUS_GRAPHICS_I2C_NOT_SUPPORTED
            /// </summary>
            STATUS_GRAPHICS_I2C_NOT_SUPPORTED = 0xc01e0580,

            /// <summary> 
            /// STATUS_GRAPHICS_I2C_DEVICE_DOES_NOT_EXIST
            /// </summary>
            STATUS_GRAPHICS_I2C_DEVICE_DOES_NOT_EXIST = 0xc01e0581,

            /// <summary> 
            /// STATUS_GRAPHICS_I2C_ERROR_TRANSMITTING_DATA
            /// </summary>
            STATUS_GRAPHICS_I2C_ERROR_TRANSMITTING_DATA = 0xc01e0582,

            /// <summary> 
            /// STATUS_GRAPHICS_I2C_ERROR_RECEIVING_DATA
            /// </summary>
            STATUS_GRAPHICS_I2C_ERROR_RECEIVING_DATA = 0xc01e0583,

            /// <summary> 
            /// STATUS_GRAPHICS_DDCCI_VCP_NOT_SUPPORTED
            /// </summary>
            STATUS_GRAPHICS_DDCCI_VCP_NOT_SUPPORTED = 0xc01e0584,

            /// <summary> 
            /// STATUS_GRAPHICS_DDCCI_INVALID_DATA
            /// </summary>
            STATUS_GRAPHICS_DDCCI_INVALID_DATA = 0xc01e0585,

            /// <summary> 
            /// STATUS_GRAPHICS_DDCCI_MONITOR_RETURNED_INVALID_TIMING_STATUS_BYTE
            /// </summary>
            STATUS_GRAPHICS_DDCCI_MONITOR_RETURNED_INVALID_TIMING_STATUS_BYTE = 0xc01e0586,

            /// <summary> 
            /// STATUS_GRAPHICS_DDCCI_INVALID_CAPABILITIES_STRING
            /// </summary>
            STATUS_GRAPHICS_DDCCI_INVALID_CAPABILITIES_STRING = 0xc01e0587,

            /// <summary> 
            /// STATUS_GRAPHICS_MCA_INTERNAL_ERROR
            /// </summary>
            STATUS_GRAPHICS_MCA_INTERNAL_ERROR = 0xc01e0588,

            /// <summary> 
            /// STATUS_GRAPHICS_DDCCI_INVALID_MESSAGE_COMMAND
            /// </summary>
            STATUS_GRAPHICS_DDCCI_INVALID_MESSAGE_COMMAND = 0xc01e0589,

            /// <summary> 
            /// STATUS_GRAPHICS_DDCCI_INVALID_MESSAGE_LENGTH
            /// </summary>
            STATUS_GRAPHICS_DDCCI_INVALID_MESSAGE_LENGTH = 0xc01e058a,

            /// <summary> 
            /// STATUS_GRAPHICS_DDCCI_INVALID_MESSAGE_CHECKSUM
            /// </summary>
            STATUS_GRAPHICS_DDCCI_INVALID_MESSAGE_CHECKSUM = 0xc01e058b,

            /// <summary> 
            /// STATUS_GRAPHICS_INVALID_PHYSICAL_MONITOR_HANDLE
            /// </summary>
            STATUS_GRAPHICS_INVALID_PHYSICAL_MONITOR_HANDLE = 0xc01e058c,

            /// <summary> 
            /// STATUS_GRAPHICS_MONITOR_NO_LONGER_EXISTS
            /// </summary>
            STATUS_GRAPHICS_MONITOR_NO_LONGER_EXISTS = 0xc01e058d,

            /// <summary> 
            /// STATUS_GRAPHICS_ONLY_CONSOLE_SESSION_SUPPORTED
            /// </summary>
            STATUS_GRAPHICS_ONLY_CONSOLE_SESSION_SUPPORTED = 0xc01e05e0,

            /// <summary> 
            /// STATUS_GRAPHICS_NO_DISPLAY_DEVICE_CORRESPONDS_TO_NAME
            /// </summary>
            STATUS_GRAPHICS_NO_DISPLAY_DEVICE_CORRESPONDS_TO_NAME = 0xc01e05e1,

            /// <summary> 
            /// STATUS_GRAPHICS_DISPLAY_DEVICE_NOT_ATTACHED_TO_DESKTOP
            /// </summary>
            STATUS_GRAPHICS_DISPLAY_DEVICE_NOT_ATTACHED_TO_DESKTOP = 0xc01e05e2,

            /// <summary> 
            /// STATUS_GRAPHICS_MIRRORING_DEVICES_NOT_SUPPORTED
            /// </summary>
            STATUS_GRAPHICS_MIRRORING_DEVICES_NOT_SUPPORTED = 0xc01e05e3,

            /// <summary> 
            /// STATUS_GRAPHICS_INVALID_POINTER
            /// </summary>
            STATUS_GRAPHICS_INVALID_POINTER = 0xc01e05e4,

            /// <summary> 
            /// STATUS_GRAPHICS_NO_MONITORS_CORRESPOND_TO_DISPLAY_DEVICE
            /// </summary>
            STATUS_GRAPHICS_NO_MONITORS_CORRESPOND_TO_DISPLAY_DEVICE = 0xc01e05e5,

            /// <summary> 
            /// STATUS_GRAPHICS_PARAMETER_ARRAY_TOO_SMALL
            /// </summary>
            STATUS_GRAPHICS_PARAMETER_ARRAY_TOO_SMALL = 0xc01e05e6,

            /// <summary> 
            /// STATUS_GRAPHICS_INTERNAL_ERROR
            /// </summary>
            STATUS_GRAPHICS_INTERNAL_ERROR = 0xc01e05e7,

            /// <summary> 
            /// STATUS_GRAPHICS_SESSION_TYPE_CHANGE_IN_PROGRESS
            /// </summary>
            STATUS_GRAPHICS_SESSION_TYPE_CHANGE_IN_PROGRESS = 0xc01e05e8,

            /// <summary> 
            /// STATUS_FVE_LOCKED_VOLUME
            /// </summary>
            STATUS_FVE_LOCKED_VOLUME = 0xc0210000,

            /// <summary> 
            /// STATUS_FVE_NOT_ENCRYPTED
            /// </summary>
            STATUS_FVE_NOT_ENCRYPTED = 0xc0210001,

            /// <summary> 
            /// STATUS_FVE_BAD_INFORMATION
            /// </summary>
            STATUS_FVE_BAD_INFORMATION = 0xc0210002,

            /// <summary> 
            /// STATUS_FVE_TOO_SMALL
            /// </summary>
            STATUS_FVE_TOO_SMALL = 0xc0210003,

            /// <summary> 
            /// STATUS_FVE_FAILED_WRONG_FS
            /// </summary>
            STATUS_FVE_FAILED_WRONG_FS = 0xc0210004,

            /// <summary> 
            /// STATUS_FVE_FAILED_BAD_FS
            /// </summary>
            STATUS_FVE_FAILED_BAD_FS = 0xc0210005,

            /// <summary> 
            /// STATUS_FVE_FS_NOT_EXTENDED
            /// </summary>
            STATUS_FVE_FS_NOT_EXTENDED = 0xc0210006,

            /// <summary> 
            /// STATUS_FVE_FS_MOUNTED
            /// </summary>
            STATUS_FVE_FS_MOUNTED = 0xc0210007,

            /// <summary> 
            /// STATUS_FVE_NO_LICENSE
            /// </summary>
            STATUS_FVE_NO_LICENSE = 0xc0210008,

            /// <summary> 
            /// STATUS_FVE_ACTION_NOT_ALLOWED
            /// </summary>
            STATUS_FVE_ACTION_NOT_ALLOWED = 0xc0210009,

            /// <summary> 
            /// STATUS_FVE_BAD_DATA
            /// </summary>
            STATUS_FVE_BAD_DATA = 0xc021000a,

            /// <summary> 
            /// STATUS_FVE_VOLUME_NOT_BOUND
            /// </summary>
            STATUS_FVE_VOLUME_NOT_BOUND = 0xc021000b,

            /// <summary> 
            /// STATUS_FVE_NOT_DATA_VOLUME
            /// </summary>
            STATUS_FVE_NOT_DATA_VOLUME = 0xc021000c,

            /// <summary> 
            /// STATUS_FVE_CONV_READ_ERROR
            /// </summary>
            STATUS_FVE_CONV_READ_ERROR = 0xc021000d,

            /// <summary> 
            /// STATUS_FVE_CONV_WRITE_ERROR
            /// </summary>
            STATUS_FVE_CONV_WRITE_ERROR = 0xc021000e,

            /// <summary> 
            /// STATUS_FVE_OVERLAPPED_UPDATE
            /// </summary>
            STATUS_FVE_OVERLAPPED_UPDATE = 0xc021000f,

            /// <summary> 
            /// STATUS_FVE_FAILED_SECTOR_SIZE
            /// </summary>
            STATUS_FVE_FAILED_SECTOR_SIZE = 0xc0210010,

            /// <summary> 
            /// STATUS_FVE_FAILED_AUTHENTICATION
            /// </summary>
            STATUS_FVE_FAILED_AUTHENTICATION = 0xc0210011,

            /// <summary> 
            /// STATUS_FVE_NOT_OS_VOLUME
            /// </summary>
            STATUS_FVE_NOT_OS_VOLUME = 0xc0210012,

            /// <summary> 
            /// STATUS_FVE_KEYFILE_NOT_FOUND
            /// </summary>
            STATUS_FVE_KEYFILE_NOT_FOUND = 0xc0210013,

            /// <summary> 
            /// STATUS_FVE_KEYFILE_INVALID
            /// </summary>
            STATUS_FVE_KEYFILE_INVALID = 0xc0210014,

            /// <summary> 
            /// STATUS_FVE_KEYFILE_NO_VMK
            /// </summary>
            STATUS_FVE_KEYFILE_NO_VMK = 0xc0210015,

            /// <summary> 
            /// STATUS_FVE_TPM_DISABLED
            /// </summary>
            STATUS_FVE_TPM_DISABLED = 0xc0210016,

            /// <summary> 
            /// STATUS_FVE_TPM_SRK_AUTH_NOT_ZERO
            /// </summary>
            STATUS_FVE_TPM_SRK_AUTH_NOT_ZERO = 0xc0210017,

            /// <summary> 
            /// STATUS_FVE_TPM_INVALID_PCR
            /// </summary>
            STATUS_FVE_TPM_INVALID_PCR = 0xc0210018,

            /// <summary> 
            /// STATUS_FVE_TPM_NO_VMK
            /// </summary>
            STATUS_FVE_TPM_NO_VMK = 0xc0210019,

            /// <summary> 
            /// STATUS_FVE_PIN_INVALID
            /// </summary>
            STATUS_FVE_PIN_INVALID = 0xc021001a,

            /// <summary> 
            /// STATUS_FVE_AUTH_INVALID_APPLICATION
            /// </summary>
            STATUS_FVE_AUTH_INVALID_APPLICATION = 0xc021001b,

            /// <summary> 
            /// STATUS_FVE_AUTH_INVALID_CONFIG
            /// </summary>
            STATUS_FVE_AUTH_INVALID_CONFIG = 0xc021001c,

            /// <summary> 
            /// STATUS_FVE_DEBUGGER_ENABLED
            /// </summary>
            STATUS_FVE_DEBUGGER_ENABLED = 0xc021001d,

            /// <summary> 
            /// STATUS_FVE_DRY_RUN_FAILED
            /// </summary>
            STATUS_FVE_DRY_RUN_FAILED = 0xc021001e,

            /// <summary> 
            /// STATUS_FVE_BAD_METADATA_POINTER
            /// </summary>
            STATUS_FVE_BAD_METADATA_POINTER = 0xc021001f,

            /// <summary> 
            /// STATUS_FVE_OLD_METADATA_COPY
            /// </summary>
            STATUS_FVE_OLD_METADATA_COPY = 0xc0210020,

            /// <summary> 
            /// STATUS_FVE_REBOOT_REQUIRED
            /// </summary>
            STATUS_FVE_REBOOT_REQUIRED = 0xc0210021,

            /// <summary> 
            /// STATUS_FVE_RAW_ACCESS
            /// </summary>
            STATUS_FVE_RAW_ACCESS = 0xc0210022,

            /// <summary> 
            /// STATUS_FVE_RAW_BLOCKED
            /// </summary>
            STATUS_FVE_RAW_BLOCKED = 0xc0210023,

            /// <summary> 
            /// STATUS_FVE_NO_AUTOUNLOCK_MASTER_KEY
            /// </summary>
            STATUS_FVE_NO_AUTOUNLOCK_MASTER_KEY = 0xc0210024,

            /// <summary> 
            /// STATUS_FVE_MOR_FAILED
            /// </summary>
            STATUS_FVE_MOR_FAILED = 0xc0210025,

            /// <summary> 
            /// STATUS_FWP_CALLOUT_NOT_FOUND
            /// </summary>
            STATUS_FWP_CALLOUT_NOT_FOUND = 0xc0220001,

            /// <summary> 
            /// STATUS_FWP_CONDITION_NOT_FOUND
            /// </summary>
            STATUS_FWP_CONDITION_NOT_FOUND = 0xc0220002,

            /// <summary> 
            /// STATUS_FWP_FILTER_NOT_FOUND
            /// </summary>
            STATUS_FWP_FILTER_NOT_FOUND = 0xc0220003,

            /// <summary> 
            /// STATUS_FWP_LAYER_NOT_FOUND
            /// </summary>
            STATUS_FWP_LAYER_NOT_FOUND = 0xc0220004,

            /// <summary> 
            /// STATUS_FWP_PROVIDER_NOT_FOUND
            /// </summary>
            STATUS_FWP_PROVIDER_NOT_FOUND = 0xc0220005,

            /// <summary> 
            /// STATUS_FWP_PROVIDER_CONTEXT_NOT_FOUND
            /// </summary>
            STATUS_FWP_PROVIDER_CONTEXT_NOT_FOUND = 0xc0220006,

            /// <summary> 
            /// STATUS_FWP_SUBLAYER_NOT_FOUND
            /// </summary>
            STATUS_FWP_SUBLAYER_NOT_FOUND = 0xc0220007,

            /// <summary> 
            /// STATUS_FWP_NOT_FOUND
            /// </summary>
            STATUS_FWP_NOT_FOUND = 0xc0220008,

            /// <summary> 
            /// STATUS_FWP_ALREADY_EXISTS
            /// </summary>
            STATUS_FWP_ALREADY_EXISTS = 0xc0220009,

            /// <summary> 
            /// STATUS_FWP_IN_USE
            /// </summary>
            STATUS_FWP_IN_USE = 0xc022000a,

            /// <summary> 
            /// STATUS_FWP_DYNAMIC_SESSION_IN_PROGRESS
            /// </summary>
            STATUS_FWP_DYNAMIC_SESSION_IN_PROGRESS = 0xc022000b,

            /// <summary> 
            /// STATUS_FWP_WRONG_SESSION
            /// </summary>
            STATUS_FWP_WRONG_SESSION = 0xc022000c,

            /// <summary> 
            /// STATUS_FWP_NO_TXN_IN_PROGRESS
            /// </summary>
            STATUS_FWP_NO_TXN_IN_PROGRESS = 0xc022000d,

            /// <summary> 
            /// STATUS_FWP_TXN_IN_PROGRESS
            /// </summary>
            STATUS_FWP_TXN_IN_PROGRESS = 0xc022000e,

            /// <summary> 
            /// STATUS_FWP_TXN_ABORTED
            /// </summary>
            STATUS_FWP_TXN_ABORTED = 0xc022000f,

            /// <summary> 
            /// STATUS_FWP_SESSION_ABORTED
            /// </summary>
            STATUS_FWP_SESSION_ABORTED = 0xc0220010,

            /// <summary> 
            /// STATUS_FWP_INCOMPATIBLE_TXN
            /// </summary>
            STATUS_FWP_INCOMPATIBLE_TXN = 0xc0220011,

            /// <summary> 
            /// STATUS_FWP_TIMEOUT
            /// </summary>
            STATUS_FWP_TIMEOUT = 0xc0220012,

            /// <summary> 
            /// STATUS_FWP_NET_EVENTS_DISABLED
            /// </summary>
            STATUS_FWP_NET_EVENTS_DISABLED = 0xc0220013,

            /// <summary> 
            /// STATUS_FWP_INCOMPATIBLE_LAYER
            /// </summary>
            STATUS_FWP_INCOMPATIBLE_LAYER = 0xc0220014,

            /// <summary> 
            /// STATUS_FWP_KM_CLIENTS_ONLY
            /// </summary>
            STATUS_FWP_KM_CLIENTS_ONLY = 0xc0220015,

            /// <summary> 
            /// STATUS_FWP_LIFETIME_MISMATCH
            /// </summary>
            STATUS_FWP_LIFETIME_MISMATCH = 0xc0220016,

            /// <summary> 
            /// STATUS_FWP_BUILTIN_OBJECT
            /// </summary>
            STATUS_FWP_BUILTIN_OBJECT = 0xc0220017,

            /// <summary> 
            /// STATUS_FWP_TOO_MANY_CALLOUTS
            /// </summary>
            STATUS_FWP_TOO_MANY_CALLOUTS = 0xc0220018,

            /// <summary> 
            /// STATUS_FWP_NOTIFICATION_DROPPED
            /// </summary>
            STATUS_FWP_NOTIFICATION_DROPPED = 0xc0220019,

            /// <summary> 
            /// STATUS_FWP_TRAFFIC_MISMATCH
            /// </summary>
            STATUS_FWP_TRAFFIC_MISMATCH = 0xc022001a,

            /// <summary> 
            /// STATUS_FWP_INCOMPATIBLE_SA_STATE
            /// </summary>
            STATUS_FWP_INCOMPATIBLE_SA_STATE = 0xc022001b,

            /// <summary> 
            /// STATUS_FWP_NULL_POINTER
            /// </summary>
            STATUS_FWP_NULL_POINTER = 0xc022001c,

            /// <summary> 
            /// STATUS_FWP_INVALID_ENUMERATOR
            /// </summary>
            STATUS_FWP_INVALID_ENUMERATOR = 0xc022001d,

            /// <summary> 
            /// STATUS_FWP_INVALID_FLAGS
            /// </summary>
            STATUS_FWP_INVALID_FLAGS = 0xc022001e,

            /// <summary> 
            /// STATUS_FWP_INVALID_NET_MASK
            /// </summary>
            STATUS_FWP_INVALID_NET_MASK = 0xc022001f,

            /// <summary> 
            /// STATUS_FWP_INVALID_RANGE
            /// </summary>
            STATUS_FWP_INVALID_RANGE = 0xc0220020,

            /// <summary> 
            /// STATUS_FWP_INVALID_INTERVAL
            /// </summary>
            STATUS_FWP_INVALID_INTERVAL = 0xc0220021,

            /// <summary> 
            /// STATUS_FWP_ZERO_LENGTH_ARRAY
            /// </summary>
            STATUS_FWP_ZERO_LENGTH_ARRAY = 0xc0220022,

            /// <summary> 
            /// STATUS_FWP_NULL_DISPLAY_NAME
            /// </summary>
            STATUS_FWP_NULL_DISPLAY_NAME = 0xc0220023,

            /// <summary> 
            /// STATUS_FWP_INVALID_ACTION_TYPE
            /// </summary>
            STATUS_FWP_INVALID_ACTION_TYPE = 0xc0220024,

            /// <summary> 
            /// STATUS_FWP_INVALID_WEIGHT
            /// </summary>
            STATUS_FWP_INVALID_WEIGHT = 0xc0220025,

            /// <summary> 
            /// STATUS_FWP_MATCH_TYPE_MISMATCH
            /// </summary>
            STATUS_FWP_MATCH_TYPE_MISMATCH = 0xc0220026,

            /// <summary> 
            /// STATUS_FWP_TYPE_MISMATCH
            /// </summary>
            STATUS_FWP_TYPE_MISMATCH = 0xc0220027,

            /// <summary> 
            /// STATUS_FWP_OUT_OF_BOUNDS
            /// </summary>
            STATUS_FWP_OUT_OF_BOUNDS = 0xc0220028,

            /// <summary> 
            /// STATUS_FWP_RESERVED
            /// </summary>
            STATUS_FWP_RESERVED = 0xc0220029,

            /// <summary> 
            /// STATUS_FWP_DUPLICATE_CONDITION
            /// </summary>
            STATUS_FWP_DUPLICATE_CONDITION = 0xc022002a,

            /// <summary> 
            /// STATUS_FWP_DUPLICATE_KEYMOD
            /// </summary>
            STATUS_FWP_DUPLICATE_KEYMOD = 0xc022002b,

            /// <summary> 
            /// STATUS_FWP_ACTION_INCOMPATIBLE_WITH_LAYER
            /// </summary>
            STATUS_FWP_ACTION_INCOMPATIBLE_WITH_LAYER = 0xc022002c,

            /// <summary> 
            /// STATUS_FWP_ACTION_INCOMPATIBLE_WITH_SUBLAYER
            /// </summary>
            STATUS_FWP_ACTION_INCOMPATIBLE_WITH_SUBLAYER = 0xc022002d,

            /// <summary> 
            /// STATUS_FWP_CONTEXT_INCOMPATIBLE_WITH_LAYER
            /// </summary>
            STATUS_FWP_CONTEXT_INCOMPATIBLE_WITH_LAYER = 0xc022002e,

            /// <summary> 
            /// STATUS_FWP_CONTEXT_INCOMPATIBLE_WITH_CALLOUT
            /// </summary>
            STATUS_FWP_CONTEXT_INCOMPATIBLE_WITH_CALLOUT = 0xc022002f,

            /// <summary> 
            /// STATUS_FWP_INCOMPATIBLE_AUTH_METHOD
            /// </summary>
            STATUS_FWP_INCOMPATIBLE_AUTH_METHOD = 0xc0220030,

            /// <summary> 
            /// STATUS_FWP_INCOMPATIBLE_DH_GROUP
            /// </summary>
            STATUS_FWP_INCOMPATIBLE_DH_GROUP = 0xc0220031,

            /// <summary> 
            /// STATUS_FWP_EM_NOT_SUPPORTED
            /// </summary>
            STATUS_FWP_EM_NOT_SUPPORTED = 0xc0220032,

            /// <summary> 
            /// STATUS_FWP_NEVER_MATCH
            /// </summary>
            STATUS_FWP_NEVER_MATCH = 0xc0220033,

            /// <summary> 
            /// STATUS_FWP_PROVIDER_CONTEXT_MISMATCH
            /// </summary>
            STATUS_FWP_PROVIDER_CONTEXT_MISMATCH = 0xc0220034,

            /// <summary> 
            /// STATUS_FWP_INVALID_PARAMETER
            /// </summary>
            STATUS_FWP_INVALID_PARAMETER = 0xc0220035,

            /// <summary> 
            /// STATUS_FWP_TOO_MANY_SUBLAYERS
            /// </summary>
            STATUS_FWP_TOO_MANY_SUBLAYERS = 0xc0220036,

            /// <summary> 
            /// STATUS_FWP_CALLOUT_NOTIFICATION_FAILED
            /// </summary>
            STATUS_FWP_CALLOUT_NOTIFICATION_FAILED = 0xc0220037,

            /// <summary> 
            /// STATUS_FWP_INVALID_AUTH_TRANSFORM
            /// </summary>
            STATUS_FWP_INVALID_AUTH_TRANSFORM = 0xc0220038,

            /// <summary> 
            /// STATUS_FWP_INVALID_CIPHER_TRANSFORM
            /// </summary>
            STATUS_FWP_INVALID_CIPHER_TRANSFORM = 0xc0220039,

            /// <summary> 
            /// STATUS_FWP_INCOMPATIBLE_CIPHER_TRANSFORM
            /// </summary>
            STATUS_FWP_INCOMPATIBLE_CIPHER_TRANSFORM = 0xc022003a,

            /// <summary> 
            /// STATUS_FWP_INVALID_TRANSFORM_COMBINATION
            /// </summary>
            STATUS_FWP_INVALID_TRANSFORM_COMBINATION = 0xc022003b,

            /// <summary> 
            /// STATUS_FWP_TCPIP_NOT_READY
            /// </summary>
            STATUS_FWP_TCPIP_NOT_READY = 0xc0220100,

            /// <summary> 
            /// STATUS_FWP_INJECT_HANDLE_CLOSING
            /// </summary>
            STATUS_FWP_INJECT_HANDLE_CLOSING = 0xc0220101,

            /// <summary> 
            /// STATUS_FWP_INJECT_HANDLE_STALE
            /// </summary>
            STATUS_FWP_INJECT_HANDLE_STALE = 0xc0220102,

            /// <summary> 
            /// STATUS_FWP_CANNOT_PEND
            /// </summary>
            STATUS_FWP_CANNOT_PEND = 0xc0220103,

            /// <summary> 
            /// STATUS_FWP_DROP_NOICMP
            /// </summary>
            STATUS_FWP_DROP_NOICMP = 0xc0220104,

            /// <summary> 
            /// STATUS_NDIS_CLOSING
            /// </summary>
            STATUS_NDIS_CLOSING = 0xc0230002,

            /// <summary> 
            /// STATUS_NDIS_BAD_VERSION
            /// </summary>
            STATUS_NDIS_BAD_VERSION = 0xc0230004,

            /// <summary> 
            /// STATUS_NDIS_BAD_CHARACTERISTICS
            /// </summary>
            STATUS_NDIS_BAD_CHARACTERISTICS = 0xc0230005,

            /// <summary> 
            /// STATUS_NDIS_ADAPTER_NOT_FOUND
            /// </summary>
            STATUS_NDIS_ADAPTER_NOT_FOUND = 0xc0230006,

            /// <summary> 
            /// STATUS_NDIS_OPEN_FAILED
            /// </summary>
            STATUS_NDIS_OPEN_FAILED = 0xc0230007,

            /// <summary> 
            /// STATUS_NDIS_DEVICE_FAILED
            /// </summary>
            STATUS_NDIS_DEVICE_FAILED = 0xc0230008,

            /// <summary> 
            /// STATUS_NDIS_MULTICAST_FULL
            /// </summary>
            STATUS_NDIS_MULTICAST_FULL = 0xc0230009,

            /// <summary> 
            /// STATUS_NDIS_MULTICAST_EXISTS
            /// </summary>
            STATUS_NDIS_MULTICAST_EXISTS = 0xc023000a,

            /// <summary> 
            /// STATUS_NDIS_MULTICAST_NOT_FOUND
            /// </summary>
            STATUS_NDIS_MULTICAST_NOT_FOUND = 0xc023000b,

            /// <summary> 
            /// STATUS_NDIS_REQUEST_ABORTED
            /// </summary>
            STATUS_NDIS_REQUEST_ABORTED = 0xc023000c,

            /// <summary> 
            /// STATUS_NDIS_RESET_IN_PROGRESS
            /// </summary>
            STATUS_NDIS_RESET_IN_PROGRESS = 0xc023000d,

            /// <summary> 
            /// STATUS_NDIS_NOT_SUPPORTED
            /// </summary>
            STATUS_NDIS_NOT_SUPPORTED = 0xc02300bb,

            /// <summary> 
            /// STATUS_NDIS_INVALID_PACKET
            /// </summary>
            STATUS_NDIS_INVALID_PACKET = 0xc023000f,

            /// <summary> 
            /// STATUS_NDIS_ADAPTER_NOT_READY
            /// </summary>
            STATUS_NDIS_ADAPTER_NOT_READY = 0xc0230011,

            /// <summary> 
            /// STATUS_NDIS_INVALID_LENGTH
            /// </summary>
            STATUS_NDIS_INVALID_LENGTH = 0xc0230014,

            /// <summary> 
            /// STATUS_NDIS_INVALID_DATA
            /// </summary>
            STATUS_NDIS_INVALID_DATA = 0xc0230015,

            /// <summary> 
            /// STATUS_NDIS_BUFFER_TOO_SHORT
            /// </summary>
            STATUS_NDIS_BUFFER_TOO_SHORT = 0xc0230016,

            /// <summary> 
            /// STATUS_NDIS_INVALID_OID
            /// </summary>
            STATUS_NDIS_INVALID_OID = 0xc0230017,

            /// <summary> 
            /// STATUS_NDIS_ADAPTER_REMOVED
            /// </summary>
            STATUS_NDIS_ADAPTER_REMOVED = 0xc0230018,

            /// <summary> 
            /// STATUS_NDIS_UNSUPPORTED_MEDIA
            /// </summary>
            STATUS_NDIS_UNSUPPORTED_MEDIA = 0xc0230019,

            /// <summary> 
            /// STATUS_NDIS_GROUP_ADDRESS_IN_USE
            /// </summary>
            STATUS_NDIS_GROUP_ADDRESS_IN_USE = 0xc023001a,

            /// <summary> 
            /// STATUS_NDIS_FILE_NOT_FOUND
            /// </summary>
            STATUS_NDIS_FILE_NOT_FOUND = 0xc023001b,

            /// <summary> 
            /// STATUS_NDIS_ERROR_READING_FILE
            /// </summary>
            STATUS_NDIS_ERROR_READING_FILE = 0xc023001c,

            /// <summary> 
            /// STATUS_NDIS_ALREADY_MAPPED
            /// </summary>
            STATUS_NDIS_ALREADY_MAPPED = 0xc023001d,

            /// <summary> 
            /// STATUS_NDIS_RESOURCE_CONFLICT
            /// </summary>
            STATUS_NDIS_RESOURCE_CONFLICT = 0xc023001e,

            /// <summary> 
            /// STATUS_NDIS_MEDIA_DISCONNECTED
            /// </summary>
            STATUS_NDIS_MEDIA_DISCONNECTED = 0xc023001f,

            /// <summary> 
            /// STATUS_NDIS_INVALID_ADDRESS
            /// </summary>
            STATUS_NDIS_INVALID_ADDRESS = 0xc0230022,

            /// <summary> 
            /// STATUS_NDIS_INVALID_DEVICE_REQUEST
            /// </summary>
            STATUS_NDIS_INVALID_DEVICE_REQUEST = 0xc0230010,

            /// <summary> 
            /// STATUS_NDIS_PAUSED
            /// </summary>
            STATUS_NDIS_PAUSED = 0xc023002a,

            /// <summary> 
            /// STATUS_NDIS_INTERFACE_NOT_FOUND
            /// </summary>
            STATUS_NDIS_INTERFACE_NOT_FOUND = 0xc023002b,

            /// <summary> 
            /// STATUS_NDIS_UNSUPPORTED_REVISION
            /// </summary>
            STATUS_NDIS_UNSUPPORTED_REVISION = 0xc023002c,

            /// <summary> 
            /// STATUS_NDIS_INVALID_PORT
            /// </summary>
            STATUS_NDIS_INVALID_PORT = 0xc023002d,

            /// <summary> 
            /// STATUS_NDIS_INVALID_PORT_STATE
            /// </summary>
            STATUS_NDIS_INVALID_PORT_STATE = 0xc023002e,

            /// <summary> 
            /// STATUS_NDIS_LOW_POWER_STATE
            /// </summary>
            STATUS_NDIS_LOW_POWER_STATE = 0xc023002f,

            /// <summary> 
            /// STATUS_NDIS_DOT11_AUTO_CONFIG_ENABLED
            /// </summary>
            STATUS_NDIS_DOT11_AUTO_CONFIG_ENABLED = 0xc0232000,

            /// <summary> 
            /// STATUS_NDIS_DOT11_MEDIA_IN_USE
            /// </summary>
            STATUS_NDIS_DOT11_MEDIA_IN_USE = 0xc0232001,

            /// <summary> 
            /// STATUS_NDIS_DOT11_POWER_STATE_INVALID
            /// </summary>
            STATUS_NDIS_DOT11_POWER_STATE_INVALID = 0xc0232002,

            /// <summary> 
            /// STATUS_NDIS_INDICATION_REQUIRED
            /// </summary>
            STATUS_NDIS_INDICATION_REQUIRED = 0x40230001,

            /// <summary> 
            /// STATUS_HV_INVALID_HYPERCALL_CODE
            /// </summary>
            STATUS_HV_INVALID_HYPERCALL_CODE = 0xc0350002,

            /// <summary> 
            /// STATUS_HV_INVALID_HYPERCALL_INPUT
            /// </summary>
            STATUS_HV_INVALID_HYPERCALL_INPUT = 0xc0350003,

            /// <summary> 
            /// STATUS_HV_INVALID_ALIGNMENT
            /// </summary>
            STATUS_HV_INVALID_ALIGNMENT = 0xc0350004,

            /// <summary> 
            /// STATUS_HV_INVALID_PARAMETER
            /// </summary>
            STATUS_HV_INVALID_PARAMETER = 0xc0350005,

            /// <summary> 
            /// STATUS_HV_ACCESS_DENIED
            /// </summary>
            STATUS_HV_ACCESS_DENIED = 0xc0350006,

            /// <summary> 
            /// STATUS_HV_INVALID_PARTITION_STATE
            /// </summary>
            STATUS_HV_INVALID_PARTITION_STATE = 0xc0350007,

            /// <summary> 
            /// STATUS_HV_OPERATION_DENIED
            /// </summary>
            STATUS_HV_OPERATION_DENIED = 0xc0350008,

            /// <summary> 
            /// STATUS_HV_UNKNOWN_PROPERTY
            /// </summary>
            STATUS_HV_UNKNOWN_PROPERTY = 0xc0350009,

            /// <summary> 
            /// STATUS_HV_PROPERTY_VALUE_OUT_OF_RANGE
            /// </summary>
            STATUS_HV_PROPERTY_VALUE_OUT_OF_RANGE = 0xc035000a,

            /// <summary> 
            /// STATUS_HV_INSUFFICIENT_MEMORY
            /// </summary>
            STATUS_HV_INSUFFICIENT_MEMORY = 0xc035000b,

            /// <summary> 
            /// STATUS_HV_PARTITION_TOO_DEEP
            /// </summary>
            STATUS_HV_PARTITION_TOO_DEEP = 0xc035000c,

            /// <summary> 
            /// STATUS_HV_INVALID_PARTITION_ID
            /// </summary>
            STATUS_HV_INVALID_PARTITION_ID = 0xc035000d,

            /// <summary> 
            /// STATUS_HV_INVALID_VP_INDEX
            /// </summary>
            STATUS_HV_INVALID_VP_INDEX = 0xc035000e,

            /// <summary> 
            /// STATUS_HV_INVALID_PORT_ID
            /// </summary>
            STATUS_HV_INVALID_PORT_ID = 0xc0350011,

            /// <summary> 
            /// STATUS_HV_INVALID_CONNECTION_ID
            /// </summary>
            STATUS_HV_INVALID_CONNECTION_ID = 0xc0350012,

            /// <summary> 
            /// STATUS_HV_INSUFFICIENT_BUFFERS
            /// </summary>
            STATUS_HV_INSUFFICIENT_BUFFERS = 0xc0350013,

            /// <summary> 
            /// STATUS_HV_NOT_ACKNOWLEDGED
            /// </summary>
            STATUS_HV_NOT_ACKNOWLEDGED = 0xc0350014,

            /// <summary> 
            /// STATUS_HV_ACKNOWLEDGED
            /// </summary>
            STATUS_HV_ACKNOWLEDGED = 0xc0350016,

            /// <summary> 
            /// STATUS_HV_INVALID_SAVE_RESTORE_STATE
            /// </summary>
            STATUS_HV_INVALID_SAVE_RESTORE_STATE = 0xc0350017,

            /// <summary> 
            /// STATUS_HV_INVALID_SYNIC_STATE
            /// </summary>
            STATUS_HV_INVALID_SYNIC_STATE = 0xc0350018,

            /// <summary> 
            /// STATUS_HV_OBJECT_IN_USE
            /// </summary>
            STATUS_HV_OBJECT_IN_USE = 0xc0350019,

            /// <summary> 
            /// STATUS_HV_INVALID_PROXIMITY_DOMAIN_INFO
            /// </summary>
            STATUS_HV_INVALID_PROXIMITY_DOMAIN_INFO = 0xc035001a,

            /// <summary> 
            /// STATUS_HV_NO_DATA
            /// </summary>
            STATUS_HV_NO_DATA = 0xc035001b,

            /// <summary> 
            /// STATUS_HV_INACTIVE
            /// </summary>
            STATUS_HV_INACTIVE = 0xc035001c,

            /// <summary> 
            /// STATUS_HV_NO_RESOURCES
            /// </summary>
            STATUS_HV_NO_RESOURCES = 0xc035001d,

            /// <summary> 
            /// STATUS_HV_FEATURE_UNAVAILABLE
            /// </summary>
            STATUS_HV_FEATURE_UNAVAILABLE = 0xc035001e,

            /// <summary> 
            /// STATUS_HV_NOT_PRESENT
            /// </summary>
            STATUS_HV_NOT_PRESENT = 0xc0351000,

            /// <summary> 
            /// STATUS_VID_DUPLICATE_HANDLER
            /// </summary>
            STATUS_VID_DUPLICATE_HANDLER = 0xc0370001,

            /// <summary> 
            /// STATUS_VID_TOO_MANY_HANDLERS
            /// </summary>
            STATUS_VID_TOO_MANY_HANDLERS = 0xc0370002,

            /// <summary> 
            /// STATUS_VID_QUEUE_FULL
            /// </summary>
            STATUS_VID_QUEUE_FULL = 0xc0370003,

            /// <summary> 
            /// STATUS_VID_HANDLER_NOT_PRESENT
            /// </summary>
            STATUS_VID_HANDLER_NOT_PRESENT = 0xc0370004,

            /// <summary> 
            /// STATUS_VID_INVALID_OBJECT_NAME
            /// </summary>
            STATUS_VID_INVALID_OBJECT_NAME = 0xc0370005,

            /// <summary> 
            /// STATUS_VID_PARTITION_NAME_TOO_LONG
            /// </summary>
            STATUS_VID_PARTITION_NAME_TOO_LONG = 0xc0370006,

            /// <summary> 
            /// STATUS_VID_MESSAGE_QUEUE_NAME_TOO_LONG
            /// </summary>
            STATUS_VID_MESSAGE_QUEUE_NAME_TOO_LONG = 0xc0370007,

            /// <summary> 
            /// STATUS_VID_PARTITION_ALREADY_EXISTS
            /// </summary>
            STATUS_VID_PARTITION_ALREADY_EXISTS = 0xc0370008,

            /// <summary> 
            /// STATUS_VID_PARTITION_DOES_NOT_EXIST
            /// </summary>
            STATUS_VID_PARTITION_DOES_NOT_EXIST = 0xc0370009,

            /// <summary> 
            /// STATUS_VID_PARTITION_NAME_NOT_FOUND
            /// </summary>
            STATUS_VID_PARTITION_NAME_NOT_FOUND = 0xc037000a,

            /// <summary> 
            /// STATUS_VID_MESSAGE_QUEUE_ALREADY_EXISTS
            /// </summary>
            STATUS_VID_MESSAGE_QUEUE_ALREADY_EXISTS = 0xc037000b,

            /// <summary> 
            /// STATUS_VID_EXCEEDED_MBP_ENTRY_MAP_LIMIT
            /// </summary>
            STATUS_VID_EXCEEDED_MBP_ENTRY_MAP_LIMIT = 0xc037000c,

            /// <summary> 
            /// STATUS_VID_MB_STILL_REFERENCED
            /// </summary>
            STATUS_VID_MB_STILL_REFERENCED = 0xc037000d,

            /// <summary> 
            /// STATUS_VID_CHILD_GPA_PAGE_SET_CORRUPTED
            /// </summary>
            STATUS_VID_CHILD_GPA_PAGE_SET_CORRUPTED = 0xc037000e,

            /// <summary> 
            /// STATUS_VID_INVALID_NUMA_SETTINGS
            /// </summary>
            STATUS_VID_INVALID_NUMA_SETTINGS = 0xc037000f,

            /// <summary> 
            /// STATUS_VID_INVALID_NUMA_NODE_INDEX
            /// </summary>
            STATUS_VID_INVALID_NUMA_NODE_INDEX = 0xc0370010,

            /// <summary> 
            /// STATUS_VID_NOTIFICATION_QUEUE_ALREADY_ASSOCIATED
            /// </summary>
            STATUS_VID_NOTIFICATION_QUEUE_ALREADY_ASSOCIATED = 0xc0370011,

            /// <summary> 
            /// STATUS_VID_INVALID_MEMORY_BLOCK_HANDLE
            /// </summary>
            STATUS_VID_INVALID_MEMORY_BLOCK_HANDLE = 0xc0370012,

            /// <summary> 
            /// STATUS_VID_PAGE_RANGE_OVERFLOW
            /// </summary>
            STATUS_VID_PAGE_RANGE_OVERFLOW = 0xc0370013,

            /// <summary> 
            /// STATUS_VID_INVALID_MESSAGE_QUEUE_HANDLE
            /// </summary>
            STATUS_VID_INVALID_MESSAGE_QUEUE_HANDLE = 0xc0370014,

            /// <summary> 
            /// STATUS_VID_INVALID_GPA_RANGE_HANDLE
            /// </summary>
            STATUS_VID_INVALID_GPA_RANGE_HANDLE = 0xc0370015,

            /// <summary> 
            /// STATUS_VID_NO_MEMORY_BLOCK_NOTIFICATION_QUEUE
            /// </summary>
            STATUS_VID_NO_MEMORY_BLOCK_NOTIFICATION_QUEUE = 0xc0370016,

            /// <summary> 
            /// STATUS_VID_MEMORY_BLOCK_LOCK_COUNT_EXCEEDED
            /// </summary>
            STATUS_VID_MEMORY_BLOCK_LOCK_COUNT_EXCEEDED = 0xc0370017,

            /// <summary> 
            /// STATUS_VID_INVALID_PPM_HANDLE
            /// </summary>
            STATUS_VID_INVALID_PPM_HANDLE = 0xc0370018,

            /// <summary> 
            /// STATUS_VID_MBPS_ARE_LOCKED
            /// </summary>
            STATUS_VID_MBPS_ARE_LOCKED = 0xc0370019,

            /// <summary> 
            /// STATUS_VID_MESSAGE_QUEUE_CLOSED
            /// </summary>
            STATUS_VID_MESSAGE_QUEUE_CLOSED = 0xc037001a,

            /// <summary> 
            /// STATUS_VID_VIRTUAL_PROCESSOR_LIMIT_EXCEEDED
            /// </summary>
            STATUS_VID_VIRTUAL_PROCESSOR_LIMIT_EXCEEDED = 0xc037001b,

            /// <summary> 
            /// STATUS_VID_STOP_PENDING
            /// </summary>
            STATUS_VID_STOP_PENDING = 0xc037001c,

            /// <summary> 
            /// STATUS_VID_INVALID_PROCESSOR_STATE
            /// </summary>
            STATUS_VID_INVALID_PROCESSOR_STATE = 0xc037001d,

            /// <summary> 
            /// STATUS_VID_EXCEEDED_KM_CONTEXT_COUNT_LIMIT
            /// </summary>
            STATUS_VID_EXCEEDED_KM_CONTEXT_COUNT_LIMIT = 0xc037001e,

            /// <summary> 
            /// STATUS_VID_KM_INTERFACE_ALREADY_INITIALIZED
            /// </summary>
            STATUS_VID_KM_INTERFACE_ALREADY_INITIALIZED = 0xc037001f,

            /// <summary> 
            /// STATUS_VID_MB_PROPERTY_ALREADY_SET_RESET
            /// </summary>
            STATUS_VID_MB_PROPERTY_ALREADY_SET_RESET = 0xc0370020,

            /// <summary> 
            /// STATUS_VID_MMIO_RANGE_DESTROYED
            /// </summary>
            STATUS_VID_MMIO_RANGE_DESTROYED = 0xc0370021,

            /// <summary> 
            /// STATUS_VID_INVALID_CHILD_GPA_PAGE_SET
            /// </summary>
            STATUS_VID_INVALID_CHILD_GPA_PAGE_SET = 0xc0370022,

            /// <summary> 
            /// STATUS_VID_RESERVE_PAGE_SET_IS_BEING_USED
            /// </summary>
            STATUS_VID_RESERVE_PAGE_SET_IS_BEING_USED = 0xc0370023,

            /// <summary> 
            /// STATUS_VID_RESERVE_PAGE_SET_TOO_SMALL
            /// </summary>
            STATUS_VID_RESERVE_PAGE_SET_TOO_SMALL = 0xc0370024,

            /// <summary> 
            /// STATUS_VID_MBP_ALREADY_LOCKED_USING_RESERVED_PAGE
            /// </summary>
            STATUS_VID_MBP_ALREADY_LOCKED_USING_RESERVED_PAGE = 0xc0370025,

            /// <summary> 
            /// STATUS_VID_MBP_COUNT_EXCEEDED_LIMIT
            /// </summary>
            STATUS_VID_MBP_COUNT_EXCEEDED_LIMIT = 0xc0370026,

            /// <summary> 
            /// STATUS_VID_SAVED_STATE_CORRUPT
            /// </summary>
            STATUS_VID_SAVED_STATE_CORRUPT = 0xc0370027,

            /// <summary> 
            /// STATUS_VID_SAVED_STATE_UNRECOGNIZED_ITEM
            /// </summary>
            STATUS_VID_SAVED_STATE_UNRECOGNIZED_ITEM = 0xc0370028,

            /// <summary> 
            /// STATUS_VID_SAVED_STATE_INCOMPATIBLE
            /// </summary>
            STATUS_VID_SAVED_STATE_INCOMPATIBLE = 0xc0370029,

            /// <summary> 
            /// STATUS_VID_REMOTE_NODE_PARENT_GPA_PAGES_USED
            /// </summary>
            STATUS_VID_REMOTE_NODE_PARENT_GPA_PAGES_USED = 0x80370001,

            /// <summary> 
            /// STATUS_IPSEC_BAD_SPI
            /// </summary>
            STATUS_IPSEC_BAD_SPI = 0xc0360001,

            /// <summary> 
            /// STATUS_IPSEC_SA_LIFETIME_EXPIRED
            /// </summary>
            STATUS_IPSEC_SA_LIFETIME_EXPIRED = 0xc0360002,

            /// <summary> 
            /// STATUS_IPSEC_WRONG_SA
            /// </summary>
            STATUS_IPSEC_WRONG_SA = 0xc0360003,

            /// <summary> 
            /// STATUS_IPSEC_REPLAY_CHECK_FAILED
            /// </summary>
            STATUS_IPSEC_REPLAY_CHECK_FAILED = 0xc0360004,

            /// <summary> 
            /// STATUS_IPSEC_INVALID_PACKET
            /// </summary>
            STATUS_IPSEC_INVALID_PACKET = 0xc0360005,

            /// <summary> 
            /// STATUS_IPSEC_INTEGRITY_CHECK_FAILED
            /// </summary>
            STATUS_IPSEC_INTEGRITY_CHECK_FAILED = 0xc0360006,

            /// <summary> 
            /// STATUS_IPSEC_CLEAR_TEXT_DROP
            /// </summary>
            STATUS_IPSEC_CLEAR_TEXT_DROP = 0xc0360007,

            /// <summary> 
            /// STATUS_VOLMGR_INCOMPLETE_REGENERATION
            /// </summary>
            STATUS_VOLMGR_INCOMPLETE_REGENERATION = 0x80380001,

            /// <summary> 
            /// STATUS_VOLMGR_INCOMPLETE_DISK_MIGRATION
            /// </summary>
            STATUS_VOLMGR_INCOMPLETE_DISK_MIGRATION = 0x80380002,

            /// <summary> 
            /// STATUS_VOLMGR_DATABASE_FULL
            /// </summary>
            STATUS_VOLMGR_DATABASE_FULL = 0xc0380001,

            /// <summary> 
            /// STATUS_VOLMGR_DISK_CONFIGURATION_CORRUPTED
            /// </summary>
            STATUS_VOLMGR_DISK_CONFIGURATION_CORRUPTED = 0xc0380002,

            /// <summary> 
            /// STATUS_VOLMGR_DISK_CONFIGURATION_NOT_IN_SYNC
            /// </summary>
            STATUS_VOLMGR_DISK_CONFIGURATION_NOT_IN_SYNC = 0xc0380003,

            /// <summary> 
            /// STATUS_VOLMGR_PACK_CONFIG_UPDATE_FAILED
            /// </summary>
            STATUS_VOLMGR_PACK_CONFIG_UPDATE_FAILED = 0xc0380004,

            /// <summary> 
            /// STATUS_VOLMGR_DISK_CONTAINS_NON_SIMPLE_VOLUME
            /// </summary>
            STATUS_VOLMGR_DISK_CONTAINS_NON_SIMPLE_VOLUME = 0xc0380005,

            /// <summary> 
            /// STATUS_VOLMGR_DISK_DUPLICATE
            /// </summary>
            STATUS_VOLMGR_DISK_DUPLICATE = 0xc0380006,

            /// <summary> 
            /// STATUS_VOLMGR_DISK_DYNAMIC
            /// </summary>
            STATUS_VOLMGR_DISK_DYNAMIC = 0xc0380007,

            /// <summary> 
            /// STATUS_VOLMGR_DISK_ID_INVALID
            /// </summary>
            STATUS_VOLMGR_DISK_ID_INVALID = 0xc0380008,

            /// <summary> 
            /// STATUS_VOLMGR_DISK_INVALID
            /// </summary>
            STATUS_VOLMGR_DISK_INVALID = 0xc0380009,

            /// <summary> 
            /// STATUS_VOLMGR_DISK_LAST_VOTER
            /// </summary>
            STATUS_VOLMGR_DISK_LAST_VOTER = 0xc038000a,

            /// <summary> 
            /// STATUS_VOLMGR_DISK_LAYOUT_INVALID
            /// </summary>
            STATUS_VOLMGR_DISK_LAYOUT_INVALID = 0xc038000b,

            /// <summary> 
            /// STATUS_VOLMGR_DISK_LAYOUT_NON_BASIC_BETWEEN_BASIC_PARTITIONS
            /// </summary>
            STATUS_VOLMGR_DISK_LAYOUT_NON_BASIC_BETWEEN_BASIC_PARTITIONS = 0xc038000c,

            /// <summary> 
            /// STATUS_VOLMGR_DISK_LAYOUT_NOT_CYLINDER_ALIGNED
            /// </summary>
            STATUS_VOLMGR_DISK_LAYOUT_NOT_CYLINDER_ALIGNED = 0xc038000d,

            /// <summary> 
            /// STATUS_VOLMGR_DISK_LAYOUT_PARTITIONS_TOO_SMALL
            /// </summary>
            STATUS_VOLMGR_DISK_LAYOUT_PARTITIONS_TOO_SMALL = 0xc038000e,

            /// <summary> 
            /// STATUS_VOLMGR_DISK_LAYOUT_PRIMARY_BETWEEN_LOGICAL_PARTITIONS
            /// </summary>
            STATUS_VOLMGR_DISK_LAYOUT_PRIMARY_BETWEEN_LOGICAL_PARTITIONS = 0xc038000f,

            /// <summary> 
            /// STATUS_VOLMGR_DISK_LAYOUT_TOO_MANY_PARTITIONS
            /// </summary>
            STATUS_VOLMGR_DISK_LAYOUT_TOO_MANY_PARTITIONS = 0xc0380010,

            /// <summary> 
            /// STATUS_VOLMGR_DISK_MISSING
            /// </summary>
            STATUS_VOLMGR_DISK_MISSING = 0xc0380011,

            /// <summary> 
            /// STATUS_VOLMGR_DISK_NOT_EMPTY
            /// </summary>
            STATUS_VOLMGR_DISK_NOT_EMPTY = 0xc0380012,

            /// <summary> 
            /// STATUS_VOLMGR_DISK_NOT_ENOUGH_SPACE
            /// </summary>
            STATUS_VOLMGR_DISK_NOT_ENOUGH_SPACE = 0xc0380013,

            /// <summary> 
            /// STATUS_VOLMGR_DISK_REVECTORING_FAILED
            /// </summary>
            STATUS_VOLMGR_DISK_REVECTORING_FAILED = 0xc0380014,

            /// <summary> 
            /// STATUS_VOLMGR_DISK_SECTOR_SIZE_INVALID
            /// </summary>
            STATUS_VOLMGR_DISK_SECTOR_SIZE_INVALID = 0xc0380015,

            /// <summary> 
            /// STATUS_VOLMGR_DISK_SET_NOT_CONTAINED
            /// </summary>
            STATUS_VOLMGR_DISK_SET_NOT_CONTAINED = 0xc0380016,

            /// <summary> 
            /// STATUS_VOLMGR_DISK_USED_BY_MULTIPLE_MEMBERS
            /// </summary>
            STATUS_VOLMGR_DISK_USED_BY_MULTIPLE_MEMBERS = 0xc0380017,

            /// <summary> 
            /// STATUS_VOLMGR_DISK_USED_BY_MULTIPLE_PLEXES
            /// </summary>
            STATUS_VOLMGR_DISK_USED_BY_MULTIPLE_PLEXES = 0xc0380018,

            /// <summary> 
            /// STATUS_VOLMGR_DYNAMIC_DISK_NOT_SUPPORTED
            /// </summary>
            STATUS_VOLMGR_DYNAMIC_DISK_NOT_SUPPORTED = 0xc0380019,

            /// <summary> 
            /// STATUS_VOLMGR_EXTENT_ALREADY_USED
            /// </summary>
            STATUS_VOLMGR_EXTENT_ALREADY_USED = 0xc038001a,

            /// <summary> 
            /// STATUS_VOLMGR_EXTENT_NOT_CONTIGUOUS
            /// </summary>
            STATUS_VOLMGR_EXTENT_NOT_CONTIGUOUS = 0xc038001b,

            /// <summary> 
            /// STATUS_VOLMGR_EXTENT_NOT_IN_PUBLIC_REGION
            /// </summary>
            STATUS_VOLMGR_EXTENT_NOT_IN_PUBLIC_REGION = 0xc038001c,

            /// <summary> 
            /// STATUS_VOLMGR_EXTENT_NOT_SECTOR_ALIGNED
            /// </summary>
            STATUS_VOLMGR_EXTENT_NOT_SECTOR_ALIGNED = 0xc038001d,

            /// <summary> 
            /// STATUS_VOLMGR_EXTENT_OVERLAPS_EBR_PARTITION
            /// </summary>
            STATUS_VOLMGR_EXTENT_OVERLAPS_EBR_PARTITION = 0xc038001e,

            /// <summary> 
            /// STATUS_VOLMGR_EXTENT_VOLUME_LENGTHS_DO_NOT_MATCH
            /// </summary>
            STATUS_VOLMGR_EXTENT_VOLUME_LENGTHS_DO_NOT_MATCH = 0xc038001f,

            /// <summary> 
            /// STATUS_VOLMGR_FAULT_TOLERANT_NOT_SUPPORTED
            /// </summary>
            STATUS_VOLMGR_FAULT_TOLERANT_NOT_SUPPORTED = 0xc0380020,

            /// <summary> 
            /// STATUS_VOLMGR_INTERLEAVE_LENGTH_INVALID
            /// </summary>
            STATUS_VOLMGR_INTERLEAVE_LENGTH_INVALID = 0xc0380021,

            /// <summary> 
            /// STATUS_VOLMGR_MAXIMUM_REGISTERED_USERS
            /// </summary>
            STATUS_VOLMGR_MAXIMUM_REGISTERED_USERS = 0xc0380022,

            /// <summary> 
            /// STATUS_VOLMGR_MEMBER_IN_SYNC
            /// </summary>
            STATUS_VOLMGR_MEMBER_IN_SYNC = 0xc0380023,

            /// <summary> 
            /// STATUS_VOLMGR_MEMBER_INDEX_DUPLICATE
            /// </summary>
            STATUS_VOLMGR_MEMBER_INDEX_DUPLICATE = 0xc0380024,

            /// <summary> 
            /// STATUS_VOLMGR_MEMBER_INDEX_INVALID
            /// </summary>
            STATUS_VOLMGR_MEMBER_INDEX_INVALID = 0xc0380025,

            /// <summary> 
            /// STATUS_VOLMGR_MEMBER_MISSING
            /// </summary>
            STATUS_VOLMGR_MEMBER_MISSING = 0xc0380026,

            /// <summary> 
            /// STATUS_VOLMGR_MEMBER_NOT_DETACHED
            /// </summary>
            STATUS_VOLMGR_MEMBER_NOT_DETACHED = 0xc0380027,

            /// <summary> 
            /// STATUS_VOLMGR_MEMBER_REGENERATING
            /// </summary>
            STATUS_VOLMGR_MEMBER_REGENERATING = 0xc0380028,

            /// <summary> 
            /// STATUS_VOLMGR_ALL_DISKS_FAILED
            /// </summary>
            STATUS_VOLMGR_ALL_DISKS_FAILED = 0xc0380029,

            /// <summary> 
            /// STATUS_VOLMGR_NO_REGISTERED_USERS
            /// </summary>
            STATUS_VOLMGR_NO_REGISTERED_USERS = 0xc038002a,

            /// <summary> 
            /// STATUS_VOLMGR_NO_SUCH_USER
            /// </summary>
            STATUS_VOLMGR_NO_SUCH_USER = 0xc038002b,

            /// <summary> 
            /// STATUS_VOLMGR_NOTIFICATION_RESET
            /// </summary>
            STATUS_VOLMGR_NOTIFICATION_RESET = 0xc038002c,

            /// <summary> 
            /// STATUS_VOLMGR_NUMBER_OF_MEMBERS_INVALID
            /// </summary>
            STATUS_VOLMGR_NUMBER_OF_MEMBERS_INVALID = 0xc038002d,

            /// <summary> 
            /// STATUS_VOLMGR_NUMBER_OF_PLEXES_INVALID
            /// </summary>
            STATUS_VOLMGR_NUMBER_OF_PLEXES_INVALID = 0xc038002e,

            /// <summary> 
            /// STATUS_VOLMGR_PACK_DUPLICATE
            /// </summary>
            STATUS_VOLMGR_PACK_DUPLICATE = 0xc038002f,

            /// <summary> 
            /// STATUS_VOLMGR_PACK_ID_INVALID
            /// </summary>
            STATUS_VOLMGR_PACK_ID_INVALID = 0xc0380030,

            /// <summary> 
            /// STATUS_VOLMGR_PACK_INVALID
            /// </summary>
            STATUS_VOLMGR_PACK_INVALID = 0xc0380031,

            /// <summary> 
            /// STATUS_VOLMGR_PACK_NAME_INVALID
            /// </summary>
            STATUS_VOLMGR_PACK_NAME_INVALID = 0xc0380032,

            /// <summary> 
            /// STATUS_VOLMGR_PACK_OFFLINE
            /// </summary>
            STATUS_VOLMGR_PACK_OFFLINE = 0xc0380033,

            /// <summary> 
            /// STATUS_VOLMGR_PACK_HAS_QUORUM
            /// </summary>
            STATUS_VOLMGR_PACK_HAS_QUORUM = 0xc0380034,

            /// <summary> 
            /// STATUS_VOLMGR_PACK_WITHOUT_QUORUM
            /// </summary>
            STATUS_VOLMGR_PACK_WITHOUT_QUORUM = 0xc0380035,

            /// <summary> 
            /// STATUS_VOLMGR_PARTITION_STYLE_INVALID
            /// </summary>
            STATUS_VOLMGR_PARTITION_STYLE_INVALID = 0xc0380036,

            /// <summary> 
            /// STATUS_VOLMGR_PARTITION_UPDATE_FAILED
            /// </summary>
            STATUS_VOLMGR_PARTITION_UPDATE_FAILED = 0xc0380037,

            /// <summary> 
            /// STATUS_VOLMGR_PLEX_IN_SYNC
            /// </summary>
            STATUS_VOLMGR_PLEX_IN_SYNC = 0xc0380038,

            /// <summary> 
            /// STATUS_VOLMGR_PLEX_INDEX_DUPLICATE
            /// </summary>
            STATUS_VOLMGR_PLEX_INDEX_DUPLICATE = 0xc0380039,

            /// <summary> 
            /// STATUS_VOLMGR_PLEX_INDEX_INVALID
            /// </summary>
            STATUS_VOLMGR_PLEX_INDEX_INVALID = 0xc038003a,

            /// <summary> 
            /// STATUS_VOLMGR_PLEX_LAST_ACTIVE
            /// </summary>
            STATUS_VOLMGR_PLEX_LAST_ACTIVE = 0xc038003b,

            /// <summary> 
            /// STATUS_VOLMGR_PLEX_MISSING
            /// </summary>
            STATUS_VOLMGR_PLEX_MISSING = 0xc038003c,

            /// <summary> 
            /// STATUS_VOLMGR_PLEX_REGENERATING
            /// </summary>
            STATUS_VOLMGR_PLEX_REGENERATING = 0xc038003d,

            /// <summary> 
            /// STATUS_VOLMGR_PLEX_TYPE_INVALID
            /// </summary>
            STATUS_VOLMGR_PLEX_TYPE_INVALID = 0xc038003e,

            /// <summary> 
            /// STATUS_VOLMGR_PLEX_NOT_RAID5
            /// </summary>
            STATUS_VOLMGR_PLEX_NOT_RAID5 = 0xc038003f,

            /// <summary> 
            /// STATUS_VOLMGR_PLEX_NOT_SIMPLE
            /// </summary>
            STATUS_VOLMGR_PLEX_NOT_SIMPLE = 0xc0380040,

            /// <summary> 
            /// STATUS_VOLMGR_STRUCTURE_SIZE_INVALID
            /// </summary>
            STATUS_VOLMGR_STRUCTURE_SIZE_INVALID = 0xc0380041,

            /// <summary> 
            /// STATUS_VOLMGR_TOO_MANY_NOTIFICATION_REQUESTS
            /// </summary>
            STATUS_VOLMGR_TOO_MANY_NOTIFICATION_REQUESTS = 0xc0380042,

            /// <summary> 
            /// STATUS_VOLMGR_TRANSACTION_IN_PROGRESS
            /// </summary>
            STATUS_VOLMGR_TRANSACTION_IN_PROGRESS = 0xc0380043,

            /// <summary> 
            /// STATUS_VOLMGR_UNEXPECTED_DISK_LAYOUT_CHANGE
            /// </summary>
            STATUS_VOLMGR_UNEXPECTED_DISK_LAYOUT_CHANGE = 0xc0380044,

            /// <summary> 
            /// STATUS_VOLMGR_VOLUME_CONTAINS_MISSING_DISK
            /// </summary>
            STATUS_VOLMGR_VOLUME_CONTAINS_MISSING_DISK = 0xc0380045,

            /// <summary> 
            /// STATUS_VOLMGR_VOLUME_ID_INVALID
            /// </summary>
            STATUS_VOLMGR_VOLUME_ID_INVALID = 0xc0380046,

            /// <summary> 
            /// STATUS_VOLMGR_VOLUME_LENGTH_INVALID
            /// </summary>
            STATUS_VOLMGR_VOLUME_LENGTH_INVALID = 0xc0380047,

            /// <summary> 
            /// STATUS_VOLMGR_VOLUME_LENGTH_NOT_SECTOR_SIZE_MULTIPLE
            /// </summary>
            STATUS_VOLMGR_VOLUME_LENGTH_NOT_SECTOR_SIZE_MULTIPLE = 0xc0380048,

            /// <summary> 
            /// STATUS_VOLMGR_VOLUME_NOT_MIRRORED
            /// </summary>
            STATUS_VOLMGR_VOLUME_NOT_MIRRORED = 0xc0380049,

            /// <summary> 
            /// STATUS_VOLMGR_VOLUME_NOT_RETAINED
            /// </summary>
            STATUS_VOLMGR_VOLUME_NOT_RETAINED = 0xc038004a,

            /// <summary> 
            /// STATUS_VOLMGR_VOLUME_OFFLINE
            /// </summary>
            STATUS_VOLMGR_VOLUME_OFFLINE = 0xc038004b,

            /// <summary> 
            /// STATUS_VOLMGR_VOLUME_RETAINED
            /// </summary>
            STATUS_VOLMGR_VOLUME_RETAINED = 0xc038004c,

            /// <summary> 
            /// STATUS_VOLMGR_NUMBER_OF_EXTENTS_INVALID
            /// </summary>
            STATUS_VOLMGR_NUMBER_OF_EXTENTS_INVALID = 0xc038004d,

            /// <summary> 
            /// STATUS_VOLMGR_DIFFERENT_SECTOR_SIZE
            /// </summary>
            STATUS_VOLMGR_DIFFERENT_SECTOR_SIZE = 0xc038004e,

            /// <summary> 
            /// STATUS_VOLMGR_BAD_BOOT_DISK
            /// </summary>
            STATUS_VOLMGR_BAD_BOOT_DISK = 0xc038004f,

            /// <summary> 
            /// STATUS_VOLMGR_PACK_CONFIG_OFFLINE
            /// </summary>
            STATUS_VOLMGR_PACK_CONFIG_OFFLINE = 0xc0380050,

            /// <summary> 
            /// STATUS_VOLMGR_PACK_CONFIG_ONLINE
            /// </summary>
            STATUS_VOLMGR_PACK_CONFIG_ONLINE = 0xc0380051,

            /// <summary> 
            /// STATUS_VOLMGR_NOT_PRIMARY_PACK
            /// </summary>
            STATUS_VOLMGR_NOT_PRIMARY_PACK = 0xc0380052,

            /// <summary> 
            /// STATUS_VOLMGR_PACK_LOG_UPDATE_FAILED
            /// </summary>
            STATUS_VOLMGR_PACK_LOG_UPDATE_FAILED = 0xc0380053,

            /// <summary> 
            /// STATUS_VOLMGR_NUMBER_OF_DISKS_IN_PLEX_INVALID
            /// </summary>
            STATUS_VOLMGR_NUMBER_OF_DISKS_IN_PLEX_INVALID = 0xc0380054,

            /// <summary> 
            /// STATUS_VOLMGR_NUMBER_OF_DISKS_IN_MEMBER_INVALID
            /// </summary>
            STATUS_VOLMGR_NUMBER_OF_DISKS_IN_MEMBER_INVALID = 0xc0380055,

            /// <summary> 
            /// STATUS_VOLMGR_VOLUME_MIRRORED
            /// </summary>
            STATUS_VOLMGR_VOLUME_MIRRORED = 0xc0380056,

            /// <summary> 
            /// STATUS_VOLMGR_PLEX_NOT_SIMPLE_SPANNED
            /// </summary>
            STATUS_VOLMGR_PLEX_NOT_SIMPLE_SPANNED = 0xc0380057,

            /// <summary> 
            /// STATUS_VOLMGR_NO_VALID_LOG_COPIES
            /// </summary>
            STATUS_VOLMGR_NO_VALID_LOG_COPIES = 0xc0380058,

            /// <summary> 
            /// STATUS_VOLMGR_PRIMARY_PACK_PRESENT
            /// </summary>
            STATUS_VOLMGR_PRIMARY_PACK_PRESENT = 0xc0380059,

            /// <summary> 
            /// STATUS_VOLMGR_NUMBER_OF_DISKS_INVALID
            /// </summary>
            STATUS_VOLMGR_NUMBER_OF_DISKS_INVALID = 0xc038005a,

            /// <summary> 
            /// STATUS_BCD_NOT_ALL_ENTRIES_IMPORTED
            /// </summary>
            STATUS_BCD_NOT_ALL_ENTRIES_IMPORTED = 0x80390001,

            /// <summary> 
            /// STATUS_BCD_TOO_MANY_ELEMENTS
            /// </summary>
            STATUS_BCD_TOO_MANY_ELEMENTS = 0xc0390002,

            /// <summary> 
            /// STATUS_BCD_NOT_ALL_ENTRIES_SYNCHRONIZED
            /// </summary>
            STATUS_BCD_NOT_ALL_ENTRIES_SYNCHRONIZED = 0x80390003,

            /// <summary> 
            /// STATUS_VHD_DRIVE_FOOTER_MISSING
            /// </summary>
            STATUS_VHD_DRIVE_FOOTER_MISSING = 0xc03a0001,

            /// <summary> 
            /// STATUS_VHD_DRIVE_FOOTER_CHECKSUM_MISMATCH
            /// </summary>
            STATUS_VHD_DRIVE_FOOTER_CHECKSUM_MISMATCH = 0xc03a0002,

            /// <summary> 
            /// STATUS_VHD_DRIVE_FOOTER_CORRUPT
            /// </summary>
            STATUS_VHD_DRIVE_FOOTER_CORRUPT = 0xc03a0003,

            /// <summary> 
            /// STATUS_VHD_FORMAT_UNKNOWN
            /// </summary>
            STATUS_VHD_FORMAT_UNKNOWN = 0xc03a0004,

            /// <summary> 
            /// STATUS_VHD_FORMAT_UNSUPPORTED_VERSION
            /// </summary>
            STATUS_VHD_FORMAT_UNSUPPORTED_VERSION = 0xc03a0005,

            /// <summary> 
            /// STATUS_VHD_SPARSE_HEADER_CHECKSUM_MISMATCH
            /// </summary>
            STATUS_VHD_SPARSE_HEADER_CHECKSUM_MISMATCH = 0xc03a0006,

            /// <summary> 
            /// STATUS_VHD_SPARSE_HEADER_UNSUPPORTED_VERSION
            /// </summary>
            STATUS_VHD_SPARSE_HEADER_UNSUPPORTED_VERSION = 0xc03a0007,

            /// <summary> 
            /// STATUS_VHD_SPARSE_HEADER_CORRUPT
            /// </summary>
            STATUS_VHD_SPARSE_HEADER_CORRUPT = 0xc03a0008,

            /// <summary> 
            /// STATUS_VHD_BLOCK_ALLOCATION_FAILURE
            /// </summary>
            STATUS_VHD_BLOCK_ALLOCATION_FAILURE = 0xc03a0009,

            /// <summary> 
            /// STATUS_VHD_BLOCK_ALLOCATION_TABLE_CORRUPT
            /// </summary>
            STATUS_VHD_BLOCK_ALLOCATION_TABLE_CORRUPT = 0xc03a000a,

            /// <summary> 
            /// STATUS_VHD_INVALID_BLOCK_SIZE
            /// </summary>
            STATUS_VHD_INVALID_BLOCK_SIZE = 0xc03a000b,

            /// <summary> 
            /// STATUS_VHD_BITMAP_MISMATCH
            /// </summary>
            STATUS_VHD_BITMAP_MISMATCH = 0xc03a000c,

            /// <summary> 
            /// STATUS_VHD_PARENT_VHD_NOT_FOUND
            /// </summary>
            STATUS_VHD_PARENT_VHD_NOT_FOUND = 0xc03a000d,

            /// <summary> 
            /// STATUS_VHD_CHILD_PARENT_ID_MISMATCH
            /// </summary>
            STATUS_VHD_CHILD_PARENT_ID_MISMATCH = 0xc03a000e,

            /// <summary> 
            /// STATUS_VHD_CHILD_PARENT_TIMESTAMP_MISMATCH
            /// </summary>
            STATUS_VHD_CHILD_PARENT_TIMESTAMP_MISMATCH = 0xc03a000f,

            /// <summary> 
            /// STATUS_VHD_METADATA_READ_FAILURE
            /// </summary>
            STATUS_VHD_METADATA_READ_FAILURE = 0xc03a0010,

            /// <summary> 
            /// STATUS_VHD_METADATA_WRITE_FAILURE
            /// </summary>
            STATUS_VHD_METADATA_WRITE_FAILURE = 0xc03a0011,

            /// <summary> 
            /// STATUS_VHD_INVALID_SIZE
            /// </summary>
            STATUS_VHD_INVALID_SIZE = 0xc03a0012,

            /// <summary> 
            /// STATUS_VHD_INVALID_FILE_SIZE
            /// </summary>
            STATUS_VHD_INVALID_FILE_SIZE = 0xc03a0013,

            /// <summary> 
            /// STATUS_QUERY_STORAGE_ERROR
            /// </summary>
            STATUS_QUERY_STORAGE_ERROR = 0x803a0001,

            /// <summary>
            /// 最大の値。
            /// </summary>
            MaximumNtStatus = 0xffffffff
        }

        /// <summary>
        /// Tells NtQuerySystemInformation and NtSetSystemInformation what you would like to read/change.
        /// </summary>
        public enum SystemInformationClass : int
        {
            /// <summary> 
            /// SystemBasicInformation
            /// </summary>
            SystemBasicInformation = 0x0000,

            /// <summary> 
            /// SystemProcessorInformation
            /// </summary>
            SystemProcessorInformation = 0x0001,

            /// <summary> 
            /// SystemPerformanceInformation
            /// </summary>
            SystemPerformanceInformation = 0x0002,

            /// <summary> 
            /// SystemTimeOfDayInformation
            /// </summary>
            SystemTimeOfDayInformation = 0x0003,

            /// <summary> 
            /// SystemPathInformation
            /// </summary>
            SystemPathInformation = 0x0004,

            /// <summary> 
            /// SystemProcessInformation
            /// </summary>
            SystemProcessInformation = 0x0005,

            /// <summary> 
            /// SystemCallCountInformation
            /// </summary>
            SystemCallCountInformation = 0x0006,

            /// <summary> 
            /// SystemDeviceInformation
            /// </summary>
            SystemDeviceInformation = 0x0007,

            /// <summary> 
            /// SystemProcessorPerformanceInformation
            /// </summary>
            SystemProcessorPerformanceInformation = 0x0008,

            /// <summary> 
            /// SystemFlagsInformation
            /// </summary>
            SystemFlagsInformation = 0x0009,

            /// <summary> 
            /// SystemCallTimeInformation
            /// </summary>
            SystemCallTimeInformation = 0x000a,

            /// <summary> 
            /// SystemModuleInformation
            /// </summary>
            SystemModuleInformation = 0x000b,

            /// <summary> 
            /// SystemLocksInformation
            /// </summary>
            SystemLocksInformation = 0x000c,

            /// <summary> 
            /// SystemStackTraceInformation
            /// </summary>
            SystemStackTraceInformation = 0x000d,

            /// <summary> 
            /// SystemPagedPoolInformation
            /// </summary>
            SystemPagedPoolInformation = 0x000e,

            /// <summary> 
            /// SystemNonPagedPoolInformation
            /// </summary>
            SystemNonPagedPoolInformation = 0x000f,

            /// <summary> 
            /// SystemHandleInformation
            /// </summary>
            SystemHandleInformation = 0x0010,

            /// <summary> 
            /// SystemObjectInformation
            /// </summary>
            SystemObjectInformation = 0x0011,

            /// <summary> 
            /// SystemPageFileInformation
            /// </summary>
            SystemPageFileInformation = 0x0012,

            /// <summary> 
            /// SystemVdmInstemulInformation
            /// </summary>
            SystemVdmInstemulInformation = 0x0013,

            /// <summary> 
            /// SystemVdmBopInformation
            /// </summary>
            SystemVdmBopInformation = 0x0014,

            /// <summary> 
            /// SystemFileCacheInformation
            /// </summary>
            SystemFileCacheInformation = 0x0015,

            /// <summary> 
            /// SystemPoolTagInformation
            /// </summary>
            SystemPoolTagInformation = 0x0016,

            /// <summary> 
            /// SystemInterruptInformation
            /// </summary>
            SystemInterruptInformation = 0x0017,

            /// <summary> 
            /// SystemDpcBehaviorInformation
            /// </summary>
            SystemDpcBehaviorInformation = 0x0018,

            /// <summary> 
            /// SystemFullMemoryInformation
            /// </summary>
            SystemFullMemoryInformation = 0x0019,

            /// <summary> 
            /// SystemLoadGdiDriverInformation
            /// </summary>
            SystemLoadGdiDriverInformation = 0x001a,

            /// <summary> 
            /// SystemUnloadGdiDriverInformation
            /// </summary>
            SystemUnloadGdiDriverInformation = 0x001b,

            /// <summary> 
            /// SystemTimeAdjustmentInformation
            /// </summary>
            SystemTimeAdjustmentInformation = 0x001c,

            /// <summary> 
            /// SystemSummaryMemoryInformation
            /// </summary>
            SystemSummaryMemoryInformation = 0x001d,

            /// <summary> 
            /// SystemMirrorMemoryInformation
            /// </summary>
            SystemMirrorMemoryInformation = 0x001e,

            /// <summary> 
            /// SystemPerformanceTraceInformation
            /// </summary>
            SystemPerformanceTraceInformation = 0x001f,

            /// <summary> 
            /// SystemCrashDumpInformation
            /// </summary>
            SystemCrashDumpInformation = 0x0020,

            /// <summary> 
            /// SystemExceptionInformation
            /// </summary>
            SystemExceptionInformation = 0x0021,

            /// <summary> 
            /// SystemCrashDumpStateInformation
            /// </summary>
            SystemCrashDumpStateInformation = 0x0022,

            /// <summary> 
            /// SystemKernelDebuggerInformation
            /// </summary>
            SystemKernelDebuggerInformation = 0x0023,

            /// <summary> 
            /// SystemContextSwitchInformation
            /// </summary>
            SystemContextSwitchInformation = 0x0024,

            /// <summary> 
            /// SystemRegistryQuotaInformation
            /// </summary>
            SystemRegistryQuotaInformation = 0x0025,

            /// <summary> 
            /// SystemExtendServiceTableInformation
            /// </summary>
            SystemExtendServiceTableInformation = 0x0026,

            /// <summary> 
            /// SystemPrioritySeperation
            /// </summary>
            SystemPrioritySeperation = 0x0027,

            /// <summary> 
            /// SystemVerifierAddDriverInformation
            /// </summary>
            SystemVerifierAddDriverInformation = 0x0028,

            /// <summary> 
            /// SystemVerifierRemoveDriverInformation
            /// </summary>
            SystemVerifierRemoveDriverInformation = 0x0029,

            /// <summary> 
            /// SystemProcessorIdleInformation
            /// </summary>
            SystemProcessorIdleInformation = 0x002a,

            /// <summary> 
            /// SystemLegacyDriverInformation
            /// </summary>
            SystemLegacyDriverInformation = 0x002b,

            /// <summary> 
            /// SystemCurrentTimeZoneInformation
            /// </summary>
            SystemCurrentTimeZoneInformation = 0x002c,

            /// <summary> 
            /// SystemLookasideInformation
            /// </summary>
            SystemLookasideInformation = 0x002d,

            /// <summary> 
            /// SystemTimeSlipNotification
            /// </summary>
            SystemTimeSlipNotification = 0x002e,

            /// <summary> 
            /// SystemSessionCreate
            /// </summary>
            SystemSessionCreate = 0x002f,

            /// <summary> 
            /// SystemSessionDetach
            /// </summary>
            SystemSessionDetach = 0x0030,

            /// <summary> 
            /// SystemSessionInformation
            /// </summary>
            SystemSessionInformation = 0x0031,

            /// <summary> 
            /// SystemRangeStartInformation
            /// </summary>
            SystemRangeStartInformation = 0x0032,

            /// <summary> 
            /// SystemVerifierInformation
            /// </summary>
            SystemVerifierInformation = 0x0033,

            /// <summary> 
            /// SystemVerifierThunkExtend
            /// </summary>
            SystemVerifierThunkExtend = 0x0034,

            /// <summary> 
            /// SystemSessionProcessInformation
            /// </summary>
            SystemSessionProcessInformation = 0x0035,

            /// <summary> 
            /// SystemLoadGdiDriverInSystemSpace
            /// </summary>
            SystemLoadGdiDriverInSystemSpace = 0x0036,

            /// <summary> 
            /// SystemNumaProcessorMap
            /// </summary>
            SystemNumaProcessorMap = 0x0037,

            /// <summary> 
            /// SystemPrefetcherInformation
            /// </summary>
            SystemPrefetcherInformation = 0x0038,

            /// <summary> 
            /// SystemExtendedProcessInformation
            /// </summary>
            SystemExtendedProcessInformation = 0x0039,

            /// <summary> 
            /// SystemRecommendedSharedDataAlignment
            /// </summary>
            SystemRecommendedSharedDataAlignment = 0x003a,

            /// <summary> 
            /// SystemComPlusPackage
            /// </summary>
            SystemComPlusPackage = 0x003b,

            /// <summary> 
            /// SystemNumaAvailableMemory
            /// </summary>
            SystemNumaAvailableMemory = 0x003c,

            /// <summary> 
            /// SystemProcessorPowerInformation
            /// </summary>
            SystemProcessorPowerInformation = 0x003d,

            /// <summary> 
            /// SystemEmulationBasicInformation
            /// </summary>
            SystemEmulationBasicInformation = 0x003e,

            /// <summary> 
            /// SystemEmulationProcessorInformation
            /// </summary>
            SystemEmulationProcessorInformation = 0x003f,

            /// <summary> 
            /// SystemExtendedHandleInformation
            /// </summary>
            SystemExtendedHandleInformation = 0x0040,

            /// <summary> 
            /// SystemLostDelayedWriteInformation
            /// </summary>
            SystemLostDelayedWriteInformation = 0x0041,

            /// <summary> 
            /// SystemBigPoolInformation
            /// </summary>
            SystemBigPoolInformation = 0x0042,

            /// <summary> 
            /// SystemSessionPoolTagInformation
            /// </summary>
            SystemSessionPoolTagInformation = 0x0043,

            /// <summary> 
            /// SystemSessionMappedViewInformation
            /// </summary>
            SystemSessionMappedViewInformation = 0x0044,

            /// <summary> 
            /// SystemHotpatchInformation
            /// </summary>
            SystemHotpatchInformation = 0x0045,

            /// <summary> 
            /// SystemObjectSecurityMode
            /// </summary>
            SystemObjectSecurityMode = 0x0046,

            /// <summary> 
            /// SystemWatchdogTimerHandler
            /// </summary>
            SystemWatchdogTimerHandler = 0x0047,

            /// <summary> 
            /// SystemWatchdogTimerInformation
            /// </summary>
            SystemWatchdogTimerInformation = 0x0048,

            /// <summary> 
            /// SystemLogicalProcessorInformation
            /// </summary>
            SystemLogicalProcessorInformation = 0x0049,

            /// <summary> 
            /// SystemWow64SharedInformationObsolete
            /// </summary>
            SystemWow64SharedInformationObsolete = 0x004a,

            /// <summary> 
            /// SystemRegisterFirmwareTableInformationHandler
            /// </summary>
            SystemRegisterFirmwareTableInformationHandler = 0x004b,

            /// <summary> 
            /// SystemFirmwareTableInformation
            /// </summary>
            SystemFirmwareTableInformation = 0x004c,

            /// <summary> 
            /// SystemModuleInformationEx
            /// </summary>
            SystemModuleInformationEx = 0x004d,

            /// <summary> 
            /// SystemVerifierTriageInformation
            /// </summary>
            SystemVerifierTriageInformation = 0x004e,

            /// <summary> 
            /// SystemSuperfetchInformation
            /// </summary>
            SystemSuperfetchInformation = 0x004f,

            /// <summary> 
            /// SystemMemoryListInformation
            /// </summary>
            SystemMemoryListInformation = 0x0050,

            /// <summary> 
            /// SystemFileCacheInformationEx
            /// </summary>
            SystemFileCacheInformationEx = 0x0051,

            /// <summary> 
            /// SystemThreadPriorityClientIdInformation
            /// </summary>
            SystemThreadPriorityClientIdInformation = 0x0052,

            /// <summary> 
            /// SystemProcessorIdleCycleTimeInformation
            /// </summary>
            SystemProcessorIdleCycleTimeInformation = 0x0053,

            /// <summary> 
            /// SystemVerifierCancellationInformation
            /// </summary>
            SystemVerifierCancellationInformation = 0x0054,

            /// <summary> 
            /// SystemProcessorPowerInformationEx
            /// </summary>
            SystemProcessorPowerInformationEx = 0x0055,

            /// <summary> 
            /// SystemRefTraceInformation
            /// </summary>
            SystemRefTraceInformation = 0x0056,

            /// <summary> 
            /// SystemSpecialPoolInformation
            /// </summary>
            SystemSpecialPoolInformation = 0x0057,

            /// <summary> 
            /// SystemProcessIdInformation
            /// </summary>
            SystemProcessIdInformation = 0x0058,

            /// <summary> 
            /// SystemErrorPortInformation
            /// </summary>
            SystemErrorPortInformation = 0x0059,

            /// <summary> 
            /// SystemBootEnvironmentInformation
            /// </summary>
            SystemBootEnvironmentInformation = 0x005a,

            /// <summary> 
            /// SystemHypervisorInformation
            /// </summary>
            SystemHypervisorInformation = 0x005b,

            /// <summary> 
            /// SystemVerifierInformationEx
            /// </summary>
            SystemVerifierInformationEx = 0x005c,

            /// <summary> 
            /// SystemTimeZoneInformation
            /// </summary>
            SystemTimeZoneInformation = 0x005d,

            /// <summary> 
            /// SystemImageFileExecutionOptionsInformation
            /// </summary>
            SystemImageFileExecutionOptionsInformation = 0x005e,

            /// <summary> 
            /// SystemCoverageInformation
            /// </summary>
            SystemCoverageInformation = 0x005f,

            /// <summary> 
            /// SystemPrefetchPatchInformation
            /// </summary>
            SystemPrefetchPatchInformation = 0x0060,

            /// <summary> 
            /// SystemVerifierFaultsInformation
            /// </summary>
            SystemVerifierFaultsInformation = 0x0061,

            /// <summary> 
            /// SystemSystemPartitionInformation
            /// </summary>
            SystemSystemPartitionInformation = 0x0062,

            /// <summary> 
            /// SystemSystemDiskInformation
            /// </summary>
            SystemSystemDiskInformation = 0x0063,

            /// <summary> 
            /// SystemProcessorPerformanceDistribution
            /// </summary>
            SystemProcessorPerformanceDistribution = 0x0064,

            /// <summary> 
            /// SystemNumaProximityNodeInformation
            /// </summary>
            SystemNumaProximityNodeInformation = 0x0065,

            /// <summary> 
            /// SystemDynamicTimeZoneInformation
            /// </summary>
            SystemDynamicTimeZoneInformation = 0x0066,

            /// <summary> 
            /// SystemCodeIntegrityInformation
            /// </summary>
            SystemCodeIntegrityInformation = 0x0067,

            /// <summary> 
            /// SystemProcessorMicrocodeUpdateInformation
            /// </summary>
            SystemProcessorMicrocodeUpdateInformation = 0x0068,

            /// <summary> 
            /// SystemProcessorBrandString
            /// </summary>
            SystemProcessorBrandString = 0x0069,

            /// <summary> 
            /// SystemVirtualAddressInformation
            /// </summary>
            SystemVirtualAddressInformation = 0x006a,

            /// <summary> 
            /// SystemLogicalProcessorAndGroupInformation
            /// </summary>
            SystemLogicalProcessorAndGroupInformation = 0x006b,

            /// <summary> 
            /// SystemProcessorCycleTimeInformation
            /// </summary>
            SystemProcessorCycleTimeInformation = 0x006c,

            /// <summary> 
            /// SystemStoreInformation
            /// </summary>
            SystemStoreInformation = 0x006d,

            /// <summary> 
            /// SystemRegistryAppendString
            /// </summary>
            SystemRegistryAppendString = 0x006e,

            /// <summary> 
            /// SystemAitSamplingValue
            /// </summary>
            SystemAitSamplingValue = 0x006f,

            /// <summary> 
            /// SystemVhdBootInformation
            /// </summary>
            SystemVhdBootInformation = 0x0070,

            /// <summary> 
            /// SystemCpuQuotaInformation
            /// </summary>
            SystemCpuQuotaInformation = 0x0071,

            /// <summary> 
            /// SystemNativeBasicInformation
            /// </summary>
            SystemNativeBasicInformation = 0x0072,

            /// <summary> 
            /// SystemErrorPortTimeouts
            /// </summary>
            SystemErrorPortTimeouts = 0x0073,

            /// <summary> 
            /// SystemLowPriorityIoInformation
            /// </summary>
            SystemLowPriorityIoInformation = 0x0074,

            /// <summary> 
            /// SystemBootEntropyInformation
            /// </summary>
            SystemBootEntropyInformation = 0x0075,

            /// <summary> 
            /// SystemVerifierCountersInformation
            /// </summary>
            SystemVerifierCountersInformation = 0x0076,

            /// <summary> 
            /// SystemPagedPoolInformationEx
            /// </summary>
            SystemPagedPoolInformationEx = 0x0077,

            /// <summary> 
            /// SystemSystemPtesInformationEx
            /// </summary>
            SystemSystemPtesInformationEx = 0x0078,

            /// <summary> 
            /// SystemNodeDistanceInformation
            /// </summary>
            SystemNodeDistanceInformation = 0x0079,

            /// <summary> 
            /// SystemAcpiAuditInformation
            /// </summary>
            SystemAcpiAuditInformation = 0x007a,

            /// <summary> 
            /// SystemBasicPerformanceInformation
            /// </summary>
            SystemBasicPerformanceInformation = 0x007b,

            /// <summary> 
            /// SystemQueryPerformanceCounterInformation
            /// </summary>
            SystemQueryPerformanceCounterInformation = 0x007c,

            /// <summary> 
            /// SystemSessionBigPoolInformation
            /// </summary>
            SystemSessionBigPoolInformation = 0x007d,

            /// <summary> 
            /// SystemBootGraphicsInformation
            /// </summary>
            SystemBootGraphicsInformation = 0x007e,

            /// <summary> 
            /// SystemScrubPhysicalMemoryInformation
            /// </summary>
            SystemScrubPhysicalMemoryInformation = 0x007f,

            /// <summary> 
            /// SystemBadPageInformation
            /// </summary>
            SystemBadPageInformation = 0x0080,

            /// <summary> 
            /// SystemProcessorProfileControlArea
            /// </summary>
            SystemProcessorProfileControlArea = 0x0081,

            /// <summary> 
            /// SystemCombinePhysicalMemoryInformation
            /// </summary>
            SystemCombinePhysicalMemoryInformation = 0x0082,

            /// <summary> 
            /// SystemEntropyInterruptTimingInformation
            /// </summary>
            SystemEntropyInterruptTimingInformation = 0x0083,

            /// <summary> 
            /// SystemConsoleInformation
            /// </summary>
            SystemConsoleInformation = 0x0084,

            /// <summary> 
            /// SystemPlatformBinaryInformation
            /// </summary>
            SystemPlatformBinaryInformation = 0x0085,

            /// <summary> 
            /// SystemThrottleNotificationInformation
            /// </summary>
            SystemThrottleNotificationInformation = 0x0086,

            /// <summary> 
            /// SystemHypervisorProcessorCountInformation
            /// </summary>
            SystemHypervisorProcessorCountInformation = 0x0087,

            /// <summary> 
            /// SystemDeviceDataInformation
            /// </summary>
            SystemDeviceDataInformation = 0x0088,

            /// <summary> 
            /// SystemDeviceDataEnumerationInformation
            /// </summary>
            SystemDeviceDataEnumerationInformation = 0x0089,

            /// <summary> 
            /// SystemMemoryTopologyInformation
            /// </summary>
            SystemMemoryTopologyInformation = 0x008a,

            /// <summary> 
            /// SystemMemoryChannelInformation
            /// </summary>
            SystemMemoryChannelInformation = 0x008b,

            /// <summary> 
            /// SystemBootLogoInformation
            /// </summary>
            SystemBootLogoInformation = 0x008c,

            /// <summary> 
            /// SystemProcessorPerformanceInformationEx
            /// </summary>
            SystemProcessorPerformanceInformationEx = 0x008d,

            /// <summary> 
            /// SystemSpare0
            /// </summary>
            SystemSpare0 = 0x008e,

            /// <summary> 
            /// SystemSecureBootPolicyInformation
            /// </summary>
            SystemSecureBootPolicyInformation = 0x008f,

            /// <summary> 
            /// SystemPageFileInformationEx
            /// </summary>
            SystemPageFileInformationEx = 0x0090,

            /// <summary> 
            /// SystemSecureBootInformation
            /// </summary>
            SystemSecureBootInformation = 0x0091,

            /// <summary> 
            /// SystemEntropyInterruptTimingRawInformation
            /// </summary>
            SystemEntropyInterruptTimingRawInformation = 0x0092,

            /// <summary> 
            /// SystemPortableWorkspaceEfiLauncherInformation
            /// </summary>
            SystemPortableWorkspaceEfiLauncherInformation = 0x0093,

            /// <summary> 
            /// SystemFullProcessInformation
            /// </summary>
            SystemFullProcessInformation = 0x0094,

            /// <summary> 
            /// MaxSystemInfoClass
            /// </summary>
            MaxSystemInfoClass = 0x0095
        }

        /// <summary>
        /// Represents the type of information to supply about an object.
        /// </summary>
        public enum ObjectInformationClass : int
        {
            /// <summary>
            /// An <see cref="OBJECT_BASIC_INFORMATION"/> structure is supplied.
            /// </summary>
            ObjectBasicInformation = 0,

            /// <summary>
            /// An <see cref="OBJECT_NAME_INFORMATION"/> structure is supplied.
            /// </summary>
            ObjectNameInformation = 1,

            /// <summary>
            /// An <see cref="OBJECT_TYPE_INFORMATION"/> structure is supplied.
            /// </summary>
            ObjectTypeInformation = 2,

            /// <summary>
            /// An <see cref="OBJECT_ALL_TYPES_INFORMATION"/> structure is supplied.
            /// </summary>
            ObjectAllTypesInformation = 3,

            /// <summary>
            /// An <see cref="OBJECT_HANDLE_INFORMATION"/> structure is supplied.
            /// </summary>
            ObjectHandleInformation = 4,

            /// <summary>
            /// An <see cref="OBJECT_SESSION_INFORMATION"/> structure is supplied.
            /// </summary>
            ObjectSessionInformation = 5
        }

        /// <summary>
        /// すべての拡張ハンドル情報を定義します。
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct SYSTEM_EXTENDED_HANDLE_INFORMATION
        {
            /// <summary>
            /// ハンドルの数。
            /// </summary>
            public IntPtr NumberOfHandles;

            /// <summary>
            /// 予約されています。
            /// </summary>
            public IntPtr Reserved;
        }

        /// <summary>
        /// 拡張ハンドル情報を定義します。
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        struct SYSTEM_HANDLE_TABLE_ENTRY_INFO_EX
        {
            /// <summary>
            /// Object.
            /// </summary>
            public IntPtr Object;

            /// <summary>
            /// 所有者のプロセス ID。
            /// </summary>
            public IntPtr UniqueProcessId;

            /// <summary>
            /// ハンドル。
            /// </summary>
            public IntPtr HandleValue;

            /// <summary>
            /// このハンドルのアクセス権。
            /// </summary>
            public uint GrantedAccess;

            /// <summary>
            /// CreatorBackTraceIndex.
            /// </summary>
            public ushort CreatorBackTraceIndex;

            /// <summary>
            /// ハンドルの型インデックス。
            /// </summary>
            public ushort ObjectTypeIndex;

            /// <summary>
            /// HandleAttributes.
            /// </summary>
            public uint HandleAttributes;

            /// <summary>
            /// Reserved.
            /// </summary>
            public uint Reserved;
        }

        /// <summary>
        /// Define Unicode strings.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct UNICODE_STRING
        {
            /// <summary>
            /// The length, in bytes, of the string stored in Buffer.
            /// </summary>
            public ushort Length;

            /// <summary>
            /// The length, in bytes, of Buffer.
            /// </summary>
            public ushort MaximumLength;

            /// <summary>
            /// Pointer to a buffer used to contain a string of wide characters.
            /// </summary>
            public IntPtr Buffer;

            /// <summary>
            /// 現在のオブジェクトを表す文字列を返します。
            /// </summary>
            /// <returns>現在のオブジェクトを表す文字列。</returns>
            public override string ToString()
            {
                if (Buffer == IntPtr.Zero)
                {
                    return string.Empty;
                }

                return Marshal.PtrToStringUni(Buffer, Length / 2);
            }
        }

        /// <summary>
        /// Defines the mapping of generic access rights to specific and standard access rights for an object.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct GENERIC_MAPPING
        {
            /// <summary>
            /// Specifies an access mask defining read access to an object.
            /// </summary>
            public uint GenericRead;

            /// <summary>
            /// Specifies an access mask defining write access to an object.
            /// </summary>
            public uint GenericWrite;

            /// <summary>
            /// Specifies an access mask defining execute access to an object.
            /// </summary>
            public uint GenericExecute;

            /// <summary>
            /// Specifies an access mask defining all possible types of access to an object.
            /// </summary>
            public uint GenericAll;
        }

        /// <summary>
        /// オブジェクトの基本情報を定義します。
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct OBJECT_BASIC_INFORMATION
        {
            /// <summary>
            /// Attributes.
            /// </summary>
            public uint Attributes;

            /// <summary>
            /// GrantedAccess.
            /// </summary>
            public uint GrantedAccess;

            /// <summary>
            /// HandleCount.
            /// </summary>
            public uint HandleCount;

            /// <summary>
            /// PointerCount.
            /// </summary>
            public uint PointerCount;

            /// <summary>
            /// PagedPoolUsage.
            /// </summary>
            public uint PagedPoolUsage;

            /// <summary>
            /// NonPagedPoolUsage.
            /// </summary>
            public uint NonPagedPoolUsage;

            /// <summary>
            /// Reserved.
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public uint[] Reserved;

            /// <summary>
            /// NameInformationLength.
            /// </summary>
            public uint NameInformationLength;

            /// <summary>
            /// TypeInformationLength.
            /// </summary>
            public uint TypeInformationLength;

            /// <summary>
            /// SecurityDescriptorLength.
            /// </summary>
            public uint SecurityDescriptorLength;

            /// <summary>
            /// CreateTime.
            /// </summary>
            public System.Runtime.InteropServices.ComTypes.FILETIME CreateTime;
        }

        /// <summary>
        /// オブジェクトの型情報を定義します。
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct OBJECT_TYPE_INFORMATION
        {
            /// <summary>
            /// Name.
            /// </summary>
            public UNICODE_STRING Name;

            /// <summary>
            /// TotalNumberOfObjects.
            /// </summary>
            public uint TotalNumberOfObjects;

            /// <summary>
            /// TotalNumberOfHandles.
            /// </summary>
            public uint TotalNumberOfHandles;

            /// <summary>
            /// TotalPagedPoolUsage.
            /// </summary>
            public uint TotalPagedPoolUsage;

            /// <summary>
            /// TotalNonPagedPoolUsage.
            /// </summary>
            public uint TotalNonPagedPoolUsage;

            /// <summary>
            /// TotalNamePoolUsage.
            /// </summary>
            public uint TotalNamePoolUsage;

            /// <summary>
            /// TotalHandleTableUsage.
            /// </summary>
            public uint TotalHandleTableUsage;

            /// <summary>
            /// HighWaterNumberOfObjects.
            /// </summary>
            public uint HighWaterNumberOfObjects;

            /// <summary>
            /// HighWaterNumberOfHandles.
            /// </summary>
            public uint HighWaterNumberOfHandles;

            /// <summary>
            /// HighWaterPagedPoolUsage.
            /// </summary>
            public uint HighWaterPagedPoolUsage;

            /// <summary>
            /// HighWaterNonPagedPoolUsage.
            /// </summary>
            public uint HighWaterNonPagedPoolUsage;

            /// <summary>
            /// HighWaterNamePoolUsage.
            /// </summary>
            public uint HighWaterNamePoolUsage;

            /// <summary>
            /// HighWaterHandleTableUsage.
            /// </summary>
            public uint HighWaterHandleTableUsage;

            /// <summary>
            /// InvalidAttributes.
            /// </summary>
            public uint InvalidAttributes;

            /// <summary>
            /// GenericMapping.
            /// </summary>
            public GENERIC_MAPPING GenericMapping;

            /// <summary>
            /// ValidAccess.
            /// </summary>
            public uint ValidAccess;

            /// <summary>
            /// SecurityRequired.
            /// </summary>
            public byte SecurityRequired;

            /// <summary>
            /// MaintainHandleDatabase.
            /// </summary>
            public byte MaintainHandleDatabase;

            /// <summary>
            /// TypeIndex.
            /// </summary>
            /// <remarks>このメンバーは Windows 8.1 以降でサポートされています。</remarks>
            public byte TypeIndex;

            /// <summary>
            /// ReservedByte.
            /// </summary>
            /// <remarks>このメンバーは Windows 8.1 以降でサポートされています。</remarks>
            public byte ReservedByte;

            /// <summary>
            /// PoolType.
            /// </summary>
            public uint PoolType;

            /// <summary>
            /// DefaultPagedPoolCharge.
            /// </summary>
            public uint DefaultPagedPoolCharge;

            /// <summary>
            /// DefaultNonPagedPoolCharge.
            /// </summary>
            public uint DefaultNonPagedPoolCharge;
        }

        /// <summary>
        /// すべての型情報を定義します。
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct OBJECT_ALL_TYPES_INFORMATION
        {
            /// <summary>
            /// 型の数。
            /// </summary>
            public uint NumberOfTypes;
        }

        /// <summary>
        /// オブジェクトのハンドル情報を定義します。
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct OBJECT_HANDLE_INFORMATION
        {
            /// <summary>
            /// 継承されたハンドルかどうか。
            /// </summary>
            public byte Inherit;

            /// <summary>
            /// クローズ操作から保護されているかどうか。
            /// </summary>
            public byte ProtectFromClose;
        }

        /// <summary>
        /// オブジェクトの名称情報を定義します。
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct OBJECT_NAME_INFORMATION
        {
            /// <summary>
            /// 名称。
            /// </summary>
            public UNICODE_STRING Name;
        }

        /// <summary>
        /// オブジェクトのセッション情報を定義します。
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct OBJECT_SESSION_INFORMATION
        {
            /// <summary>
            /// セッション ID。
            /// </summary>
            public UInt32 SessionId;
        }

        /// <summary>
        /// Retrieves the specified system information.
        /// </summary>
        /// <param name="infoClass">Indicate the kind of system information to be retrieved.</param>
        /// <param name="info">A buffer that receives the requested information.</param>
        /// <param name="size">The allocation size of the buffer pointed to by Info.</param>
        /// <param name="length">If null, ignored. Otherwise tells you the size of the information returned by the kernel.</param>
        /// <returns>Status Information.</returns>
        [DllImport("ntdll.dll")]
        public static extern NtStatus NtQuerySystemInformation(SystemInformationClass infoClass, IntPtr info, uint size, out uint length);

        /// <summary>
        /// Retrieves the specified system information.
        /// </summary>
        /// <param name="infoClass">Indicate the kind of system information to be retrieved.</param>
        /// <param name="infoLength">The allocation size of the buffer pointed to by Info and size of the information returned by the kernel.</param>
        /// <returns>A buffer that receives the requested information.</returns>
        /// <exception cref="Win32Exception">API の呼び出しに失敗しました。</exception>
        public static IntPtr NtQuerySystemInformation(SystemInformationClass infoClass, ref uint infoLength)
        {
            NtStatus result = NtStatus.STATUS_SUCCESS;

            if (infoLength == 0)
            {
                infoLength = 0x10000;
            }

            for (int tries = 0; tries < 5; tries++)
            {
                IntPtr infoPtr = Marshal.AllocCoTaskMem((int)infoLength);
                result = NtQuerySystemInformation(infoClass, infoPtr, infoLength, out infoLength);

                if (result == NtStatus.STATUS_SUCCESS)
                {
                    return infoPtr;
                }

                Marshal.FreeCoTaskMem(infoPtr);

                if ((result != NtStatus.STATUS_INFO_LENGTH_MISMATCH) &&
                    (result != NtStatus.STATUS_BUFFER_OVERFLOW) &&
                    (result != NtStatus.STATUS_BUFFER_TOO_SMALL))
                {
                    throw new Win32Exception(result.ToString());
                }
            }

            throw new Win32Exception(result.ToString());
        }

        /// <summary>
        /// Creates a handle that is a duplicate of the specified source handle.
        /// </summary>
        /// <param name="SourceProcessHandle">A handle to the source process for the handle being duplicated.</param>
        /// <param name="SourceHandle">The handle to duplicate.</param>
        /// <param name="TargetProcessHandle">A handle to the target process that is to receive the new handle.</param>
        /// <param name="TargetHandle">A pointer to a HANDLE variable into which the routine writes the new duplicated handle.</param>
        /// <param name="DesiredAccess">An ACCESS_MASK value that specifies the desired access for the new handle.</param>
        /// <param name="Attributes">A ULONG that specifies the desired attributes for the new handle.</param>
        /// <param name="Options">A set of flags to control the behavior of the duplication operation.</param>
        /// <returns><see cref="NtStatus.STATUS_SUCCESS"/> if the call is successful. Otherwise, it returns an appropriate error status code.</returns>
        [DllImport("ntdll.dll")]
        static extern NtStatus NtDuplicateObject(IntPtr SourceProcessHandle, IntPtr SourceHandle, IntPtr TargetProcessHandle, out IntPtr TargetHandle, uint DesiredAccess, uint Attributes, uint Options);

        /// <summary>
        /// Retrieves various kinds of object information.
        /// </summary>
        /// <param name="objectHandle">The handle of the object for which information is being queried.</param>
        /// <param name="objectInformationClass">One of the following values, as enumerated in <see cref="ObjectInformationClass"/>, indicating the kind of object information to be retrieved.</param>
        /// <param name="objectInformation">An optional pointer to a buffer where the requested information is to be returned.</param>
        /// <param name="objectInformationLength">The size of the buffer pointed to by the <see para="objectInformation"/> parameter, in bytes.</param>
        /// <param name="returnLength">An optional pointer to a location where the function writes the actual size of the information requested.</param>
        /// <returns>Returns an <see cref="NtStatus"/> or error code.</returns>
        [DllImport("ntdll.dll")]
        public static extern NtStatus NtQueryObject(IntPtr objectHandle, ObjectInformationClass objectInformationClass, IntPtr objectInformation, uint objectInformationLength, out uint returnLength);

        /// <summary>
        /// Retrieves the specified object information.
        /// </summary>
        /// <param name="infoClass">Indicate the kind of object information to be retrieved.</param>
        /// <param name="infoLength">The allocation size of the buffer pointed to by Info and size of the information returned by the kernel.</param>
        /// <returns>A buffer that receives the requested information.</returns>
        /// <exception cref="Win32Exception">API の呼び出しに失敗しました。</exception>
        public static IntPtr NtQueryObject(ObjectInformationClass infoClass, ref uint infoLength)
        {
            NtStatus result = NtStatus.STATUS_SUCCESS;

            if (infoLength == 0)
            {
                infoLength = 0x10000;
            }

            for (int tries = 0; tries < 5; tries++)
            {
                IntPtr infoPtr = Marshal.AllocCoTaskMem((int)infoLength);
                result = NtQueryObject(IntPtr.Zero, infoClass, infoPtr, infoLength, out infoLength);

                if (result == NtStatus.STATUS_SUCCESS)
                {
                    return infoPtr;
                }

                Marshal.FreeCoTaskMem(infoPtr);

                if ((result != NtStatus.STATUS_INFO_LENGTH_MISMATCH) &&
                    (result != NtStatus.STATUS_BUFFER_OVERFLOW) &&
                    (result != NtStatus.STATUS_BUFFER_TOO_SMALL))
                {
                    throw new Win32Exception(result.ToString());
                }
            }

            throw new Win32Exception(result.ToString());
        }

        /// <summary>
        /// This routine is used to set handle information about a specified handle.
        /// </summary>
        /// <param name="handle">Supplies the handle being modified.</param>
        /// <param name="objectInformationClass">Specifies the class of information being modified.  The only accepted value is ObjectHandleFlagInformation.</param>
        /// <param name="objectInformation">Supplies the buffer containing the handle flag information structure.</param>
        /// <param name="objectInformationLength">Specifies the length, in bytes, of the object information buffer.</param>
        /// <returns>An appropriate status value.</returns>
        [DllImport("ntdll.dll")]
        public static extern NtStatus NtSetInformationObject(IntPtr handle, ObjectInformationClass objectInformationClass, IntPtr objectInformation, uint objectInformationLength);

        #endregion

        /// <summary>
        /// ローカルファイルのパスの先頭に含まれる文字列を表します。
        /// </summary>
        private const string HARDDISK_PREFIX = @"\Device\Harddisk";

        /// <summary>
        /// ネットワークファイルのパスの先頭に含まれる文字列を表します。
        /// </summary>
        private const string NETWORK_PREFIX = @"\Device\Mup\";

        /// <summary>
        /// MS-DOS デバイス名と論理ドライブ名の関連付けを保持します。
        /// </summary>
        private static Dictionary<string, string> dosDeviceToLogicalDevice = null;

        /// <summary>
        /// デバイスパスを論理パスに変換します。
        /// </summary>
        /// <param name="strRawName">デバイスパス。</param>
        /// <returns>論理パス。</returns>
        private static string GetRegularFileNameFromDevice(string strRawName)
        {
            if (strRawName.StartsWith(NETWORK_PREFIX) == true)
            {
                return @"\\" + strRawName.Substring(NETWORK_PREFIX.Length);
            }

            if (strRawName.StartsWith(HARDDISK_PREFIX) == false)
            {
                return strRawName;
            }

            if (dosDeviceToLogicalDevice == null)
            {
                dosDeviceToLogicalDevice = new Dictionary<string, string>();
                foreach (string drvPath in Environment.GetLogicalDrives())
                {
                    string drv = drvPath.Substring(0, 2);
                    StringBuilder sb = new StringBuilder(MAX_PATH);
                    if (QueryDosDevice(drv, sb, MAX_PATH) != 0)
                    {
                        string drvRoot = sb.ToString();
                        dosDeviceToLogicalDevice.Add(drvRoot, drv);
                    }
                }
            }

            string strFileName = strRawName;
            foreach (string drvRoot in dosDeviceToLogicalDevice.Keys)
            {
                if (strFileName.StartsWith(drvRoot))
                {
                    strFileName = dosDeviceToLogicalDevice[drvRoot] + strFileName.Substring(drvRoot.Length);
                    break;
                }
            }

            return strFileName;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            IntPtr pExHandleInfo = IntPtr.Zero;
            IntPtr hProcess = IntPtr.Zero;
            IntPtr processId = IntPtr.Zero;
            Dictionary<ushort, string> typeIndexToTypeName = new Dictionary<ushort, string>();
            IntPtr pAttrInfo = IntPtr.Zero;

            // Process.GetCurrentProcess() を連続で呼び出すと、GC されるまでハンドルがたまってしまう。
            // このハンドルはキャッシュしておく。
            IntPtr CurrentProcessHandle = Process.GetCurrentProcess().Handle;

            try
            {
                // デバッガ特権を有効にする
                EnablePrivilege(SE_DEBUG_NAME);

                // 型の一覧を取得する。
                IntPtr pTypesInfo = IntPtr.Zero;
                uint typesLength = 0;
                // 取得に失敗した場合は、例外で通知される。
                pTypesInfo = NtQueryObject(ObjectInformationClass.ObjectAllTypesInformation, ref typesLength);
                OBJECT_ALL_TYPES_INFORMATION typesInfo = (OBJECT_ALL_TYPES_INFORMATION)Marshal.PtrToStructure(pTypesInfo, typeof(OBJECT_ALL_TYPES_INFORMATION));
                uint nTypes = (uint)typesInfo.NumberOfTypes;
                IntPtr pRecord = pTypesInfo + Marshal.SizeOf(typeof(OBJECT_ALL_TYPES_INFORMATION));
                for (ushort nType=0;nType< nTypes;nType++)
                {
                    while ((long)pRecord % Marshal.SizeOf(typeof(IntPtr)) != 0)
                    {
                        pRecord = IntPtr.Add(pRecord, Marshal.SizeOf(typeof(IntPtr)) - (int)((long)pRecord % Marshal.SizeOf(typeof(IntPtr))));
                    }
                    OBJECT_TYPE_INFORMATION typeInfo;
                    typeInfo = (OBJECT_TYPE_INFORMATION)Marshal.PtrToStructure(pRecord, typeof(OBJECT_TYPE_INFORMATION));
                    typeIndexToTypeName.Add((ushort)(nType + 2), typeInfo.Name.ToString());
                    pRecord = pRecord + Marshal.SizeOf(typeof(OBJECT_TYPE_INFORMATION)) + typeInfo.Name.MaximumLength;
                }
                Marshal.FreeCoTaskMem(pTypesInfo);

                // ハンドル解放時の属性変更用パラメータを事前に生成しておく
                OBJECT_HANDLE_INFORMATION objectHandleAttributeInformation = new OBJECT_HANDLE_INFORMATION()
                {
                    ProtectFromClose = FALSE
                };
                pAttrInfo = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(OBJECT_HANDLE_INFORMATION)));
                Marshal.StructureToPtr(objectHandleAttributeInformation, pAttrInfo, false);

                // ハンドルの一覧を取得する。取得に失敗した場合は、例外で通知される。
                uint infoLength = 0;
                pExHandleInfo = NtQuerySystemInformation(SystemInformationClass.SystemExtendedHandleInformation, ref infoLength);

                // 取得できたハンドルの個数を得る
                SYSTEM_EXTENDED_HANDLE_INFORMATION exHandleInfo = (SYSTEM_EXTENDED_HANDLE_INFORMATION)Marshal.PtrToStructure(pExHandleInfo, typeof(SYSTEM_EXTENDED_HANDLE_INFORMATION));
                int nHandles = (int)exHandleInfo.NumberOfHandles;

                //Console.WriteLine("Handles: {0}\r\n", nHandles);

                Console.WriteLine("\"Handle\"\t\"PID\"\t\"GrantedAccess\"\t\"Attributes\"\t\"Count\"\t\"Type\"\t\"Name\"");

                // 取得できたハンドル分繰り返す
                for (int iHandle = 0; iHandle < nHandles; iHandle++)
                {
                    IntPtr hDuplicated = IntPtr.Zero;
                    IntPtr pTypeInfo = IntPtr.Zero;
                    IntPtr pNameInfo = IntPtr.Zero;

                    try
                    {
                        // ハンドルの基本情報を取得
                        SYSTEM_HANDLE_TABLE_ENTRY_INFO_EX handleInfo;
                        handleInfo = (SYSTEM_HANDLE_TABLE_ENTRY_INFO_EX)Marshal.PtrToStructure(IntPtr.Add(pExHandleInfo, Marshal.SizeOf(typeof(SYSTEM_EXTENDED_HANDLE_INFORMATION)) + Marshal.SizeOf(typeof(SYSTEM_HANDLE_TABLE_ENTRY_INFO_EX)) * iHandle), typeof(SYSTEM_HANDLE_TABLE_ENTRY_INFO_EX));

                        // プロセスを開く(ループの前回値と同じだったらそのままハンドルを使いまわす)
                        if (processId != handleInfo.UniqueProcessId)
                        {
                            if (hProcess != IntPtr.Zero)
                            {
                                CloseHandle(hProcess);
                            }
                            hProcess = OpenProcess(ProcessAccessFlags.DuplicateHandle, false, (int)handleInfo.UniqueProcessId);
                            processId = handleInfo.UniqueProcessId;
                        }

                        if (Marshal.SizeOf(typeof(IntPtr)) == 4)
                        {
                            Console.Write("0x{0:x8}\t{1}\t0x{2:x8}\t0x{3:x8}", (ulong)handleInfo.HandleValue, (ulong)handleInfo.UniqueProcessId, handleInfo.GrantedAccess, handleInfo.HandleAttributes);
                        }
                        else
                        {
                            Console.Write("0x{0:x16}\t{1}\t0x{2:x8}\t0x{3:x8}", (ulong)handleInfo.HandleValue, (ulong)handleInfo.UniqueProcessId, handleInfo.GrantedAccess, handleInfo.HandleAttributes);
                        }

                        if (hProcess == IntPtr.Zero)
                        {
                            // そのプロセスは開けなかった。
                            Console.WriteLine("\t\t\"{0}\"\t", typeIndexToTypeName[handleInfo.ObjectTypeIndex]);
                            continue;
                        }

                        // 自身のプロセスにハンドルを複製
                        NtStatus dupResult = NtDuplicateObject(hProcess, handleInfo.HandleValue, CurrentProcessHandle, out hDuplicated, 0, 0, DUPLICATE_SAME_ATTRIBUTES);

                        if (dupResult != NtStatus.STATUS_SUCCESS)
                        {
                            // そのハンドルは複製できなかった。
                            Console.WriteLine("\t\t\"{0}\"\t", typeIndexToTypeName[handleInfo.ObjectTypeIndex]);
                            continue;
                        }

                        // オブジェクト基本情報を取得
                        uint basicLen = (uint)Marshal.SizeOf(typeof(OBJECT_BASIC_INFORMATION));
                        uint basicLenOut = 0;
                        OBJECT_BASIC_INFORMATION basicInfo;

                        IntPtr pBasicInfo = Marshal.AllocCoTaskMem((int)basicLen);
                        ZeroMemory(pBasicInfo, new IntPtr(basicLen));

                        NtStatus basicInfoResult = NtQueryObject(hDuplicated, ObjectInformationClass.ObjectBasicInformation, pBasicInfo, basicLen, out basicLenOut);
                        basicInfo = (OBJECT_BASIC_INFORMATION)Marshal.PtrToStructure(pBasicInfo, typeof(OBJECT_BASIC_INFORMATION));
                        Marshal.FreeCoTaskMem(pBasicInfo);

                        if (basicInfoResult != NtStatus.STATUS_SUCCESS)
                        {
                            // オブジェクト基本情報を取得できなかった。
                            Console.WriteLine("\t\t\"{0}\"\t", typeIndexToTypeName[handleInfo.ObjectTypeIndex]);
                            continue;
                        }

                        // ファイルの場合は、詳細種別を付加する
                        string typeInfoName = typeIndexToTypeName[handleInfo.ObjectTypeIndex];
                        FileType fileType = GetFileType(hDuplicated);
                        if (typeInfoName == "File")
                        {
                            typeInfoName = string.Concat(typeInfoName, string.Format("({0})", fileType));
                        }

                        // オブジェクトの名称を取得する
                        OBJECT_NAME_INFORMATION nameInfo;
                        uint nameLen = basicInfo.NameInformationLength;
                        string objectName = null;
                        if (nameLen == 0)
                        {
                            // ファイルパスの場合は、既定の長さでバッファを確保する
                            nameLen = MAX_PATH * 2;
                        }

                        if ((typeIndexToTypeName[handleInfo.ObjectTypeIndex] == "File") &&
                            ((fileType == FileType.Pipe) || (handleInfo.GrantedAccess == 0x0012019f)))
                        {
                            // 名前を問い合わせると帰ってこないケースは名称問い合わせをスキップする
                            // Query the object name (unless it has an access of 0x0012019f, on which NtQueryObject could hang.
                        }
                        else
                        {
                            uint nameLenOut = 0;

                            pNameInfo = Marshal.AllocCoTaskMem((int)nameLen);
                            ZeroMemory(pNameInfo, new IntPtr(nameLen));

                            NtStatus nameInfoResult = NtQueryObject(hDuplicated, ObjectInformationClass.ObjectNameInformation, pNameInfo, nameLen, out nameLenOut);
                            if (nameInfoResult == NtStatus.STATUS_SUCCESS)
                            {
                                nameInfo = (OBJECT_NAME_INFORMATION)Marshal.PtrToStructure(pNameInfo, typeof(OBJECT_NAME_INFORMATION));
                                objectName = nameInfo.Name.ToString();
                            }
                        }

                        if (string.IsNullOrEmpty(objectName) != true)
                        {
                            objectName = string.Concat("\"", GetRegularFileNameFromDevice(objectName), "\"");
                        }

                        // 結果の表示
                        // basicInfo.HandleCount は、複製しているため必ず 1 つ参照が増えている。これを減らして表示する。
                        Console.WriteLine("\t{0}\t\"{1}\"\t{2}", basicInfo.HandleCount - 1, typeInfoName, objectName);

                        // ハンドルを複製したときに、元のハンドルにクローズ操作の保護属性が含まれていた場合、
                        // 保護状態のままハンドルを複製する。そのため、保護を解いてあげないと、閉じることができない。
                        // handleInfo.HandleAttributes だけで判断せずすべてのハンドルで保護を解除しないと、例外が出るケースがある。
                        //×if ((handleInfo.HandleAttributes & PROTECT_FROM_CLOSE) != 0)
                        //×{
                        NtStatus setHandleInfoResult = NtSetInformationObject(hDuplicated, ObjectInformationClass.ObjectHandleInformation, pAttrInfo, (uint)Marshal.SizeOf(typeof(OBJECT_HANDLE_INFORMATION)));
                        //×}
                    }
                    finally
                    {
                        if (pNameInfo != IntPtr.Zero)
                        {
                            Marshal.FreeCoTaskMem(pNameInfo);
                        }
                        if (hDuplicated != IntPtr.Zero)
                        {
                            CloseHandle(hDuplicated);
                        }
                    }
                }

                //Console.WriteLine("\r\nDone.");
            }
            finally
            {
                if (pAttrInfo != IntPtr.Zero)
                {
                    Marshal.FreeCoTaskMem(pAttrInfo);
                }
                if (hProcess != IntPtr.Zero)
                {
                    CloseHandle(hProcess);
                }
                if (pExHandleInfo != IntPtr.Zero)
                {
                    Marshal.FreeCoTaskMem(pExHandleInfo);
                }
            }
        }
    }
}
