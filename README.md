# CSparse.Interop - C# bindings for sparse matrix solvers

This project contains bindings for some popular solvers of sparse linear systems ([SuiteSparse](http://faculty.cse.tamu.edu/davis/suitesparse.html) (CHOLMOD, UMFPACK etc.), [SuperLU](http://crd-legacy.lbl.gov/~xiaoye/SuperLU/)). It is supposed to be an extension to [CSparse.NET](https://github.com/wo80/CSparse.NET), a C# port of CSparse (part of SuiteSparse).

## Todo

1. Testing (both x86 and x64)
   * SuiteSparse has to be tested in x64 mode (sparse matrix storage might expect 8 byte integers, which is not compatible with CSparse.NET). See Cholmod/NativeMethods.cs for usage of conditional compilation symbol **X64**.
2. Documentation
