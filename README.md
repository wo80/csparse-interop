# CSparse.Interop

This project contains bindings for some popular solvers of sparse linear systems. It is supposed to be an extension to [CSparse.NET](https://github.com/wo80/CSparse.NET).

## Content

| Name      | Type |          | Version | Test (x64) |
|----------:|-----:|---------:|:-------:|:----------:|
| AMD     | Ordering      | [SuiteSparse](https://github.com/DrTimothyAldenDavis/SuiteSparse) | 5.10.1 | OK |
| CHOLMOD | Direct solver | [SuiteSparse](https://github.com/DrTimothyAldenDavis/SuiteSparse) | 5.10.1 | OK |
| CXSparse | Direct solver | [SuiteSparse](https://github.com/DrTimothyAldenDavis/SuiteSparse) | 5.10.1 | OK |
| UMFPACK | Direct solver | [SuiteSparse](https://github.com/DrTimothyAldenDavis/SuiteSparse) | 5.10.1 | OK |
| SPQR    | Direct solver | [SuiteSparse](https://github.com/DrTimothyAldenDavis/SuiteSparse) | 5.10.1 | OK |
| Metis   | Graph partitioning | [SuiteSparse](https://github.com/DrTimothyAldenDavis/SuiteSparse) | 5.10.1 | OK |
| SuperLU | Direct solver | [SuperLU](https://github.com/xiaoyeli/superlu) | 5.2.1 | OK |
| PARDISO | Direct solver | [oneAPI MKL](https://www.intel.com/content/www/us/en/develop/documentation/onemkl-developer-reference-c/top/sparse-solver-routines/onemkl-pardiso-parallel-direct-sparse-solver-iface.html) | 2022.0 | OK |
| FEAST   | Eigenvalues   | [oneAPI MKL](https://www.intel.com/content/www/us/en/develop/documentation/onemkl-developer-reference-fortran/top/extended-eigensolver-routines/ext-eigensolve-ifaces-for-eigenval-within-interval/extended-eigensolver-predefined-interfaces.html) | 2022.0 | OK |
| Extended Eigensolver | Eigenvalues   | [oneAPI MKL](https://www.intel.com/content/www/us/en/develop/documentation/onemkl-developer-reference-fortran/top/extended-eigensolver-routines/extended-eigensolver-interfaces/ext-eigensolve-ifaces-find-large-small-eigenvalues.html) | 2022.0 | OK |
| ARPACK  | Eigenvalues   | [arpack-ng](https://github.com/opencollab/arpack-ng) | 3.7.0 | OK |
| Spectra  | Eigenvalues   | [Spectra](https://github.com/yixuan/spectra) | 1.0.0 | OK |

View [test results](https://github.com/wo80/csparse-interop/wiki/Test-Results) in the wiki.

## Related projects

* [vs-suitesparse](https://github.com/wo80/vs-suitesparse/) - Visual Studio solution to build SuiteSparse.
* [vs-superlu](https://github.com/wo80/vs-superlu/) - Visual Studio solution to build SuperLU.
* [vs-arpack](https://github.com/wo80/vs-arpack/) - Visual Studio solution to build ARPACK.
* [vs-spectra](https://github.com/wo80/vs-spectra/) - Visual Studio solution to build Spectra.

Pre-compiled binaries for windows users can be found [here](http://wo80.bplaced.net/packages/#tag:math).

## Dependencies

MKL solvers depend on the corresponding runtime to be present. Read more about those [dependencies](https://github.com/wo80/csparse-interop/wiki/Dependencies) in the wiki.
