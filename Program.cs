namespace WinVerifyTrust
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("=== Win Verify Trust - File Signature Checker ===\n");

            if (args.Length == 0)
            {
                Console.WriteLine("Usage: WinVerifyTrust <filepath>");
                Console.WriteLine("\nExample:");
                Console.WriteLine("  WinVerifyTrust C:\\Windows\\System32\\notepad.exe");
                return;
            }

            string filePath = args[0];

            if (!File.Exists(filePath))
            {
                Console.WriteLine($"Error: File not found - {filePath}");
                return;
            }

            Console.WriteLine($"Checking file: {filePath}\n");

            SignatureVerifier verifier = new();
            var result = verifier.VerifyFile(filePath);

            Console.WriteLine($"Verification Result: {result.Status}");
            Console.WriteLine($"Trust Status: {result.TrustStatus}");

            if (!string.IsNullOrEmpty(result.ErrorMessage))
            {
                Console.WriteLine($"Details: {result.ErrorMessage}");
            }

            Console.WriteLine($"\nIs Trusted: {result.IsTrusted}");
        }
    }
}