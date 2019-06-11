﻿using System;

namespace eidss.model.AVR.DataBase
{
    [Serializable]
    public class AvrDataException : ApplicationException
    {
        public AvrDataException()
        {
        }

        public AvrDataException(string message) : base(message)
        {
        }

        public AvrDataException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}