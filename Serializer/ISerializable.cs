using System.Windows.Controls;

namespace BookingApp.Serializer
{
    public interface ISerializable
    {
        string GetHeader();
        string[] ToCsv();
        void FromCsv(string[] values);
    }
}
