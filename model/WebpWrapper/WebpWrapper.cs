using System;

namespace Tviewer.model.WebpWrapper
{
    internal class WebpWrapper
    {
        public static VP8StatusCode WebPGetFeatures(IntPtr data, long data_size, out WebPFeatures features)
        {
            VP8StatusCode status = LibWebp.WebPGetFeaturesInternal(data, data_size, out WebPBitstreamFeatures feature, LibWebp.WEBP_DECODER_ABI_VERSION);
            features = new WebPFeatures(feature);
            return status;
        }

    }
    
    internal struct WebPFeatures
    {
        public int width;
        public int height;
        public bool has_alpha;
        public bool has_animation; // 1 = animation
        public Lossy format; //0=mixed 1=lossy 2=loseless
        public readonly uint[] pad;
        public override string ToString()
        {
            string report = $"width : {width}\nheight : {height}\nalpha : {has_alpha}\nanimation : {has_animation}\nformat : {format}\n";
            return report;
        }
        public WebPFeatures(WebPBitstreamFeatures value)
        {
            width = value.width;
            height = value.height;
            has_alpha = value.has_alpha == 1;
            has_animation = value.has_animation == 1;
            switch (value.format)
            {
                case 0:
                    format = Lossy.Unknown;
                    break;
                case 1:
                    format = Lossy.Lossy;
                    break;
                case 2:
                    format = Lossy.Loseless;
                    break;
            }
            pad = value.pad;
        }
    }
}
