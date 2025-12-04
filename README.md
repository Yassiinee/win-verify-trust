# WinVerifyTrust

Minimal C# console utility that wraps the native `WinVerifyTrust` API so you can check whether any Windows PE file is signed and if that signature chains to a trusted root. It prints a short verdict plus the detailed trust status/error returned by the OS, which makes it handy for quick triage of unsigned or revoked binaries.

## Requirements

- Windows 10 or later
- [.NET SDK 10.0](https://dotnet.microsoft.com/) (Preview at the time of writing)

## Build & Run

```powershell
git clone https://github.com/<your-account>/WinVerifyTrust.git
cd WinVerifyTrust
dotnet build
dotnet run -- "C:\Windows\System32\notepad.exe"
```

The executable is emitted to `bin/Debug/net10.0/WinVerifyTrust.exe`, so you can also call it directly once built.

## Usage

```
WinVerifyTrust <path-to-file>
```

Examples:

```powershell
WinVerifyTrust "C:\Windows\System32\notepad.exe"
WinVerifyTrust "D:\Downloads\driver.sys"
```

## Sample Output

```
=== Win Verify Trust - File Signature Checker ===

Checking file: C:\Windows\System32\notepad.exe

Verification Result: SUCCESS
Trust Status: Trusted - Signature is valid and trusted

Is Trusted: True
```

If the API reports an error (expired cert, untrusted root, missing signature, etc.) the tool surfaces both the failure code (`FAILED (0xXXXXXX)`) and a friendly description pulled from the Win32 error table, which can help you decide whether the binary needs to be rejected, re-signed, or examined further.

