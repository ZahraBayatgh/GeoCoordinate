using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Opeqe.Infrastructure.Toolkits.PersianToolkit
{
    public static class UnicodeExtensions
    {
        public static string RemoveDiacritics(this string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return string.Empty;
            }

            var normalizedString = text.Normalize(NormalizationForm.FormKC);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        public static string CleanUnderLines(this string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return string.Empty;
            }

            const char chr1600 = (char)1600; //ـ=1600
            const char chr8204 = (char)8204; //‌=8204

            return text.Replace(chr1600.ToString(), "")
                       .Replace(chr8204.ToString(), "");
        }

        public static string RemovePunctuation(this string text)
        {
            return string.IsNullOrWhiteSpace(text) ?
                string.Empty :
                new string(text.Where(c => !char.IsPunctuation(c)).ToArray());
        }
        public static string PersianToEnglish(this string persianStr)
        {
            var LettersDictionary = new Dictionary<string, string>
            {
                ["۰"] = "0",
                ["۱"] = "1",
                ["۲"] = "2",
                ["۳"] = "3",
                ["۴"] = "4",
                ["۵"] = "5",
                ["۶"] = "6",
                ["۷"] = "7",
                ["۸"] = "8",
                ["۹"] = "9",
                ["ض"] = "q",
                ["ص"] = "w",
                ["ث"] = "e",
                ["ق"] = "r",
                ["ف"] = "t",
                ["غ"] = "y",
                ["ع"] = "u",
                ["ه"] = "i",
                ["خ"] = "o",
                ["ح"] = "p",
                ["ج"] = "[",
                ["چ"] = "]",
                ["ش"] = "a",
                ["س"] = "s",
                ["ی"] = "d",
                ["ب"] = "b",
                ["ل"] = "g",
                ["ا"] = "h",
                ["ت"] = "j",
                ["ن"] = "k",
                ["م"] = "l",
                ["ک"] = ";",
                ["گ"] = "'",
                ["پ"] = "\\",
                ["ظ"] = "z",
                ["ط"] = "x",
                ["ز"] = "c",
                ["ژ"] = "C",
                ["ر"] = "v",
                ["ذ"] = "b",
                ["د"] = "n",
                ["ئ"] = "m",
                ["و"] = ",",
            };
            return LettersDictionary.Aggregate(persianStr, (current, item) =>
                         current.Replace(item.Key, item.Value));
        }
    }

}