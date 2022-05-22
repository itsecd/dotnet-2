using System;

namespace Lab2.NotFoundDataException
{
    public class NotFoundException : Exception
    {
        public NotFoundException() { }

        public NotFoundException(string mess) : base(mess) { }
    }
}
