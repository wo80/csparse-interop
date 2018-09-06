
namespace CSparse.Interop.CUDA
{
    using System;

    #region Core

    /// <summary>
    /// Compute mode that device is currently in.
    /// </summary>
    internal enum ComputeMode
    {
        /// <summary>
        /// Default compute mode (Multiple threads can use cudaSetDevice() with this device)
        /// </summary>
        Default = 0,

        /// <summary>
        /// Compute-exclusive-thread mode (Only one thread in one process will be able to use cudaSetDevice() with this device)
        /// </summary>
        Exclusive = 1,

        /// <summary>
        /// Compute-prohibited mode (No threads can use cudaSetDevice() with this device)
        /// </summary>
        Prohibited = 2,

        /// <summary>
        /// Compute-exclusive-process mode (Many threads in one process will be able to use cudaSetDevice() with this device)
        /// </summary>
        ExclusiveProcess = 3
    }

    /// <summary>
    /// Device properties
    /// </summary>
    internal enum DeviceAttribute
    {
        /// <summary>
        /// Maximum number of threads per block
        /// </summary>
        MaxThreadsPerBlock = 1,

        /// <summary>
        /// Maximum block dimension X
        /// </summary>
        MaxBlockDimX = 2,

        /// <summary>
        /// Maximum block dimension Y
        /// </summary>
        MaxBlockDimY = 3,

        /// <summary>
        /// Maximum block dimension Z
        /// </summary>
        MaxBlockDimZ = 4,

        /// <summary>
        /// Maximum grid dimension X
        /// </summary>
        MaxGridDimX = 5,

        /// <summary>
        /// Maximum grid dimension Y
        /// </summary>
        MaxGridDimY = 6,

        /// <summary>
        /// Maximum grid dimension Z
        /// </summary>
        MaxGridDimZ = 7,

        /// <summary>
        /// Maximum amount of shared memory
        /// available to a thread block in bytes; this amount is shared by all thread blocks simultaneously resident on a
        /// multiprocessor
        /// </summary>
        MaxSharedMemoryPerBlock = 8,

        /// <summary>
        /// Memory available on device for __constant__ variables in a CUDA C kernel in bytes
        /// </summary>
        TotalConstantMemory = 9,

        /// <summary>
        /// Warp size in threads
        /// </summary>
        WarpSize = 10,

        /// <summary>
        /// Maximum pitch in bytes allowed by the memory copy functions
        /// that involve memory regions allocated through <see cref="DriverAPINativeMethods.MemoryManagement.cuMemAllocPitch_v2"/>
        /// </summary>
        MaxPitch = 11,

        /// <summary>
        /// Maximum number of 32-bit registers available
        /// to a thread block; this number is shared by all thread blocks simultaneously resident on a multiprocessor
        /// </summary>
        MaxRegistersPerBlock = 12,

        /// <summary>
        /// Typical clock frequency in kilohertz
        /// </summary>
        ClockRate = 13,

        /// <summary>
        /// Alignment requirement; texture base addresses
        /// aligned to textureAlign bytes do not need an offset applied to texture fetches
        /// </summary>
        TextureAlignment = 14,

        /// <summary>
        /// 1 if the device can concurrently copy memory between host
        /// and device while executing a kernel, or 0 if not
        /// </summary>
        GPUOverlap = 15,

        /// <summary>
        /// Number of multiprocessors on device
        /// </summary>
        MultiProcessorCount = 0x10,

        /// <summary>
        /// Specifies whether there is a run time limit on kernels. <para/>
        /// 1 if there is a run time limit for kernels executed on the device, or 0 if not
        /// </summary>
        KernelExecTimeout = 0x11,

        /// <summary>
        /// Device is integrated with host memory. 1 if the device is integrated with the memory subsystem, or 0 if not
        /// </summary>
        Integrated = 0x12,

        /// <summary>
        /// Device can map host memory into CUDA address space. 1 if the device can map host memory into the
        /// CUDA address space, or 0 if not
        /// </summary>
        CanMapHostMemory = 0x13,

        /// <summary>
        /// Compute mode (See <see cref="CUComputeMode"/> for details)
        /// </summary>
        ComputeMode = 20,


        /// <summary>
        /// Maximum 1D texture width
        /// </summary>
        MaximumTexture1DWidth = 21,

        /// <summary>
        /// Maximum 2D texture width
        /// </summary>
        MaximumTexture2DWidth = 22,

        /// <summary>
        /// Maximum 2D texture height
        /// </summary>
        MaximumTexture2DHeight = 23,

        /// <summary>
        /// Maximum 3D texture width
        /// </summary>
        MaximumTexture3DWidth = 24,

        /// <summary>
        /// Maximum 3D texture height
        /// </summary>
        MaximumTexture3DHeight = 25,

        /// <summary>
        /// Maximum 3D texture depth
        /// </summary>
        MaximumTexture3DDepth = 26,

