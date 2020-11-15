[![.NET Core](https://github.com/imazen/imageflow-dotnet/workflows/.NET%20Core/badge.svg)](https://github.com/imazen/imageflow-dotnet/actions?query=workflow%3A%22.NET+Core%22)
[![Build status](https://ci.appveyor.com/api/projects/status/vqfofqe3bwqwdu4a?svg=true)](https://ci.appveyor.com/project/imazen/imageflow-dotnet)

Imageflow.NET is a .NET API for Imageflow, the fast image optimization and processing library for web servers. Imageflow focuses on security, quality, and performance - in that order.

```
PM> Install-Package Imageflow.Net
PM> Install-Package Imageflow.NativeRuntime.win-x86 -pre
PM> Install-Package Imageflow.NativeRuntime.win-x86_64 -pre
PM> Install-Package Imageflow.NativeRuntime.osx_10_11-x86_64 -pre
PM> Install-Package Imageflow.NativeRuntime.ubuntu_16_04-x86_64 -pre
```

Note: You must install the [appropriate NativeRuntime(s)](https://www.nuget.org/packages?q=Imageflow+AND+NativeRuntime) in the project you are deploying - they have to copy imageflow.dll to the output folder.

Also note: Older versions of Windows may not have the C Runtime
installed ([Install 32-bit](https://aka.ms/vs/16/release/vc_redist.x86.exe) or [64-bit](https://aka.ms/vs/16/release/vc_redist.x64.exe)).

[NativeRuntimes](https://www.nuget.org/packages?q=Imageflow+AND+NativeRuntime) that are suffixed with -haswell (2013, AVX2 support) require a CPU of that generation or later.

### License

- Imageflow is dual licensed under a commercial license and the AGPLv3.
- Imageflow.NET is tri-licensed under a commercial license, the AGPLv3, and the Apache 2 license.
- Imageflow.NET Server is dual licensed under a commercial license and the AGPLv3.
- We offer commercial licenses at https://imageresizing.net/pricing
- Imageflow.NET's Apache 2 license allows for integration with non-copyleft products, as long as jobs are not actually executed (since the AGPLv3/commercial license is needed when libimageflow is linked at runtime). This can allow end-users to benefit from optional imageflow integration in products.
