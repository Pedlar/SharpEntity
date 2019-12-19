using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEngine
{
    public interface IComponent
    {
        void Setup();
        void OnLoaded();
    }
}
