using System;
using System.Windows;
using System.Windows.Controls;

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
    }
}
