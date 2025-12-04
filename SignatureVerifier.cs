using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;

namespace WinVerifyTrust
{
    public class SignatureVerifier
    {
        #region WinVerifyTrust API

        [DllImport("wintrust.dll", ExactSpelling = true, SetLastError = false, CharSet = CharSet.Unicode)]
        private static extern uint WinVerifyTrust(
            IntPtr hwnd,
            [MarshalAs(UnmanagedType.LPStruct)] Guid pgActionID,
            IntPtr pWVTData
        );

        private static readonly Guid WINTRUST_ACTION_GENERIC_VERIFY_V2 =
            new("{00AAC56B-CD44-11d0-8CC2-00C04FC295EE}");

        private const uint WTD_UI_NONE = 2;
        private const uint WTD_REVOKE_NONE = 0;
        private const uint WTD_CHOICE_FILE = 1;
        private const uint WTD_STATEACTION_VERIFY = 1;
        private const uint WTD_STATEACTION_CLOSE = 2;

        private const uint TRUST_E_NOSIGNATURE = 0x800B0100;
        private const uint TRUST_E_EXPLICIT_DISTRUST = 0x800B0111;
        private const uint TRUST_E_SUBJECT_NOT_TRUSTED = 0x800B0004;
        private const uint CERT_E_EXPIRED = 0x800B0101;
        private const uint CERT_E_REVOKED = 0x800B010C;
        private const uint CERT_E_UNTRUSTEDROOT = 0x800B0109;

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private struct WINTRUST_FILE_INFO
        {
            public uint cbStruct;
            public IntPtr pcwszFilePath;
            public IntPtr hFile;
            public IntPtr pgKnownSubject;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private struct WINTRUST_DATA
        {
            public uint cbStruct;
            public IntPtr pPolicyCallbackData;
            public IntPtr pSIPClientData;
            public uint dwUIChoice;
            public uint fdwRevocationChecks;
            public uint dwUnionChoice;
            public IntPtr pFile;
            public uint dwStateAction;
            public IntPtr hWVTStateData;
            public IntPtr pwszURLReference;
            public uint dwProvFlags;
            public uint dwUIContext;
        }

        #endregion

        public VerificationResult VerifyFile(string filePath)
        {
            VerificationResult result = new();

            IntPtr fileInfoPtr = IntPtr.Zero;
            IntPtr trustDataPtr = IntPtr.Zero;
            IntPtr filePathPtr = IntPtr.Zero;

            try
            {
                result.Certificate = GetCertificateFromFile(filePath);

                filePathPtr = Marshal.StringToCoTaskMemUni(filePath);

                WINTRUST_FILE_INFO fileInfo = new()
                {
                    cbStruct = (uint)Marshal.SizeOf(typeof(WINTRUST_FILE_INFO)),
                    pcwszFilePath = filePathPtr,
                    hFile = IntPtr.Zero,
                    pgKnownSubject = IntPtr.Zero
                };

                fileInfoPtr = Marshal.AllocHGlobal(Marshal.SizeOf(fileInfo));
                Marshal.StructureToPtr(fileInfo, fileInfoPtr, false);

                WINTRUST_DATA trustData = new()
                {
                    cbStruct = (uint)Marshal.SizeOf(typeof(WINTRUST_DATA)),
                    pPolicyCallbackData = IntPtr.Zero,
                    pSIPClientData = IntPtr.Zero,
                    dwUIChoice = WTD_UI_NONE,
                    fdwRevocationChecks = WTD_REVOKE_NONE,
                    dwUnionChoice = WTD_CHOICE_FILE,
                    pFile = fileInfoPtr,
                    dwStateAction = WTD_STATEACTION_VERIFY,
                    hWVTStateData = IntPtr.Zero,
                    pwszURLReference = IntPtr.Zero,
                    dwProvFlags = 0,
                    dwUIContext = 0
                };

                trustDataPtr = Marshal.AllocHGlobal(Marshal.SizeOf(trustData));
                Marshal.StructureToPtr(trustData, trustDataPtr, false);

                uint trustResult = WinVerifyTrust(
                    IntPtr.Zero,
                    WINTRUST_ACTION_GENERIC_VERIFY_V2,
                    trustDataPtr
                );

                result.IsTrusted = trustResult == 0;
                result.Status = GetStatusDescription(trustResult);
                result.TrustStatus = GetTrustStatusDescription(trustResult);

                if (trustResult != 0)
                {
                    result.ErrorMessage = $"Error Code: 0x{trustResult:X8}";
                }

                trustData.dwStateAction = WTD_STATEACTION_CLOSE;
                Marshal.StructureToPtr(trustData, trustDataPtr, true);
                WinVerifyTrust(IntPtr.Zero, WINTRUST_ACTION_GENERIC_VERIFY_V2, trustDataPtr);
            }
            catch (Exception ex)
            {
                result.Status = "Error";
                result.TrustStatus = "Verification Failed";
                result.ErrorMessage = ex.Message;
                result.IsTrusted = false;
            }
            finally
            {
                if (filePathPtr != IntPtr.Zero) Marshal.FreeCoTaskMem(filePathPtr);
                if (fileInfoPtr != IntPtr.Zero) Marshal.FreeHGlobal(fileInfoPtr);
                if (trustDataPtr != IntPtr.Zero) Marshal.FreeHGlobal(trustDataPtr);
            }

            return result;
        }

        private X509Certificate2 GetCertificateFromFile(string filePath)
        {
            try
            {
                // Sometimes this works directly with signed files
                return new X509Certificate2(filePath);
            }
            catch
            {
                return null;
            }
        }

        private string GetStatusDescription(uint result)
        {
            return result == 0 ? "TRUSTED" : "NOT TRUSTED";
        }

        private string GetTrustStatusDescription(uint result)
        {
            switch (result)
            {
                case 0:
                    return "The digital signature is valid and trusted by Windows";
                case TRUST_E_NOSIGNATURE:
                    return "This file does not have a digital signature";
                case TRUST_E_EXPLICIT_DISTRUST:
                    return "The signature is present but has been explicitly marked as untrusted";
                case TRUST_E_SUBJECT_NOT_TRUSTED:
                    return "The signature is present but the certificate is not trusted";
                case CERT_E_EXPIRED:
                    return "The signing certificate has expired";
                case CERT_E_REVOKED:
                    return "The certificate has been revoked by the issuing authority";
                case CERT_E_UNTRUSTEDROOT:
                    return "The certificate chain root is not trusted";
                default:
                    return $"Unknown verification status (Code: 0x{result:X8})";
            }
        }
    }
}
