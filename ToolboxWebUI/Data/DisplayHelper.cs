namespace ToolboxWebUI.Data;

public class DisplayHelper : IDisplayHelper
{
    public string AddNumericComma(int integer)
    {
        string CommaSeparatedNumber = integer.ToString();

        if (CommaSeparatedNumber.Length > 3)
        {
            decimal CommaCalculation = (CommaSeparatedNumber.Length - 1) / 3;
            int CommasNeeded = (int)Math.Floor(CommaCalculation);
            int StringBuffer = CommaSeparatedNumber.Length - (CommasNeeded * 3);

            for (int i = 1; i <= CommasNeeded; i++)
            {
                CommaSeparatedNumber = CommaSeparatedNumber.Insert(StringBuffer + (i - 1) + ((i - 1) * 3), ",");
            }
        }

        return CommaSeparatedNumber;
    }
}
