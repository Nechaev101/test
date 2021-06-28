using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace AutomaticMEPTrace
{
    [ContentProperty("Children")]
    public partial class StepPanel : UserControl
    {

        public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register(nameof(IsOpen), typeof(bool), typeof(StepPanel), new PropertyMetadata(default(bool)));

        public bool IsOpen
        {
            get { return (bool) GetValue(IsOpenProperty); }
            set { SetValue(IsOpenProperty, value); }
        }
        
        public UIElementCollection Children { get { return this.StackPanel.Children; } }

        public static readonly DependencyProperty ItemTemplateProperty = DependencyProperty.Register(
            nameof(ItemTemplate), typeof(DataTemplate), typeof(StepPanel), new PropertyMetadata(default(DataTemplate)));

        public DataTemplate ItemTemplate
        {
            get => (DataTemplate)GetValue(ItemTemplateProperty);
            set => SetValue(ItemTemplateProperty, value);
        }

        public StepPanel()
        {
            InitializeComponent();
        }
    }
}