
namespace CSparse.Interop.Hypre
{
    using CSparse.Interop.Common;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    public class HypreVector<T> : IDisposable
        where T : struct
    {
        HYPRE_IJVector vec;

        GCHandle pi, pv;

        int length;

        public HypreVector(T[] values)
            : this(values, values.Length)
        {
        }

        public HypreVector(T[] values, int length)
        {
            this.length = length;

            NativeMethods.HYPRE_IJVectorCreate(Constants.MPI_COMM_WORLD, 0, length, out vec);
            NativeMethods.HYPRE_IJVectorSetObjectType(vec, Constants.HYPRE_PARCSR);
            NativeMethods.HYPRE_IJVectorInitialize(vec);

            var indices = GetIndices(0, length);

            pi = GCHandle.Alloc(indices, GCHandleType.Pinned);
            pv = GCHandle.Alloc(values, GCHandleType.Pinned);

            NativeMethods.HYPRE_IJVectorSetValues(vec, length,
                pi.AddrOfPinnedObject(),
                pv.AddrOfPinnedObject());

            NativeMethods.HYPRE_IJVectorAssemble(vec);
        }

        internal int Synchronize()
        {
            return NativeMethods.HYPRE_IJVectorGetValues(vec, length,
                pi.AddrOfPinnedObject(),
                pv.AddrOfPinnedObject());
        }

        internal HYPRE_ParVector GetObject()
        {
            NativeMethods.HYPRE_IJVectorGetObject(vec, out var par_vec);

            return par_vec;
        }

        // TODO: use cache so we don't have to create the same indices all time.
        private static int[] GetIndices(int start, int end)
        {
            int length = end - start;

            var indices = new int[length];

            for (int i = 0; i < length; i++)
            {
                indices[i] = start + i;
            }

            return indices;
        }

        public void Dispose()
        {
            pi.Free();
            pv.Free();

            if (vec.Ptr != IntPtr.Zero)
            {
                NativeMethods.HYPRE_IJVectorDestroy(vec);
                vec.Ptr = IntPtr.Zero;
            }
        }
    }
}
