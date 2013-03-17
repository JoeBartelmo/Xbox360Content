using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xbox360Content.STFS.Metadata
{
    internal class Version1 : Header
    {
        public Version1(ref IO io)
            : base(ref io)
        {
            if (base.Version2)
                throw new InvalidOperationException();
            else
            {

            }
        }
    }
}