        /// <summary>
        /// Maximum texture array width
        /// </summary>
        MaximumTexture2DArray_Width = 27,

        /// <summary>
        /// Maximum texture array height
        /// </summary>
        MaximumTexture2DArray_Height = 28,

        /// <summary>
        /// Maximum slices in a texture array
        /// </summary>
        MaximumTexture2DArray_NumSlices = 29,

        /// <summary>
        /// Alignment requirement for surfaces
        /// </summary>
        SurfaceAllignment = 30,

        /// <summary>
        /// Device can possibly execute multiple kernels concurrently. <para/>
        /// 1 if the device supports executing multiple kernels
        /// within the same context simultaneously, or 0 if not. It is not guaranteed that multiple kernels will be resident on
        /// the device concurrently so this feature should not be relied upon for correctness.
        /// </summary>
        ConcurrentKernels = 31,

        /// <summary>
        /// Device has ECC support enabled. 1 if error correction is enabled on the device, 0 if error correction
        /// is disabled or not supported by the device.
        /// </summary>
        ECCEnabled = 32,

        /// <summary>
        /// PCI bus ID of the device
        /// </summary>
        PCIBusID = 33,

        /// <summary>
        /// PCI device ID of the device
        /// </summary>
        PCIDeviceID = 34,

        /// <summary>
        /// Device is using TCC driver model
        /// </summary>
        TCCDriver = 35,

        /// <summary>
        /// Peak memory clock frequency in kilohertz
        /// </summary>
        MemoryClockRate = 36,

        /// <summary>
        /// Global memory bus width in bits
        /// </summary>
        GlobalMemoryBusWidth = 37,

        /// <summary>
        /// Size of L2 cache in bytes
        /// </summary>
        L2CacheSize = 38,

        /// <summary>
        /// Maximum resident threads per multiprocessor
        /// </summary>
        MaxThreadsPerMultiProcessor = 39,

        /// <summary>
        /// Number of asynchronous engines
        /// </summary>
        AsyncEngineCount = 40,

        /// <summary>
        /// Device shares a unified address space with the host
        /// </summary>
        UnifiedAddressing = 41,

        /// <summary>
        /// Maximum 1D layered texture width
        /// </summary>
        MaximumTexture1DLayeredWidth = 42,

        /// <summary>
        /// Maximum layers in a 1D layered texture
        /// </summary>
        MaximumTexture1DLayeredLayers = 43,

        /// <summary>
        /// PCI domain ID of the device
        /// </summary>
        PCIDomainID = 50,

