using System;
using RPD.DAL;

namespace RPD.Presentation.Mocks.DAL
{
    class DataPointMock : IDataPoint
    {
        public DataPointMock(double value, DateTime time)
        {
            Value = value;
            Time = time;
            Valid = true;
        }

        #region Implementation of IDataPoint

        public double Value { get; }
        public DateTime Time { get; }
        public bool Valid { get; }

        public int DataPosition => 0;

        public byte[] GetCommandBytes()
        {
            return new byte[] {1, 2, 4, 5, 6};
        }

        #endregion
    }
}