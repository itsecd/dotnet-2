using System;

namespace Lab2.Repositories
{
    public class NotFoundException : Exception
    {
        public NotFoundException() { }

        public NotFoundException(string mess) : base(mess) { }
    }
}