        /// <summary>
        /// Pitch alignment requirement for textures
        /// </summary>
        TexturePitchAlignment = 51,
        /// <summary>
        /// Maximum cubemap texture width/height
        /// </summary>
        MaximumTextureCubeMapWidth = 52,
        /// <summary>
        /// Maximum cubemap layered texture width/height
        /// </summary>
        MaximumTextureCubeMapLayeredWidth = 53,
        /// <summary>
        /// Maximum layers in a cubemap layered texture
        /// </summary>
        MaximumTextureCubeMapLayeredLayers = 54,
        /// <summary>
        /// Maximum 1D surface width
        /// </summary>
        MaximumSurface1DWidth = 55,
        /// <summary>
        /// Maximum 2D surface width
        /// </summary>
        MaximumSurface2DWidth = 56,
        /// <summary>
        /// Maximum 2D surface height
        /// </summary>
        MaximumSurface2DHeight = 57,
        /// <summary>
        /// Maximum 3D surface width
        /// </summary>
        MaximumSurface3DWidth = 58,
        /// <summary>
        /// Maximum 3D surface height
        /// </summary>
        MaximumSurface3DHeight = 59,
        /// <summary>
        /// Maximum 3D surface depth
        /// </summary>
        MaximumSurface3DDepth = 60,
        /// <summary>
        /// Maximum 1D layered surface width
        /// </summary>
        MaximumSurface1DLayeredWidth = 61,
        /// <summary>
        /// Maximum layers in a 1D layered surface
        /// </summary>
        MaximumSurface1DLayeredLayers = 62,
        /// <summary>
        /// Maximum 2D layered surface width
        /// </summary>
        MaximumSurface2DLayeredWidth = 63,
        /// <summary>
        /// Maximum 2D layered surface height
        /// </summary>
        MaximumSurface2DLayeredHeight = 64,
        /// <summary>
        /// Maximum layers in a 2D layered surface
        /// </summary>
        MaximumSurface2DLayeredLayers = 65,
        /// <summary>
        /// Maximum cubemap surface width
        /// </summary>
        MaximumSurfaceCubemapWidth = 66,
        /// <summary>
        /// Maximum cubemap layered surface width
        /// </summary>
        MaximumSurfaceCubemapLayeredWidth = 67,
        /// <summary>
        /// Maximum layers in a cubemap layered surface
        /// </summary>
        MaximumSurfaceCubemapLayeredLayers = 68,
        /// <summary>
        /// Maximum 1D linear texture width
        /// </summary>
        MaximumTexture1DLinearWidth = 69,
        /// <summary>
        /// Maximum 2D linear texture width
        /// </summary>
        MaximumTexture2DLinearWidth = 70,
        /// <summary>
        /// Maximum 2D linear texture height
        /// </summary>
        MaximumTexture2DLinearHeight = 71,
        /// <summary>
        /// Maximum 2D linear texture pitch in bytes
        /// </summary>
        MaximumTexture2DLinearPitch = 72,
        /// <summary>
        /// Maximum mipmapped 2D texture width
        /// </summary>
        MaximumTexture2DMipmappedWidth = 73,
        /// <summary>
        /// Maximum mipmapped 2D texture height
        /// </summary>
        MaximumTexture2DMipmappedHeight = 74,
        /// <summary>
        /// Major compute capability version number
        /// </summary>
        ComputeCapabilityMajor = 75,
        /// <summary>
        /// Minor compute capability version number
        /// </summary>
        ComputeCapabilityMinor = 76,
        /// <summary>
        /// Maximum mipmapped 1D texture width
        /// </summary>
        MaximumTexture1DMipmappedWidth = 77,
        /// <summary>
        /// Device supports stream priorities
        /// </summary>
        StreamPrioritiesSupported = 78,
        /// <summary>
        /// Device supports caching globals in L1
        /// </summary>
        GlobalL1CacheSupported = 79,
        /// <summary>
        /// Device supports caching locals in L1
        /// </summary>
        LocalL1CacheSupported = 80,
        /// <summary>
        /// Maximum shared memory available per multiprocessor in bytes
        /// </summary>
        MaxSharedMemoryPerMultiprocessor = 81,
        /// <summary>
        /// Maximum number of 32-bit registers available per multiprocessor
        /// </summary>
        MaxRegistersPerMultiprocessor = 82,
        /// <summary>
        /// Device can allocate managed memory on this system
        /// </summary>
        ManagedMemory = 83,
        /// <summary>
        /// Device is on a multi-GPU board
        /// </summary>
        MultiGpuBoard = 84,
        /// <summary>
        /// Unique id for a group of devices on the same multi-GPU board
        /// </summary>
        MultiGpuBoardGroupID = 85,
        /// <summary>
        /// Link between the device and the host supports native atomic operations (this is a placeholder attribute, and is not supported on any current hardware)
        /// </summary>
        HostNativeAtomicSupported = 86,
        /// <summary>
        /// Ratio of single precision performance (in floating-point operations per second) to double precision performance
        /// </summary>
        SingleToDoublePrecisionPerfRatio = 87,
        /// <summary>
        /// Device supports coherently accessing pageable memory without calling cudaHostRegister on it
        /// </summary>
        PageableMemoryAccess = 88,
        /// <summary>
        /// Device can coherently access managed memory concurrently with the CPU
        /// </summary>
        ConcurrentManagedAccess = 89,
        /// <summary>
        /// Device supports compute preemption.
        /// </summary>
        ComputePreemptionSupported = 90,
        /// <summary>
        /// Device can access host registered memory at the same virtual address as the CPU.
        /// </summary>
        CanUseHostPointerForRegisteredMem = 91,
        /// <summary>
        /// ::cuStreamBatchMemOp and related APIs are supported.
        /// </summary>
        CanUseStreamMemOps = 92,
        /// <summary>
        /// 64-bit operations are supported in ::cuStreamBatchMemOp and related APIs.
        /// </summary>
        CanUse64BitStreamMemOps = 93,
        /// <summary>
        /// ::CU_STREAM_WAIT_VALUE_NOR is supported.
        /// </summary>
        CanUseStreamWaitValueNOr = 94,
        /// <summary>
        /// Device supports launching cooperative kernels via ::cuLaunchCooperativeKernel
        /// </summary>
        CooperativeLaunch = 95,
        /// <summary>
        /// Device can participate in cooperative kernels launched via ::cuLaunchCooperativeKernelMultiDevice
        /// </summary>
        CooperativeMultiDeviceLaunch = 96,
        /// <summary>
        /// Maximum optin shared memory per block
        /// </summary>
        MaxSharedMemoryPerBlockOptin = 97,
        /// <summary>
        /// Max elems...
        /// </summary>
        MAX
    }

    /// <summary>
    /// CUDA stream flags
    /// </summary>
    [Flags]
    internal enum StreamFlags : uint
    {
        /// <summary>
        /// For compatibilty with pre Cuda 5.0, equal to Default
        /// </summary>
        None = 0,
        /// <summary>
        /// Default stream flag
        /// </summary>
        Default = 0x0,
        /// <summary>
        /// Stream does not synchronize with stream 0 (the NULL stream)
        /// </summary>
        NonBlocking = 0x1,
    }

