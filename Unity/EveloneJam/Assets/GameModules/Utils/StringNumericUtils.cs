namespace Project.Utils
{
    public static class StringNumericUtils
    {
        public static string GetOrdinalSuffix(int num)
        {
            string number = num.ToString();
            if (number.EndsWith("11")) return "ый";
            if (number.EndsWith("12")) return "ый";
            if (number.EndsWith("13")) return "ый";
            if (number.EndsWith("14")) return "ый";
            if (number.EndsWith("15")) return "ый";
            if (number.EndsWith("16")) return "ый";
            if (number.EndsWith("17")) return "ый";
            if (number.EndsWith("18")) return "ый";
            if (number.EndsWith("19")) return "ый";
            if (number.EndsWith("0")) return "ый";
            if (number.EndsWith("1")) return "ый";
            if (number.EndsWith("2")) return "ой";
            if (number.EndsWith("3")) return "ий";
            if (number.EndsWith("4")) return "ый";
            if (number.EndsWith("5")) return "ый";
            if (number.EndsWith("6")) return "ой";
            if (number.EndsWith("7")) return "ой";
            if (number.EndsWith("8")) return "ой";
            if (number.EndsWith("9")) return "ый";
            return "ый";
        }
    }
}
