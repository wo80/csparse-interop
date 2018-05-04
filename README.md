# CSparse.Interop - C# bindings for sparse matrix solvers

This project contains bindings for some popular solvers of sparse linear systems. It is supposed to be an extension to [CSparse.NET](https://github.com/wo80/CSparse.NET).

## Content

| Name      | Type |          | Version | Test (x86)  | Test (x64) |
|----------:|-----:|---------:|:-------:|:-----------:|:----------:|
| CHOLMOD | Direct solver | [SuiteSparse](http://faculty.cse.tamu.edu/davis/suitesparse.html) | 5.2.0 | OK | OK |
| UMFPACK | Direct solver | [SuiteSparse](http://faculty.cse.tamu.edu/davis/suitesparse.html) | 5.2.0 | OK | OK |
| SPQR    | Direct solver | [SuiteSparse](http://faculty.cse.tamu.edu/davis/suitesparse.html) | 5.2.0 | OK | -  |
| SuperLU | Direct solver | [SuperLU](http://crd-legacy.lbl.gov/~xiaoye/SuperLU/) | 5.2.1 | OK | OK |
| PARDISO | Direct solver | [MKL](https://software.intel.com/en-us/mkl-developer-reference-c-intel-mkl-pardiso-parallel-direct-sparse-solver-interface) | 2018.1 | OK | OK |
| FEAST   | Eigenvalues   | [MKL](https://software.intel.com/en-us/mkl-developer-reference-c-the-feast-algorithm) | 2018.1 | OK | OK |

## Todo

1. Testing (both x86 and x64)
   * SuiteSparse has to be tested in x64 mode (sparse matrix storage might expect 8 byte integers, which is not compatible with CSparse.NET). See Cholmod/NativeMethods.cs for usage of conditional compilation symbol **X64**.
2. Documentation