    /// <summary>
    /// Error codes returned by CUDA driver API calls
    /// </summary>
    public enum CudaResult
    {
        Success = 0,
        ///The API call returned with no errors. In the case of query calls, this can also
        ///mean that the operation being queried is complete (see cudaEventQuery() and
        ///cudaStreamQuery()).
        MissingConfiguration = 1,
        ///The device function being invoked (usually via cudaLaunchKernel()) was not
        ///previously configured via the cudaConfigureCall() function.
        MemoryAllocation = 2,
        ///The API call failed because it was unable to allocate enough memory to perform the
        ///requested operation.
        InitializationError = 3,
        ///The API call failed because the CUDA driver and runtime could not be initialized.
        LaunchFailure = 4,
        ///An exception occurred on the device while executing a kernel. Common causes
        ///include dereferencing an invalid device pointer and accessing out of bounds shared
        ///memory. The device cannot be used until cudaThreadExit() is called. All existing
        ///device memory allocations are invalid and must be reconstructed if the program is to
        ///continue using CUDA.
        PriorLaunchFailure = 5,
        ///This indicated that a previous kernel launch failed. This was previously used for
        ///device emulation of kernel launches. Deprecated This error return is deprecated as of
        ///CUDA 3.1. Device emulation mode was removed with the CUDA 3.1 release.
        LaunchTimeout = 6,
        ///This indicates that the device kernel took too long to execute. This can only occur
        ///if timeouts are enabled - see the device property kernelExecTimeoutEnabled for
        ///more information. This leaves the process in an inconsistent state and any further
        ///CUDA work will return the same error. To continue using CUDA, the process must
        ///be terminated and relaunched.
        LaunchOutOfResources = 7,
        ///This indicates that a launch did not occur because it did not have appropriate
        ///resources. Although this error is similar to         InvalidConfiguration, this error
        ///usually indicates that the user has attempted to pass too many arguments to the
        ///device kernel, or the kernel launch specifies too many threads for the kernel's register
        ///count.
        InvalidDeviceFunction = 8,
        ///The requested device function does not exist or is not compiled for the proper device
        ///architecture.
        InvalidConfiguration = 9,
        ///This indicates that a kernel launch is requesting resources that can never be satisfied
        ///by the current device. Requesting more shared memory per block than the device
        ///supports will trigger this error, as will requesting too many threads or blocks. See
        ///cudaDeviceProp for more device limitations.
        InvalidDevice = 10,
        ///This indicates that the device ordinal supplied by the user does not correspond to a
        ///valid CUDA device.
        InvalidValue = 11,
        ///This indicates that one or more of the parameters passed to the API call is not within
        ///an acceptable range of values.
        InvalidPitchValue = 12,
        ///This indicates that one or more of the pitch-related parameters passed to the API call
        ///is not within the acceptable range for pitch.
        InvalidSymbol = 13,
        ///This indicates that the symbol name/identifier passed to the API call is not a valid
        ///name or identifier.
        MapBufferObjectFailed = 14,
        ///This indicates that the buffer object could not be mapped.
        UnmapBufferObjectFailed = 15,
        ///This indicates that the buffer object could not be unmapped.
        InvalidHostPointer = 16,
        ///This indicates that at least one host pointer passed to the API call is not a valid host
        ///pointer.
        InvalidDevicePointer = 17,
        ///This indicates that at least one device pointer passed to the API call is not a valid
        ///device pointer.
        InvalidTexture = 18,
        ///This indicates that the texture passed to the API call is not a valid texture.
        InvalidTextureBinding = 19,
        ///This indicates that the texture binding is not valid. This occurs if you call
        ///cudaGetTextureAlignmentOffset() with an unbound texture.
        InvalidChannelDescriptor = 20,
        ///This indicates that the channel descriptor passed to the API call is not valid. This
        ///occurs if the format is not one of the formats specified by cudaChannelFormatKind,
        ///or if one of the dimensions is invalid.
        InvalidMemcpyDirection = 21,
        ///This indicates that the direction of the memcpy passed to the API call is not one of the
        ///types specified by cudaMemcpyKind.
        AddressOfConstant = 22,
        ///This indicated that the user has taken the address of a constant variable, which was
        ///forbidden up until the CUDA 3.1 release. Deprecated This error return is deprecated
        ///as of CUDA 3.1. Variables in constant memory may now have their address taken by
        ///the runtime via cudaGetSymbolAddress().
        TextureFetchFailed = 23,
        ///This indicated that a texture fetch was not able to be performed. This was previously
        ///used for device emulation of texture operations. Deprecated This error return is
        ///deprecated as of CUDA 3.1. Device emulation mode was removed with the CUDA 3.1
        ///release.
        TextureNotBound = 24,
        ///This indicated that a texture was not bound for access. This was previously used for
        ///device emulation of texture operations. Deprecated This error return is deprecated as
        ///of CUDA 3.1. Device emulation mode was removed with the CUDA 3.1 release.
        SynchronizationError = 25,
        ///This indicated that a synchronization operation had failed. This was previously used
        ///for some device emulation functions. Deprecated This error return is deprecated as
        ///of CUDA 3.1. Device emulation mode was removed with the CUDA 3.1 release.
        InvalidFilterSetting = 26,
        ///This indicates that a non-float texture was being accessed with linear filtering. This is
        ///not supported by CUDA.
        InvalidNormSetting = 27,
        ///This indicates that an attempt was made to read a non-float texture as a normalized
        ///float. This is not supported by CUDA.
        MixedDeviceExecution = 28,
        ///Mixing of device and device emulation code was not allowed. Deprecated This error
        ///return is deprecated as of CUDA 3.1. Device emulation mode was removed with the
        ///CUDA 3.1 release.
        CudartUnloading = 29,
        ///This indicates that a CUDA Runtime API call cannot be executed because it is being
        ///called during process shut down, at a point in time after CUDA driver has been
        ///unloaded.
        Unknown = 30,
        ///This indicates that an unknown internal error has occurred.
        NotYetImplemented = 31,
        ///This indicates that the API call is not yet implemented. Production releases of CUDA
        ///will never return this error. Deprecated This error return is deprecated as of CUDA
        ///4.1.
        MemoryValueTooLarge = 32,
        ///This indicated that an emulated device pointer exceeded the 32-bit address range.
        ///Deprecated This error return is deprecated as of CUDA 3.1. Device emulation mode
        ///was removed with the CUDA 3.1 release.
        InvalidResourceHandle = 33,
        ///This indicates that a resource handle passed to the API call was not valid. Resource
        ///handles are opaque types like cudaStream_t and cudaEvent_t.
        NotReady = 34,
        ///This indicates that asynchronous operations issued previously have not completed
        ///yet. This result is not actually an error, but must be indicated differently than
        ///cudaSuccess (which indicates completion). Calls that may return this value include
        ///cudaEventQuery() and cudaStreamQuery().
        InsufficientDriver = 35,
        ///This indicates that the installed NVIDIA CUDA driver is older than the CUDA
        ///runtime library. This is not a supported configuration. Users should install an
        ///updated NVIDIA display driver to allow the application to run.
        SetOnActiveProcess = 36,
        ///This indicates that the user has called cudaSetValidDevices(), cudaSetDeviceFlags(),
        ///cudaD3D9SetDirect3DDevice(), cudaD3D10SetDirect3DDevice,
        ///cudaD3D11SetDirect3DDevice(), or cudaVDPAUSetVDPAUDevice() after initializing
        ///the CUDA runtime by calling non-device management operations (allocating
        ///memory and launching kernels are examples of non-device management operations).
        ///This error can also be returned if using runtime/driver interoperability and there is an
        ///existing CUcontext active on the host thread.
        InvalidSurface = 37,
        ///This indicates that the surface passed to the API call is not a valid surface.
        NoDevice = 38,
        ///This indicates that no CUDA-capable devices were detected by the installed CUDA
        ///driver.
        ECCUncorrectable = 39,
        ///This indicates that an uncorrectable ECC error was detected during execution.
        SharedObjectSymbolNotFound = 40,
        ///This indicates that a link to a shared object failed to resolve.
        SharedObjectInitFailed = 41,
        ///This indicates that initialization of a shared object failed.
        UnsupportedLimit = 42,
        ///This indicates that the cudaLimit passed to the API call is not supported by the active
        ///device.
        DuplicateVariableName = 43,
        ///This indicates that multiple global or constant variables (across separate CUDA
        ///source files in the application) share the same string name.
        DuplicateTextureName = 44,
        ///This indicates that multiple textures (across separate CUDA source files in the
        ///application) share the same string name.
        DuplicateSurfaceName = 45,
        ///This indicates that multiple surfaces (across separate CUDA source files in the
        ///application) share the same string name.
        DevicesUnavailable = 46,
        ///This indicates that all CUDA devices are busy or unavailable at the current time.
        ///Devices are often busy/unavailable due to use of cudaComputeModeExclusive,
        ///cudaComputeModeProhibited or when long running CUDA kernels have filled up
        ///the GPU and are blocking new work from starting. They can also be unavailable
        ///due to memory constraints on a device that already has active CUDA work being
        ///performed.
        InvalidKernelImage = 47,
        ///This indicates that the device kernel image is invalid.
        NoKernelImageForDevice = 48,
        ///This indicates that there is no kernel image available that is suitable for the device.
        ///This can occur when a user specifies code generation options for a particular CUDA
        ///source file that do not include the corresponding device configuration.
        IncompatibleDriverContext = 49,
        ///This indicates that the current context is not compatible with this the CUDA Runtime.
        ///This can only occur if you are using CUDA Runtime/Driver interoperability and have
        ///created an existing Driver context using the driver API. The Driver context may be
        ///incompatible either because the Driver context was created using an older version
        ///of the API, because the Runtime API call expects a primary driver context and the
        ///Driver context is not primary, or because the Driver context has been destroyed.
        ///Please see Interactions with the CUDA Driver API" for more information.
        PeerAccessAlreadyEnabled = 50,
        ///This error indicates that a call to cudaDeviceEnablePeerAccess() is trying to re-enable
        ///peer addressing on from a context which has already had peer addressing enabled.
        PeerAccessNotEnabled = 51,
        ///This error indicates that cudaDeviceDisablePeerAccess() is trying to disable peer
        ///addressing which has not been enabled yet via cudaDeviceEnablePeerAccess().
        DeviceAlreadyInUse = 54,
        ///This indicates that a call tried to access an exclusive-thread device that is already in
        ///use by a different thread.
        ProfilerDisabled = 55,
        ///This indicates profiler is not initialized for this run. This can happen when the
        ///application is running with external profiling tools like visual profiler.
        ProfilerNotInitialized = 56,
        ///Deprecated This error return is deprecated as of CUDA 5.0. It is no longer an error
        ///to attempt to enable/disable the profiling via cudaProfilerStart or cudaProfilerStop
        ///without initialization.
        ProfilerAlreadyStarted = 57,
        ///Deprecated This error return is deprecated as of CUDA 5.0. It is no longer an error to
        ///call cudaProfilerStart() when profiling is already enabled.
        ProfilerAlreadyStopped = 58,
        ///Deprecated This error return is deprecated as of CUDA 5.0. It is no longer an error to
        ///call cudaProfilerStop() when profiling is already disabled.
        Assert = 59,
        ///An assert triggered in device code during kernel execution. The device cannot be
        ///used again until cudaThreadExit() is called. All existing allocations are invalid and
        ///must be reconstructed if the program is to continue using CUDA.
        TooManyPeers = 60,
        ///This error indicates that the hardware resources required to enable peer access have
        ///been exhausted for one or more of the devices passed to cudaEnablePeerAccess().
        HostMemoryAlreadyRegistered = 61,
        ///This error indicates that the memory range passed to cudaHostRegister() has already
        ///been registered.
        HostMemoryNotRegistered = 62,
        ///This error indicates that the pointer passed to cudaHostUnregister() does not
        ///correspond to any currently registered memory region.
        OperatingSystem = 63,
        ///This error indicates that an OS call failed.
        PeerAccessUnsupported = 64,
        ///This error indicates that P2P access is not supported across the given devices.
        LaunchMaxDepthExceeded = 65,
        ///This error indicates that a device runtime grid launch did not occur because the
        ///depth of the child grid would exceed the maximum supported number of nested grid
        ///launches.
        LaunchFileScopedTex = 66,
        ///This error indicates that a grid launch did not occur because the kernel uses filescoped
        ///textures which are unsupported by the device runtime. Kernels launched via
        ///the device runtime only support textures created with the Texture Object API's.
        LaunchFileScopedSurf = 67,
        ///This error indicates that a grid launch did not occur because the kernel uses filescoped
        ///surfaces which are unsupported by the device runtime. Kernels launched via
        ///the device runtime only support surfaces created with the Surface Object API's.
        SyncDepthExceeded = 68,
        ///This error indicates that a call to cudaDeviceSynchronize made from the
        ///device runtime failed because the call was made at grid depth greater
        ///than than either the default (2 levels of grids) or user specified device
        ///limit cudaLimitDevRuntimeSyncDepth. To be able to synchronize on
        ///launched grids at a greater depth successfully, the maximum nested depth
        ///at which cudaDeviceSynchronize will be called must be specified with the
        ///cudaLimitDevRuntimeSyncDepth limit to the cudaDeviceSetLimit api before the
        ///host-side launch of a kernel using the device runtime. Keep in mind that additional
        ///levels of sync depth require the runtime to reserve large amounts of device memory
        ///that cannot be used for user allocations.
        LaunchPendingCountExceeded = 69,
        ///This error indicates that a device runtime grid launch failed because the launch
        ///would exceed the limit cudaLimitDevRuntimePendingLaunchCount. For this
        ///launch to proceed successfully, cudaDeviceSetLimit must be called to set the
        ///cudaLimitDevRuntimePendingLaunchCount to be higher than the upper bound of
        ///outstanding launches that can be issued to the device runtime. Keep in mind that
        ///raising the limit of pending device runtime launches will require the runtime to
        ///reserve device memory that cannot be used for user allocations.
        NotPermitted = 70,
        ///This error indicates the attempted operation is not permitted.
        NotSupported = 71,
        ///This error indicates the attempted operation is not supported on the current system
        ///or device.
        HardwareStackError = 72,
        ///Device encountered an error in the call stack during kernel execution, possibly due
        ///to stack corruption or exceeding the stack size limit. This leaves the process in an
        ///inconsistent state and any further CUDA work will return the same error. To continue
        ///using CUDA, the process must be terminated and relaunched.
        IllegalInstruction = 73,
        ///The device encountered an illegal instruction during kernel execution This leaves
        ///the process in an inconsistent state and any further CUDA work will return the same
        ///error. To continue using CUDA, the process must be terminated and relaunched.
        MisalignedAddress = 74,
        ///The device encountered a load or store instruction on a memory address which is not
        ///aligned. This leaves the process in an inconsistent state and any further CUDA work
        ///will return the same error. To continue using CUDA, the process must be terminated
        ///and relaunched.
        InvalidAddressSpace = 75,
        ///While executing a kernel, the device encountered an instruction which can only
        ///operate on memory locations in certain address spaces (global, shared, or local),
        ///but was supplied a memory address not belonging to an allowed address space.
        ///This leaves the process in an inconsistent state and any further CUDA work will
        ///return the same error. To continue using CUDA, the process must be terminated and
        ///relaunched.
        InvalidPc = 76,
        ///The device encountered an invalid program counter. This leaves the process in an
        ///inconsistent state and any further CUDA work will return the same error. To continue
        ///using CUDA, the process must be terminated and relaunched.
        IllegalAddress = 77,
        ///The device encountered a load or store instruction on an invalid memory address.
        ///This leaves the process in an inconsistent state and any further CUDA work will
        ///return the same error. To continue using CUDA, the process must be terminated and
        ///relaunched.
        InvalidPtx = 78,
        ///A PTX compilation failed. The runtime may fall back to compiling PTX if an
        ///application does not contain a suitable binary for the current device.
        InvalidGraphicsContext = 79,
        ///This indicates an error with the OpenGL or DirectX context.
        NvlinkUncorrectable = 80,
        ///This indicates that an uncorrectable NVLink error was detected during the execution.
        JitCompilerNotFound = 81,
        ///This indicates that the PTX JIT compiler library was not found. The JIT Compiler
        ///library is used for PTX compilation. The runtime may fall back to compiling PTX if an
        ///application does not contain a suitable binary for the current device.
        CooperativeLaunchTooLarge = 82,
        ///This error indicates that the number of blocks launched per grid for a
        ///kernel that was launched via either cudaLaunchCooperativeKernel or
        ///cudaLaunchCooperativeKernelMultiDevice exceeds the maximum number
        ///of blocks as allowed by cudaOccupancyMaxActiveBlocksPerMultiprocessor
        ///or cudaOccupancyMaxActiveBlocksPerMultiprocessorWithFlags times
        ///the number of multiprocessors as specified by the device attribute
        ///cudaDevAttrMultiProcessorCount.
        StartupFailure = 0x7f,
        ///This indicates an internal startup failure in the CUDA runtime.
        ApiFailureBase = 10000
        ///Any unhandled CUDA driver error is added to this value and returned via the
        ///runtime. Production releases of CUDA should not return such errors. Deprecated
        ///This error return is deprecated as of CUDA 4.1.
    }

