using System.Security.Cryptography.X509Certificates;

namespace WinVerifyTrust
{
    public class VerificationResult
    {
        public string Status { get; set; }
        public string TrustStatus { get; set; }
        public bool IsTrusted { get; set; }
        public string ErrorMessage { get; set; }
        public X509Certificate2 Certificate { get; set; }
    }
}
