using System;
using System.Windows;

namespace CsvExcelTransformer.Classes
{
    internal static class ConverterMethods
    {
        internal static Visibility GetVisibilityMode(object parameter)
        {
            Visibility mode = Visibility.Visible;

            if (parameter is null)
            {
                return mode;
            }

            if (parameter is Visibility)
            {
                mode = (Visibility)parameter;
            }
            else
            {
                // Let's try to parse the parameter as a Visibility value,
                // throwing an exception when the parsing fails
                try
                {

#pragma warning disable CS8604 // Possible null reference argument.

                    mode = (Visibility)Enum.Parse(typeof(Visibility),
                        parameter.ToString(), true);

#pragma warning restore CS8604 

                }
                catch (FormatException e)
                {
                    throw new FormatException("Invalid Visibility specified as " +
                        "the ConverterParameter.  Use Visible or Collapsed.", e);
                }
            }

            // Return the detected mode
            return mode;
        }

        internal static bool IsVisibilityInverted(object parameter)
        {
            return (GetVisibilityMode(parameter) == Visibility.Collapsed);
        }
    }
}