    internal enum MemcpyKind
    {
        /// <summary>
        /// Host -> Host
        /// </summary>
        HostToHost = 0,

        /// <summary>
        /// Host -> Device
        /// </summary>
        HostToDevice = 1,

        /// <summary>
        /// Device -> Host
        /// </summary>
        DeviceToHost = 2,

        /// <summary>
        /// Device -> Device
        /// </summary>
        DeviceToDevice = 3,

        /// <summary>
        /// Direction of the transfer
        /// </summary>
        Default = 4
    }

    #endregion

    #region Sparse

    public enum SparseStatus
    {
        /// <summary>
        /// The operation completed successfully.
        /// </summary>
        Success = 0,
        /// <summary>
        /// "The CUSPARSE library was not initialized. This is usually caused by the lack of a prior 
        /// cusparseCreate() call, an error in the CUDA Runtime API called by the CUSPARSE routine, or an 
        /// error in the hardware setup. To correct: call cusparseCreate() prior to the function call; and
        ///  check that the hardware, an appropriate version of the driver, and the CUSPARSE library are 
        /// correctly installed.
        /// </summary>
        NotInitialized = 1,
        /// <summary>
        ///  "Resource allocation failed inside the CUSPARSE library. This is usually caused by a 
        /// cudaMalloc() failure. To correct: prior to the function call, deallocate previously allocated
        /// memory as much as possible.
        /// </summary>
        AllocFailed = 2,
        /// <summary>
        /// "An unsupported value or parameter was passed to the function (a negative vector size, 
        /// for example). To correct: ensure that all the parameters being passed have valid values.
        /// </summary>
        InvalidValue = 3,
        /// <summary>
        /// "The function requires a feature absent from the device architecture; usually caused by 
        /// the lack of support for atomic operations or double precision. To correct: compile and run the
        ///  application on a device with appropriate compute capability, which is 1.1 for 32-bit atomic 
        /// operations and 1.3 for double precision.
        /// </summary>
        ArchMismatch = 4,
        /// <summary>
        /// "An access to GPU memory space failed, which is usually caused by a failure to bind a texture. 
        /// To correct: prior to the function call, unbind any previously bound textures.
        /// </summary>
        MappingError = 5,
        /// <summary>
        /// "The GPU program failed to execute. This is often caused by a launch failure of the kernel on 
        /// the GPU, which can be caused by multiple reasons. To correct: check that the hardware, an appropriate
        ///  version of the driver, and the CUSPARSE library are correctly installed.
        /// </summary>
        ExecutionFailed = 6,
        /// <summary>
        /// "An internal CUSPARSE operation failed. This error is usually caused by a cudaMemcpyAsync() 
        /// failure. To correct: check that the hardware, an appropriate version of the driver, and the CUSPARSE
        ///  library are correctly installed. Also, check that the memory passed as a parameter to the routine 
        /// is not being deallocated prior to the routine’s completion.
        /// </summary>
        InternalError = 7,
        /// <summary>
        /// "The matrix type is not supported by this function. This is usually caused by passing an invalid 
        /// matrix descriptor to the function. To correct: check that the fields in IntPtr_t descrA were 
        /// set correctly.
        /// </summary>
        MatrixTypeNotSupported = 8,
        /// <summary>
        ///
        /// </summary>
        ZeroPivot = 9
    }

