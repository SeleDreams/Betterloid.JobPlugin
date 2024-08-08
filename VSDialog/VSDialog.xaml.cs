using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace VSDialog
{
    public partial class VSDialog : Window
    {
        private Dictionary<string, object> _vsDialogResults;
        public Dictionary<string, object> Results => _vsDialogResults != null ? _vsDialogResults : _vsDialogResults = new Dictionary<string, object>();

        public VSDialog()
        {
            InitializeComponent();
        }

        public bool DoModal()
        {
            Results.Clear();
            bool result = ShowDialog() ?? false;
            if (result)
            {
                var fieldGrid = xFieldGrid;
                foreach (UIElement element in fieldGrid.Children)
                {
                    if (Grid.GetColumn(element) == 1)
                    {
                        if (element is TextBox)
                        {
                            TextBox tex = (TextBox)element;
                            Results.Add(tex.Name, tex.Text);
                        }
                        else if (element is CheckBox)
                        {
                            CheckBox check = (CheckBox)element;
                            Results.Add(check.Name, check.IsChecked ?? false);
                        }
                        else if (element is ComboBox)
                        {
                            ComboBox combo = (ComboBox)element;
                            Results.Add(combo.Name, combo.Text);
                        }
                        
                    }
                }
            }
            ClearFields();
            return result;
        }

        private void AddField(string labelText, UIElement inputControl)
        {
            RowDefinition rowDefinition = new RowDefinition();
            xFieldGrid.RowDefinitions.Add(rowDefinition);

            // Create a new label
            Label label = new Label
            {
                Content = labelText,
                Height = 30,
                Margin = new Thickness(4),
                VerticalAlignment = VerticalAlignment.Center,
            };
            Grid.SetRow(label, xFieldGrid.RowDefinitions.Count - 1);
            Grid.SetColumn(label, 0);
            xFieldGrid.Children.Add(label);

            // Set common properties for the input control
            inputControl.SetValue(Grid.RowProperty, xFieldGrid.RowDefinitions.Count - 1);
            inputControl.SetValue(Grid.ColumnProperty, 1);
            xFieldGrid.Children.Add(inputControl);

            xFieldGrid.UpdateLayout();
        }

        public string GetTextField(string fieldName)
        {
            return (string)Results[fieldName];
        }

        public bool GetBoolField(string fieldName)
        {
            return (bool)Results[fieldName];
        }

        public float GetFloatField(string fieldName)
        {
            string field = (string)Results[fieldName];
            return float.Parse(field);
        }

        public int GetIntField(string fieldName)
        {
            string field = (string)Results[fieldName];
            return int.Parse(field);
        }


        public void AddTextField(string labelText, string fieldName, string initialValue = "")
        {
            TextBox textBox = new TextBox
            {
                VerticalContentAlignment = VerticalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Height = double.NaN,
                Width = 250,
                MaxWidth = 250,
                Margin = new Thickness(4),
                Name = fieldName,
                TextWrapping = TextWrapping.Wrap,
                Text = initialValue
                
            };
            InputMethod.SetIsInputMethodEnabled(textBox, true);
            
            AddField(labelText, textBox);
        }

        public void AddFloatField(string labelText, string fieldName,float initialValue = 0)
        {
            TextBox numberBox = new TextBox()
            {
                VerticalContentAlignment = VerticalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Height = double.NaN,
                Width = 250,
                MaxWidth = 250,
                Margin = new Thickness(4),
                Name = fieldName,
                MaxLength = 12,
                Text = initialValue.ToString()
            };
            numberBox.PreviewTextInput += FloatBox_PreviewTextInput;
            AddField(labelText, numberBox);
        }

        public void AddIntField(string labelText, string fieldName, int initialValue)
        {
            TextBox numberBox = new TextBox()
            {
                VerticalContentAlignment = VerticalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Height = double.NaN,
                Width = 250,
                MaxWidth = 250,
                Margin = new Thickness(4),
                Name = fieldName,
                MaxLength = 11,
                Text = initialValue.ToString()
            };
            numberBox.PreviewTextInput += IntBox_PreviewTextInput;
            AddField(labelText, numberBox);
        }

        public void AddBooleanField(string labelText, string fieldName, bool initialValue = false)
        {
            CheckBox checkBox = new CheckBox()
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                Height = 30,
                Margin = new Thickness(4),
                Name = fieldName,
                VerticalAlignment = VerticalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
                HorizontalContentAlignment = HorizontalAlignment.Left,
                IsChecked = initialValue
            };
            AddField(labelText, checkBox);
        }

        public void AddEnumField(string labelText, string fieldName, string[] options,int initialValue = 0)
        {
            ComboBox comboBox = new ComboBox()
            {
                VerticalContentAlignment = VerticalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Height = double.NaN,
                Width = 250,
                MaxWidth = 250,
                Margin = new Thickness(4),
                Name = fieldName,
                ItemsSource = options,
                SelectedIndex = initialValue
            };
            AddField(labelText, comboBox);
        }

        private void FloatBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9.-]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        private void IntBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        public void ClearFields()
        {
            xFieldGrid.Children.Clear();
        }


        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}