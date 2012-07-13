using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pici.Collections.Algorithm
{
    public interface IPathNode
    {
        Boolean IsBlocked(int x,int y, bool lastTile);
    }
}