    public enum MatrixType
    {
        /// <summary>
        /// the matrix is general.
        /// </summary>
        General = 0,
        /// <summary>
        /// the matrix is symmetric.
        /// </summary>
        Symmetric = 1,
        /// <summary>
        /// the matrix is Hermitian.
        /// </summary>
        Hermitian = 2,
        /// <summary>
        /// the matrix is triangular.
        /// </summary>
        Triangular = 3
    }

    internal enum FillMode
    {
        /// <summary>
        /// the lower triangular part is stored.
        /// </summary>
        Lower = 0,
        /// <summary>
        /// the upper triangular part is stored.
        /// </summary>
        Upper = 1
    }

    internal enum DiagonalType
    {
        /// <summary>
        /// the matrix diagonal has non-unit elements.
        /// </summary>
        NonUnit = 0,
        /// <summary>
        /// the matrix diagonal has unit elements.
        /// </summary>
        Unit = 1
    }

    internal enum IndexBase
    {
        /// <summary>
        /// the base index is zero.
        /// </summary>
        Zero = 0,
        /// <summary>
        /// the base index is one.
        /// </summary>
        One = 1
    }

    #endregion

    #region Solver

    public enum SolverStatus
    {
        /// <summary>
        /// The operation completed successfully
        /// </summary>
        Success = 0,
        /// <summary>
        /// The cuSolver library was not initialized. This is usually caused by the
        /// lack of a prior call, an error in the CUDA Runtime API called by the
        /// cuSolver routine, or an error in the hardware setup.<para/>
        /// To correct: call cusolverCreate() prior to the function call; and
        /// check that the hardware, an appropriate version of the driver, and the
        /// cuSolver library are correctly installed.
        /// </summary>
        NotInititialized = 1,
        /// <summary>
        /// Resource allocation failed inside the cuSolver library. This is usually
        /// caused by a cudaMalloc() failure.<para/>
        /// To correct: prior to the function call, deallocate previously allocated
        /// memory as much as possible.
        /// </summary>
        AllocFailed = 2,
        /// <summary>
        /// An unsupported value or parameter was passed to the function (a
        /// negative vector size, for example).<para/>
        /// To correct: ensure that all the parameters being passed have valid
        /// values.
        /// </summary>
        InvalidValue = 3,
        /// <summary>
        /// The function requires a feature absent from the device architecture;
        /// usually caused by the lack of support for atomic operations or double
        /// precision.<para/>
        /// To correct: compile and run the application on a device with compute
        /// capability 2.0 or above.
        /// </summary>
        ArchMismatch = 4,
        /// <summary>
        /// 
        /// </summary>
        MappingError = 5,
        /// <summary>
        /// The GPU program failed to execute. This is often caused by a launch
        /// failure of the kernel on the GPU, which can be caused by multiple
        /// reasons.<para/>
        /// To correct: check that the hardware, an appropriate version of the
        /// driver, and the cuSolver library are correctly installed.
        /// </summary>
        ExecutionFailed = 6,
        /// <summary>
        /// An internal cuSolver operation failed. This error is usually caused by a
        /// cudaMemcpyAsync() failure.<para/>
        /// To correct: check that the hardware, an appropriate version of the
        /// driver, and the cuSolver library are correctly installed. Also, check
        /// that the memory passed as a parameter to the routine is not being
        /// deallocated prior to the routine’s completion.
        /// </summary>
        InternalError = 7,
        /// <summary>
        /// The matrix type is not supported by this function. This is usually caused
        /// by passing an invalid matrix descriptor to the function.<para/>
        /// To correct: check that the fields in descrA were set correctly.
        /// </summary>
        MatrixTypeNotSupported = 8,
        /// <summary>
        /// 
        /// </summary>
        NotSupported = 9,
        /// <summary>
        /// 
        /// </summary>
        ZeroPivot = 10,
        /// <summary>
        /// 
        /// </summary>
        InvalidLicense = 11
    }
    
    #endregion
}
