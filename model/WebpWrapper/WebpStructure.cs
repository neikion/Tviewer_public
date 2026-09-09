using System.Runtime.InteropServices;

namespace Tviewer.model.WebpWrapper
{
    [StructLayout(LayoutKind.Sequential)]
    public struct WebPBitstreamFeatures
    {
        public int width;
        public int height;
        public int has_alpha;
        public int has_animation; // 1 = animation
        public int format; //0=mixed 1=lossy 2=loseless
        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 5, ArraySubType = UnmanagedType.U4)]
        public readonly uint[] pad;
        public override string ToString()
        {
            string report = $"width : {width}\nheight : {height}\nalpha : {has_alpha}\nanimation : {has_animation}\nformat : {format}";
            return report;
        }
    }
    public enum VP8StatusCode
    {
        VP8_STATUS_OK = 0,
        VP8_STATUS_OUT_OF_MEMORY,
        VP8_STATUS_INVALID_PARAM,
        VP8_STATUS_BITSTREAM_ERROR,
        VP8_STATUS_UNSUPPORTED_FEATURE,
        VP8_STATUS_SUSPENDED,
        VP8_STATUS_USER_ABORT,
        VP8_STATUS_NOT_ENOUGH_DATA
    }
}
