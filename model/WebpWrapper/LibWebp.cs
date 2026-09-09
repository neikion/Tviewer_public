using System;
using System.Runtime.InteropServices;

namespace Tviewer.model.WebpWrapper
{
    
    internal sealed class LibWebp
    {
        public const int WEBP_DECODER_ABI_VERSION = 0x0210;    // MAJOR(8b) + MINOR(8b)

        [DllImport("./Library/libwebp.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int WebPGetInfo(IntPtr p, long data_size, out int width, out int height);

        [DllImport("./Library/libwebp.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int WebPFree(IntPtr p);

        [DllImport("./Library/libwebp.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern VP8StatusCode WebPGetFeaturesInternal(IntPtr data, long data_size, out WebPBitstreamFeatures features, int version);
    }

}
