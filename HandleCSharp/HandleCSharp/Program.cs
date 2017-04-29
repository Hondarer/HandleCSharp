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
    class Program
    {
        #region 基本的な宣言

        /// <summary>
        /// <c>false</c> を表します。
        /// </summary>
        public const int FALSE = 0;

        #endregion

        #region エラー処理

        /// <summary>
        /// Retrieves the calling thread's last-error code value.
        /// </summary>
        /// <returns>Calling thread's last-error code.</returns>
        [DllImport("kernel32.dll")]
        public static extern uint GetLastError();

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
                    uint lastError = GetLastError();
                    throw new Win32Exception(lastError.ToString());
                }


                if (OpenProcessToken(GetCurrentProcess(), TOKEN_ADJUST_PRIVILEGES, out hToken) == false)
                {
                    uint lastError = GetLastError();
                    throw new Win32Exception(lastError.ToString());
                }

                if (AdjustTokenPrivileges(hToken, false, ref privileges, (uint)Marshal.SizeOf(privileges), IntPtr.Zero, IntPtr.Zero) == false)
                {
                    uint lastError = GetLastError();
                    throw new Win32Exception(lastError.ToString());
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
        public enum NtStatus : uint
        {
            /// <summary>
            /// 操作は正常に終了しました。
            /// </summary>
            Success = 0x00000000,

            Wait0 = 0x00000000,
            Wait1 = 0x00000001,
            Wait2 = 0x00000002,
            Wait3 = 0x00000003,
            Wait63 = 0x0000003f,
            Abandoned = 0x00000080,
            AbandonedWait0 = 0x00000080,
            AbandonedWait1 = 0x00000081,
            AbandonedWait2 = 0x00000082,
            AbandonedWait3 = 0x00000083,
            AbandonedWait63 = 0x000000bf,
            UserApc = 0x000000c0,
            KernelApc = 0x00000100,
            Alerted = 0x00000101,
            Timeout = 0x00000102,
            Pending = 0x00000103,
            Reparse = 0x00000104,
            MoreEntries = 0x00000105,
            NotAllAssigned = 0x00000106,
            SomeNotMapped = 0x00000107,
            OpLockBreakInProgress = 0x00000108,
            VolumeMounted = 0x00000109,
            RxActCommitted = 0x0000010a,
            NotifyCleanup = 0x0000010b,
            NotifyEnumDir = 0x0000010c,
            NoQuotasForAccount = 0x0000010d,
            PrimaryTransportConnectFailed = 0x0000010e,
            PageFaultTransition = 0x00000110,
            PageFaultDemandZero = 0x00000111,
            PageFaultCopyOnWrite = 0x00000112,
            PageFaultGuardPage = 0x00000113,
            PageFaultPagingFile = 0x00000114,
            CrashDump = 0x00000116,
            ReparseObject = 0x00000118,
            NothingToTerminate = 0x00000122,
            ProcessNotInJob = 0x00000123,
            ProcessInJob = 0x00000124,
            ProcessCloned = 0x00000129,
            FileLockedWithOnlyReaders = 0x0000012a,
            FileLockedWithWriters = 0x0000012b,

            // Informational
            Informational = 0x40000000,
            ObjectNameExists = 0x40000000,
            ThreadWasSuspended = 0x40000001,
            WorkingSetLimitRange = 0x40000002,
            ImageNotAtBase = 0x40000003,
            RegistryRecovered = 0x40000009,

            // Warning
            Warning = 0x80000000,
            GuardPageViolation = 0x80000001,
            DatatypeMisalignment = 0x80000002,
            Breakpoint = 0x80000003,
            SingleStep = 0x80000004,

            /// <summary>
            /// データが大きすぎるため、指定したバッファに格納できません。
            /// </summary>
            BufferOverflow = 0x80000005,

            NoMoreFiles = 0x80000006,
            HandlesClosed = 0x8000000a,
            PartialCopy = 0x8000000d,
            DeviceBusy = 0x80000011,
            InvalidEaName = 0x80000013,
            EaListInconsistent = 0x80000014,
            NoMoreEntries = 0x8000001a,
            LongJump = 0x80000026,
            DllMightBeInsecure = 0x8000002b,

            // Error
            Error = 0xc0000000,
            Unsuccessful = 0xc0000001,
            NotImplemented = 0xc0000002,
            InvalidInfoClass = 0xc0000003,

            /// <summary>
            /// 指定した情報レコードの長さは、指定した情報クラスに対して必要な長さと一致しません。
            /// </summary>
            InfoLengthMismatch = 0xc0000004,

            AccessViolation = 0xc0000005,
            InPageError = 0xc0000006,
            PagefileQuota = 0xc0000007,
            InvalidHandle = 0xc0000008,
            BadInitialStack = 0xc0000009,
            BadInitialPc = 0xc000000a,
            InvalidCid = 0xc000000b,
            TimerNotCanceled = 0xc000000c,
            InvalidParameter = 0xc000000d,
            NoSuchDevice = 0xc000000e,
            NoSuchFile = 0xc000000f,
            InvalidDeviceRequest = 0xc0000010,
            EndOfFile = 0xc0000011,
            WrongVolume = 0xc0000012,
            NoMediaInDevice = 0xc0000013,
            NoMemory = 0xc0000017,
            NotMappedView = 0xc0000019,
            UnableToFreeVm = 0xc000001a,
            UnableToDeleteSection = 0xc000001b,
            IllegalInstruction = 0xc000001d,
            AlreadyCommitted = 0xc0000021,
            AccessDenied = 0xc0000022,

            /// <summary>
            /// 要求した操作で必要なオブジェクトの種類と要求に指定したオブジェクトの種類が一致しません。
            /// </summary>
            BufferTooSmall = 0xc0000023,

            ObjectTypeMismatch = 0xc0000024,
            NonContinuableException = 0xc0000025,
            BadStack = 0xc0000028,
            NotLocked = 0xc000002a,
            NotCommitted = 0xc000002d,
            InvalidParameterMix = 0xc0000030,
            ObjectNameInvalid = 0xc0000033,
            ObjectNameNotFound = 0xc0000034,
            ObjectNameCollision = 0xc0000035,
            ObjectPathInvalid = 0xc0000039,
            ObjectPathNotFound = 0xc000003a,
            ObjectPathSyntaxBad = 0xc000003b,
            DataOverrun = 0xc000003c,
            DataLate = 0xc000003d,
            DataError = 0xc000003e,
            CrcError = 0xc000003f,
            SectionTooBig = 0xc0000040,
            PortConnectionRefused = 0xc0000041,
            InvalidPortHandle = 0xc0000042,
            SharingViolation = 0xc0000043,
            QuotaExceeded = 0xc0000044,
            InvalidPageProtection = 0xc0000045,
            MutantNotOwned = 0xc0000046,
            SemaphoreLimitExceeded = 0xc0000047,
            PortAlreadySet = 0xc0000048,
            SectionNotImage = 0xc0000049,
            SuspendCountExceeded = 0xc000004a,
            ThreadIsTerminating = 0xc000004b,
            BadWorkingSetLimit = 0xc000004c,
            IncompatibleFileMap = 0xc000004d,
            SectionProtection = 0xc000004e,
            EasNotSupported = 0xc000004f,
            EaTooLarge = 0xc0000050,
            NonExistentEaEntry = 0xc0000051,
            NoEasOnFile = 0xc0000052,
            EaCorruptError = 0xc0000053,
            FileLockConflict = 0xc0000054,
            LockNotGranted = 0xc0000055,
            DeletePending = 0xc0000056,
            CtlFileNotSupported = 0xc0000057,
            UnknownRevision = 0xc0000058,
            RevisionMismatch = 0xc0000059,
            InvalidOwner = 0xc000005a,
            InvalidPrimaryGroup = 0xc000005b,
            NoImpersonationToken = 0xc000005c,
            CantDisableMandatory = 0xc000005d,
            NoLogonServers = 0xc000005e,
            NoSuchLogonSession = 0xc000005f,
            NoSuchPrivilege = 0xc0000060,
            PrivilegeNotHeld = 0xc0000061,
            InvalidAccountName = 0xc0000062,
            UserExists = 0xc0000063,
            NoSuchUser = 0xc0000064,
            GroupExists = 0xc0000065,
            NoSuchGroup = 0xc0000066,
            MemberInGroup = 0xc0000067,
            MemberNotInGroup = 0xc0000068,
            LastAdmin = 0xc0000069,
            WrongPassword = 0xc000006a,
            IllFormedPassword = 0xc000006b,
            PasswordRestriction = 0xc000006c,
            LogonFailure = 0xc000006d,
            AccountRestriction = 0xc000006e,
            InvalidLogonHours = 0xc000006f,
            InvalidWorkstation = 0xc0000070,
            PasswordExpired = 0xc0000071,
            AccountDisabled = 0xc0000072,
            NoneMapped = 0xc0000073,
            TooManyLuidsRequested = 0xc0000074,
            LuidsExhausted = 0xc0000075,
            InvalidSubAuthority = 0xc0000076,
            InvalidAcl = 0xc0000077,
            InvalidSid = 0xc0000078,
            InvalidSecurityDescr = 0xc0000079,
            ProcedureNotFound = 0xc000007a,
            InvalidImageFormat = 0xc000007b,
            NoToken = 0xc000007c,
            BadInheritanceAcl = 0xc000007d,
            RangeNotLocked = 0xc000007e,
            DiskFull = 0xc000007f,
            ServerDisabled = 0xc0000080,
            ServerNotDisabled = 0xc0000081,
            TooManyGuidsRequested = 0xc0000082,
            GuidsExhausted = 0xc0000083,
            InvalidIdAuthority = 0xc0000084,
            AgentsExhausted = 0xc0000085,
            InvalidVolumeLabel = 0xc0000086,
            SectionNotExtended = 0xc0000087,
            NotMappedData = 0xc0000088,
            ResourceDataNotFound = 0xc0000089,
            ResourceTypeNotFound = 0xc000008a,
            ResourceNameNotFound = 0xc000008b,
            ArrayBoundsExceeded = 0xc000008c,
            FloatDenormalOperand = 0xc000008d,
            FloatDivideByZero = 0xc000008e,
            FloatInexactResult = 0xc000008f,
            FloatInvalidOperation = 0xc0000090,
            FloatOverflow = 0xc0000091,
            FloatStackCheck = 0xc0000092,
            FloatUnderflow = 0xc0000093,
            IntegerDivideByZero = 0xc0000094,
            IntegerOverflow = 0xc0000095,
            PrivilegedInstruction = 0xc0000096,
            TooManyPagingFiles = 0xc0000097,
            FileInvalid = 0xc0000098,
            InstanceNotAvailable = 0xc00000ab,
            PipeNotAvailable = 0xc00000ac,
            InvalidPipeState = 0xc00000ad,
            PipeBusy = 0xc00000ae,
            IllegalFunction = 0xc00000af,
            PipeDisconnected = 0xc00000b0,
            PipeClosing = 0xc00000b1,
            PipeConnected = 0xc00000b2,
            PipeListening = 0xc00000b3,
            InvalidReadMode = 0xc00000b4,
            IoTimeout = 0xc00000b5,
            FileForcedClosed = 0xc00000b6,
            ProfilingNotStarted = 0xc00000b7,
            ProfilingNotStopped = 0xc00000b8,
            NotSameDevice = 0xc00000d4,
            FileRenamed = 0xc00000d5,
            CantWait = 0xc00000d8,
            PipeEmpty = 0xc00000d9,
            CantTerminateSelf = 0xc00000db,
            InternalError = 0xc00000e5,
            InvalidParameter1 = 0xc00000ef,
            InvalidParameter2 = 0xc00000f0,
            InvalidParameter3 = 0xc00000f1,
            InvalidParameter4 = 0xc00000f2,
            InvalidParameter5 = 0xc00000f3,
            InvalidParameter6 = 0xc00000f4,
            InvalidParameter7 = 0xc00000f5,
            InvalidParameter8 = 0xc00000f6,
            InvalidParameter9 = 0xc00000f7,
            InvalidParameter10 = 0xc00000f8,
            InvalidParameter11 = 0xc00000f9,
            InvalidParameter12 = 0xc00000fa,
            MappedFileSizeZero = 0xc000011e,
            TooManyOpenedFiles = 0xc000011f,
            Cancelled = 0xc0000120,
            CannotDelete = 0xc0000121,
            InvalidComputerName = 0xc0000122,
            FileDeleted = 0xc0000123,
            SpecialAccount = 0xc0000124,
            SpecialGroup = 0xc0000125,
            SpecialUser = 0xc0000126,
            MembersPrimaryGroup = 0xc0000127,
            FileClosed = 0xc0000128,
            TooManyThreads = 0xc0000129,
            ThreadNotInProcess = 0xc000012a,
            TokenAlreadyInUse = 0xc000012b,
            PagefileQuotaExceeded = 0xc000012c,
            CommitmentLimit = 0xc000012d,
            InvalidImageLeFormat = 0xc000012e,
            InvalidImageNotMz = 0xc000012f,
            InvalidImageProtect = 0xc0000130,
            InvalidImageWin16 = 0xc0000131,
            LogonServer = 0xc0000132,
            DifferenceAtDc = 0xc0000133,
            SynchronizationRequired = 0xc0000134,
            DllNotFound = 0xc0000135,
            IoPrivilegeFailed = 0xc0000137,
            OrdinalNotFound = 0xc0000138,
            EntryPointNotFound = 0xc0000139,
            ControlCExit = 0xc000013a,
            PortNotSet = 0xc0000353,
            DebuggerInactive = 0xc0000354,
            CallbackBypass = 0xc0000503,
            PortClosed = 0xc0000700,
            MessageLost = 0xc0000701,
            InvalidMessage = 0xc0000702,
            RequestCanceled = 0xc0000703,
            RecursiveDispatch = 0xc0000704,
            LpcReceiveBufferExpected = 0xc0000705,
            LpcInvalidConnectionUsage = 0xc0000706,
            LpcRequestsNotAllowed = 0xc0000707,
            ResourceInUse = 0xc0000708,
            ProcessIsProtected = 0xc0000712,
            VolumeDirty = 0xc0000806,
            FileCheckedOut = 0xc0000901,
            CheckOutRequired = 0xc0000902,
            BadFileType = 0xc0000903,
            FileTooLarge = 0xc0000904,
            FormsAuthRequired = 0xc0000905,
            VirusInfected = 0xc0000906,
            VirusDeleted = 0xc0000907,
            TransactionalConflict = 0xc0190001,
            InvalidTransaction = 0xc0190002,
            TransactionNotActive = 0xc0190003,
            TmInitializationFailed = 0xc0190004,
            RmNotActive = 0xc0190005,
            RmMetadataCorrupt = 0xc0190006,
            TransactionNotJoined = 0xc0190007,
            DirectoryNotRm = 0xc0190008,
            CouldNotResizeLog = 0xc0190009,
            TransactionsUnsupportedRemote = 0xc019000a,
            LogResizeInvalidSize = 0xc019000b,
            RemoteFileVersionMismatch = 0xc019000c,
            CrmProtocolAlreadyExists = 0xc019000f,
            TransactionPropagationFailed = 0xc0190010,
            CrmProtocolNotFound = 0xc0190011,
            TransactionSuperiorExists = 0xc0190012,
            TransactionRequestNotValid = 0xc0190013,
            TransactionNotRequested = 0xc0190014,
            TransactionAlreadyAborted = 0xc0190015,
            TransactionAlreadyCommitted = 0xc0190016,
            TransactionInvalidMarshallBuffer = 0xc0190017,
            CurrentTransactionNotValid = 0xc0190018,
            LogGrowthFailed = 0xc0190019,
            ObjectNoLongerExists = 0xc0190021,
            StreamMiniversionNotFound = 0xc0190022,
            StreamMiniversionNotValid = 0xc0190023,
            MiniversionInaccessibleFromSpecifiedTransaction = 0xc0190024,
            CantOpenMiniversionWithModifyIntent = 0xc0190025,
            CantCreateMoreStreamMiniversions = 0xc0190026,
            HandleNoLongerValid = 0xc0190028,
            NoTxfMetadata = 0xc0190029,
            LogCorruptionDetected = 0xc0190030,
            CantRecoverWithHandleOpen = 0xc0190031,
            RmDisconnected = 0xc0190032,
            EnlistmentNotSuperior = 0xc0190033,
            RecoveryNotNeeded = 0xc0190034,
            RmAlreadyStarted = 0xc0190035,
            FileIdentityNotPersistent = 0xc0190036,
            CantBreakTransactionalDependency = 0xc0190037,
            CantCrossRmBoundary = 0xc0190038,
            TxfDirNotEmpty = 0xc0190039,
            IndoubtTransactionsExist = 0xc019003a,
            TmVolatile = 0xc019003b,
            RollbackTimerExpired = 0xc019003c,
            TxfAttributeCorrupt = 0xc019003d,
            EfsNotAllowedInTransaction = 0xc019003e,
            TransactionalOpenNotAllowed = 0xc019003f,
            TransactedMappingUnsupportedRemote = 0xc0190040,
            TxfMetadataAlreadyPresent = 0xc0190041,
            TransactionScopeCallbacksNotSet = 0xc0190042,
            TransactionRequiredPromotion = 0xc0190043,
            CannotExecuteFileInTransaction = 0xc0190044,
            TransactionsNotFrozen = 0xc0190045,

            MaximumNtStatus = 0xffffffff
        }

        /// <summary>
        /// Tells NtQuerySystemInformation and NtSetSystemInformation what you would like to read/change.
        /// </summary>
        public enum SystemInformationClass
        {
            SystemBasicInformation = 0x0000,
            SystemProcessorInformation = 0x0001,
            SystemPerformanceInformation = 0x0002,
            SystemTimeOfDayInformation = 0x0003,
            SystemPathInformation = 0x0004,
            SystemProcessInformation = 0x0005,
            SystemCallCountInformation = 0x0006,
            SystemDeviceInformation = 0x0007,
            SystemProcessorPerformanceInformation = 0x0008,
            SystemFlagsInformation = 0x0009,
            SystemCallTimeInformation = 0x000A,
            SystemModuleInformation = 0x000B,
            SystemLocksInformation = 0x000C,
            SystemStackTraceInformation = 0x000D,
            SystemPagedPoolInformation = 0x000E,
            SystemNonPagedPoolInformation = 0x000F,
            SystemHandleInformation = 0x0010,
            SystemObjectInformation = 0x0011,
            SystemPageFileInformation = 0x0012,
            SystemVdmInstemulInformation = 0x0013,
            SystemVdmBopInformation = 0x0014,
            SystemFileCacheInformation = 0x0015,
            SystemPoolTagInformation = 0x0016,
            SystemInterruptInformation = 0x0017,
            SystemDpcBehaviorInformation = 0x0018,
            SystemFullMemoryInformation = 0x0019,
            SystemLoadGdiDriverInformation = 0x001A,
            SystemUnloadGdiDriverInformation = 0x001B,
            SystemTimeAdjustmentInformation = 0x001C,
            SystemSummaryMemoryInformation = 0x001D,
            SystemMirrorMemoryInformation = 0x001E,
            SystemPerformanceTraceInformation = 0x001F,
            SystemCrashDumpInformation = 0x0020,
            SystemExceptionInformation = 0x0021,
            SystemCrashDumpStateInformation = 0x0022,
            SystemKernelDebuggerInformation = 0x0023,
            SystemContextSwitchInformation = 0x0024,
            SystemRegistryQuotaInformation = 0x0025,
            SystemExtendServiceTableInformation = 0x0026,
            SystemPrioritySeperation = 0x0027,
            SystemVerifierAddDriverInformation = 0x0028,
            SystemVerifierRemoveDriverInformation = 0x0029,
            SystemProcessorIdleInformation = 0x002A,
            SystemLegacyDriverInformation = 0x002B,
            SystemCurrentTimeZoneInformation = 0x002C,
            SystemLookasideInformation = 0x002D,
            SystemTimeSlipNotification = 0x002E,
            SystemSessionCreate = 0x002F,
            SystemSessionDetach = 0x0030,
            SystemSessionInformation = 0x0031,
            SystemRangeStartInformation = 0x0032,
            SystemVerifierInformation = 0x0033,
            SystemVerifierThunkExtend = 0x0034,
            SystemSessionProcessInformation = 0x0035,
            SystemLoadGdiDriverInSystemSpace = 0x0036,
            SystemNumaProcessorMap = 0x0037,
            SystemPrefetcherInformation = 0x0038,
            SystemExtendedProcessInformation = 0x0039,
            SystemRecommendedSharedDataAlignment = 0x003A,
            SystemComPlusPackage = 0x003B,
            SystemNumaAvailableMemory = 0x003C,
            SystemProcessorPowerInformation = 0x003D,
            SystemEmulationBasicInformation = 0x003E,
            SystemEmulationProcessorInformation = 0x003F,
            SystemExtendedHandleInformation = 0x0040,
            SystemLostDelayedWriteInformation = 0x0041,
            SystemBigPoolInformation = 0x0042,
            SystemSessionPoolTagInformation = 0x0043,
            SystemSessionMappedViewInformation = 0x0044,
            SystemHotpatchInformation = 0x0045,
            SystemObjectSecurityMode = 0x0046,
            SystemWatchdogTimerHandler = 0x0047,
            SystemWatchdogTimerInformation = 0x0048,
            SystemLogicalProcessorInformation = 0x0049,
            SystemWow64SharedInformationObsolete = 0x004A,
            SystemRegisterFirmwareTableInformationHandler = 0x004B,
            SystemFirmwareTableInformation = 0x004C,
            SystemModuleInformationEx = 0x004D,
            SystemVerifierTriageInformation = 0x004E,
            SystemSuperfetchInformation = 0x004F,
            SystemMemoryListInformation = 0x0050,
            SystemFileCacheInformationEx = 0x0051,
            SystemThreadPriorityClientIdInformation = 0x0052,
            SystemProcessorIdleCycleTimeInformation = 0x0053,
            SystemVerifierCancellationInformation = 0x0054,
            SystemProcessorPowerInformationEx = 0x0055,
            SystemRefTraceInformation = 0x0056,
            SystemSpecialPoolInformation = 0x0057,
            SystemProcessIdInformation = 0x0058,
            SystemErrorPortInformation = 0x0059,
            SystemBootEnvironmentInformation = 0x005A,
            SystemHypervisorInformation = 0x005B,
            SystemVerifierInformationEx = 0x005C,
            SystemTimeZoneInformation = 0x005D,
            SystemImageFileExecutionOptionsInformation = 0x005E,
            SystemCoverageInformation = 0x005F,
            SystemPrefetchPatchInformation = 0x0060,
            SystemVerifierFaultsInformation = 0x0061,
            SystemSystemPartitionInformation = 0x0062,
            SystemSystemDiskInformation = 0x0063,
            SystemProcessorPerformanceDistribution = 0x0064,
            SystemNumaProximityNodeInformation = 0x0065,
            SystemDynamicTimeZoneInformation = 0x0066,
            SystemCodeIntegrityInformation = 0x0067,
            SystemProcessorMicrocodeUpdateInformation = 0x0068,
            SystemProcessorBrandString = 0x0069,
            SystemVirtualAddressInformation = 0x006A,
            SystemLogicalProcessorAndGroupInformation = 0x006B,
            SystemProcessorCycleTimeInformation = 0x006C,
            SystemStoreInformation = 0x006D,
            SystemRegistryAppendString = 0x006E,
            SystemAitSamplingValue = 0x006F,
            SystemVhdBootInformation = 0x0070,
            SystemCpuQuotaInformation = 0x0071,
            SystemNativeBasicInformation = 0x0072,
            SystemErrorPortTimeouts = 0x0073,
            SystemLowPriorityIoInformation = 0x0074,
            SystemBootEntropyInformation = 0x0075,
            SystemVerifierCountersInformation = 0x0076,
            SystemPagedPoolInformationEx = 0x0077,
            SystemSystemPtesInformationEx = 0x0078,
            SystemNodeDistanceInformation = 0x0079,
            SystemAcpiAuditInformation = 0x007A,
            SystemBasicPerformanceInformation = 0x007B,
            SystemQueryPerformanceCounterInformation = 0x007C,
            SystemSessionBigPoolInformation = 0x007D,
            SystemBootGraphicsInformation = 0x007E,
            SystemScrubPhysicalMemoryInformation = 0x007F,
            SystemBadPageInformation = 0x0080,
            SystemProcessorProfileControlArea = 0x0081,
            SystemCombinePhysicalMemoryInformation = 0x0082,
            SystemEntropyInterruptTimingInformation = 0x0083,
            SystemConsoleInformation = 0x0084,
            SystemPlatformBinaryInformation = 0x0085,
            SystemThrottleNotificationInformation = 0x0086,
            SystemHypervisorProcessorCountInformation = 0x0087,
            SystemDeviceDataInformation = 0x0088,
            SystemDeviceDataEnumerationInformation = 0x0089,
            SystemMemoryTopologyInformation = 0x008A,
            SystemMemoryChannelInformation = 0x008B,
            SystemBootLogoInformation = 0x008C,
            SystemProcessorPerformanceInformationEx = 0x008D,
            SystemSpare0 = 0x008E,
            SystemSecureBootPolicyInformation = 0x008F,
            SystemPageFileInformationEx = 0x0090,
            SystemSecureBootInformation = 0x0091,
            SystemEntropyInterruptTimingRawInformation = 0x0092,
            SystemPortableWorkspaceEfiLauncherInformation = 0x0093,
            SystemFullProcessInformation = 0x0094,
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
            NtStatus result = NtStatus.Success;

            if (infoLength == 0)
            {
                infoLength = 0x10000;
            }

            for (int tries = 0; tries < 5; tries++)
            {
                IntPtr infoPtr = Marshal.AllocCoTaskMem((int)infoLength);
                result = NtQuerySystemInformation(infoClass, infoPtr, infoLength, out infoLength);

                if (result == NtStatus.Success)
                {
                    return infoPtr;
                }

                Marshal.FreeCoTaskMem(infoPtr);

                if ((result != NtStatus.InfoLengthMismatch) &&
                    (result != NtStatus.BufferOverflow) &&
                    (result != NtStatus.BufferTooSmall))
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
        /// <returns><see cref="NtStatus.Success"/> if the call is successful. Otherwise, it returns an appropriate error status code.</returns>
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
            NtStatus result = NtStatus.Success;

            if (infoLength == 0)
            {
                infoLength = 0x10000;
            }

            for (int tries = 0; tries < 5; tries++)
            {
                IntPtr infoPtr = Marshal.AllocCoTaskMem((int)infoLength);
                result = NtQueryObject(IntPtr.Zero, infoClass, infoPtr, infoLength, out infoLength);

                if (result == NtStatus.Success)
                {
                    return infoPtr;
                }

                Marshal.FreeCoTaskMem(infoPtr);

                if ((result != NtStatus.InfoLengthMismatch) &&
                    (result != NtStatus.BufferOverflow) &&
                    (result != NtStatus.BufferTooSmall))
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

                Console.WriteLine("Handles: {0}\r\n", nHandles);

                Console.WriteLine("Handle\tPID\tGrantedAccess\tAttributes\tCount\tType\tName");

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
                            Console.WriteLine("\t-1\t\"{0}\"\t\"\"", typeIndexToTypeName[handleInfo.ObjectTypeIndex]);
                            continue;
                        }

                        // 自身のプロセスにハンドルを複製
                        NtStatus dupResult = NtDuplicateObject(hProcess, handleInfo.HandleValue, CurrentProcessHandle, out hDuplicated, 0, 0, DUPLICATE_SAME_ATTRIBUTES);

                        if (dupResult != NtStatus.Success)
                        {
                            // そのハンドルは複製できなかった。
                            Console.WriteLine("\t-1\t\"{0}\"\t\"\"", typeIndexToTypeName[handleInfo.ObjectTypeIndex]);
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

                        if (basicInfoResult != NtStatus.Success)
                        {
                            // オブジェクト基本情報を取得できなかった。
                            Console.WriteLine("\t-1\t\"{0}\"\t\"\"", typeIndexToTypeName[handleInfo.ObjectTypeIndex]);
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
                            nameInfo = default(OBJECT_NAME_INFORMATION);
                        }
                        else
                        {
                            uint nameLenOut = 0;

                            pNameInfo = Marshal.AllocCoTaskMem((int)nameLen);
                            ZeroMemory(pNameInfo, new IntPtr(nameLen));

                            NtStatus nameInfoResult = NtQueryObject(hDuplicated, ObjectInformationClass.ObjectNameInformation, pNameInfo, nameLen, out nameLenOut);
                            if (nameInfoResult == NtStatus.Success)
                            {
                                nameInfo = (OBJECT_NAME_INFORMATION)Marshal.PtrToStructure(pNameInfo, typeof(OBJECT_NAME_INFORMATION));
                            }
                            else
                            {
                                // オブジェクトの名称を取得できなかった。
                                nameInfo = default(OBJECT_NAME_INFORMATION);
                            }
                        }

                        // 結果の表示
                        // basicInfo.HandleCount は、複製しているため必ず 1 つ参照が増えている。これを減らして表示する。
                        Console.WriteLine("\t{0}\t\"{1}\"\t\"{2}\"", basicInfo.HandleCount - 1, typeInfoName, GetRegularFileNameFromDevice(nameInfo.Name.ToString()));

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

                Console.WriteLine("\r\nDone.");
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
