# CSparse.Interop

This project contains bindings for some popular solvers of sparse linear systems. It is supposed to be an extension to [CSparse.NET](https://github.com/wo80/CSparse.NET).

## Content

| Name      | Type |          | Version | Test (x86)  | Test (x64) |
|----------:|-----:|---------:|:-------:|:-----------:|:----------:|
| AMD     | Ordering      | [SuiteSparse](https://github.com/DrTimothyAldenDavis/SuiteSparse) | 5.7.2 | - | OK |
| CHOLMOD | Direct solver | [SuiteSparse](https://github.com/DrTimothyAldenDavis/SuiteSparse) | 5.7.2 | - | OK |
| CXSparse | Direct solver | [SuiteSparse](https://github.com/DrTimothyAldenDavis/SuiteSparse) | 5.7.2 | - | OK |
| UMFPACK | Direct solver | [SuiteSparse](https://github.com/DrTimothyAldenDavis/SuiteSparse) | 5.7.2 | - | OK |
| SPQR    | Direct solver | [SuiteSparse](https://github.com/DrTimothyAldenDavis/SuiteSparse) | 5.7.2 | - | OK |
| Metis   | Graph partitioning | [SuiteSparse](https://github.com/DrTimothyAldenDavis/SuiteSparse) | 5.7.2 | - | OK |
| SuperLU | Direct solver | [SuperLU](https://github.com/xiaoyeli/superlu) | 5.2.1 | - | OK |
| PARDISO | Direct solver | [MKL](https://software.intel.com/en-us/mkl-developer-reference-c-intel-mkl-pardiso-parallel-direct-sparse-solver-interface) | 2020.4 | - | OK |
| FEAST   | Eigenvalues   | [MKL](https://software.intel.com/en-us/mkl-developer-reference-c-the-feast-algorithm) | 2020.4 | - | OK |
| Extended Eigensolver | Eigenvalues   | [MKL](https://software.intel.com/content/www/us/en/develop/documentation/mkl-developer-reference-c/top/extended-eigensolver-routines) | 2020.4 | - | OK |
| ARPACK  | Eigenvalues   | [arpack-ng](https://github.com/opencollab/arpack-ng) | 3.7.0 | - | OK |
| Spectra  | Eigenvalues   | [Spectra](https://github.com/yixuan/spectra) | 1.y.z | - | OK |
| CUDA QR       | Direct solver | [CUSOLVER](https://developer.nvidia.com/cusolver) | 9.2 | - | Buggy |
| CUDA Cholesky | Direct solver | [CUSOLVER](https://developer.nvidia.com/cusolver) | 9.2 | - | Buggy |

View [test results](https://github.com/wo80/csparse-interop/wiki/Test-Results) in the wiki.

## Related projects

* [vs-suitesparse](https://github.com/wo80/vs-suitesparse/) - Visual Studio solution to build SuiteSparse.
* [vs-superlu](https://github.com/wo80/vs-superlu/) - Visual Studio solution to build SuperLU.
* [vs-arpack](https://github.com/wo80/vs-arpack/) - Visual Studio solution to build ARPACK.
* [vs-spectra](https://github.com/wo80/vs-spectra/) - Visual Studio solution to build Spectra.

Pre-compiled binaries for windows users can be found [here](http://wo80.bplaced.net/math/packages.html).

## Dependencies

MKL and CUDA solvers depend on the corresponding runtime to be present. Read more about those [dependencies](https://github.com/wo80/csparse-interop/wiki/Dependencies) in the wiki.
