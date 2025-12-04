## WinVerifyTrust

<p align="center">
  <img src="icon.ico" alt="icon" width="80"/>
</p>

Modern C# **Windows Forms** application that wraps the native `WinVerifyTrust` API so you can check whether any Windows PE file is signed and if that signature chains to a trusted root. It shows a rich, icon-based UI with color-coded results plus detailed trust status/error returned by the OS, which makes it handy for quick triage of unsigned or revoked binaries.

## UI Screenshots

<p align="center">
  <img src="screenshots/screenshot.png" alt="WinVerifyTrust - Trusted signature" width="750"/>
</p>

<p align="center">
  <img src="screenshots/trusted.PNG" alt="WinVerifyTrust - Trusted signature" width="750"/>
</p>

<p align="center">
  <img src="screenshots/not-trusted.PNG" alt="WinVerifyTrust - Not trusted / invalid signature" width="750"/>
</p>

## Requirements

- Windows 10 or later
- [.NET SDK 10.0](https://dotnet.microsoft.com/) (Windows desktop, preview at the time of writing)

## Build & Run (GUI)

```powershell
git clone https://github.com/Yassiinee/win-verify-trust
cd WinVerifyTrust
dotnet build
dotnet run
```

The executable is emitted to `bin/Debug/net10.0-windows/WinVerifyTrust.exe` and includes a custom application icon (`icon.ico`), so you can also launch it directly from Explorer.

## Open in Visual Studio

- Open `WinVerifyTrust.sln` in Visual Studio 2022 or later.
- Set the configuration to **Debug** or **Release** and build/run with `F5` or `Ctrl+F5`.
- To pass an argument (file path) when debugging, set it under **Project Properties → Debug → Application arguments**.

## Usage (Windows Forms UI)

- Start `WinVerifyTrust.exe` (or run the project from Visual Studio / `dotnet run`).
- Click **Browse** to select the executable, DLL, SYS, CAB, MSI, etc. you want to verify.
- Click **Verify** to run `WinVerifyTrust` and display the result.
- The main panel shows a green check or red cross icon with the overall status, while the lower section shows detailed certificate information (subject, issuer, validity period, serial number, thumbprint).

## Sample Output

```
=== Win Verify Trust - File Signature Checker ===

Checking file: C:\Windows\System32\notepad.exe

Verification Result: SUCCESS
Trust Status: Trusted - Signature is valid and trusted

Is Trusted: True
```

If the API reports an error (expired cert, untrusted root, missing signature, etc.) the tool surfaces both the failure code (`FAILED (0xXXXXXX)`) and a friendly description pulled from the Win32 error table, which can help you decide whether the binary needs to be rejected, re-signed, or examined further.

