using System;

namespace Exam7
{
    public static class StringExtentions
    {
        
        public static string Genitive(this decimal count, Currency currency)
        {
            var lastDigit = count % 10;
            var lastTwoDigit = count % 100;

            if (lastTwoDigit >= 10 && lastTwoDigit < 20)
            {
                switch (currency)
                {
                    case Currency.RUB:
                        return $"{count} рублей";
                    case Currency.USD:
                        return $"{count} долларов";
                    default:
                        return $"{count} евро";
                }
            }


            switch (lastDigit)
            {
                case 0:
                case 5:
                case 6:
                case 7:
                case 8:
                case 9:
                    {
                        switch (currency)
                        {
                            case Currency.RUB:
                                return $"{count} рублей";
                            case Currency.USD:
                                return $"{count} долларов";
                            default:
                                return $"{count} евро";
                        }
                    }
                case 1:
                    {
                        switch (currency)
                        {
                            case Currency.RUB:
                                return $"{count} рубль";
                            case Currency.USD:
                                return $"{count} доллар";
                            default:
                                return $"{count} евро";
                        }
                    }
                case 2:
                case 3:
                case 4:
                    {
                        switch (currency)
                        {
                            case Currency.RUB:
                                return $"{count} рубля";
                            case Currency.USD:
                                return $"{count} доллара";
                            default:
                                return $"{count} евро";
                        }
                    }
                default:
                    return $"{count} евро";
            }
        }
        public static string RandomString(int length, Random random)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
