using System.Globalization;
namespace Opeqe.Infrastructure
{
    /// <summary>
    /// Converts English digits of a given number to their equivalent Persian digits.
    /// </summary>
    public static class PersianNumbersUtils
    {
        /// <summary>
        /// Converts English digits of a given number to their equivalent Persian digits.
        /// </summary>
        public static string ToPersianNumbers(this int number, string format = "")
        {
            return ((!string.IsNullOrEmpty(format)) ? number.ToString(format) : number.ToString(CultureInfo.InvariantCulture)).ToPersianNumbers();
        }

        /// <summary>
        /// Converts English digits of a given number to their equivalent Persian digits.
        /// </summary>
        public static string ToPersianNumbers(this long number, string format = "")
        {
            return ((!string.IsNullOrEmpty(format)) ? number.ToString(format) : number.ToString(CultureInfo.InvariantCulture)).ToPersianNumbers();
        }

        /// <summary>
        /// Converts English digits of a given number to their equivalent Persian digits.
        /// </summary>
        public static string ToPersianNumbers(this int? number, string format = "")
        {
            if (!number.HasValue)
            {
                number = 0;
            }
            int value;
            object data;
            if (string.IsNullOrEmpty(format))
            {
                value = number.Value;
                data = value.ToString(CultureInfo.InvariantCulture);
            }
            else
            {
                value = number.Value;
                data = value.ToString(format);
            }
            return ((string)data).ToPersianNumbers();
        }

        /// <summary>
        /// Converts English digits of a given number to their equivalent Persian digits.
        /// </summary>
        public static string ToPersianNumbers(this long? number, string format = "")
        {
            if (!number.HasValue)
            {
                number = 0L;
            }
            long value;
            object data;
            if (string.IsNullOrEmpty(format))
            {
                value = number.Value;
                data = value.ToString(CultureInfo.InvariantCulture);
            }
            else
            {
                value = number.Value;
                data = value.ToString(format);
            }
            return ((string)data).ToPersianNumbers();
        }

        /// <summary>
        /// Converts English digits of a given string to their equivalent Persian digits.
        /// </summary>
        /// <param name="data">English number</param>
        /// <returns></returns>
        public static string ToPersianNumbers(this string data)
        {
            if (string.IsNullOrWhiteSpace(data))
            {
                return string.Empty;
            }
            return data.ToEnglishNumbers().Replace("0", "۰").Replace("1", "۱")
                .Replace("2", "۲")
                .Replace("3", "۳")
                .Replace("4", "۴")
                .Replace("5", "۵")
                .Replace("6", "۶")
                .Replace("7", "۷")
                .Replace("8", "۸")
                .Replace("9", "۹")
                .Replace(".", ",");
        }

        /// <summary>
        /// Converts Persian and Arabic digits of a given string to their equivalent English digits.
        /// </summary>
        /// <param name="data">Persian number</param>
        /// <returns></returns>
        public static string ToEnglishNumbers(this string data)
        {
            if (string.IsNullOrWhiteSpace(data))
            {
                return string.Empty;
            }
            return data.Replace("٠", "0").Replace("۰", "0").Replace("١", "1")
                .Replace("۱", "1")
                .Replace("٢", "2")
                .Replace("۲", "2")
                .Replace("٣", "3")
                .Replace("۳", "3")
                .Replace("٤", "4")
                .Replace("۴", "4")
                .Replace("٥", "5")
                .Replace("۵", "5")
                .Replace("٦", "6")
                .Replace("۶", "6")
                .Replace("٧", "7")
                .Replace("۷", "7")
                .Replace("٨", "8")
                .Replace("۸", "8")
                .Replace("٩", "9")
                .Replace("۹", "9");
        }
    }
}
