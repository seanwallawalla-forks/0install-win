Zero Install
============
Zero Install is a decentralized cross-platform software-installation system available under the LGPL. Visit http://0install.de/ for more information.


Developer information
---------------------
See http://0install.de/dev/ for information development tools, build servers, API docs, etc..


Source directory structure
--------------------------
- The directory `src` contains the Visual Studio solution with the actual source code.
- The directory `lib` contains pre-compiled 3rd party libraries which are not available via NuGet.
- The directory `doc` contains scripts for generating source code and developer documentation.
- The directory `nuget` contains NuGet spec files used to generate NuGet packages for Zero Install.
- The directory `installer` contains scripts for creating a Windows installer for Zero Install.
- The directory `bundled` contains an external solver (all platforms).
- The directory `build` contains the results of various compilation processes. It is created on first usage. It can contain the following subdirectories:
  - Debug: Contains Debug builds produced from the source code.
  - Release: Contains Release builds produced from the source code.
    - Backend: Contains the libraries forming the Zero Install Backend.
    - Frontend: Contains the executables for the Zero Install Frontend plus all required libraries (including the Backend).
    - Tools: Contains the executables for Zero Install Tools such as the Feed Editor plus all required libraries (including the Backend).
    - Updater: Contains the executables for the Zero Install self-updater.
    - Samples: Contains the executables for the Zero Install API samples.
  - Packages: Contains the generated NuGet packages.
  - Installer: Contains the generated installers.
  - Documentation: Contains the generated source code documentation.
- The top-level directory contains local Zero Install feeds referencing the contents of the `build` directory. They can be registered with `0install add-feed` in order to replace the online versions of Zero Install and its tools with your local builds.

`VERSION` and `VERSION_UPDATER` contain the version numbers used by build scripts.
Use `Set-Version.ps1 X.Y.Z` and `Set-Version.ps1 updater X.Y.Z` to change the version numbers. This ensures that the version also gets set in other locations (e.g. AssemblyInfo).


Building on Windows
-------------------
The external solver (required) is not included in the repository. To get it run `bundled/download-solver.ps1`.

`build.cmd` will call build scripts in subdirectories to create a Zero Install for Windows installer in `build/Frontend/Installer`.
Note: Please read `installer/readme.txt` as well for information about required tools.

If you wish to add an AuthentiCode signature to the compiled binaries set the `signing_cert_path` environment variable to the certificate's file path and `signing_cert_pass` to the password used to decrypt the file before executing the build scripts.
For example:
```
set signing_cert_path=C:\mycert.pfx
set signing_cert_pass=mypass
build.cmd
```

`cleanup.cmd` will delete any temporary files created by the build process or Visual Studio.


Building on Linux
-----------------
The external solver (required) is not included in the repository. To get it run `bundled/download-solver.sh`.

`build.sh` will perform a partial debug compilation using Mono's xbuild. A installer package will not be built.

`cleanup.sh` will delete any temporary files created by the xbuild build process.

`test.sh` will run the unit tests using the NUnit console runner.
Note: You must perform a Debug build first (using `src/build.sh`) before you can run the unit tests.


Environment variables
---------------------
- `ZEROINSTALL_PORTABLE_BASE`: Set by the C# code to to inform the Python code of Portable mode.
- `ZEROINSTALL_EXTERNAL_FETCHER`: Set by the C# code to make the Python code delegate downloading files back to the C# implementation.
- `ZEROINSTALL_EXTERNAL_STORE`: Set by the C# code to make the Python code delegate extracting archives back to the C# implementation.
