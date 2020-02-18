using Util;

namespace Api.CashDrawerApi
{
    internal static class Constants
    {
        internal static readonly string CloseReturnValue = Util.Util.HexToString("14 00 00 0F");
        internal static readonly string OpenReturnValue = Util.Util.HexToString("10 00 00 0F");
    }
}