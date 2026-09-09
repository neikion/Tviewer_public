using System.Windows.Controls;
using Tviewer.Interfaces;

namespace Tviewer.view
{
    public partial class ImageView : UserControl, IAwake
    {
        bool init = false;
        public ImageView()
        {
            if(!init) InitializeComponent();
        }

        /// <summary>
        /// <para>
        /// Preventing OpenCL conflicts between SkiaSharp and Magick.NET.<br/>
        /// When using SKGLelement (use GPU), It must be executed before Magick.NET(Magick.NET OpenCL).
        /// Avoid conflicts by running A first
        /// </para>
        /// <para>
        /// the following is a list of image operators that have been OpenCL accelerated <br/>
        /// Reference : https://imagemagick.org/opencl/#gsc.tab=0
        /// <list type="bullet">
        /// <listheader>
        /// </listheader>
        /// <item>blur</item>
        /// <item>contrast</item>
        /// <item>charcoal</item>
        /// <item>function</item>
        /// <item>grayscale</item>
        /// <item>motion - blur</item>
        /// <item>resize</item>
        /// </list>
        /// and  they require alpha channel to be enabled
        /// <list type="bullet">
        /// <listheader>
        /// </listheader>
        /// <item>despeckle</item>
        /// <item>equalize</item>
        /// <item>modulate</item>
        /// </list>
        /// </para>
        /// </summary>
        public void Awake()
        {
            init = true;
            InitializeComponent();
        }
    }
}
