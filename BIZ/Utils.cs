using System;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic; 

namespace BIZ
{
    public static class Utils
    {
        public static MessageBoxResult ConfirmMessageBox(string message)
        {
            string title = "Confirm Message Window";
            MessageBoxResult result = MessageBox.Show(message, title, MessageBoxButton.OKCancel);
            return result;
        }

        public static void ShowMessageBox(string message)
        {
            string title = "Information Message Window";
            MessageBox.Show(message, title, MessageBoxButton.OK);
        }
        public static void PopulateComboBox(ComboBox comboBox, string textItem)
        {
            for (int i = 0; i < comboBox.Items.Count; i++)
            {
                var item = comboBox.Items[i];
                if (item.ToString() == textItem)
                {
                    comboBox.SelectedIndex = i;
                    return;
                }
            }
        }

    }
}
