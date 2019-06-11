using System;
using bv.common;
using bv.common.win;

namespace EIDSS.Tests.RAM.Helpers.Fake
{
    public class BaseFormStub : BaseForm
    {

        public override bool Post(PostType postType)
        {
            switch (postType)
            {
                case PostType.FinalPosting:
                    return true;
                case PostType.IntermediatePosting:
                    return false;
            }
            throw new ApplicationException("not implemented");
        }
    }
}