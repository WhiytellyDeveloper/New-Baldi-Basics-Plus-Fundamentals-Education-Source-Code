using System;
using System.Collections.Generic;
using System.Text;

namespace nbppfe.PrefabSystem
{
    public interface INPCPrefab : IPrefab
    {
        public void PostLoading();
    }
}
