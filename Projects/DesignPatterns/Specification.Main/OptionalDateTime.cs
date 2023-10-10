using System;
using System.IO;

namespace ConsoleApp
{
    public class OptionalDateTime
    {
        private DateTime? dateTime;
        private OptionalDateTime() { }
        public OptionalDateTime(DateTime dateTime) => this.dateTime = dateTime;
        public DateTime Value
        {
            get => dateTime ?? throw new InvalidDataException();
            set => dateTime = value;
        }
        public bool IsSpecified => dateTime.HasValue;
        public static OptionalDateTime Unspecified => new OptionalDateTime();
    }
}
