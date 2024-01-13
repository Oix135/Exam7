namespace Exam7
{
    public static class StringExtentions
    {
        public static string Genitive(this int count, Currency currency)
        {
            var lastDigit = count % 10;

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
                                return $"{count} рублея";
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

    }
}
