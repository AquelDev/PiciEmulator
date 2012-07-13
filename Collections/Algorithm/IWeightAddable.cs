using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pici.Collections.Algorithm
{
    public interface IWeightAddable<T>
    {
        T WeightChange { get; set; }
    }
}
