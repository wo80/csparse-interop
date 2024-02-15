# CSparse.Interop

This project contains bindings for some popular solvers of sparse linear systems. It is supposed to be an extension to [CSparse.NET](https://github.com/wo80/CSparse.NET).

## Content

| Name      | Type |          | Version | Test (x64) |
|----------:|-----:|---------:|:-------:|:----------:|
| AMD     | Ordering      | [SuiteSparse](https://github.com/DrTimothyAldenDavis/SuiteSparse) | 7.6.0 | OK |
| CHOLMOD | Direct solver | [SuiteSparse](https://github.com/DrTimothyAldenDavis/SuiteSparse) | 7.6.0 | OK |
| CXSparse | Direct solver | [SuiteSparse](https://github.com/DrTimothyAldenDavis/SuiteSparse) | 7.6.0 | OK |
| UMFPACK | Direct solver | [SuiteSparse](https://github.com/DrTimothyAldenDavis/SuiteSparse) | 7.6.0 | OK |
| SPQR    | Direct solver | [SuiteSparse](https://github.com/DrTimothyAldenDavis/SuiteSparse) | 7.6.0 | OK |
| METIS   | Graph partitioning | [METIS](https://github.com/KarypisLab/METIS) | 5.2.1 | OK |
| SuperLU | Direct solver | [SuperLU](https://github.com/xiaoyeli/superlu) | 6.0.1 | OK |
| PARDISO | Direct solver | [oneAPI MKL](https://www.intel.com/content/www/us/en/docs/onemkl/developer-reference-c/2024-0/onemkl-pardiso-parallel-direct-sparse-solver-iface.html) | 2024.0 | OK |
| FEAST   | Eigenvalues   | [oneAPI MKL](https://www.intel.com/content/www/us/en/docs/onemkl/developer-reference-c/2024-0/extended-eigensolver-predefined-interfaces.html) | 2024.0 | OK |
| Extended Eigensolver | Eigenvalues   | [oneAPI MKL](https://www.intel.com/content/www/us/en/docs/onemkl/developer-reference-c/2024-0/ext-eigensolve-ifaces-find-large-small-eigenvalues.html) | 2024.0 | OK |
| ARPACK  | Eigenvalues   | [arpack-ng](https://github.com/opencollab/arpack-ng) | 3.9.1 | OK |
| Spectra  | Eigenvalues   | [Spectra](https://github.com/yixuan/spectra) | 1.0.1 | OK |

View [test results](https://github.com/wo80/csparse-interop/wiki/Test-Results) in the wiki.

## Related projects

* [vs-suitesparse](https://github.com/wo80/vs-suitesparse/) - Visual Studio solution to build SuiteSparse.
* [vs-arpack](https://github.com/wo80/vs-arpack/) - Visual Studio solution to build ARPACK.
* [vs-spectra](https://github.com/wo80/vs-spectra/) - Visual Studio solution to build Spectra.

Pre-compiled binaries for windows users can be found [here](http://wo80.bplaced.net/packages/#tag:math).

## Dependencies

MKL solvers depend on the corresponding runtime to be present. Read more about those [dependencies](https://github.com/wo80/csparse-interop/wiki/Dependencies) in the wiki.
