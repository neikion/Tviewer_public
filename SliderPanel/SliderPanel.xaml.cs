using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPF_Practice.model;

namespace WPF_Practice.SliderPanel
{
    /// <summary>
    /// SliderPanel.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class SliderPanel : UserControl
    {

        public ObservableCollection<ImageUserCollection> UserCollection
        {
            get { return (ObservableCollection<ImageUserCollection>)GetValue(UserCollectionProperty); }
            set { SetValue(UserCollectionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for UserCollection.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UserCollectionProperty =
            DependencyProperty.Register(nameof(UserCollection), typeof(ObservableCollection<ImageUserCollection>), typeof(SliderPanel), new PropertyMetadata(null));



        public ICommand MenuCommand
        {
            get { return (ICommand)GetValue(MenuCommandProperty); }
            set { SetValue(MenuCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MenuCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MenuCommandProperty =
            DependencyProperty.Register(nameof(MenuCommand), typeof(ICommand), typeof(SliderPanel), new PropertyMetadata(null));



        public ICommand ConfigCommand
        {
            get { return (ICommand)GetValue(ConfigCommandProperty); }
            set { SetValue(ConfigCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ConfigCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ConfigCommandProperty =
            DependencyProperty.Register(nameof(ConfigCommand), typeof(ICommand), typeof(SliderPanel), new PropertyMetadata(null));




        public SliderPanel()
        {
            InitializeComponent();
        }
    }
}
